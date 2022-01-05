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
        public ObservableCollection<String> BLEDevicesCollection { get { return BLEDevices; } }

        public ViewPairedDevices()
        {
            InitializeComponent();
            AddDevicesToListview();
            DeviceView.ItemsSource = BLEDevicesCollection;
        }

        //get the selected device
        void OnSelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            //add code to allow the user to delete a paired device
        }

        void AddDevicesToListview()
        {
            //read the local sqlite db
            var results = localDBConnnection.getPairedDevice();
            foreach (BLEDevice PairedDevice in results) 
            {
                BLEDevices.Add(PairedDevice._name);
            }
        }
    }
}
