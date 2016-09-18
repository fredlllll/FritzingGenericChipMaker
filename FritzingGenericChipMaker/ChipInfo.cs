using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public enum ChipType
    {
        ThrougholeSIP,//Single Inline Package
        ThrougholeDIP,//Dual Inline Package
        SOIC,//Small Outline Integrated Circuit
        QFP//Quad Flat Pack
    }

    public abstract class ChipInfo : INotifyPropertyChanged
    {
        public int PinCount { get; set; } = 8;
        public string Label { get; set; }

        public abstract double CalculateSketchX_MM();
        public abstract double CalculateSketchY_MM();

        public abstract Dictionary<Layer, List<SVGElement>> getSVGElements();

        public string GetPCBSVG()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            double w = CalculateSketchX_MM();
            double h = CalculateSketchY_MM();
            sb.AppendLine("<svg xmlns='http://www.w3.org/2000/svg' version='1.2' x='0' y='0' id='svg2' width='" + SVGElement.Format(w) + "mm' height='" + SVGElement.Format(h) + "mm' viewBox='0 0 " + SVGElement.Format(w) + " " + SVGElement.Format(h) + "'>");

            var elements = getSVGElements();

            foreach(var e in elements)
            {
                int groups = 1;
                switch(e.Key)
                {
                    case Layer.Silkscreen:
                        sb.AppendLine("<g id='silkscreen'>");
                        break;
                    case Layer.Copper0:
                        sb.AppendLine("<g id='copper0'>");
                        break;
                    case Layer.Copper1:
                        sb.AppendLine("<g id='copper1'>");
                        break;
                    case Layer.BothCopper:
                        groups = 2;
                        sb.AppendLine("<g id='copper0'><g id='copper1'>");
                        break;
                }
                foreach(var se in e.Value)
                {
                    sb.AppendLine(se.Emit());
                }

                for(int i = 0; i < groups; i++)
                {
                    sb.AppendLine("</g>");
                }
            }

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(object sender, PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(sender, args);
        }
    }
}
