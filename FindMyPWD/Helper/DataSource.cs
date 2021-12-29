/*
using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using FindMyPWD;

namespace FindMyPWD.Helper
{
    public class DataSource : Service
    {

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public const int ServiceRunningNotifID = 9000;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            Notification notif = DependencyService.Get<INotification>().ReturnNotif();
            StartForeground(ServiceRunningNotifID, notif);

            _ = DoLongRunningOperationThings();

            return StartCommandResult.Sticky;
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
}
*/