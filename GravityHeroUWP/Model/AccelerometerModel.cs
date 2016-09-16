using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Microsoft.Band.Sensors;

namespace GravityHero
{
    /// <summary>
    ///     Kevin Ashley, Microsoft
    /// </summary>
    public class AccelerometerModel : ViewModel
    {
        public delegate void ChangedHandler(SensorReading force);
        
        public event ChangedHandler Changed;

        public void Init()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                BandModel.BandClient.SensorManager.Accelerometer.ReportingInterval = TimeSpan.FromMilliseconds(16.0);
                BandModel.BandClient.SensorManager.Accelerometer.StartReadingsAsync(new CancellationToken());
            }
        }

        private void Accelerometer_ReadingChanged(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> e)
        {
            CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    var reading = new SensorReading
                    {
                        X = e.SensorReading.AccelerationX,
                        Y = e.SensorReading.AccelerationY,
                        Z = e.SensorReading.AccelerationZ,
                        Timestamp = e.SensorReading.Timestamp
                    };
                    Changed?.Invoke(reading);
                });
        }
    }
}