using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class Measurement : INotifyPropertyChanged, ICustomTypeDescriptor
    {
        double mm;
        public event PropertyChangedEventHandler PropertyChanged;

        public double Inches
        {
            get { return mm / 25.4; }
            set { mm = value * 25.4; Changed(); }
        }
        public double Millimeters
        {
            get
            {
                return mm;
            }
            set
            {
                mm = value;
                Changed();
            }
        }

        void Changed()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Inches"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Millimeters"));
        }

        public Measurement(double value, bool inches = false)
        {
            if(inches)
            {
                Inches = value;
            }
            else
            {
                Millimeters = value;
            }
        }

        public String GetClassName()
        {
            return "Measurement";//            TypeDescriptor.GetClassName(this, true);
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public String GetComponentName()
        {
            return "Measurement";
            //return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        public PropertyDescriptorCollection GetProperties()
        {
            var pdc = new PropertyDescriptorCollection(null);
            PropertyDescriptor pd = new MeasurementPropertyDescriptor(this, "mm");
            pdc.Add(pd);
            pd = new MeasurementPropertyDescriptor(this, "inches");
            pdc.Add(pd);
            return pdc;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }
    }

    public class MeasurementPropertyDescriptor : PropertyDescriptor
    {
        private Measurement m;

        public MeasurementPropertyDescriptor(Measurement m, string id) : base(id, null)
        {
            this.m = m;
            ComponentType = m.GetType();
        }

        public override string DisplayName
        {
            get
            {
                if(Name == "mm")
                {
                    return "Millimeters";
                }
                else
                {
                    return "Inches";
                }
            }
        }

        public override Type ComponentType
        {
            get;
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override Type PropertyType
        {
            get { return typeof(double); }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            if(Name == "mm")
            {
                return m.Millimeters;
            }
            else
            {
                return m.Inches;
            }
        }

        public override void ResetValue(object component)
        {
            m.Millimeters = 0;
        }

        public override void SetValue(object component, object value)
        {
            if(Name == "mm")
            {
                m.Millimeters = (double)value;
            }
            else
            {
                m.Inches = (double)value;
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}
