using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private GyroscopeSensor _gyroscopeSensor;
        private HeartRateSensor _heartRateSensor;

        private List<IBandAccelerometerReading> _accelerometerReadings
            = new List<IBandAccelerometerReading>();

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

            UsersViewModel.PointList.Add(new Point { X = DateTime.Now.Second, Y = reading.SensorReading.AccelerationX });
        }

        private void GyroscopeSensorChanged(BandSensorReadingEventArgs<IBandGyroscopeReading> reading)
        {
            angular_X.Text = string.Format("AngularX: {0:F3}{1}", reading.SensorReading.AngularVelocityX,
                GyroscopeSensor.Unit);
            angular_Y.Text = string.Format("AngularY: {0:F3}{1}", reading.SensorReading.AngularVelocityY,
                GyroscopeSensor.Unit);
            angular_Z.Text = string.Format("AngularZ: {0:F3}{1}", reading.SensorReading.AngularVelocityZ,
                GyroscopeSensor.Unit);
        }

        private void HeartRateSensorChanged(BandSensorReadingEventArgs<IBandHeartRateReading> reading)
        {
            heartRate.Text = string.Format("HR: {0:F3} bpm", reading.SensorReading.HeartRate);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var exportFilename = filename.Text;
            var storageFolder =
                ApplicationData.Current.LocalFolder;
            var file =
                await storageFolder.CreateFileAsync(exportFilename,
                    CreationCollisionOption.ReplaceExisting);

            filename.Text = "Loading data";
            await FileIO.AppendTextAsync(file, "time,aX,aY,aZ\n");

            for (int index = 0; index < _accelerometerReadings.Count; index++)
            {
                var reading = _accelerometerReadings[index];

                filename.Text = string.Format("Loading data ({0})", index);

                await
                    FileIO.AppendTextAsync(file,
                        $"{reading.Timestamp.ToUnixTimeMilliseconds()},{reading.AccelerationX},{reading.AccelerationY},{reading.AccelerationZ}\n");
            }

            _accelerometerReadings.Clear();
            filename.Text = "Available here: " + file.Path;
        }
    }

    public class Point

    {

        public double X { get; set; }

        public double Y { get; set; }

    }

    public class UsersViewModel

    {

        public UsersViewModel()

        {
            PointList = new Buffer.SlidingBuffer<Point>(100);
        }

        public static ObservableCollection<Point> PointList

        {

            get; set;

        }

    }
}