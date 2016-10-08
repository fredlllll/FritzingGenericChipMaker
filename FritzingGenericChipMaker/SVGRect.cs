using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class SVGRect : SVGElement
    {
        public XMLAttribute<double> X { get; } = new XMLAttribute<double>("x", 0);
        public XMLAttribute<double> Y { get; } = new XMLAttribute<double>("y", 0);
        public XMLAttribute<double> Width { get; } = new XMLAttribute<double>("width", 0);
        public XMLAttribute<double> Height { get; } = new XMLAttribute<double>("height", 0);
        public XMLAttribute<Color> FillColor { get; } = new XMLAttribute<Color>("fill", Color.Transparent);
        public XMLAttribute<Color> StrokeColor { get; } = new XMLAttribute<Color>("stroke", Color.Transparent);
        public XMLAttribute<double> StrokeWidth { get; } = new XMLAttribute<double>("stroke-width", 0);

        public SVGRect():base("rect")
        {
            Attributes.Add(X);
            Attributes.Add(Y);
            Attributes.Add(Width);
            Attributes.Add(Height);
            Attributes.Add(FillColor);
            Attributes.Add(StrokeColor);
            Attributes.Add(StrokeWidth);
        }
    }
}
