using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using FindMyPWD.Model;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using FindMyPWD.Helper;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPairedDevices : ContentPage
    {
        ObservableCollection<String> BLEDevices = new ObservableCollection<String>();
        public ObservableCollection<String> BLEDevicesCollection { get { return BLEDevices; } } //this is binded to the viewlist

        public ViewPairedDevices()
        {
            InitializeComponent();
            updateViewList();
            DeviceView.ItemsSource = BLEDevicesCollection;
        }

        //get the selected device
        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            //add code to allow the user to delete a paired device
        }
        void refreshListView(object sender, EventArgs e)
        {
            updateViewList();
        }

        void updateViewList() 
        {
            List<BLEDevice> PairedDevice = localDBConnnection.getPairedDevice(); //read the local sqlite db
            foreach (BLEDevice Device in PairedDevice) 
            {
                if (!BLEDevices.Contains(Device._name.ToString())) //if the device is already in the list don't add it
                {
                    BLEDevices.Add(Device._name);
                }
            }
            //TODO: remove device that were deleted in sqlite
            //maybe there is a way to do it with binding ?
        }

    }
}
