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
        public Measurement PCB_HoleDiameter { get; set; } = new Measurement(0.8);
        public Measurement PCB_RingWidth { get; set; } = new Measurement();

        public override double CalculatePCBSketchX()
        {
            return PCB_PinSpacingX.Millimeters * (Pins.Count - 1) + PCB_HoleDiameter.Millimeters + PCB_RingWidth.Millimeters * 2;
        }

        public override double CalculatePCBSketchY()
        {
            return PCB_PinSpacingY.Millimeters + PCB_HoleDiameter.Millimeters;
        }

        public override Dictionary<PCBLayer,List<XMLElement>> getPCBSVGElements()
        {
            throw new NotImplementedException();
        }

        public override List<XMLElement> getSchematicSVGElements()
        {
            throw new NotImplementedException();
        }

        public override List<XMLElement> getBreadboardSVGElements()
        {
            throw new NotImplementedException();
        }

        public override List<XMLElement> getIconSVGElements()
        {
            throw new NotImplementedException();
        }

        public override double CalculateSchematicSketchX()
        {
            throw new NotImplementedException();
        }

        public override double CalculateBreadboardSketchX()
        {
            throw new NotImplementedException();
        }

        public override double CalculateIconSketchX()
        {
            throw new NotImplementedException();
        }

        public override double CalculateSchematicSketchY()
        {
            throw new NotImplementedException();
        }

        public override double CalculateBreadboardSketchY()
        {
            throw new NotImplementedException();
        }

        public override double CalculateIconSketchY()
        {
            throw new NotImplementedException();
        }
    }
}
