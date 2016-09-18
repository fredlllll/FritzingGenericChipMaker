using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
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
            return PinSpacingX.Millimeters * (PinCount / 2 + 1);
        }

        public override double CalculateSketchY_MM()
        {
            return PinSpacingY.Millimeters + HoleOuterDiam.Millimeters;
        }

        public override Dictionary<Layer,List<SVGElement>> getSVGElements()
        {
            throw new NotImplementedException();
        }
    }
}
