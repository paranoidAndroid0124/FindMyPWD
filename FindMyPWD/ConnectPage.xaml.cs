using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
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
        public ConnectPage()
        {
            InitializeComponent();
            ble = CrossBluetoothLE.Current;
        }
        void Select(object sender, System.EventArgs e)
        {
            TempLbl.Text = ((Button)sender).Text;
            //send to setupscan if not setup or temp, else just scan
        }
        async void ScanBLE(object sender, System.EventArgs e)
        {
            adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
            await adapter.StartScanningForDevicesAsync();
        }
    }
}