using FindMyPLWD;
using FindMyPWD;
using Xamarin.Forms;

namespace FindMyPLWD
{
    public partial class App : Application
    {
        public static string FilePath;

        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new MainPage(new CurrentDevicePage()));
            MainPage = new NavigationPage(new LandingPage(new CurrentDevicePage()));
        }
        public App(string filePath)
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new MainPage(new CurrentDevicePage()));
            MainPage = new NavigationPage(new LandingPage(new CurrentDevicePage()));

            FilePath = filePath;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
