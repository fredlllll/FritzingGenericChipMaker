using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public interface IXMLAttribute
    {
        string ToString();
    }

    public class XMLAttribute<T> : IXMLAttribute
    {
        public string Name { get; }
        public T Value { get; set; }
        MethodInfo formatInfo;
        FormatDelegate format;
        delegate string FormatDelegate(T value);

        public XMLAttribute(string name, T value)
        {
            Name = name;
            Value = value;
            formatInfo = typeof(Formatter).GetMethod("Format", BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(T) }, null);
            if(formatInfo == null)
            {
                format = ValueToString;
            }
            else
            {
                format = (FormatDelegate)formatInfo.CreateDelegate(typeof(FormatDelegate));
            }
        }

        string ValueToString(T value)
        {
            try
            {
                return value.ToString();//will throw error if null
            }
            catch
            {
                return string.Empty;
            }
        }

        public override string ToString()
        {
            string formatted = format(Value);
            if(!string.IsNullOrEmpty(formatted))
            {
                return Name + "='" + formatted + "'";
            }
            return string.Empty;
        }
    }
}
