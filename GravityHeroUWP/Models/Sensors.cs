using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using SensorStream.Models;
using Microsoft.Band.Sensors;

namespace SensorStream.Models
{
    namespace Sensors
    {
        public class AccelerometerSensor : Sensor<IBandSensor<IBandAccelerometerReading>, IBandAccelerometerReading>
        {
            public static string Unit = "G";

            public AccelerometerSensor() : base(BandModel.BandClient.SensorManager.Accelerometer)
            {
            }
        }

        public class HeartRateSensor : Sensor<IBandSensor<IBandHeartRateReading>, IBandHeartRateReading>
        {
            public static string Unit = "BPM";

            public HeartRateSensor() : base(BandModel.BandClient.SensorManager.HeartRate)
            {
            }
        }

        public class GyroscopeSensor : Sensor<IBandSensor<IBandGyroscopeReading>, IBandGyroscopeReading>
        {
            public static string Unit = "deg/sec";
            public GyroscopeSensor() : base(BandModel.BandClient.SensorManager.Gyroscope)
            {
            }
        }
    }

}