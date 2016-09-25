using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;

namespace FritzingGenericChipMaker
{
    public enum ChipType
    {
        ThrougholeSIP,//Single Inline Package
        ThrougholeDIP,//Dual Inline Package
        SOIC,//Small Outline Integrated Circuit
        QFP//Quad Flat Pack
    }

    public enum ConnectorType
    {
        male,
        female,
        wire,
        pad
    }

    public enum PCBLayer
    {
        None,
        Silkscreen,
        Copper0,//todo determine copper top and copper botton instead of 0 and 1
        Copper1,
        BothCopper,
    }

    public abstract class ChipInfo
    {
        public static readonly Color copperColor = Color.FromArgb(255, 191, 0);
        public static readonly Color pinGrayColor = Color.FromArgb(140, 140, 140);
        public static readonly Color chipBlackColor = Color.FromArgb(20, 20, 20);

        [Editor(typeof(PinCollectionEditor), typeof(UITypeEditor))]
        public List<PinInfo> Pins { get; set; } = new List<PinInfo>();
        public string ChipName { get; set; }
        public string ModuleID { get; set; }
        public XMLAttribute<string> ReferenceFile { get; } = new XMLAttribute<string>("referenceFile", "");

        public XMLElement Version { get; } = new XMLElement("version");
        public XMLElement Author { get; } = new XMLElement("author");
        public XMLElement Description { get; } = new XMLElement("description");
        [Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
        public List<string> Tags { get; set; } = new List<string>();
        public Dictionary<string, string> Properties { get; } = new Dictionary<string, string>();

        public ChipInfo()
        {
            Properties["package"] = "";
            Properties["family"] = "";
            Properties["variant"] = "";
        }

        public abstract double CalculatePCBSketchX();
        public abstract double CalculateSchematicSketchX();
        public abstract double CalculateBreadboardSketchX();
        public abstract double CalculateIconSketchX();

        public abstract double CalculatePCBSketchY();
        public abstract double CalculateSchematicSketchY();
        public abstract double CalculateBreadboardSketchY();
        public abstract double CalculateIconSketchY();

        public abstract Dictionary<PCBLayer, List<XMLElement>> getPCBSVGElements();
        public abstract List<XMLElement> getSchematicSVGElements();
        public abstract List<XMLElement> getBreadboardSVGElements();
        public abstract List<XMLElement> getIconSVGElements();

        public string GetFZPXML(string filename)
        {
            StringBuilder sb = new StringBuilder();

            filename = Path.GetFileNameWithoutExtension(filename);

            sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            sb.Append("<module ");
            string refFile = ReferenceFile.ToString();
            if(!string.IsNullOrEmpty(refFile))
            {
                sb.Append(refFile);
                sb.Append(' ');
            }
            sb.Append("moduleId='");
            sb.Append(ModuleID);
            sb.AppendLine("' fritzingVersion='0.9.3'>");

            sb.AppendLine(Version.ToString());
            sb.AppendLine(Author.ToString());
            sb.AppendLine(Description.ToString());
            var elem = new XMLElement("title");
            elem.Value = ChipName;
            sb.AppendLine(elem.ToString());
            elem.Tag = "label";
            sb.AppendLine(elem.ToString());
            elem.Tag = "date";
            elem.Value = DateTime.Now.ToString("yyyy-MM-dd");
            sb.AppendLine(elem.ToString());

            sb.AppendLine("<tags>");
            elem.Tag = "tag";
            foreach(var t in Tags)
            {
                elem.Value = t;
                sb.AppendLine(elem.ToString());
            }
            sb.AppendLine("</tags>");

            sb.AppendLine("<properties>");
            elem.Tag = "property";
            XMLAttribute<string> att = new XMLAttribute<string>("name", "");
            elem.Attributes.Add(att);
            foreach(var p in Properties)
            {
                att.Value = p.Key;
                elem.Value = p.Value;
                sb.AppendLine(elem.ToString());
            }
            sb.AppendLine("</properties>");

            sb.AppendLine("<views>");

            sb.AppendLine("<pcbView>");
            sb.AppendLine("<layers image='pcb/" + filename + "_pcb.svg'>");
            sb.AppendLine("<layer layerId='silkscreen'/>");
            sb.AppendLine("<layer layerId='copper1'/>");
            sb.AppendLine("<layer layerId='copper0'/>");
            sb.AppendLine("</layers>");
            sb.AppendLine("</pcbView>");

            sb.AppendLine("<schematicView>");
            sb.AppendLine("<layers image='schematic/" + filename + "_schematic.svg'>");
            sb.AppendLine("<layer layerId='schematic'/>");
            sb.AppendLine("</layers>");
            sb.AppendLine("</schematicView>");

            sb.AppendLine("<breadboardView>");
            sb.AppendLine("<layers image='breadboard/" + filename + "_breadboard.svg'>");
            sb.AppendLine("<layer layerId='breadboard'/>");
            sb.AppendLine("</layers>");
            sb.AppendLine("</breadboardView>");

            sb.AppendLine("<iconView>");
            sb.AppendLine("<layers image='icon/" + filename + "_icon.svg'>");
            sb.AppendLine("<layer layerId='icon'/>");
            sb.AppendLine("</layers>");
            sb.AppendLine("</iconView>");

            sb.AppendLine("</views>");

            sb.AppendLine("<connectors>");
            for(int i = 0; i < Pins.Count; i++)
            {
                var pin = Pins[i];
                sb.AppendLine("<connector name='" + pin.Name + "' type='' id='connector" + i + "'>");
                sb.Append("<description>");
                sb.Append(pin.Description);
                sb.AppendLine("</description>");
                sb.AppendLine("<views>");

                sb.AppendLine("<pcbView>");
                sb.AppendLine("<p layer='copper1' svgId='connector-pad-" + i + "'/>");
                sb.AppendLine("</pcbView>");

                sb.AppendLine("<schematicView>");
                sb.AppendLine("<p layer='schematic' terminalId='connector-terminal-" + i + "' svgId='connector-pin-" + i + "'/>");
                sb.AppendLine("</schematicView>");

                sb.AppendLine("<breadboardView>");
                sb.AppendLine("<p layer='breadboard' svgId='connector-pin-" + i + "'/>");
                sb.AppendLine("</breadboardView>");

                sb.AppendLine("</views>");
                sb.AppendLine("</connector>");
            }
            sb.AppendLine("</connectors>");

            sb.AppendLine("</module>");

            return sb.ToString();
        }

        public string GetPCBSVG()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            double w = CalculatePCBSketchX();
            double h = CalculatePCBSketchY();
            sb.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' version='1.2' x='0' y='0' id='svg2' width='" + Formatter.Format(w) + "mm' height='" + Formatter.Format(h) + "mm' viewBox='0 0 " + Formatter.Format(w) + " " + Formatter.Format(h) + "'>");

            var elements = getPCBSVGElements();

            foreach(var e in elements)
            {
                switch(e.Key)
                {
                    case PCBLayer.Silkscreen:
                        sb.AppendLine("<g id='silkscreen'>");
                        break;
                    case PCBLayer.Copper0:
                        sb.AppendLine("<g id='copper0'>");
                        break;
                    case PCBLayer.Copper1:
                        sb.AppendLine("<g id='copper1'>");
                        break;
                    case PCBLayer.BothCopper:
                        sb.AppendLine("<g id='copper0'>");
                        sb.AppendLine("<g id='copper1'>");
                        break;
                }

                foreach(var se in e.Value)
                {
                    sb.AppendLine(se.Emit());
                }

                if(e.Key == PCBLayer.BothCopper)
                {
                    sb.AppendLine("</g>");
                    sb.AppendLine("</g>");
                }
                else
                {
                    sb.AppendLine("</g>");
                }
            }

            sb.AppendLine("</svg>");

            return sb.ToString();
        }

