using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using FindMyPWD.Model;
using System.Collections.Generic;
using FindMyPWD.Helper;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPairedDevices : ContentPage
    {
        ObservableCollection<string> BLEDevices = new ObservableCollection<string>();
        public ObservableCollection<string> BLEDevicesCollection
        {
            get
            {
                return BLEDevices;
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

        void updateViewList() 
        {
            List<BLEDevice> PairedDevice = localStorage.getPairedDevice(); //read the Json file
            foreach (BLEDevice Device in PairedDevice) 
            {
                if (!BLEDevices.Contains(Device._name)) //if the device is already in the list don't add it
                {
                    BLEDevices.Add(Device._name);
                }
            }            
        }

    }
}
