using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : ContentPage
    {
        IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        IDevice device;
        PermissionStatus perStatus;
        public ConnectPage()
        {
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();

            //set scan mode
            adapter.ScanMode = ScanMode.LowPower;
        }
        void Select(object sender, System.EventArgs e)
        {
            TempLbl.Text = ((Button)sender).Text;
            //send to setupscan if not setup or temp, else just scan
        }
        bool BLEStatus()
        {
            var state = ble.State;
            if (state.ToString() == "On")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task CheckLocPer()
        {
            perStatus = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
        }
        async void ScanBLE(object sender, System.EventArgs e)
        {
            await CheckLocPer();
            if (BLEStatus() && perStatus == PermissionStatus.Granted) //check if scanning is possible
            {
                adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
                await adapter.StartScanningForDevicesAsync();
                for(int i = 0; i < deviceList.Count(); i++)
                {
                    if(deviceList[i].Name != null)
                    {
                        TempLbl.Text += deviceList[i].Name;
                    }
                }
            }
            else //if scanning is not possible
            {
                if (perStatus == PermissionStatus.Denied)
                {
                    var response = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
                    perStatus = response;
                    TempLbl.Text = "Please enable loc per";
                }
                else
                {
                    TempLbl.Text = "BLE is off!!!";
                }
            }

        }
    }
}
