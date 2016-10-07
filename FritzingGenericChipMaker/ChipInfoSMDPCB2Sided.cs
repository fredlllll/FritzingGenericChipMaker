using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    class ChipInfoSMDPCB2Sided : ChipInfo2Sided
    {
        public Measurement PCB_BoardWidth { get; set; } = new Measurement(19.7);
        public Measurement PCB_BoardHeight { get; set; } = new Measurement(16);

        public Measurement PCB_PadSideClearance { get; set; } = new Measurement(0.4);
        public Measurement PCB_PadSpacing { get; set; } = new Measurement(2);
        public Measurement PCB_PadWidth { get; set; } = new Measurement(1.2);
        public Measurement PCB_PadDepth { get; set; } = new Measurement(2);
        public Measurement PCB_PadOvershoot { get; set; } = new Measurement(0.25);

        public ChipInfoSMDPCB2Sided()
        {
            for(int i = 0; i < 16; i++)
            {
                PinInfo pi = new PinInfo();
                pi.Name = "pin " + i;
                Pins.Add(pi);
            }
        }

        public override double CalculatePCBSketchX()
        {
            return PCB_BoardWidth.Millimeters + 2 * PCB_PadOvershoot.Millimeters;
        }

        public override double CalculatePCBSketchY()
        {
            return PCB_BoardHeight.Millimeters;
        }

        double GetPCBPinX(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculatePCBSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = 0;
                    break;
                case 1://right
                    retval = size - PCB_PadOvershoot.Millimeters - PCB_PadDepth.Millimeters;
                    break;
            }
            return retval;
        }

        double GetPCBPinY(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = PCB_PadSideClearance.Millimeters + PCB_PadSpacing.Millimeters * i;
                    break;
                case 1://right
                    retval = CalculatePCBSketchY() - (PCB_PadSideClearance.Millimeters + PCB_PadWidth.Millimeters) - PCB_PadSpacing.Millimeters * i;
                    break;
            }
            return retval;
        }

        public override Dictionary<PCBLayer, List<XMLElement>> getPCBSVGElements()
        {
            Dictionary<PCBLayer, List<XMLElement>> dict = new Dictionary<PCBLayer, List<XMLElement>>();

            double w = CalculatePCBSketchX();
            double h = CalculatePCBSketchY();

            //silkscreen
            List<XMLElement> silkscreen = new List<XMLElement>();
            dict[PCBLayer.Silkscreen] = silkscreen;

            silkscreen.Add(GetPCBChipOutline());

            //copperlayers
            List<XMLElement> copper = new List<XMLElement>();
            dict[PCBLayer.BothCopper] = copper;

            for(int i = 0; i < Pins.Count; i++)
            {
                SVGRect rect = new SVGRect();
                rect.X.Value = GetPCBPinX(i);
                rect.Y.Value = GetPCBPinY(i);
                rect.Width.Value = PCB_PadDepth.Millimeters + PCB_PadOvershoot.Millimeters;
                rect.Height.Value = PCB_PadWidth.Millimeters;
                rect.FillColor.Value = copperColor;
                rect.ID.Value = "connector-pad-" + i;
                copper.Add(rect);
            }
            return dict;
        }
    }
}
