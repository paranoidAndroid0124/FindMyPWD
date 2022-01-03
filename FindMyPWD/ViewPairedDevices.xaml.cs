using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FindMyPWD.Helper;
using System.Collections.ObjectModel;
using FindMyPWD.Model;
using System.Collections.Generic;
using SQLite;
using Plugin.BLE.Abstractions.Contracts;
using System.Linq;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewPairedDevices : ContentPage
    {
        private readonly BLEScanneHelper BLEHelper;
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

        //there is very similiar code in StartServiceAndroid.cs....maybe put both in a helper class
        void AddDevicesToListview()
        {
            //read the local sqlite db
            List<BLEDevice> results = new List<BLEDevice>();
            //Read the sqlite db to know which devices are paired
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                //check if table exist...TODO: test if this works
                if (conn.TableMappings.Count() > 0) //might be better to check for the exact table
                {
                    results = conn.Table<BLEDevice>().ToList();//return paired devices
                }
            }
            foreach (BLEDevice PairedDevice in results) 
            {
                BLEDevices.Add(PairedDevice._name);
            }
        }
    }
}
