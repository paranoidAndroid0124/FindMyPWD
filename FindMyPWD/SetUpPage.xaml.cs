using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FindMyPWD.Helper;
using FindMyPWD.Model;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Contracts;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetUpPage : ContentPage
    {
        //private CurrentDevicePage _cdp;
        ObservableCollection<IDevice> BLEscan;
        private readonly BLEScanneHelper BLEHelper;
        ObservableCollection<IDevice> BLEDevices = new ObservableCollection<IDevice>();
        public ObservableCollection<IDevice> BLEDevicesCollection { get { return BLEDevices; } }
        private IDevice pairedDevice;

        public SetUpPage(CurrentDevicePage cdp)
        {
            InitializeComponent();
            BLEHelper = new BLEScanneHelper();
            DeviceView.ItemsSource = BLEDevicesCollection;
        }

        async void Pairing_Clicked(object sender, EventArgs e) //why do I always need to click this twice to work 
        {
            BLEscan = await BLEHelper.ScanBLE();

            for (int i = 0; i < BLEscan.Count(); i++) 
            {
                if (BLEscan[i].Name != null)
                {
                    BLEDevices.Add(BLEscan[i]);
                }
            }

        }

        //get the selected device
        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            TempLbl.Text = "in onSelectedItem method";
            string selection = e.SelectedItem.ToString();
            
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
            savePairedDevice(pairedDevice);
        }

        //save paired device
        void savePairedDevice(IDevice pairedDevice) 
        {
            BLEDevice bLEDevice = new BLEDevice(pairedDevice.Name, pairedDevice.Id.ToString());
            localStorage.write(bLEDevice);
        }

    }
}