using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class Formatter
    {
        public const string doubleFormat = "0.#################################";

        public static string Format(Color c)
        {
            return c.A > 0 ? "rgb(" + c.R + "," + c.G + "," + c.B + ")" : "none";
        }

        public static string Format(double d)
        {
            return d.ToString(doubleFormat, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
