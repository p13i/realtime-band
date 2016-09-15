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

        private SensorReading _last;
        private SensorReading _prev;

        private DateTimeOffset _startedTime = DateTimeOffset.MinValue;
        private double lastTime;
        private readonly double MIN = 0.4;
        private double totalTime;
        public event ChangedHandler Changed;

        public void Init()
        {
            if (BandModel.IsConnected)
            {
                BandModel.BandClient.SensorManager.Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                BandModel.BandClient.SensorManager.Accelerometer.ReportingInterval = TimeSpan.FromMilliseconds(16.0);
                BandModel.BandClient.SensorManager.Accelerometer.StartReadingsAsync(new CancellationToken());
                totalTime = 0.0;
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
                        Z = e.SensorReading.AccelerationZ
                    };
                    _prev = _last;
                    _last = reading;
                    Recalculate();
                });
        }


        private void Recalculate()
        {
            if (_last.Value <= MIN)
            {
                if (_startedTime > DateTimeOffset.MinValue)
                    lastTime = (DateTimeOffset.Now - _startedTime).TotalSeconds;
                else
                    _startedTime = DateTimeOffset.Now;
            }
            else
            {
                if (_startedTime > DateTimeOffset.MinValue)
                {
                    lastTime = (DateTimeOffset.Now - _startedTime).TotalSeconds;
                    totalTime += lastTime;
                    lastTime = 0.0;
                    _startedTime = DateTimeOffset.MinValue;
                    if (Changed != null)
                        Changed(_last);
                }
            }
        }
    }
}