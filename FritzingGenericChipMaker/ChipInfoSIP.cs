using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class ChipInfoSIP : ChipInfo
    {
        public Measurement PCB_OutlineWidth { get; set; } = new Measurement(0.05);
        public Measurement PCB_PinSpacing { get; set; } = new Measurement(0.1, false);
        public Measurement PCB_HoleDiameter { get; set; } = new Measurement(0.9);
        public Measurement PCB_RingWidth { get; set; } = new Measurement(0.5);

        public Measurement Schematic_PinSpacing { get; set; } = new Measurement(5);
        public Measurement Schematic_PinLength { get; set; } = new Measurement(5);
        public Measurement Schematic_PinWidth { get; set; } = new Measurement(0.5);
        public Measurement Schematic_TerminalWidth { get; set; } = new Measurement(1);
        public Measurement Schematic_TerminalHeight { get; set; } = new Measurement(1);
        public Measurement Schematic_OutlineWidth { get; set; } = new Measurement(0.5);
        public Measurement Schematic_TextIndentation { get; set; } = new Measurement(1);
        public Measurement Schematic_FontSize { get; set; } = new Measurement(3);

        public ChipInfoSIP()
        {
            for(int i = 0; i < 5; i++)
            {
                Pins.Add(new PinInfo());
            }
            ChipName = "SIP Chip";
        }

        public override double CalculatePCBSketchX()
        {
            return PCB_PinSpacing.Millimeters * (Pins.Count - 1) + PCB_HoleDiameter.Millimeters + PCB_RingWidth.Millimeters * 2;
        }

        public override double CalculateSchematicSketchX()
        {
            //TODO: measure longest text?
            double len = 0;
            foreach(var pin in Pins)
            {
                len = Math.Max(len, SVGText.MeasureText(pin.Name, Schematic_FontSize.Millimeters).Width);
            }
            return len + Schematic_TextIndentation.Millimeters * 2 + Schematic_PinLength.Millimeters;
        }

        public override double CalculateBreadboardSketchX()
        {
            throw new NotImplementedException();
        }

        public override double CalculateIconSketchX()
        {
            throw new NotImplementedException();
        }

        public override double CalculatePCBSketchY()
        {
            return PCB_HoleDiameter.Millimeters + PCB_RingWidth.Millimeters * 2;
        }

        public override double CalculateSchematicSketchY()
        {
            return Pins.Count * Schematic_PinSpacing.Millimeters;
        }

        public override double CalculateBreadboardSketchY()
        {
            throw new NotImplementedException();
        }

        public override double CalculateIconSketchY()
        {
            throw new NotImplementedException();
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
            dict[PCBLayer.BothCopper] = copper;

            double d = PCB_HoleDiameter.Millimeters + PCB_RingWidth.Millimeters;
            double x = (d + PCB_RingWidth.Millimeters) / 2; //so we get outer diameter we add another ringwidth
            double y = h / 2;
            double strokeWidth = PCB_RingWidth.Millimeters;
            for(int i = 0; i < Pins.Count; i++, x += PCB_PinSpacing.Millimeters)
            {
                SVGCircle circle = new SVGCircle();
                circle.CenterX.Value = x;
                circle.CenterY.Value = y;
                circle.Radius.Value = d / 2;
                circle.StrokeWidth.Value = strokeWidth;
                circle.StrokeColor.Value = copperColor;
                circle.ID.Value = "connector-pad-" + i;
                copper.Add(circle);
            }

            return dict;
        }

        public override List<XMLElement> getSchematicSVGElements()
        {
            List<XMLElement> elements = new List<XMLElement>();
            double w = CalculateSchematicSketchX();
            double h = CalculateSchematicSketchY();

            var rect = new SVGRect();
            rect.StrokeWidth.Value = Schematic_OutlineWidth.Millimeters;
            rect.X.Value = rect.StrokeWidth.Value / 2;
            rect.Y.Value = rect.StrokeWidth.Value / 2;
            rect.StrokeColor.Value = Color.Black;
            rect.Width.Value = w - Schematic_PinLength.Millimeters - rect.StrokeWidth.Value;
            rect.Height.Value = h - rect.StrokeWidth.Value;
            elements.Add(rect);

            double y = Schematic_PinSpacing.Millimeters / 2;
            for(int i = 0; i < Pins.Count; i++, y += Schematic_PinSpacing.Millimeters)
            {
                var line = new SVGLine();
                line.X1.Value = w - Schematic_PinLength.Millimeters;
                line.X2.Value = w;
                line.Y1.Value = line.Y2.Value = y;
                line.StrokeColor.Value = Color.Black;
                line.StrokeWidth.Value = Schematic_PinWidth.Millimeters;
                line.ID.Value = "connector-pin-" + i;
                elements.Add(line);
                rect = new SVGRect();
                rect.Height.Value = Schematic_TerminalHeight.Millimeters;
                rect.Width.Value = Schematic_TerminalWidth.Millimeters;
                rect.X.Value = w - rect.Width.Value / 2;
                rect.Y.Value = y - rect.Height.Value / 2;
                rect.ID.Value = "connector-terminal-" + i;
                elements.Add(rect);
                var text = new SVGText();
                text.X.Value = w - Schematic_PinLength.Millimeters - Schematic_TextIndentation.Millimeters;
                text.Y.Value = y + Schematic_FontSize.Millimeters / 2;
                text.FillColor.Value = Color.Black;
                text.FontSize.Value = Schematic_FontSize.Millimeters;
                text.TextAnchor.Value = TextAnchor.end;
                text.Value = Pins[i].Name;
                elements.Add(text);
            }

            var label = new SVGText();
            var transform = new XMLAttribute<string>("transform", "matrix(0, -1.0000001, 0.99999993, 0, 0, 0)");
            label.Attributes.Add(transform);
            label.X.Value = -h / 2;
            label.Y.Value = Schematic_FontSize.Millimeters;
            label.TextAnchor.Value = TextAnchor.middle;
            label.Value = ChipName;
            elements.Add(label);

            return elements;
        }

        public override List<XMLElement> getBreadboardSVGElements()
        {
            throw new NotImplementedException();
        }

        public override List<XMLElement> getIconSVGElements()
        {
            throw new NotImplementedException();
        }
    }
}
