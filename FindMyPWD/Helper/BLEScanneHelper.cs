using System.Collections.ObjectModel;
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
            return ble.State.ToString() == "On";
        }

        private async Task CheckLocPer()
        {
            perStatus = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
        }

        public async Task<string> CheckBLE()
        {
            string result; 
            if (perStatus == PermissionStatus.Denied) //wrong permission
            {
                var response = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
                perStatus = response;
                result = "Please enable loc per";
            }
            else //BLE is on
            {
                result = "BLE is on!!!";
            }
            return result;
        }

        public async Task<ObservableCollection<IDevice>> ScanBLE()
        {
            await CheckLocPer();
            if (BLEStatus() && perStatus == PermissionStatus.Granted) //check if scanning is possible
            {
                adapter.DeviceDiscovered += (s, a) => deviceList.Add(a.Device);
                await adapter.StartScanningForDevicesAsync();
                return deviceList;
            }
            return null;
        }
    }
}
