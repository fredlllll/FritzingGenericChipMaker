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
        public Measurement PCB_PinSpacing { get; set; } = new Measurement(0.1, false);
        public Measurement PCB_HoleDiameter { get; set; } = new Measurement(0.9);
        public Measurement PCB_RingWidth { get; set; } = new Measurement(0.5);

        public override double CalculateSketchX_MM()
        {
            return PCB_PinSpacing.Millimeters * (PinCount - 1) + PCB_HoleDiameter.Millimeters + PCB_RingWidth.Millimeters*2;
        }

        public override double CalculateSketchY_MM()
        {
            return PCB_HoleDiameter.Millimeters + PCB_RingWidth.Millimeters * 2;
        }

        public override Dictionary<PCBLayer, List<SVGElement>> getPCBSVGElements()
        {
            Dictionary<PCBLayer, List<SVGElement>> dict = new Dictionary<PCBLayer, List<SVGElement>>();

            double w = CalculateSketchX_MM();
            double h = CalculateSketchY_MM();

            //silkscreen
            List<SVGElement> silkscreen = new List<SVGElement>();
            dict[PCBLayer.Silkscreen] = silkscreen;

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
            dict[PCBLayer.BothCopper] = copper;

            double d = PCB_HoleDiameter.Millimeters + PCB_RingWidth.Millimeters;
            double x = (d + PCB_RingWidth.Millimeters) / 2; //so we get outer diameter we add another ringwidth
            double y = h / 2;
            double strokeWidth = PCB_RingWidth.Millimeters;
            for(int i = 0; i < PinCount; i++, x += PCB_PinSpacing.Millimeters)
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

        public override List<SVGElement> getSchematicSVGElements()
        {
            throw new NotImplementedException();
        }

        public override Dictionary<BreadboardLayer, List<SVGElement>> getBreadboardSVGElements()
        {
            throw new NotImplementedException();
        }
    }
}
