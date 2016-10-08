using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            cmbChipType.SelectedIndex = 0;
            //yes this is the sole reason i included System.Reactive into this project... :D .... :) .... :| ... okay i get rid of it
            /*Observable.FromEventPattern<EventHandler, ComboBox, EventArgs>(
                (ev) => cmbChipType.SelectedIndexChanged += ev,
                (ev) => cmbChipType.SelectedIndexChanged -= ev).Subscribe((patt) =>
                {
                    button1.Enabled = patt.Sender.SelectedItem != null;
                });*/
        }

        private void cmbChipType_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = cmbChipType.SelectedItem != null;
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
                FileInfo fzpz = new FileInfo(sfd.FileName);
                ChipInfo currentChip = (ChipInfo)propertyGrid1.SelectedObject;
                FritzingHelper.SaveChip(currentChip, fzpz);
            }
        }
    }
}
