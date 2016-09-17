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
        public int PinCount { get; set; }
        public string Label { get; set; }

        public abstract double CalculateSketchX_MM();
        public abstract double CalculateSketchY_MM();

        public abstract List<SVGElement> getSVGElements();

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

    public class ChipInfoSIP : ChipInfo
    {
        public Measurement PinSpacing { get; set; } = new Measurement(0.1, true);
        public Measurement HoleOuterDiam { get; set; } = new Measurement(0.8);

        public ChipInfoSIP()
        {
            PinSpacing.PropertyChanged += RaisePropertyChangedEvent;
            HoleOuterDiam.PropertyChanged += RaisePropertyChangedEvent;
        }

        public override double CalculateSketchX_MM()
        {
            return PinSpacing.Millimeters*(PinCount+1);
        }

        public override double CalculateSketchY_MM()
        {
            return HoleOuterDiam.Millimeters;
        }

        public override List<SVGElement> getSVGElements()
        {
            throw new NotImplementedException();
        }
    }

    public class ChipInfoDIP : ChipInfo
    {
        public Measurement PinSpacingX { get; set; } = new Measurement(0.1, true);
        public Measurement PinSpacingY { get; set; } = new Measurement(15);
        public Measurement HoleOuterDiam { get; set; } = new Measurement(0.8);

        public ChipInfoDIP()
        {
            PinSpacingX.PropertyChanged += RaisePropertyChangedEvent;
            PinSpacingY.PropertyChanged += RaisePropertyChangedEvent;
            HoleOuterDiam.PropertyChanged += RaisePropertyChangedEvent;
        }

        public override double CalculateSketchX_MM()
        {
            return PinSpacingX.Millimeters * (PinCount/2+1);
        }

        public override double CalculateSketchY_MM()
        {
            return PinSpacingY.Millimeters + HoleOuterDiam.Millimeters;
        }

        public override List<SVGElement> getSVGElements()
        {
            throw new NotImplementedException();
        }
    }
}
