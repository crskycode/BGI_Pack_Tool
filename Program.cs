using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGI_Pack_Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  Create archive : BGI_Pack_Tool <folder> ...");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }

            foreach (var path in args)
            {
                if (!Util.PathIsFolder(path))
                    continue;

                try
                {
                    var filePath = Path.ChangeExtension(path, ".arc");
                    var fileName = Path.GetFileName(filePath);

                    Console.WriteLine($"Creating archive {fileName}");

                    Archive.Create(filePath, path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
