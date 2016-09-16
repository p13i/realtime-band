﻿using System;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using GravityHeroUWP.Model;
using Microsoft.Band.Sensors;

namespace GravityHeroUWP.Model
{
    public class AccelerometerSensor : ISensor<IBandSensor<IBandAccelerometerReading>, IBandAccelerometerReading>
    {
        public delegate void ChangedHandler(BandSensorReadingEventArgs<IBandAccelerometerReading> reading);
        public event ChangedHandler Changed;

        private int _readingCount = 0;

        public void Init(TimeSpan reportingInterval)
        {
            if (!BandModel.IsConnected) return;

            BandModel.BandClient.SensorManager.Accelerometer.ReadingChanged += ReadingChanged;
            BandModel.BandClient.SensorManager.Accelerometer.ReportingInterval = reportingInterval;
        }

        public async void ReadingChanged(object sender, BandSensorReadingEventArgs<IBandAccelerometerReading> reading)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                    _readingCount++;
                    Changed?.Invoke(reading);
            });
        }

        public void Start()
        {
            BandModel.BandClient.SensorManager.Accelerometer.StartReadingsAsync();
        }

        public void Stop()
        {
            BandModel.BandClient.SensorManager.Accelerometer.StopReadingsAsync();
        }

        public int GetReadingCount()
        {
            return _readingCount;
        }
    }
}