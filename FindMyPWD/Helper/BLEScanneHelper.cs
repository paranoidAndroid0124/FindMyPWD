using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace FindMyPWD.Helper
{
    public class BLEScanneHelper
    {
        public IBluetoothLE ble;
        IAdapter adapter;
        ObservableCollection<IDevice> deviceList;
        IDevice device;
        PermissionStatus perStatus;
        public BLEScanneHelper()
        {
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            deviceList = new ObservableCollection<IDevice>();

            //set scan mode
            adapter.ScanMode = ScanMode.LowPower;
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
        public async Task<string[]> ScanBLE(object sender, System.EventArgs e)
        {
            List<string> list = new List<string>();
            string[] result;
            await CheckLocPer();
            if (BLEStatus() && perStatus == PermissionStatus.Granted) //check if scanning is possible
            {
                adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
                await adapter.StartScanningForDevicesAsync();
                for (int i = 0; i < deviceList.Count(); i++)
                {
                    if (deviceList[i].Name != null)
                    {
                        list.Add(deviceList[i].Name);
                    }
                }
                result = list.ToArray();
                return result;
            }
            else //if scanning is not possible
            {
                if (perStatus == PermissionStatus.Denied)
                {
                    var response = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
                    perStatus = response;
                    list.Add("Please enable loc per");
                }
                else
                {
                    list.Add("BLE is off!!!");
                }
                result = list.ToArray();
                return result;
            }

        }
    }
}
