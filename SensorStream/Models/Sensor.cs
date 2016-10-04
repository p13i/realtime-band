using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using SensorStream.Models;
using Microsoft.Band;
using Microsoft.Band.Sensors;

namespace SensorStream.Models
{
    public abstract class Sensor<TSensor, TReading> where TSensor : IBandSensor<TReading> where TReading : IBandSensorReading
    {

        public delegate void ChangedHandler(BandSensorReadingEventArgs<TReading> reading);
        public event ChangedHandler Changed;

        public TSensor SensorManager;
        public int ReadingCount { get; set; }

        public IBandClient Band { get; } = BandModel.BandClient;

        protected Sensor(TSensor sensor)
        {
            SensorManager = sensor;
        }

        public void Init(TimeSpan reportingInterval)
        {
            SensorManager.ReadingChanged += ReadingChanged;
            SensorManager.ReportingInterval = reportingInterval;
        }

        public async void ReadingChanged(object sender, BandSensorReadingEventArgs<TReading> readingEvent)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                ReadingCount++;
                Changed?.Invoke(readingEvent);
            });
        }

        public void Start()
        {
            SensorManager.StartReadingsAsync();
        }

        public void Stop()
        {
            SensorManager.StopReadingsAsync();
        }
    }
}
