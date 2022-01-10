using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using FindMyPLWD;
using System.IO;
using Android.Content;
using Xamarin.Forms;
using FindMyPWD.Interface;

namespace FindMyPWD.Droid
{
    [Activity(Label = "FindMyPWD", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity {
        
        public static Context Instance = Android.App.Application.Context;
        //StartServiceAndroid scanService = new StartServiceAndroid();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            //storage of paired devices
            string fileName = "device_db.json";
            string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);//this is specific to androind...ios needs a specific file path
            string completePath = Path.Combine(folderPath, fileName);


            //call method to start service
            DependencyService.Get<IAndroidService>().StartService();

            LoadApplication(new App(completePath));
        }

        protected override void OnStart()
        {
            base.OnStart();

        }

        //This function allows to prompt the user for permission
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

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