using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    [Flags]
    public enum Layer
    {
        None = 0,
        Silkscreen = 1,
        Copper0 = 2,
        Copper1 = 4,
        BothCopper = Copper0|Copper1,
    }

    public abstract class SVGElement
    {
        public Layer Layer { get; set; }

        public const string doubleFormat = "#.#################################";

        public static string Format(Color c)
        {
            return c.A > 0 ? c.R + "," + c.G + "," + c.B : "none";
        }

        public static string Format(double d)
        {
            return d.ToString(doubleFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public abstract string Emit(string id);
    }
}
