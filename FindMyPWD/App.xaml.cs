using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage(new CurrentDevicePage()));
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
