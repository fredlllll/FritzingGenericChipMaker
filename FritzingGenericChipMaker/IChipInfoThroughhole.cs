using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public interface IChipInfoThroughhole//oh thx compiler makers for thinking noone would ever need multi inheritance you fucks. how do you get rid of boilerplate code? oh you dont!!
    {
        Measurement PCB_OutlineWidth { get; set; }// = new Measurement(0.05);
        Measurement PCB_HoleInnerDiameter { get; set; }// = new Measurement(1);
        Measurement PCB_RingWidth { get; set; }// = new Measurement(0.4);
    }
}
