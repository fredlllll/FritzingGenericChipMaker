using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    [TypeConverter(typeof(MeasurementConverter))]
    public class Measurement
    {
        double mm;
        [Browsable(false)]
        public bool IsMM
        {
            get; private set;
        }

        public double Inches
        {
            get { return mm / 25.4; }
            set { mm = value * 25.4; IsMM = false; }
        }
        public double Millimeters
        {
            get { return mm; }
            set { mm = value; IsMM = true; }
        }

        public Measurement(double value = 0, bool ismm = true)
        {
            if(ismm)
            {
                Millimeters = value;
            }
            else
            {
                Inches = value;
            }
        }

        public override string ToString()
        {
            if(IsMM)
            {
                return Millimeters.ToString(SVGElement.doubleFormat) + "mm";
            }
            else
            {
                return Inches.ToString(SVGElement.doubleFormat) + "in";
            }
        }
    }

    public class MeasurementConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return typeof(string) == destinationType;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if(destinationType == typeof(string))
            {
                Measurement m = value as Measurement;
                return m.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return typeof(string) == sourceType;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string str = value as string;
            if(str != null)
            {
                var tmp = context.Instance;
                Measurement m = new Measurement();
                if(str.EndsWith("mm"))
                {
                    m.Millimeters = double.Parse(str.Substring(0, str.Length - 2));
                }
                else if(str.EndsWith("in"))
                {
                    m.Inches = double.Parse(str.Substring(0, str.Length - 2));
                }
                else //assume mm
                {
                    try
                    {
                        m.Millimeters = double.Parse(str);
                    }
                    catch { }
                }
                return m;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
