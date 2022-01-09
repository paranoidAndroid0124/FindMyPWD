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
        ObservableCollection<BLEDevice> BLEDevices = new ObservableCollection<BLEDevice>();
        public ObservableCollection<string> BLEDevicesCollection { 
            get 
                {
                    ObservableCollection<string> result = new ObservableCollection<string>();
                    foreach (BLEDevice device in BLEDevices) 
                    {
                        result.Add(device._name);
                    }
                    return result;
                }//this is binded to the viewlist
        } 

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

        //Note: maybe there is a way to do it with binding ?
        void updateViewList() 
        {
            List<BLEDevice> PairedDevice = localDBConnnection.getPairedDevice(); //read the local sqlite db
            BLEDevices = new ObservableCollection<BLEDevice>(); //delete everything ???
            foreach (BLEDevice Device in PairedDevice) 
            {
                if (!BLEDevices.Contains(Device)) //if the device is already in the list don't add it
                {
                    BLEDevices.Add(Device);
                }
            }            
        }

    }
}
