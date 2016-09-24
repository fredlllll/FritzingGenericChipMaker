using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
  
    public class XMLElement
    {
        public XMLAttribute<string> ID { get; } = new XMLAttribute<string>("id", null);
        public string Tag { get; set; }
        public string Value { get; set; }
        public List<IXMLAttribute> Attributes { get; } = new List<IXMLAttribute>();

        public XMLElement(string tag)
        {
            Tag = tag;
            Attributes.Add(ID);
        }

        public string Emit()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('<');
            sb.Append(Tag);

            foreach(var att in Attributes)
            {
                string attstr = att.ToString();
                if(!string.IsNullOrEmpty(attstr))
                {
                    sb.Append(' ');
                    sb.Append(attstr);
                }
            }

            if(string.IsNullOrEmpty(Value))
            {
                sb.Append("/>");
            }
            else
            {
                sb.Append('>');
                sb.Append(Value);
                sb.Append("</");
                sb.Append(Tag);
                sb.Append('>');
            }

            return sb.ToString();
        }
    }
}
