using System;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GravityHero;
using Microsoft.Band.Notifications;
using Microsoft.Band.Sensors;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GravityHeroUWP
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly AccelerometerModel _accelerometerModel = new AccelerometerModel();

        public MainPage()
        {
            InitializeComponent();
        }

        private async void start_Click(object sender, RoutedEventArgs e)
        {
            await BandModel.InitAsync();
            _accelerometerModel.Init();
            _accelerometerModel.Changed += _accelerometerModel_Changed;
        }

        private void _accelerometerModel_Changed(BandSensorReadingEventArgs<IBandAccelerometerReading> reading)
        {
            accel_X.Text = string.Format("X: {0:F3}G", reading.SensorReading.AccelerationX);
            accel_Y.Text = string.Format("Y: {0:F3}G", reading.SensorReading.AccelerationY);
            accel_Z.Text = string.Format("Z: {0:F3}G", reading.SensorReading.AccelerationZ);
        }
    }
}