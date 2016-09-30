using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class ChipInfoQFN : ChipInfo4Sided
    {
        public Measurement PCB_PinSideClearance { get; set; } = new Measurement(0.54);
        public Measurement PCB_PinWidth { get; set; } = new Measurement(0.22);
        public Measurement PCB_PinSpacing { get; set; } = new Measurement(0.45);
        public Measurement PCB_PinLength { get; set; } = new Measurement(0.4);
        public Measurement PCB_OuterPinLength { get; set; } = new Measurement(0.28);//outer pins are often shorter
        public Measurement PCB_OutlineWidth { get; set; } = new Measurement(0.05);
        public Measurement PCB_ThermalPadSize { get; set; } = new Measurement(2.4);

        CacheableResult<int, double> QFNSize;

        public ChipInfoQFN()
        {
            for(int i = 0; i < 24; i++)
            {
                var pin = new PinInfo();
                pin.Name = "pin " + i;
                Pins.Add(pin);
            }

            QFNSize = new CacheableResult<int, double>(() => { return this.Pins.Count; },
            () =>
            {
                int pinsPerSide = base.pinsPerSide.Get();
                return this.PCB_PinWidth.Millimeters + (pinsPerSide - 1) * this.PCB_PinSpacing.Millimeters + 2 * this.PCB_PinSideClearance.Millimeters;
            }
            );


        }

        /*double GetPCBPinLength(int index)
        {
            int pinsPerSide = base.pinsPerSide.Get();
            int i = index % pinsPerSide;//index on side
            double retval = 0;
            if(i == 0 || i == pinsPerSide - 1)//side pins
            {
                retval = PCB_OuterPinLength.Millimeters;
            }
            else
            {
                retval = PCB_PinLength.Millimeters;
            }
            return retval;
        }*/

        double GetPCBPinX(int index)
        {
            int pinsPerSide = base.pinsPerSide.Get();
            int i = index % pinsPerSide;//index on side
            double width = QFNSize.Get();
            double retval = 0;
            switch(index / pinsPerSide)
            {
                case 0://left
                    retval = 0;
                    break;
                case 1://bottom
                    retval = PCB_PinSideClearance.Millimeters + i * PCB_PinSpacing.Millimeters;
                    break;
                case 2://right
                    if(i == 0 || i == pinsPerSide - 1)//side pins
                    {
                        retval = width - PCB_OuterPinLength.Millimeters;
                    }
                    else
                    {
                        retval = width - PCB_PinLength.Millimeters;
                    }
                    break;
                case 3://top;
                    retval = width - (PCB_PinSideClearance.Millimeters + PCB_PinWidth.Millimeters + i * PCB_PinSpacing.Millimeters);
                    break;
            }
            return retval;
        }

        double GetPCBPinY(int index)
        {
            int pinsPerSide = base.pinsPerSide.Get();
            int i = index % pinsPerSide;//index on side
            double width = QFNSize.Get();
            double retval = 0;
            switch(index / pinsPerSide)
            {
                case 0://left
                    retval = PCB_PinSideClearance.Millimeters + i * PCB_PinSpacing.Millimeters;
                    break;
                case 1://bottom
                    if(i == 0 || i == pinsPerSide - 1)//side pins
                    {
                        retval = width - PCB_OuterPinLength.Millimeters;
                    }
                    else
                    {
                        retval = width - PCB_PinLength.Millimeters;
                    }
                    break;
                case 2://right
                    retval = width - (PCB_PinSideClearance.Millimeters + PCB_PinWidth.Millimeters + i * PCB_PinSpacing.Millimeters);
                    break;
                case 3://top;
                    retval = 0;
                    break;
            }
            return retval;
        }

        double GetPCBPinWidth(int index)
        {
            int pinsPerSide = base.pinsPerSide.Get();
            int i = index % pinsPerSide;//index on side
            double retval = 0;
            switch(index / pinsPerSide)
            {
                case 0://left
                case 2://right
                    if(i == 0 || i == pinsPerSide - 1)//side pins
                    {
                        retval = PCB_OuterPinLength.Millimeters;
                    }
                    else
                    {
                        retval = PCB_PinLength.Millimeters;
                    }
                    break;
                case 1://bottom
                case 3://top;
                    retval = PCB_PinWidth.Millimeters;
                    break;
            }
            return retval;
        }

        double GetPCBPinHeight(int index)
        {
            int pinsPerSide = base.pinsPerSide.Get();
            int i = index % pinsPerSide;//index on side
            double retval = 0;
            switch(index / pinsPerSide)
            {
                case 0://left
                case 2://right
                    retval = PCB_PinWidth.Millimeters;
                    break;
                case 1://bottom
                case 3://top;
                    if(i == 0 || i == pinsPerSide - 1)//side pins
                    {
                        retval = PCB_OuterPinLength.Millimeters;
                    }
                    else
                    {
                        retval = PCB_PinLength.Millimeters;
                    }
                    break;
            }
            return retval;
        }

        public override double CalculatePCBSketchX()
        {
            return QFNSize.Get();
        }

        public override double CalculatePCBSketchY()
        {
            return CalculatePCBSketchX();//always quadratic
        }

        public override Dictionary<PCBLayer, List<XMLElement>> getPCBSVGElements()
        {
            Dictionary<PCBLayer, List<XMLElement>> dict = new Dictionary<PCBLayer, List<XMLElement>>();

            double w = CalculatePCBSketchX();
            double h = CalculatePCBSketchY();

            //silkscreen
            List<XMLElement> silkscreen = new List<XMLElement>();
            dict[PCBLayer.Silkscreen] = silkscreen;

            SVGLine line = new SVGLine(); //bottom;
            double halfLineWidth = PCB_OutlineWidth.Millimeters / 2;
            line.X1.Value = halfLineWidth;
            line.Y1.Value = h - halfLineWidth;
            line.X2.Value = w - halfLineWidth;
            line.Y2.Value = line.Y1.Value;
            line.StrokeColor.Value = Color.White;
            line.StrokeWidth.Value = PCB_OutlineWidth.Millimeters;
            silkscreen.Add(line);
            line = new SVGLine(); //top
            line.X1.Value = halfLineWidth;
            line.Y1.Value = halfLineWidth;
            line.X2.Value = w - halfLineWidth;
            line.Y2.Value = halfLineWidth;
            line.StrokeColor.Value = Color.White;
            line.StrokeWidth.Value = PCB_OutlineWidth.Millimeters;
            silkscreen.Add(line);
            line = new SVGLine(); //left
            line.X1.Value = halfLineWidth;
            line.Y1.Value = halfLineWidth;
            line.X2.Value = halfLineWidth;
            line.Y2.Value = h - halfLineWidth;
            line.StrokeColor.Value = Color.White;
            line.StrokeWidth.Value = PCB_OutlineWidth.Millimeters;
            silkscreen.Add(line);
            line = new SVGLine(); //right
            line.X1.Value = w - halfLineWidth;
            line.Y1.Value = halfLineWidth;
            line.X2.Value = line.X1.Value;
            line.Y2.Value = h - halfLineWidth;
            line.StrokeColor.Value = Color.White;
            line.StrokeWidth.Value = PCB_OutlineWidth.Millimeters;
            silkscreen.Add(line);

            //copperlayers
            List<XMLElement> copper = new List<XMLElement>();
            dict[PCBLayer.Copper1] = copper;

            SVGRect rect = new SVGRect();
            rect.Width.Value = rect.Height.Value = PCB_ThermalPadSize.Millimeters;
            rect.X.Value = rect.Y.Value = w / 2 - PCB_ThermalPadSize.Millimeters / 2;
            rect.FillColor.Value = copperColor;
            copper.Add(rect);

            for(int i = 0; i < Pins.Count; i++)
            {
                rect = new SVGRect();
                rect.X.Value = GetPCBPinX(i);
                rect.Y.Value = GetPCBPinY(i);
                rect.Width.Value = GetPCBPinWidth(i);
                rect.Height.Value = GetPCBPinHeight(i);
                rect.FillColor.Value = copperColor;
                rect.ID.Value = "connector-pad-" + i;
                copper.Add(rect);
            }

            return dict;
        }
    }
}
