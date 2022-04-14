using FindMyPLWD;
using FindMyPWD.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace FindMyPWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        private CurrentDevicePage cdp;
        public LandingPage(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();
        }

        public void signUpButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindMyPLWD.UserSetup(this.cdp));

        }

        private void signInButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindMyPLWD.SignInPage(this.cdp));
        }

        private void tempHomeButton_Clicked(object sender, EventArgs e)
        {
            ActiveUser user = new ActiveUser("","","");
            Navigation.PushAsync(new FindMyPLWD.MainPage(this.cdp, user));

        }
    }
}