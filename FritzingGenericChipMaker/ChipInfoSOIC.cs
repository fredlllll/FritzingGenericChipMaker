using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class ChipInfoSOIC : ChipInfo2Sided, IChipInfoSMD
    {
        public Measurement PCB_PinSpacing { get; set; } = new Measurement(1.27);
        public Measurement PCB_PinRowSpacing { get; set; } = new Measurement(3.9);
        public Measurement PCB_PinSideClearance { get; set; } = new Measurement(0.5);
        public Measurement PCB_PinWidth { get; set; } = new Measurement(0.43);
        public Measurement PCB_PinLength { get; set; } = new Measurement(1.05);

        public ChipInfoSOIC()
        {
            for(int i = 0; i < 14; i++)
            {
                PinInfo pi = new PinInfo();
                pi.Name = "pin " + i;
                Pins.Add(pi);
            }
        }

        public override double CalculatePCBSketchX()
        {
            return PCB_PinRowSpacing.Millimeters + PCB_PinLength.Millimeters;
        }

        public override double CalculatePCBSketchY()
        {
            return PCB_PinSideClearance.Millimeters * 2 + PCB_PinSpacing.Millimeters * (pinsPerSide.Get() - 1) + PCB_PinWidth.Millimeters;
        }

        double GetPCBPinX(int index)
        {
            int pps = pinsPerSide.Get();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = 0;
                    break;
                case 1://right
                    retval = CalculatePCBSketchX() - PCB_PinLength.Millimeters;
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
                case 0://left;
                    retval = PCB_PinSideClearance.Millimeters + PCB_PinSpacing.Millimeters * i;
                    break;
                case 1://right
                    retval = CalculatePCBSketchY() - PCB_PinSideClearance.Millimeters - PCB_PinWidth.Millimeters - PCB_PinSpacing.Millimeters * i;
                    break;
            }
            return retval;
        }

        public override Dictionary<PCBLayer, List<SVGElement>> getPCBSVGElements()
        {
            var dict = new Dictionary<PCBLayer, List<SVGElement>>();

            double w = CalculatePCBSketchX();
            double h = CalculatePCBSketchY();

            //silkscreen
            var silkscreen = new List<SVGElement>();
            dict[PCBLayer.Silkscreen] = silkscreen;

            silkscreen.Add(GetPCBChipOutline());

            SVGCircle circle = new SVGCircle();
            circle.CenterX.Value = GetPCBPinX(0) + PCB_PinLength.Millimeters + 1;
            circle.CenterY.Value = GetPCBPinY(0) + PCB_PinWidth.Millimeters / 2;
            circle.Radius.Value = 1;
            circle.FillColor.Value = Color.White;
            silkscreen.Add(circle);


            //copperlayers
            var copper = new List<SVGElement>();
            dict[PCBLayer.BothCopper] = copper;

            for(int i = 0; i < Pins.Count; i++)
            {
                SVGRect rect = new SVGRect();
                rect.X.Value = GetPCBPinX(i);
                rect.Y.Value = GetPCBPinY(i);
                rect.Width.Value = PCB_PinLength.Millimeters;
                rect.Height.Value = PCB_PinWidth.Millimeters;
                rect.FillColor.Value = copperColor;
                rect.ID.Value = "connector-pad-" + i;
                copper.Add(rect);
            }
            return dict;
        }
    }
}
