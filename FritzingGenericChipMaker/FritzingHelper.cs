using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public static class FritzingHelper
    {
        public static void SaveChip(ChipInfo chipInfo, FileInfo fzpz)
        {
            var filename = Path.GetFileNameWithoutExtension(fzpz.Name);

            FileInfo fzp = new FileInfo(Path.Combine(Folders.TempFolder.FullName, filename + ".fzp"));
            FileInfo icon = new FileInfo(Path.Combine(Folders.TempFolder.FullName, filename + "_icon.svg"));
            FileInfo breadboard = new FileInfo(Path.Combine(Folders.TempFolder.FullName, filename + "_breadboard.svg"));
            FileInfo schematic = new FileInfo(Path.Combine(Folders.TempFolder.FullName, filename + "_schematic.svg"));
            FileInfo pcb = new FileInfo(Path.Combine(Folders.TempFolder.FullName, filename + "_pcb.svg"));

            using(FileStream fs = new FileStream(fzp.FullName, FileMode.Create))
            using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(chipInfo.GetFZPXML(fzp.FullName));
            }
            using(FileStream fs = new FileStream(pcb.FullName, FileMode.Create))
            using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(chipInfo.GetPCBSVG());
            }
            using(FileStream fs = new FileStream(schematic.FullName, FileMode.Create))
            using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(chipInfo.GetSchematicSVG());
            }
            using(FileStream fs = new FileStream(breadboard.FullName, FileMode.Create))
            using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(chipInfo.GetBreadboardSVG());
            }
            using(FileStream fs = new FileStream(icon.FullName, FileMode.Create))
            using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(chipInfo.GetIconSVG());
            }

            MakeFZPZ(fzpz, fzp, icon, breadboard, schematic, pcb);
        }

        public static void MakeFZPZ(FileInfo destination, FileInfo fzp, FileInfo icon, FileInfo breadboard, FileInfo schematic, FileInfo pcb)
        {
            var fs = new FileStream(destination.FullName, FileMode.Create, FileAccess.Write, FileShare.Read);
            var zip = new ZipArchive(fs, ZipArchiveMode.Create);

            CreateEntryFromFile(zip, "part.", fzp);
            CreateEntryFromFile(zip, "svg.icon.", icon);
            CreateEntryFromFile(zip, "svg.breadboard.", breadboard);
            CreateEntryFromFile(zip, "svg.schematic.", schematic);
            CreateEntryFromFile(zip, "svg.pcb.", pcb);
        }

        static ZipArchiveEntry CreateEntryFromFile(ZipArchive archive, string prefix, FileInfo file)
        {
            var entry = archive.CreateEntry(prefix + file.Name);
            using(var zipStream = entry.Open())
            using(var srcStream = file.OpenRead())
            {
                srcStream.CopyTo(zipStream);
            }
            return entry;
        }
    }
}
