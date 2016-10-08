using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class ChipInfoSIP : ChipInfo, IChipInfoThroughhole
    {
        public Measurement PCB_HoleInnerDiameter { get; set; } = new Measurement(1);
        public Measurement PCB_RingWidth { get; set; } = new Measurement(0.4);

        public Measurement PCB_PinSpacing { get; set; } = new Measurement(0.1, false);

        public Measurement Schematic_PinSpacing { get; set; } = new Measurement(5);
        public Measurement Schematic_PinLength { get; set; } = new Measurement(5);
        public Measurement Schematic_PinWidth { get; set; } = new Measurement(0.5);
        public Measurement Schematic_TerminalWidth { get; set; } = new Measurement(1);
        public Measurement Schematic_TerminalHeight { get; set; } = new Measurement(1);
        public Measurement Schematic_OutlineWidth { get; set; } = new Measurement(0.5);
        public Measurement Schematic_TextIndentation { get; set; } = new Measurement(1);
        public Measurement Schematic_FontSize { get; set; } = new Measurement(3);

        public Measurement Breadboard_PinSpacing { get; set; } = new Measurement(5);
        public Measurement Breadboard_PinLength { get; set; } = new Measurement(3);
        public Measurement Breadboard_PinWidth { get; set; } = new Measurement(1);
        public Measurement Breadboard_ChipHeight { get; set; } = new Measurement(5);
        public Measurement Breadboard_TerminalWidth { get; set; } = new Measurement(1);
        public Measurement Breadboard_TerminalHeight { get; set; } = new Measurement(1);

        public ChipInfoSIP()
        {
            for(int i = 0; i < 5; i++)
            {
                Pins.Add(new PinInfo() { Name = "pin " + i });
            }
            ChipName = "SIP Chip";
        }

        public override double CalculatePCBSketchX()
        {
            return PCB_PinSpacing.Millimeters * (Pins.Count - 1) + PCB_HoleInnerDiameter.Millimeters + PCB_RingWidth.Millimeters * 2;
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
            return Pins.Count * Breadboard_PinSpacing.Millimeters;
        }

        public override double CalculateIconSketchX()
        {
            return CalculateBreadboardSketchX();
        }

        public override double CalculatePCBSketchY()
        {
            return PCB_HoleInnerDiameter.Millimeters + PCB_RingWidth.Millimeters * 2;
        }

        public override double CalculateSchematicSketchY()
        {
            return Pins.Count * Schematic_PinSpacing.Millimeters;
        }

        public override double CalculateBreadboardSketchY()
        {
            return Breadboard_ChipHeight.Millimeters + Breadboard_PinLength.Millimeters;
        }

        public override double CalculateIconSketchY()
        {
            return CalculateBreadboardSketchY();
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

            //copperlayers
            var copper = new List<SVGElement>();
            dict[PCBLayer.BothCopper] = copper;

            double d = PCB_HoleInnerDiameter.Millimeters + PCB_RingWidth.Millimeters;
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

        public override List<SVGElement> getSchematicSVGElements()
        {
            var elements = new List<SVGElement>();
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

        public override List<SVGElement> getBreadboardSVGElements()
        {
            var elements = new List<SVGElement>();
            double w = CalculateBreadboardSketchX();
            double h = CalculateBreadboardSketchY();

            var rect = new SVGRect();
            rect.X.Value = 0;
            rect.Y.Value = 0;
            rect.Width.Value = w;
            rect.Height.Value = Breadboard_ChipHeight.Millimeters;
            rect.FillColor.Value = chipBlackColor;
            elements.Add(rect);

            double x = Breadboard_PinSpacing.Millimeters / 2 - Breadboard_PinWidth.Millimeters / 2;
            for(int i = 0; i < Pins.Count; i++, x += Breadboard_PinSpacing.Millimeters)
            {
                rect = new SVGRect();
                rect.X.Value = x - Breadboard_PinWidth.Millimeters / 2;
                rect.Y.Value = Breadboard_ChipHeight.Millimeters;
                rect.Width.Value = Breadboard_PinWidth.Millimeters;
                rect.Height.Value = Breadboard_PinLength.Millimeters;
                rect.FillColor.Value = pinGrayColor;
                rect.ID.Value = "connector-pin-" + i;
                elements.Add(rect);
                rect = new SVGRect();
                rect.X.Value = x - Breadboard_TerminalWidth.Millimeters / 2;
                rect.Y.Value = h - Breadboard_TerminalHeight.Millimeters / 2;
                rect.Width.Value = Breadboard_TerminalWidth.Millimeters;
                rect.Height.Value = Breadboard_TerminalHeight.Millimeters;
                rect.FillColor.Value = pinGrayColor;
                rect.ID.Value = "connector-terminal-" + i;
                elements.Add(rect);
            }

            return elements;
        }

        public override List<SVGElement> getIconSVGElements()
        {
            return getBreadboardSVGElements();
        }
    }
}
