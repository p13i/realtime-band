using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Band;
using Microsoft.Band.Notifications;

namespace SensorStream.Models
{
    public class BandModel
    {
        public static IBandInfo SelectedBand { get; set; }

        public static IBandClient BandClient { get; set; }


        public static bool IsConnected => BandClient != null;

        public static async Task FindDevicesAsync()
        {
            var bands = await BandClientManager.Instance.GetBandsAsync();
            if ((bands != null) && (bands.Length > 0))
                SelectedBand = bands[0]; // take the first band
        }

        public static async Task Init()
        {
            if (IsConnected)
                return;

            try
            {
                await FindDevicesAsync();
                if (SelectedBand != null)
                {
                    // Connect
                    BandClient = await BandClientManager.Instance.ConnectAsync(SelectedBand);
                    await BandClient.NotificationManager.VibrateAsync(VibrationType.NotificationAlarm);
                }
            }
            catch (BandException x)
            {
                Debug.WriteLine(x.Message);
            }
        }
    }
}