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

//[assembly: Dependency(typeof(StartServiceAndroid))] //this registers the service
//[assembly: Xamarin.Forms.Dependency(typeof(IStartService))]
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

    [Service]
    public class DataSource : Service
    {
        private BLEScanneHelper BLEHelper;
        ObservableCollection<IDevice> BLEscan = new ObservableCollection<IDevice>();

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public const int ServiceRunningNotifID = 9000;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Notification notif = DependencyService.Get<INotification>().ReturnNotif();
            StartForeground(ServiceRunningNotifID, notif);

            if (intent == null)
            {
                return StartCommandResult.StickyCompatibility;
            }
            else
            {
                _ = MainActivity.Instance.StartForegroundService(intent);
            }

            scan();

            return StartCommandResult.Sticky;
        }
        private async void scan()
        {
            BLEHelper = new BLEScanneHelper();
            BLEscan = await BLEHelper.ScanBLE();
            //checkPaired(BLEscan); //TODO: fix this feature
        }

        private bool checkPaired(ObservableCollection<IDevice> device)
        {
            throw new NotImplementedException();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override bool StopService(Intent name)
        {
            return base.StopService(name);
        }


    }

    /*
    [Service]
    public class scaniningService : Service
    {
        private BLEScanneHelper BLEHelper;
        ObservableCollection<IDevice> BLEscan = new ObservableCollection<IDevice>();

        public override IBinder OnBind(Intent intent)
        {
            //var binder = base.OnBind(intent);
            StopForeground(true);
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            // Code not directly related to publishing the notification has been omitted for clarity.
            // Normally, this method would hold the code to be run when the service is started.

            //Write want you want to do here
            if (intent == null)
            {
                return StartCommandResult.StickyCompatibility;
    }
            else
            {
                _ = MainActivity.Instance.StartForegroundService(intent);
            }

            scan();

            return StartCommandResult.Sticky; //tells the os to restart the service once there is enough ram available
        }

        private async void scan()
        {
            BLEHelper = new BLEScanneHelper();
            BLEscan = await BLEHelper.ScanBLE();
            checkPaired(BLEscan);
        }

        private bool checkPaired(ObservableCollection<IDevice> device)
        {
            throw new NotImplementedException();
        }
    }*/
}