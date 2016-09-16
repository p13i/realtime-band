using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using GravityHeroUWP.Model;
using Microsoft.Band.Sensors;

namespace GravityHeroUWP.Model
{
    public class HeartRateSensor : ISensor<IBandSensor<IBandHeartRateReading>, IBandHeartRateReading>
    {
        public delegate void ChangedHandler(BandSensorReadingEventArgs<IBandHeartRateReading> reading);
        public event ChangedHandler Changed;

        private int _readingCount;

        public void Init(TimeSpan reportingInterval)
        {
            if (!BandModel.IsConnected) return;

            BandModel.BandClient.SensorManager.HeartRate.ReadingChanged += ReadingChanged;
            BandModel.BandClient.SensorManager.HeartRate.ReportingInterval = reportingInterval;
        }

        public async void ReadingChanged(object sender, BandSensorReadingEventArgs<IBandHeartRateReading> reading)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                    _readingCount++;
                    Changed?.Invoke(reading);
            });
        }

        public void Start()
        {
            BandModel.BandClient.SensorManager.HeartRate.StartReadingsAsync();
        }

        public void Stop()
        {
            BandModel.BandClient.SensorManager.HeartRate.StopReadingsAsync();
        }

        public int GetReadingCount()
        {
            return _readingCount;
        }
    }
}