using System;
using System.Linq;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Band.Notifications;
using Microsoft.Band.Sensors;
using SensorStream.Models;
using SensorStream.Models.Sensors;

namespace SensorStream
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private AccelerometerSensor _accelerometerSensor;
        private HeartRateSensor _heartRateSensor;
        private GyroscopeSensor _gyroscopeSensor;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void start_Click(object sender, RoutedEventArgs e)
        {
            Dispose();

            await BandModel.Init();

            _accelerometerSensor = new AccelerometerSensor();
            _heartRateSensor = new HeartRateSensor();
            _gyroscopeSensor = new GyroscopeSensor();

            _accelerometerSensor.Init(TimeSpan.FromMilliseconds(16.0));
            _accelerometerSensor.Changed += AccelerometerSensorModelChanged;
            _accelerometerSensor.Start();

            _heartRateSensor.Init(BandModel.BandClient.SensorManager.HeartRate.SupportedReportingIntervals.First());
            _heartRateSensor.Changed += HeartRateSensorChanged;
            _heartRateSensor.Start();

            _gyroscopeSensor.Init(BandModel.BandClient.SensorManager.Gyroscope.SupportedReportingIntervals.First());
            _gyroscopeSensor.Changed += GyroscopeSensorChanged;
            _gyroscopeSensor.Start();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            Dispose();
        }

        private void Dispose()
        {
            _accelerometerSensor?.Stop();
            _heartRateSensor?.Stop();
            _gyroscopeSensor?.Stop();
        }

        private void AccelerometerSensorModelChanged(BandSensorReadingEventArgs<IBandAccelerometerReading> reading)
        {
            accel_X.Text = string.Format("X: {0:F3}G", reading.SensorReading.AccelerationX);
            accel_Y.Text = string.Format("Y: {0:F3}G", reading.SensorReading.AccelerationY);
            accel_Z.Text = string.Format("Z: {0:F3}G", reading.SensorReading.AccelerationZ);
        }

        private void GyroscopeSensorChanged(BandSensorReadingEventArgs<IBandGyroscopeReading> reading)
        {
            angular_X.Text = string.Format("AngularX: {0:F3}{1}", reading.SensorReading.AngularVelocityX, GyroscopeSensor.Unit);
            angular_Y.Text = string.Format("AngularY: {0:F3}{1}", reading.SensorReading.AngularVelocityY, GyroscopeSensor.Unit);
            angular_Z.Text = string.Format("AngularZ: {0:F3}{1}", reading.SensorReading.AngularVelocityZ, GyroscopeSensor.Unit);
        }

        private void HeartRateSensorChanged(BandSensorReadingEventArgs<IBandHeartRateReading> reading)
        {
            heartRate.Text = string.Format("HR: {0:F3} bpm", reading.SensorReading.HeartRate);
        }
    }
}