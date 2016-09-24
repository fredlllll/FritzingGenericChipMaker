using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class SVGLine : XMLElement
    {
        public XMLAttribute<double> X1 { get; } = new XMLAttribute<double>("x1", 0);
        public XMLAttribute<double> Y1 { get; } = new XMLAttribute<double>("y1", 0);
        public XMLAttribute<double> X2 { get; } = new XMLAttribute<double>("x2", 0);
        public XMLAttribute<double> Y2 { get; } = new XMLAttribute<double>("y2", 0);
        public XMLAttribute<Color> StrokeColor { get; } = new XMLAttribute<Color>("stroke", Color.Transparent);
        public XMLAttribute<double> StrokeWidth { get; } = new XMLAttribute<double>("stroke-width", 0);

        public SVGLine():base("line")
        {
            Attributes.Add(X1);
            Attributes.Add(Y1);
            Attributes.Add(X2);
            Attributes.Add(Y2);
            Attributes.Add(StrokeColor);
            Attributes.Add(StrokeWidth);
        }
    }
}
