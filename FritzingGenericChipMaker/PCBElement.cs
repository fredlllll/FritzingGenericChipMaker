using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public abstract class PCBElement
    {
        public PCBLayer Layer { get; set; }
    }
}
