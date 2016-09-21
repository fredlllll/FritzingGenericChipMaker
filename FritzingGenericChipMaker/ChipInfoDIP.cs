using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class ChipInfoDIP : ChipInfo
    {
        public Measurement PCB_PinSpacingX { get; set; } = new Measurement(0.1, true);
        public Measurement PCB_PinSpacingY { get; set; } = new Measurement(15);
        public Measurement PCB_HoleOuterDiam { get; set; } = new Measurement(0.8);

        public override double CalculateSketchX_MM()
        {
            return PCB_PinSpacingX.Millimeters * (PinCount / 2 + 1);
        }

        public override double CalculateSketchY_MM()
        {
            return PCB_PinSpacingY.Millimeters + PCB_HoleOuterDiam.Millimeters;
        }

        public override Dictionary<PCBLayer,List<SVGElement>> getPCBSVGElements()
        {
            throw new NotImplementedException();
        }

        public override List<SVGElement> getSchematicSVGElements()
        {
            throw new NotImplementedException();
        }

        public override Dictionary<BreadboardLayer, List<SVGElement>> getBreadboardSVGElements()
        {
            throw new NotImplementedException();
        }
    }
}
