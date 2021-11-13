using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FindMyPWD.Helper;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Contracts;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetUpPage : ContentPage
    {
        private CurrentDevicePage _cdp;
        ObservableCollection<IDevice> BLEscan;
        private readonly BLEScanneHelper BLEHelper;
        ObservableCollection<String> BLEDevices = new ObservableCollection<String>();
        public ObservableCollection<String> BLEDevicesCollection { get { return BLEDevices; } }

        public SetUpPage(CurrentDevicePage cdp)
        {
            _cdp = cdp;
            InitializeComponent();
            BLEHelper = new BLEScanneHelper();
            for (int i = 0; i < 3; i++) //why is this hard coded...do we even need this
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
           BLEscan = await BLEHelper.ScanBLE(sender, e);

            for (int i = 0; i < BLEscan.Count(); i++) 
            {
                if (BLEscan[i].Name != null)
                {
                    BLEDevices.Add(BLEscan[i].Name);
                }
            }

        }

        //get the selected device
        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            TempLbl.Text = "in onSelectedItem method";
            string selection = e.SelectedItem.ToString();
            IDevice pairedDevice;
            //find the selected device
            for (int i = 0; ; i++)
            {
                if (selection == BLEscan[i].Name)
                {
                    pairedDevice = BLEscan[i];
                    break;
                }
            }
            TempLbl.Text = pairedDevice.Id.ToString();
            // get id, name and address
        }
    }
}