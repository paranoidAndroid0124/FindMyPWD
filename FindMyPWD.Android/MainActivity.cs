using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FindMyPLWD;
using System.IO;
using Android.Content;
using Xamarin.Forms;
using FindMyPWD.Interface;
using Plugin.Permissions;
using FindMyPWD.Helper;
using Android;

namespace FindMyPWD.Droid
{
    [Activity(Label = "FindMyPWD", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        
        public static Context Instance = Android.App.Application.Context;
        //StartServiceAndroid scanService = new StartServiceAndroid();

        const int RequestLocationId = 0;
        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };


        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            //storage of paired devices
            string fileName = "device_db.db3";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);//this is specific to androind...ios needs a specific file path
            string completePath = Path.Combine(folderPath, fileName);


            //call method to start service
            DependencyService.Get<IAndroidService>().StartService();

            LoadApplication(new App(completePath));
        }

        protected override void OnStart()
        {
            base.OnStart();
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }

        //This function allows to prompt the user for permission
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == RequestLocationId)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
                {
                    // Permissions granted - display a message.
                }
                else
                {
                    // Permissions denied - display a message.
                }
            }
            else
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
                  
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
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
    }
}