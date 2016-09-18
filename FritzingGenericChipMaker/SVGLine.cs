using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class SVGLine : SVGElement
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public Color StrokeColor { get; set; } = Color.White;
        public double StrokeWidth { get; set; } = 0.1;
        public override string Emit()
        {
            string x1 = Format(X1);
            string y1 = Format(Y1);
            string x2 = Format(X2);
            string y2 = Format(Y2);
            string strokeColor = Format(StrokeColor);
            string strokeWidth = Format(StrokeWidth);
            return string.Format("<line x1=\"{0}\" y1=\"{1}\" x2=\"{2}\" y2=\"{3}\" stroke=\"rgb({4})\" stroke-width=\"{5}\" id=\"{6}\"/>",
                x1,y1,x2,y2,strokeColor,strokeWidth,ID);
        }
    }
}
