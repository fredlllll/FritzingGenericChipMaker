﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FritzingGenericChipMaker
{
    public partial class Form1 : Form
    {
        Dictionary<string, ChipInfo> chips = new Dictionary<string, ChipInfo>();

        public Form1()
        {
            chips["SIP"] = new ChipInfoSIP();
            chips["DIP"] = new ChipInfoDIP();
            chips["SOIC"] = new ChipInfoSOIC();
            chips["QFN"] = new ChipInfoQFN();
            chips["SMD PCB 3 Sides"] = new ChipInfoSMDPCB3Sided();
            chips["SMD PCB 2 Sides"] = new ChipInfoSMDPCB2Sided();

            InitializeComponent();

            foreach(var kv in chips)
            {
                cmbChipType.Items.Add(kv.Key);
            }
        }

        private void cmbChipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = chips[(string)cmbChipType.SelectedItem];
            }
            catch
            {
                propertyGrid1.SelectedObject = null;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(propertyGrid1.SelectedObject != null && sfd.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(sfd.FileName);
                var filename = Path.GetFileNameWithoutExtension(file.Name);
                FileInfo icon = new FileInfo(Path.Combine(Path.GetDirectoryName(file.FullName), filename + "_icon.svg"));
                FileInfo breadboard = new FileInfo(Path.Combine(Path.GetDirectoryName(file.FullName), filename + "_breadboard.svg"));
                FileInfo schematic = new FileInfo(Path.Combine(Path.GetDirectoryName(file.FullName), filename + "_schematic.svg"));
                FileInfo pcb = new FileInfo(Path.Combine(Path.GetDirectoryName(file.FullName), filename + "_pcb.svg"));

                ChipInfo currentChip = (ChipInfo)propertyGrid1.SelectedObject;

                using(FileStream fs = new FileStream(file.FullName, FileMode.Create))
                using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(currentChip.GetFZPXML(file.FullName));
                }
                using(FileStream fs = new FileStream(pcb.FullName, FileMode.Create))
                using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(currentChip.GetPCBSVG());
                }
                using(FileStream fs = new FileStream(schematic.FullName, FileMode.Create))
                using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(currentChip.GetSchematicSVG());
                }
                using(FileStream fs = new FileStream(breadboard.FullName, FileMode.Create))
                using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(currentChip.GetBreadboardSVG());
                }
                using(FileStream fs = new FileStream(icon.FullName, FileMode.Create))
                using(StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.Write(currentChip.GetIconSVG());
                }
            }
        }
    }
}
