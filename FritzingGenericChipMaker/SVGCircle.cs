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
        public double Diameter { get; set; } = 2.54;
        public Color StrokeColor { get; set; } = Color.FromArgb(255, 191, 0);
        public double StrokeWidth { get; set; } = 0.5;

        public override string Emit()
        {
            string cx = Format(CenterX);
            string cy = Format(CenterY);
            string r = Format(Diameter / 2);
            string color = Format(StrokeColor);
            string strokeWidth = Format(StrokeWidth);
            return string.Format("<circle fill=\"none\" cx=\"{0}\" cy=\"{1}\" stroke=\"rgb({2})\" id=\"{3}\" r=\"{4}\" stroke-width=\"{5}\"/>",
                cx, cy, color, ID, r, strokeWidth);
        }
    }
}
