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

[assembly: Dependency(typeof(StartServiceAndroid))] //this registers the service
namespace FindMyPWD.Droid
{
    public class StartServiceAndroid : IStartService
    {
        public void StartForegroundServiceCompat()
        {
            var intent = new Intent(MainActivity.Instance, typeof(scaniningService));


            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                _ = MainActivity.Instance.StartForegroundService(intent);
                //Context.StartForegroundService(intent);
            }
            else
            {
                _ = MainActivity.Instance.StartService(intent);
            }

        }
    }

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

            //scan();

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
    }
}