using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FritzingGenericChipMaker
{
    public enum TextAnchor
    {
        inherit = 0,
        start,
        middle,
        end
    }

    public class SVGText : XMLElement
    {
        public XMLAttribute<double> X { get; } = new XMLAttribute<double>("x", 0);
        public XMLAttribute<double> Y { get; } = new XMLAttribute<double>("y", 0);
        public XMLAttribute<Color> FillColor { get; } = new XMLAttribute<Color>("fill", Color.Transparent);
        public XMLAttribute<Color> StrokeColor { get; } = new XMLAttribute<Color>("stroke", Color.Transparent);
        public XMLAttribute<double> StrokeWidth { get; } = new XMLAttribute<double>("stroke-width", 0);
        public XMLAttribute<double> FontSize { get; } = new XMLAttribute<double>("font-size", 1);
        public XMLAttribute<TextAnchor> TextAnchor { get; } = new XMLAttribute<TextAnchor>("text-anchor", FritzingGenericChipMaker.TextAnchor.start);

        public SVGText() : base("text")
        {
            Attributes.Add(X);
            Attributes.Add(Y);
            Attributes.Add(FillColor);
            Attributes.Add(StrokeColor);
            Attributes.Add(StrokeWidth);
            Attributes.Add(FontSize);
            Attributes.Add(TextAnchor);
        }

        public static Size MeasureText(string text, double size)
        {
            Font f = new Font("Arial", (float)size);
            return TextRenderer.MeasureText(text, f);
        }
    }
}
