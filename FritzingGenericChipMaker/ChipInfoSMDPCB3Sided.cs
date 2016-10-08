using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    class ChipInfoSMDPCB3Sided : ChipInfo2Sided
    {
        public Measurement PCB_BoardWidth { get; set; } = new Measurement(12.7);
        public Measurement PCB_BoardHeight { get; set; } = new Measurement(27);

        public Measurement PCB_PadTopClearance { get; set; } = new Measurement(7);
        public Measurement PCB_PadBottomClearance { get; set; } = new Measurement(1);
        public Measurement PCB_PadSideClearance { get; set; } = new Measurement(0.6);
        public Measurement PCB_PadSpacingSide { get; set; } = new Measurement(1.5);
        public Measurement PCB_PadSpacingBottom { get; set; } = new Measurement(1.5);
        public Measurement PCB_PadWidth { get; set; } = new Measurement(1);
        public Measurement PCB_PadDepth { get; set; } = new Measurement(0.5);
        public Measurement PCB_PadOvershoot { get; set; } = new Measurement(0.8);
        public int PCB_BottomPadCount { get; set; } = 8;

        public ChipInfoSMDPCB3Sided()
        {
            for(int i = 0; i < 34; i++)
            {
                var pin = new PinInfo();
                pin.Name = "Pin " + i;
                Pins.Add(pin);
            }
        }

        public override double CalculatePCBSketchX()
        {
            return PCB_BoardWidth.Millimeters + 2 * PCB_PadOvershoot.Millimeters;
        }

        public override double CalculatePCBSketchY()
        {
            return PCB_BoardHeight.Millimeters + PCB_PadOvershoot.Millimeters;
        }

        double GetPCBPadX(int index)
        {
            double size = CalculatePCBSketchX();
            int pinCountSide = (Pins.Count - PCB_BottomPadCount) / 2;
            double retval = 0;
            if(index >= pinCountSide && index < pinCountSide + PCB_BottomPadCount)//bottom
            {
                index -= pinCountSide;
                retval = PCB_PadOvershoot.Millimeters + PCB_PadSideClearance.Millimeters + index * PCB_PadSpacingBottom.Millimeters;
            }
            else if(index >= pinCountSide + PCB_BottomPadCount) //right
            {
                retval = size - PCB_PadOvershoot.Millimeters - PCB_PadDepth.Millimeters;
            }
            else//left
            {
                retval = 0;
            }
            return retval;
        }

        double GetPCBPadY(int index)
        {
            double size = CalculatePCBSketchY();
            int pinCountSide = (Pins.Count - PCB_BottomPadCount) / 2;
            double retval = 0;
            if(index >= pinCountSide && index < pinCountSide + PCB_BottomPadCount)//bottom
            {
                retval = size - PCB_PadOvershoot.Millimeters - PCB_PadDepth.Millimeters;
            }
            else if(index >= pinCountSide + PCB_BottomPadCount) //right
            {
                index -= pinCountSide + PCB_BottomPadCount;
                retval = size - PCB_PadOvershoot.Millimeters - PCB_PadBottomClearance.Millimeters - PCB_PadWidth.Millimeters - (index * PCB_PadSpacingSide.Millimeters);
            }
            else//left
            {
                retval = PCB_PadTopClearance.Millimeters + index * PCB_PadSpacingSide.Millimeters;
            }
            return retval;
        }

        double GetPCBPadWidth(int index)
        {
            int pinCountSide = (Pins.Count - PCB_BottomPadCount) / 2;
            double retval = 0;
            if(index >= pinCountSide && index < pinCountSide + PCB_BottomPadCount)//bottom
            {
                retval = PCB_PadWidth.Millimeters;
            }
            else//left or right
            {
                retval = PCB_PadDepth.Millimeters + PCB_PadOvershoot.Millimeters;
            }
            return retval;
        }

        double GetPCBPadHeight(int index)
        {
            int pinCountSide = (Pins.Count - PCB_BottomPadCount) / 2;
            double retval = 0;
            if(index >= pinCountSide && index < pinCountSide + PCB_BottomPadCount)//bottom
            {
                retval = PCB_PadDepth.Millimeters + PCB_PadOvershoot.Millimeters;
            }
            else//left or right
            {
                retval = PCB_PadWidth.Millimeters;
            }
            return retval;
        }

        public override Dictionary<PCBLayer, List<SVGElement>> getPCBSVGElements()
        {
            Dictionary<PCBLayer, List<SVGElement>> dict = new Dictionary<PCBLayer, List<SVGElement>>();

            double w = CalculatePCBSketchX();
            double h = CalculatePCBSketchY();

            //silkscreen
            List<SVGElement> silkscreen = new List<SVGElement>();
            dict[PCBLayer.Silkscreen] = silkscreen;

            silkscreen.Add(GetPCBChipOutline());

            //copperlayers
            List<SVGElement> copper = new List<SVGElement>();
            dict[PCBLayer.BothCopper] = copper;

            for(int i = 0; i < Pins.Count; i++)
            {
                SVGRect rect = new SVGRect();
                rect.X.Value = GetPCBPadX(i);
                rect.Y.Value = GetPCBPadY(i);
                rect.Width.Value = GetPCBPadWidth(i);
                rect.Height.Value = GetPCBPadHeight(i);
                rect.FillColor.Value = copperColor;
                rect.ID.Value = "connector-pad-" + i;
                copper.Add(rect);
            }
            return dict;
        }
    }
}
