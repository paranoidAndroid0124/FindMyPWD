using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    public partial class App : Application
    {
        public static string FilePath;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage(new CurrentDevicePage()));
        }
        public App(string filePath)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage(new CurrentDevicePage()));

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
