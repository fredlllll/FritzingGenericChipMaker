using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class Measurement : INotifyPropertyChanged
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
    }
}