        public string GetSchematicSVG()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            double w = CalculateSchematicSketchX();
            double h = CalculateSchematicSketchY();
            sb.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' version='1.2' x='0' y='0' id='svg2' width='" + Formatter.Format(w) + "mm' height='" + Formatter.Format(h) + "mm' viewBox='0 0 " + Formatter.Format(w) + " " + Formatter.Format(h) + "'>");
            sb.AppendLine("<g id='schematic'>");

            var elements = getSchematicSVGElements();

            foreach(var e in elements)
            {
                sb.AppendLine(e.Emit());
            }

            sb.AppendLine("</g>");
            sb.AppendLine("</svg>");

            return sb.ToString();
        }

        public string GetBreadboardSVG()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            double w = CalculateBreadboardSketchX();
            double h = CalculateBreadboardSketchY();
            sb.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' version='1.2' x='0' y='0' id='svg2' width='" + Formatter.Format(w) + "mm' height='" + Formatter.Format(h) + "mm' viewBox='0 0 " + Formatter.Format(w) + " " + Formatter.Format(h) + "'>");
            sb.AppendLine("<g id='breadboard'>");

            var elements = getBreadboardSVGElements();

            foreach(var e in elements)
            {
                sb.AppendLine(e.Emit());
            }

            sb.AppendLine("</g>");
            sb.AppendLine("</svg>");

            return sb.ToString();
        }

        public string GetIconSVG()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            double w = CalculateIconSketchX();
            double h = CalculateIconSketchY();
            sb.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' version='1.2' x='0' y='0' id='svg2' width='" + Formatter.Format(w) + "mm' height='" + Formatter.Format(h) + "mm' viewBox='0 0 " + Formatter.Format(w) + " " + Formatter.Format(h) + "'>");
            sb.AppendLine("<g id='icon'>");

            var elements = getIconSVGElements();

            foreach(var e in elements)
            {
                sb.AppendLine(e.Emit());
            }

            sb.AppendLine("</g>");
            sb.AppendLine("</svg>");

            return sb.ToString();
        }

        /*public bool ThermalPad { get; set; }
        public float ThermalPadSize { get; set; }


        public float PinInset { get; set; }
        public float PinWidth { get; set; }
        public float PinDepth { get; set; }
        public bool OutermostPinDiffDepth { get; set; }
        public float OutermostPinDepth { get; set; }
        public float PinOverlength { get; set; }*/
    }
}
