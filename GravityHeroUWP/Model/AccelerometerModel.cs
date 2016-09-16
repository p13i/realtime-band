using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using GravityHeroUWP.Model;
using Microsoft.Band.Sensors;

namespace GravityHero
{
    /// <summary>
    ///     Kevin Ashley, Microsoft
    /// </summary>
    public class Accelerometer : ISensor<IBandSensor<IBandAccelerometerReading>, IBandAccelerometerReading>
    {
        public delegate void ChangedHandler(BandSensorReadingEventArgs<IBandAccelerometerReading> reading);
        public event ChangedHandler Changed;

        public void Init(TimeSpan reportingInterval)
        {
            if (!BandModel.IsConnected) return;

            BandModel.BandClient.SensorManager.Accelerometer.ReadingChanged += ReadingChanged;
            BandModel.BandClient.SensorManager.Accelerometer.ReportingInterval = reportingInterval;
            BandModel.BandClient.SensorManager.Accelerometer.StartReadingsAsync(new CancellationToken());
        }

        public async void ReadingChanged(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> reading)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    Changed?.Invoke(reading);
            });
        }
    }
}