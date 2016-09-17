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
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Diameter { get; set; }
        public Color StrokeColor { get; set; }
        public double StrokeWidth { get; set; }

        public override string Emit(string id)
        {
            string cx = Format(CenterX);
            string cy = Format(CenterX);
            string r = Format(Diameter / 2);
            string color = Format(StrokeColor);
            string strokeWidth = Format(StrokeWidth);
            return string.Format("<circle fill=\"none\" cx=\"{0}\" cy=\"{1}\" stroke=\"rgb({2})\" id=\"{3}\" r=\"{4}\" stroke-width=\"{5}\"/>",
                cx, cy, color, id, r, strokeWidth);
        }
    }
}
