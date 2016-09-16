using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using GravityHeroUWP.Model;
using Microsoft.Band.Sensors;

namespace GravityHeroUWP.Model
{
    public class GyroscopeSensor : ISensor<IBandSensor<IBandGyroscopeReading>, IBandGyroscopeReading>
    {
        public delegate void ChangedHandler(BandSensorReadingEventArgs<IBandGyroscopeReading> reading);
        public event ChangedHandler Changed;

        private int _readingCount;

        public void Init(TimeSpan reportingInterval)
        {
            if (!BandModel.IsConnected) return;

            BandModel.BandClient.SensorManager.Gyroscope.ReadingChanged += ReadingChanged;
            BandModel.BandClient.SensorManager.Gyroscope.ReportingInterval = reportingInterval;
        }

        public async void ReadingChanged(object sender, BandSensorReadingEventArgs<IBandGyroscopeReading> reading)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _readingCount++;
                Changed?.Invoke(reading);
            });
        }

        public void Start()
        {
            BandModel.BandClient.SensorManager.Gyroscope.StartReadingsAsync();
        }

        public void Stop()
        {
            BandModel.BandClient.SensorManager.Gyroscope.StopReadingsAsync();
        }

        public int GetReadingCount()
        {
            return _readingCount;
        }
    }
}