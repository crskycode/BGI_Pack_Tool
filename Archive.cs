using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGI_Pack_Tool
{
    static class Archive
    {
        public static void Create(string filePath, string folderPath)
        {
            using (Stream stream = File.Create(filePath))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                var version = Encoding.ASCII.GetBytes("BURIKO ARC20");
                writer.Write(version);

                var entries = new List<Entry>();

                // Grab entries
                foreach (var path in Directory.EnumerateFiles(folderPath))
                {
                    var info = new FileInfo(path);

                    if (info.Length == 0)
                        continue;
                    if (info.Name.Length >= 0x60)
                        continue;

                    var entry = new Entry();
                    entry.Path = path;
                    entry.Name = info.Name;
                    entry.Size = Convert.ToUInt32(info.Length);

                    entries.Add(entry);
                }

                writer.Write(entries.Count);

                var encoding = Encoding.GetEncoding("utf-8");

                uint offset = 0;

                // Write entry index
                foreach (var entry in entries)
                {
                    // Write name
                    var name = encoding.GetBytes(entry.Name);
                    writer.Write(name);

                    // Write padding zero bytes
                    int padding = 0x60 - name.Length;
                    if (padding > 0)
                        writer.Write(new byte[padding]);

                    // Write entry information
                    writer.Write(offset);
                    writer.Write(entry.Size);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);
                    writer.Write(0);

                    offset += entry.Size;
                }

                // Write entry data
                foreach (var entry in entries)
                {
                    var data = File.ReadAllBytes(entry.Path);
                    writer.Write(data);
                }

                writer.Flush();
            }
        }

        struct Entry
        {
            public string Path;
            public string Name;
            public uint Size;
        }
    }
}
