using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class ChipInfoSIP : ChipInfo
    {
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Measurement PinSpacing { get; set; } = new Measurement(0.1, true);
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Measurement HoleDiameter { get; set; } = new Measurement(0.9);
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public Measurement RingWidth { get; set; } = new Measurement(0.5);

        public ChipInfoSIP()
        {
            PinSpacing.PropertyChanged += RaisePropertyChangedEvent;
            HoleDiameter.PropertyChanged += RaisePropertyChangedEvent;
        }

        public override double CalculateSketchX_MM()
        {
            return PinSpacing.Millimeters * (PinCount - 1) + HoleDiameter.Millimeters + RingWidth.Millimeters*2;
        }

        public override double CalculateSketchY_MM()
        {
            return HoleDiameter.Millimeters + RingWidth.Millimeters * 2;
        }

        public override Dictionary<Layer, List<SVGElement>> getSVGElements()
        {
            Dictionary<Layer, List<SVGElement>> dict = new Dictionary<Layer, List<SVGElement>>();

            double w = CalculateSketchX_MM();
            double h = CalculateSketchY_MM();

            //silkscreen
            List<SVGElement> silkscreen = new List<SVGElement>();
            dict[Layer.Silkscreen] = silkscreen;

            SVGLine line = new SVGLine(); //top;
            double hw = line.StrokeWidth / 2;
            line.X1 = hw;
            line.Y1 = h - hw;
            line.X2 = w - hw;
            line.Y2 = line.Y1;
            silkscreen.Add(line);
            line = new SVGLine(); //bottom
            line.X1 = hw;
            line.Y1 = hw;
            line.X2 = w - hw;
            line.Y2 = hw;
            silkscreen.Add(line);
            line = new SVGLine(); //left
            line.X1 = hw;
            line.Y1 = hw;
            line.X2 = hw;
            line.Y2 = h - hw;
            silkscreen.Add(line);
            line = new SVGLine(); //right
            line.X1 = w - hw;
            line.Y1 = hw;
            line.X2 = line.X1;
            line.Y2 = h - hw;
            silkscreen.Add(line);

            //copperlayers
            List<SVGElement> copper = new List<SVGElement>();
            dict[Layer.BothCopper] = copper;

            double d = HoleDiameter.Millimeters + RingWidth.Millimeters;
            double x = (d + RingWidth.Millimeters) / 2; //so we get outer diameter we add another ringwidth
            double y = h / 2;
            double strokeWidth = RingWidth.Millimeters;
            for(int i = 0; i < PinCount; i++, x += PinSpacing.Millimeters)
            {
                SVGCircle circle = new SVGCircle();
                circle.CenterX = x;
                circle.CenterY = y;
                circle.Diameter = d;
                circle.StrokeWidth = strokeWidth;
                circle.ID = "connector-pad-" + i;
                copper.Add(circle);
            }

            return dict;
        }
    }
}
