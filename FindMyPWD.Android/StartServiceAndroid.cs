using Android.App;
using Android.Content;
using Android.OS;
using FindMyPWD.Droid;
using FindMyPWD.Helper;
using FindMyPWD.Interface;
using System;
using Xamarin.Forms;
using FindMyPWD.Model;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using FindMyPWD.Droid.Interface;
using AndroidX.Core.App;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationHelper))]
[assembly: Xamarin.Forms.Dependency(typeof(AndroidServiceHelper))]

namespace FindMyPWD.Droid
{
    internal class NotificationHelper : INotification
    {
        private static string foregroundChannelId = "9001";
        private static Context context = global::Android.App.Application.Context;


        public Notification ReturnNotif()
        {
            // Building intent
            var intent = new Intent(context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.SingleTop);
            intent.PutExtra("Title", "Message");

            var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.UpdateCurrent);

            var notifBuilder = new NotificationCompat.Builder(context, foregroundChannelId)
                .SetContentTitle("Find My PWD")
                .SetContentText("Scanning")
                .SetSmallIcon(Resource.Drawable.abc_ab_share_pack_mtrl_alpha)
                .SetOngoing(false) //testing changing this to false
                .SetContentIntent(pendingIntent);

            // Building channel if API verion is 26 or above
            if (global::Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel(foregroundChannelId, "Title", NotificationImportance.High);
                notificationChannel.Importance = NotificationImportance.Low;
                notificationChannel.EnableLights(false); //don't need to show user
                notificationChannel.EnableVibration(false); //don't need to show user
                notificationChannel.SetShowBadge(false); //don't need to show user
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                var notifManager = context.GetSystemService(Context.NotificationService) as NotificationManager;
                if (notifManager != null)
                {
                    notifBuilder.SetChannelId(foregroundChannelId);
                    notifManager.CreateNotificationChannel(notificationChannel);
                }
            }

            return notifBuilder.Build();
        }
    }

    /*implementation of the shared code*/
    internal class AndroidServiceHelper : IAndroidService
    {
        private static Context context = global::Android.App.Application.Context;

        public void StartService()
        {
        var intent = new Intent(context, typeof(DataSource));

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
        }

        public void StopService()
        {
            var intent = new Intent(context, typeof(DataSource));
            context.StopService(intent);
        }
    }

    /*Registers a foreground service with android and scan BLE*/
    [Service]
    public class DataSource : Service //overide the android service class
    {
        private BLEScanneHelper BLEHelper;
        ObservableCollection<IDevice> BLEscan = new ObservableCollection<IDevice>();
        public const int ServiceRunningNotifID = 9000; //process id of the service
        bool scanning = false; //state of the bluetooth scanner

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        //Start the foreground service
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Notification notif = DependencyService.Get<INotification>().ReturnNotif();
            StartForeground(ServiceRunningNotifID, notif);

            if (intent == null)
            {
                return StartCommandResult.StickyCompatibility;
            }
            else if(intent.Action == "STOPFOREGROUND_ACTION") //kill service if the user demands it
            {
                //TODO: fix the stopping of the service (it might be the condition above)
                StopService(intent);
                return StartCommandResult.StickyCompatibility;
            }
            else
            {
                _ = MainActivity.Instance.StartForegroundService(intent);
            }

            if (!scanning) //check if the service is already started
            {
                //making the scan scanning periodic
                var startTimeSpan = TimeSpan.Zero; //move the variable to top of the function before pr
                var periodTimeSpan = TimeSpan.FromSeconds(10); //10 sec between automatic scans

                var timer = new System.Threading.Timer(async (e) => {
                    await scan();
                }, null, startTimeSpan, periodTimeSpan);

                scanning = true;
            }

            return StartCommandResult.Sticky;
        }
        //scanning code
        private async Task scan()
        {
            List<BLEDevice> devices;
            BLEHelper = new BLEScanneHelper();
            //using (new TimedLock(App.FilePath).Lock(TimeSpan.FromSeconds(10))) { } //thread locking 
            BLEscan = await BLEHelper.ScanBLE();

            if (BLEscan != null)
            {
                Console.WriteLine("BLEScan:" + BLEscan.Count());

                devices = checkPaired(BLEscan); //returns all paired devices found during the scan
                                                //TODO: update the sql db saying you found a pair device aka the watch
                if (devices.Count > 0)
                {
                    Console.WriteLine("found paired device: " + devices.First().ToString());
                }
                else
                {
                    Console.WriteLine("No paired device found");
                    Console.WriteLine("Trying to scan again...");
                    Thread.Sleep(100000);
                    BLEscan = await BLEHelper.ScanBLE();
                    Console.WriteLine("DONE");
                }
            }          
        }

        private List<BLEDevice> checkPaired(ObservableCollection<IDevice> devices)
        {
            //Read the JSON file storing the pairedDevice
            List<BLEDevice> paired = localStorage.getPairedDevice();
            List <BLEDevice> pairedList = new List<BLEDevice>();
            bool deviceFound = false;

            foreach (BLEDevice pairedDevice in paired) 
            {
                deviceFound = devices.Any(x => x.Name == pairedDevice._name);
                if (deviceFound) 
                {
                    pairedList.Add(pairedDevice);
                }
            }
            return pairedList;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        //code to stop the service (currently not in used)
        public override bool StopService(Intent name)
        {
            return base.StopService(name);
        }


    }
}