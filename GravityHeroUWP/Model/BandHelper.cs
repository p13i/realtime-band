using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Band;
using Microsoft.Band.Notifications;

namespace GravityHero
{
    public class BandModel
    {
        public static IBandInfo SelectedBand { get; set; }

        public static IBandClient BandClient { get; set; }


        public static bool IsConnected
        {
            get { return BandClient != null; }
        }

        public static async Task FindDevicesAsync()
        {
            var bands = await BandClientManager.Instance.GetBandsAsync();
            if ((bands != null) && (bands.Length > 0))
                SelectedBand = bands[0]; // take the first band
        }

        public static async Task InitAsync()
        {
            try
            {
                if (IsConnected)
                    return;

                await FindDevicesAsync();
                if (SelectedBand != null)
                {
                    BandClient = await BandClientManager.Instance.ConnectAsync(SelectedBand);
                    // connected!
                    await BandClient.NotificationManager.VibrateAsync(VibrationType.NotificationAlarm);
                }
            }
            catch (Exception x)
            {
                Debug.WriteLine(x.Message);
            }
        }
    }
}