using System;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GravityHero;
using Microsoft.Band.Notifications;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GravityHeroUWP
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly AccelerometerModel _accelerometerModel = new AccelerometerModel();
        private int bandCount;
        private bool isAchievementUnlocked;
        private readonly bool isSecondAchievementUnlocked = false;
        private int maxCount = 10;
        private double maxForce;
        private SpeechSynthesizer synth;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void start_Click(object sender, RoutedEventArgs e)
        {
            await BandModel.InitAsync();
            Reset();
            _accelerometerModel.Init();
            _accelerometerModel.Changed += _accelerometerModel_Changed;
        }

        private void _accelerometerModel_Changed(SensorReading reading)
        {
            accel_X.Text = string.Format("X: {0:F3}G", reading.X);
            accel_Y.Text = string.Format("Y: {0:F3}G", reading.Y);
            accel_Z.Text = string.Format("Z: {0:F3}G", reading.Z);
        }

        private void Reset()
        {
            if (BandModel.BandClient != null)
            {
                var random = new Random();
                maxCount = random.Next(10, 20);
            }
        }
    }
}