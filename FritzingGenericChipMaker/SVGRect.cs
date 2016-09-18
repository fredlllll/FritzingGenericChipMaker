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
        public double Left { get; set; }
        public double Bottom { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Color FillColor { get; set; }
        public Color StrokeColor { get; set; }
        public double StrokeWidth { get; set; }

        public override string Emit()
        {
            string x = Format(Left);
            string y = Format(Bottom);
            string w = Format(Width);
            string h = Format(Height);
            string fillColor = Format(FillColor);
            string strokeColor = Format(StrokeColor);
            string strokeWidth = Format(StrokeWidth);
            return string.Format("<rect x=\"{0}\" y=\"{1}\" width=\"{2}\" height=\"{3}\" fill=\"{4}\" stroke=\"rgb({5})\" stroke-width=\"{6}\" id=\"{7}\"/>",
                x, y, w, h, fillColor, strokeColor, strokeWidth, ID);
        }
    }
}
