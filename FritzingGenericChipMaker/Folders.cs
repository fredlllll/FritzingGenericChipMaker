using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public static class Folders
    {
        public static  DirectoryInfo TempFolder { get; }

        static Folders()
        {
            TempFolder = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory,"temp"));
            Directory.CreateDirectory(TempFolder.FullName);
        }
    }
}
