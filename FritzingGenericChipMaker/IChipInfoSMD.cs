using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public interface IChipInfoSMD
    {
        Measurement PCB_PinSideClearance { get; set; }// = new Measurement(0.54);
        Measurement PCB_PinWidth { get; set; }// = new Measurement(0.22);
        Measurement PCB_PinSpacing { get; set; }// = new Measurement(0.45);
        Measurement PCB_PinLength { get; set; }// = new Measurement(0.4);
    }
}
