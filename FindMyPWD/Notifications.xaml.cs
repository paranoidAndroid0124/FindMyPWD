using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using Xamarin.Essentials;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeoFencingPage : ContentPage
    {
        private CurrentDevicePage cdp;
        public GeoFencingPage(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();

            NotificationCenter.Current.NotificationTapped += Current_NotificationTapped;

        }

        private void Current_NotificationTapped(NotificationEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
           {
               await Launcher.OpenAsync("https://www.google.com/maps/dir/Carleton+University,+1125+Colonel+By+Dr,+Ottawa,+ON+K1S+5B6/University+of+Ottawa+Roger+Guindon+Hall,+Smyth+Road,+Ottawa,+ON/@45.3943972,-75.6904727,14z/data=!3m1!4b1!4m13!4m12!1m5!1m1!1s0x4cce05d8d37fecc3:0xbf5d8d7821b8dcdc!2m2!1d-75.6960202!2d45.3875812!1m5!1m1!1s0x4cce0f63c2b611ff:0xcdc5141867f59d77!2m2!1d-75.6504821!2d45.4024124");
           });

        }

        private void SampleNotification_Clicked(object sender, System.EventArgs e)
        {
            var notification = new NotificationRequest
            {
                BadgeNumber = 1,
                Title = "Attention Required",
                Description = "Person out of range of caregivers. Click to view location!",
                NotificationId = 1,
                Schedule =
                {
                    NotifyTime = System.DateTime.Now.AddSeconds(5)
                }
                


            };
            NotificationCenter.Current.Show(notification);
        }
        
    }
}