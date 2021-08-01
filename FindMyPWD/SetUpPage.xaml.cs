using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetUpPage : ContentPage
    {
        private CurrentDevicePage cdp;
        public SetUpPage(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();
            for (int i = 0; i < 3; i++)
            {
                if (this.cdp.getData(i).getName() != null && this.cdp.getData(i).getAddress() != null)
                {
                    if (i == 0)
                    {
                        ((Button)B1).IsVisible = false;
                    }
                    else if (i == 1)
                    {
                        ((Button)B2).IsVisible = false;
                    }
                    else
                    {
                        ((Button)B3).IsVisible = false;
                    }
                    //checks if the device is available to be setup
                }
            }
        }
        public async void Dev_select(object sender, System.EventArgs e)
        {
            if (((Button)sender).Text == "Device 1")
            {
                await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.AddDevice(this.cdp, 0)));
            }
            else if (((Button)sender).Text == "Device 2")
            {
                await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.AddDevice(this.cdp, 1)));
            }
            else if (((Button)sender).Text == "Device 3")
            {
                await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.AddDevice(this.cdp, 2)));
            }

        }
    }
}