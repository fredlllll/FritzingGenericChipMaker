using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class SVGCircle : SVGElement
    {
        public XMLAttribute<double> CenterX { get; } = new XMLAttribute<double>("cx", 0);
        public XMLAttribute<double> CenterY { get; } = new XMLAttribute<double>("cy", 0);
        public XMLAttribute<double> Radius { get; } = new XMLAttribute<double>("r", 1.27);
        public XMLAttribute<Color> FillColor { get; } = new XMLAttribute<Color>("fill", Color.Transparent);
        public XMLAttribute<Color> StrokeColor { get; } = new XMLAttribute<Color>("stroke", Color.Transparent);
        public XMLAttribute<double> StrokeWidth { get; } = new XMLAttribute<double>("stroke-width", 0);

        public SVGCircle() : base("circle")
        {
            Attributes.Add(CenterX);
            Attributes.Add(CenterY);
            Attributes.Add(Radius);
            Attributes.Add(FillColor);
            Attributes.Add(StrokeColor);
            Attributes.Add(StrokeWidth);
        }
    }
}
