using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class PinCollectionEditor : CollectionEditor
    {
        public PinCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(PinInfo);
        }

        protected override object CreateInstance(Type itemType)
        {
            if(typeof(PinInfo) == itemType)
            {
                PinInfo pi = new PinInfo();
                ChipInfo info = base.Context.Instance as ChipInfo;
                string index = "0";
                if(info != null)
                {
                    index = info.Pins.Count.ToString();
                }
                pi.Name = "Pin " + index;
                return pi;
            }
            return base.CreateInstance(itemType);
        }
    }
}
