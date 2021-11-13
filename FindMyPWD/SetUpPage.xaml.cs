using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FindMyPWD.Helper;
using System.Collections.ObjectModel;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetUpPage : ContentPage
    {
        private CurrentDevicePage _cdp;
        private readonly BLEScanneHelper BLEHelper;
        ObservableCollection<String> BLEDevices = new ObservableCollection<String>();
        public ObservableCollection<String> BLEDevicesCollection { get { return BLEDevices; } }
        public SetUpPage(CurrentDevicePage cdp)
        {
            _cdp = cdp;
            InitializeComponent();
            BLEHelper = new BLEScanneHelper();
            for (int i = 0; i < 3; i++)
            {
                if (_cdp.getData(i).getName() != null && _cdp.getData(i).getAddress() != null)
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

            //connect from front end of the list
            DeviceView.ItemsSource = BLEDevicesCollection;
        }
        public async void Dev_select(object sender, System.EventArgs e)
        {
            if (((Button)sender).Text == "Device 1")
            {
                await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.AddDevice(_cdp, 0)));
            }
            else if (((Button)sender).Text == "Device 2")
            {
                await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.AddDevice(_cdp, 1)));
            }
            else if (((Button)sender).Text == "Device 3")
            {
                await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.AddDevice(_cdp, 2)));
            }

        }

        async void Pairing_Clicked(object sender, EventArgs e)
        {
            string[] BLEscan = await BLEHelper.ScanBLE(sender, e);

            foreach(string item in BLEscan) 
            {
                BLEDevices.Add(item);
            }

        }
    }
}