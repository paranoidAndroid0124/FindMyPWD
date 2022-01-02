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
using SQLite;
using FindMyPLWD;
using System.Collections.ObjectModel;
using FindMyPWD.Droid.Interface;
using AndroidX.Core.App;
using System.Linq;
using System.Collections.Generic;

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

            //making the scan scanning periodic
            var startTimeSpan = TimeSpan.Zero; //move the variable to top of the function before pr
            var periodTimeSpan = TimeSpan.FromSeconds(300); //5 minutes between automatic scans

            var timer = new System.Threading.Timer((e) =>
            {
                scan();
            }, null, startTimeSpan, periodTimeSpan);

            return StartCommandResult.Sticky;
        }
        //scanning code
        private async void scan()
        {
            List<IDevice> devices;
            BLEHelper = new BLEScanneHelper();
            BLEscan = await BLEHelper.ScanBLE();
            if (BLEscan.Count > 0) //delete this after testing
            {
                Console.WriteLine("BLEScan:" + BLEscan.Count());
            }          
            devices = checkPaired(BLEscan); //returns all paired devices found during the scan
            //TODO: update the sql db saying you found a pair device aka the watch
            if (devices.Count > 0)
            {
                Console.WriteLine("found paired device: " + devices.First().ToString());
            }
            else 
            {
                Console.WriteLine("No paired device found");
            }
        }

        private List<IDevice> checkPaired(ObservableCollection<IDevice> devices)
        {
            List<BLEDevice> results;
            //Read the sqlite db to know which devices are paired
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                //check if table exist...TODO: test if this works
                if (conn.TableMappings.Count() > 0) //might be better to check for the exact table
                {
                    results = conn.Table<BLEDevice>().ToList();//return paired devices
                }
                else 
                {
                    //there is no paired devices
                    return new List<IDevice>(); //return an empty list
                }
            }
            return devices.Where(device =>
            {
                var match = results.Select( x => x._name == device.Name);
                return match != null;
            }).ToList();
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