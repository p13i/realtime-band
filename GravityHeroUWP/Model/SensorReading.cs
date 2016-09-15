using System;
using System.Runtime.Serialization;

namespace GravityHero
{
    [DataContract]
    public class SensorReading : ViewModel
    {
        private DateTimeOffset _timestamp;

        private double _x;

        private double _y;

        private double _z;

        [DataMember]
        public DateTimeOffset Timestamp
        {
            get { return _timestamp; }
            set { SetValue(ref _timestamp, value, "Timestamp"); }
        }

        [DataMember]
        public double X
        {
            get { return _x; }
            set { SetValue(ref _x, value, "X", "Value"); }
        }

        [DataMember]
        public double Y
        {
            get { return _y; }
            set { SetValue(ref _y, value, "Y", "Value"); }
        }

        [DataMember]
        public double Z
        {
            get { return _z; }
            set { SetValue(ref _z, value, "Z", "Value"); }
        }


        public double Value
        {
            get { return Math.Sqrt(X*X + Y*Y + Z*Z); }
        }
    }
}