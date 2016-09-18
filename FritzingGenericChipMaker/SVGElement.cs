using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public enum Layer
    {
        None,
        Silkscreen,
        Copper0,
        Copper1,
        BothCopper,
    }

    public abstract class SVGElement
    {
        public Layer Layer { get; set; }
        public string ID { get; set; }

        public const string doubleFormat = "0.#################################";

        public static string Format(Color c)
        {
            return c.A > 0 ? c.R + "," + c.G + "," + c.B : "none";
        }

        public static string Format(double d)
        {
            return d.ToString(doubleFormat, System.Globalization.CultureInfo.InvariantCulture);
        }

        public abstract string Emit();
    }
}
