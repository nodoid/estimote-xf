using System.ComponentModel;

namespace estimote
{
    public class BeaconData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string macAddress;

        public string MacAddress
        { 
            get
            {
                return macAddress;
            } 

            set
            {
                if (macAddress != value)
                {
                    macAddress = value;
                    OnPropertyChanged("MacAddress");
                }
            }
        }

        private int major;

        public int Major
        {
            get
            {
                return major;
            }
            set
            {
                if (major != value)
                {
                    major = value;
                    OnPropertyChanged("Major");
                }
            }
        }

        private int measuredPower;

        public int MeasuredPower
        {
            get
            {
                return measuredPower;
            }
            set
            {
                if (measuredPower != value)
                {
                    measuredPower = value;
                    OnPropertyChanged("MeasuredPower");
                }
            }
        }

        private int minor;

        public int Minor
        {
            get
            {
                return minor;
            }
            set
            {
                if (minor != value)
                {
                    minor = value;
                    OnPropertyChanged("Minor");
                }
            }
        }

        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string proximityUUID;

        public string ProximityUUID
        {
            get
            {
                return proximityUUID;
            }
            set
            {
                if (proximityUUID != value)
                {
                    proximityUUID = value;
                    OnPropertyChanged("ProximityUUID");
                }
            }
        }

        private int rssi;

        public int Rssi
        {
            get
            {
                return rssi;
            }
            set
            {
                if (rssi != value)
                {
                    rssi = value;
                    OnPropertyChanged("RSSI");
                }
            }
        }

        private RegionData region;

        public RegionData Region
        {
            get
            {
                return region;
            }
            set
            {
                if (region != value)
                {
                    region = value;
                    OnPropertyChanged("RegionData");
                }
            }
        }
    }
}

