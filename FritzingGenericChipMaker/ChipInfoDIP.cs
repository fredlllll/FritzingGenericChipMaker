using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class ChipInfoDIP : ChipInfo2Sided, IChipInfoThroughhole
    {
        public Measurement PCB_HoleInnerDiameter { get; set; } = new Measurement(1);
        public Measurement PCB_RingWidth { get; set; } = new Measurement(0.4);

        public Measurement PCB_PinSpacing { get; set; } = new Measurement(0.1, false);
        public Measurement PCB_PinRowSpacing { get; set; } = new Measurement(7.9375);

        public ChipInfoDIP()
        {
            for(int i = 0; i < 28; i++)
            {
                PinInfo pi = new PinInfo();
                pi.Name = "pin " + i;
                Pins.Add(pi);
            }
        }

        public override double CalculatePCBSketchX()
        {
            return PCB_PinRowSpacing.Millimeters + PCB_HoleInnerDiameter.Millimeters + PCB_RingWidth.Millimeters;
        }

        public override double CalculatePCBSketchY()
        {
            return PCB_PinSpacing.Millimeters * (pinsPerSide.Get() - 1) + PCB_HoleInnerDiameter.Millimeters + PCB_RingWidth.Millimeters;
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
                    retval = PCB_HoleInnerDiameter.Millimeters / 2 + PCB_RingWidth.Millimeters;
                    break;
                case 1://right
                    retval = size - (PCB_HoleInnerDiameter.Millimeters / 2 + PCB_RingWidth.Millimeters);
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
                    retval = (PCB_HoleInnerDiameter.Millimeters / 2 + PCB_RingWidth.Millimeters) + PCB_PinSpacing.Millimeters * i;
                    break;
                case 1://right
                    retval = CalculatePCBSketchY() - (PCB_HoleInnerDiameter.Millimeters / 2 + PCB_RingWidth.Millimeters) - PCB_PinSpacing.Millimeters * i;
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
            circle.CenterX.Value = GetPCBPinX(0);
            circle.CenterY.Value = GetPCBPinY(0);
            circle.Radius.Value = PCB_HoleInnerDiameter.Millimeters / 2 + PCB_RingWidth.Millimeters;
            circle.StrokeColor.Value = Color.White;
            circle.StrokeWidth.Value = PCB_OutlineWidth.Millimeters;
            silkscreen.Add(circle);


            //copperlayers
            var copper = new List<SVGElement>();
            dict[PCBLayer.BothCopper] = copper;

            for(int i = 0; i < Pins.Count; i++)
            {
                circle = new SVGCircle();
                circle.CenterX.Value = GetPCBPinX(i);
                circle.CenterY.Value = GetPCBPinY(i);
                circle.Radius.Value = PCB_HoleInnerDiameter.Millimeters / 2 + PCB_RingWidth.Millimeters / 2;
                circle.StrokeWidth.Value = PCB_RingWidth.Millimeters;
                circle.StrokeColor.Value = copperColor;
                circle.ID.Value = "connector-pad-" + i;
                copper.Add(circle);
            }
            return dict;
        }
    }
}
