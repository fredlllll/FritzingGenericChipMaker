using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public abstract class ChipInfo4Sided : ChipInfo
    {
        public Measurement Schematic_PinSpacing { get; set; } = new Measurement(5);
        public Measurement Schematic_PinLength { get; set; } = new Measurement(5);
        public Measurement Schematic_PinWidth { get; set; } = new Measurement(0.5);
        public Measurement Schematic_TerminalSize { get; set; } = new Measurement(1);
        public Measurement Schematic_OutlineWidth { get; set; } = new Measurement(1);

        public Measurement Breadboard_PinSpacing { get; set; } = new Measurement(5);
        public Measurement Breadboard_PinLength { get; set; } = new Measurement(3);
        public Measurement Breadboard_PinWidth { get; set; } = new Measurement(1);
        public Measurement Breadboard_TerminalSize { get; set; } = new Measurement(1);


        protected CacheableResult<int, int> pinsPerSide = null;

        public ChipInfo4Sided()
        {
            pinsPerSide = new CacheableResult<int, int>(() => { return this.Pins.Count; }, () =>
            {
                int pinsPerSide = Pins.Count / 4;
                if(Pins.Count % 4 != 0)
                {
                    pinsPerSide += 1;
                }
                return pinsPerSide;
            });
        }

        double GetBreadboardPinX(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculateBreadboardSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = 0;
                    break;
                case 1://bottom
                    retval = Breadboard_PinLength.Millimeters + i * Breadboard_PinSpacing.Millimeters;
                    break;
                case 2://right
                    retval = size - Breadboard_PinLength.Millimeters;
                    break;
                case 3://top;
                    retval = size - (Breadboard_PinLength.Millimeters + Breadboard_PinWidth.Millimeters + i * Breadboard_PinSpacing.Millimeters);
                    break;
            }
            return retval;
        }

        double GetBreadboardPinY(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculateBreadboardSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = Breadboard_PinLength.Millimeters + i * Breadboard_PinSpacing.Millimeters;
                    break;
                case 1://bottom
                    retval = size - Breadboard_PinLength.Millimeters;
                    break;
                case 2://right
                    retval = size - (Breadboard_PinLength.Millimeters + Breadboard_PinWidth.Millimeters + i * Breadboard_PinSpacing.Millimeters);
                    break;
                case 3://top;
                    retval = 0;
                    break;
            }
            return retval;
        }

        double GetBreadboardTerminalX(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculateBreadboardSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = 0;
                    break;
                case 1://bottom
                    retval = Breadboard_PinLength.Millimeters + Breadboard_PinWidth.Millimeters / 2 - Breadboard_TerminalSize.Millimeters / 2 + i * Breadboard_TerminalSize.Millimeters;
                    break;
                case 2://right
                    retval = size - Breadboard_TerminalSize.Millimeters;
                    break;
                case 3://top;
                    retval = size - (Breadboard_PinLength.Millimeters + Breadboard_PinWidth.Millimeters / 2 + Breadboard_TerminalSize.Millimeters / 2 + i * Breadboard_TerminalSize.Millimeters);
                    break;
            }
            return retval;
        }

        double GetBreadboardTerminalY(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculateBreadboardSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = Breadboard_PinLength.Millimeters + Breadboard_PinWidth.Millimeters / 2 - Breadboard_TerminalSize.Millimeters / 2 + i * Breadboard_PinSpacing.Millimeters;
                    break;
                case 1://bottom
                    retval = size - Breadboard_PinLength.Millimeters;
                    break;
                case 2://right
                    retval = size - (Breadboard_PinLength.Millimeters + Breadboard_PinWidth.Millimeters / 2 + Breadboard_TerminalSize.Millimeters / 2 + i * Breadboard_PinSpacing.Millimeters);
                    break;
                case 3://top;
                    retval = 0;
                    break;
            }
            return retval;
        }

        double GetBreadboardPinWidth(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                case 2://right
                    retval = Breadboard_PinLength.Millimeters;
                    break;
                case 1://bottom
                case 3://top;
                    retval = Breadboard_PinWidth.Millimeters;
                    break;
            }
            return retval;
        }

        double GetBreadboardPinHeight(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                case 2://right
                    retval = Breadboard_PinWidth.Millimeters;
                    break;
                case 1://bottom
                case 3://top;
                    retval = Breadboard_PinLength.Millimeters;
                    break;
            }
            return retval;
        }

        /*
        * schematic stuff
        * 
        */

        double GetSchematicPinX(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculateSchematicSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = 0;
                    break;
                case 1://bottom
                    retval = Schematic_PinLength.Millimeters + i * Schematic_PinSpacing.Millimeters;
                    break;
                case 2://right
                    retval = size - Schematic_PinLength.Millimeters;
                    break;
                case 3://top;
                    retval = size - (Schematic_PinLength.Millimeters + Schematic_PinWidth.Millimeters + i * Schematic_PinSpacing.Millimeters);
                    break;
            }
            return retval;
        }

        double GetSchematicPinY(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculateSchematicSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = Schematic_PinLength.Millimeters + i * Schematic_PinSpacing.Millimeters;
                    break;
                case 1://bottom
                    retval = size - Schematic_PinLength.Millimeters;
                    break;
                case 2://right
                    retval = size - (Schematic_PinLength.Millimeters + Schematic_PinWidth.Millimeters + i * Schematic_PinSpacing.Millimeters);
                    break;
                case 3://top;
                    retval = 0;
                    break;
            }
            return retval;
        }

        double GetSchematicTerminalX(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculateSchematicSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = 0;
                    break;
                case 1://bottom
                    retval = Schematic_PinLength.Millimeters + Schematic_PinWidth.Millimeters / 2 - Schematic_TerminalSize.Millimeters / 2 + i * Schematic_TerminalSize.Millimeters;
                    break;
                case 2://right
                    retval = size - Schematic_TerminalSize.Millimeters;
                    break;
                case 3://top;
                    retval = size - (Schematic_PinLength.Millimeters + Schematic_PinWidth.Millimeters / 2 + Schematic_TerminalSize.Millimeters / 2 + i * Schematic_TerminalSize.Millimeters);
                    break;
            }
            return retval;
        }

        double GetSchematicTerminalY(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double size = CalculateSchematicSketchX();
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                    retval = Schematic_PinLength.Millimeters + Schematic_PinWidth.Millimeters / 2 - Schematic_TerminalSize.Millimeters / 2 + i * Schematic_PinSpacing.Millimeters;
                    break;
                case 1://bottom
                    retval = size - Schematic_PinLength.Millimeters;
                    break;
                case 2://right
                    retval = size - (Schematic_PinLength.Millimeters + Schematic_PinWidth.Millimeters / 2 + Schematic_TerminalSize.Millimeters / 2 + i * Schematic_PinSpacing.Millimeters);
                    break;
                case 3://top;
                    retval = 0;
                    break;
            }
            return retval;
        }

        double GetSchematicPinWidth(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                case 2://right
                    retval = Schematic_PinLength.Millimeters;
                    break;
                case 1://bottom
                case 3://top;
                    retval = Schematic_PinWidth.Millimeters;
                    break;
            }
            return retval;
        }

        double GetSchematicPinHeight(int index)
        {
            int pps = pinsPerSide.Get();
            int i = index % pps;//index on side
            double retval = 0;
            switch(index / pps)
            {
                case 0://left
                case 2://right
                    retval = Schematic_PinWidth.Millimeters;
                    break;
                case 1://bottom
                case 3://top;
                    retval = Schematic_PinLength.Millimeters;
                    break;
            }
            return retval;
        }

        public override double CalculateBreadboardSketchX()
        {
            return Breadboard_PinLength.Millimeters * 2 + Breadboard_PinSpacing.Millimeters * (pinsPerSide.Get()-1) + Breadboard_PinWidth.Millimeters;
        }

        public override double CalculateBreadboardSketchY()
        {
            return CalculateBreadboardSketchX();
        }

        public override double CalculateIconSketchX()
        {
            return CalculateBreadboardSketchX();
        }

        public override double CalculateIconSketchY()
        {
            return CalculateBreadboardSketchY();
        }

        public override double CalculateSchematicSketchX()
        {
            return Schematic_PinLength.Millimeters * 2 + Schematic_PinSpacing.Millimeters * (pinsPerSide.Get() - 1) + Schematic_PinWidth.Millimeters;
        }

        public override double CalculateSchematicSketchY()
        {
            return CalculateSchematicSketchX();
        }

        public override List<SVGElement> getBreadboardSVGElements()
        {
            var elements = new List<SVGElement>();
            double w = CalculateBreadboardSketchX();
            double h = CalculateBreadboardSketchY();

            var rect = new SVGRect();
            rect.X.Value = rect.Y.Value = Breadboard_PinLength.Millimeters;
            rect.Width.Value = rect.Height.Value = w - 2 * Breadboard_PinLength.Millimeters;
            rect.FillColor.Value = chipBlackColor;
            elements.Add(rect);

            for(int i = 0; i < Pins.Count; i++)
            {
                rect = new SVGRect();
                rect.X.Value = GetBreadboardPinX(i);
                rect.Y.Value = GetBreadboardPinY(i);
                rect.Width.Value = GetBreadboardPinWidth(i);
                rect.Height.Value = GetBreadboardPinHeight(i);
                rect.FillColor.Value = pinGrayColor;
                rect.ID.Value = "connector-pin-" + i;
                elements.Add(rect);
                rect = new SVGRect();
                rect.X.Value = GetBreadboardTerminalX(i);
                rect.Y.Value = GetBreadboardTerminalY(i);
                rect.Height.Value = rect.Width.Value = Breadboard_TerminalSize.Millimeters;
                rect.ID.Value = "connector-terminal-" + i;
                elements.Add(rect);
            }

            return elements;
        }

        public override List<SVGElement> getIconSVGElements()
        {
            return getBreadboardSVGElements();
        }

        public override List<SVGElement> getSchematicSVGElements()
        {
            var elements = new List<SVGElement>();
            double w = CalculateSchematicSketchX();
            double h = CalculateSchematicSketchY();

            var rect = new SVGRect();
            rect.X.Value = rect.Y.Value = Schematic_PinLength.Millimeters;
            rect.Width.Value = rect.Height.Value = w - 2 * Schematic_PinLength.Millimeters;
            rect.StrokeColor.Value = Color.Black;
            rect.StrokeWidth.Value = Schematic_OutlineWidth.Millimeters;
            elements.Add(rect);

            for(int i = 0; i < Pins.Count; i++)
            {
                rect = new SVGRect();
                rect.X.Value = GetSchematicPinX(i);
                rect.Y.Value = GetSchematicPinY(i);
                rect.Width.Value = GetSchematicPinWidth(i);
                rect.Height.Value = GetSchematicPinHeight(i);
                rect.FillColor.Value = Color.Black;
                rect.ID.Value = "connector-pin-" + i;
                elements.Add(rect);
                rect = new SVGRect();
                rect.X.Value = GetSchematicTerminalX(i);
                rect.Y.Value = GetSchematicTerminalY(i);
                rect.Height.Value = rect.Width.Value = Schematic_TerminalSize.Millimeters;
                rect.ID.Value = "connector-terminal-" + i;
                elements.Add(rect);
            }

            return elements;
        }
    }
}
