using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FindMyPWD.Helper;
using FindMyPWD.Model;
using System.Collections.ObjectModel;
using Plugin.BLE.Abstractions.Contracts;
using System.Data.SqlClient;
using System.Data;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetUpPage : ContentPage
    {
        private CurrentDevicePage cdp;
        //private CurrentDevicePage _cdp;
        ObservableCollection<IDevice> BLEscan;
        private readonly BLEScanneHelper BLEHelper;
        ObservableCollection<IDevice> BLEDevices = new ObservableCollection<IDevice>();
        public ObservableCollection<IDevice> BLEDevicesCollection { get { return BLEDevices; } }
        private IDevice pairedDevice;
        
        private string Caregiver_ID;
        private string PLWD_ID;
        private string watchID;

        //This is an object for accessing the SQL database
        DBAccess objdbaccess = new DBAccess();

        DataTable dtWatch = new DataTable();

        public SetUpPage(CurrentDevicePage cdp, string given_Caregiver, string givern_PLWD)
        {
            InitializeComponent();
            BLEHelper = new BLEScanneHelper();
            DeviceView.ItemsSource = BLEDevicesCollection;
            Caregiver_ID = given_Caregiver;
            PLWD_ID = givern_PLWD;
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
            TempLbl.Text = pairedDevice.Id.ToString(); //SEND TO DATABASE
            savePairedDevice(pairedDevice);
        }

        //save paired device
        void savePairedDevice(IDevice pairedDevice) 
        {
            BLEDevice bLEDevice = new BLEDevice(pairedDevice.Name, pairedDevice.Id.ToString());
            localStorage.write(bLEDevice);
            //UPDATE DATABASE HERE
            string deviceName = pairedDevice.Name;
            string deviceMacAddress = pairedDevice.Id.ToString();

            //Creating SQL command for accessing database to create an entry
            SqlCommand insertCommand = new SqlCommand("INSERT INTO Watch (device_name,device__mac_address,plwd_id) VALUES(@device_name,@device_mac_address,@plwd_id)");
            insertCommand.Parameters.AddWithValue("@device_name", deviceName);
            insertCommand.Parameters.AddWithValue("@device_mac_address", deviceMacAddress);
            insertCommand.Parameters.AddWithValue("@plwd_id", PLWD_ID);

            //This will create a new row in the database with the values given 
            int row = objdbaccess.executeQuery(insertCommand);

            //This means the data entry was a sucess 
            if (row == 1)
            {
                //SqlCommand updateCommand = new SqlCommand("UPDATE PLWD SET watch_id='@watch_id' WHERE id = '@plwd_id';");
                DisplayAlert("Sucess", "Watch Paired and Saved", "OK");

                string query3 = "SELECT id FROM Watch WHERE plwd_id='" + PLWD_ID + "'";
                //string watchID; 
                objdbaccess.readDatathroughAdapter(query3, dtWatch);

                //If the query from the database was sucessful and returned the caregiver ID
                if (dtWatch.Rows.Count == 1)
                {
                    watchID = dtWatch.Rows[0]["id"].ToString();

                }

                //SqlCommand updateCommand = new SqlCommand("INSERT INTO Watch (device_name,device__mac_address,plwd_id) VALUES(@device_name,@device_mac_address,@plwd_id)");
                //SqlCommand updateCommand = new SqlCommand("UPDATE PLWD SET watch_id='"+ watchID + "' WHERE id = '" + PLWD_ID +"'");
                SqlCommand updateCommand = new SqlCommand("UPDATE PLWD SET watch_id= @watchID WHERE id = @PLWD_ID");
                updateCommand.Parameters.AddWithValue("@watchID", watchID);
                updateCommand.Parameters.AddWithValue("@PLWD_ID", PLWD_ID);

                int newrow = objdbaccess.executeQuery(updateCommand);

                if (newrow == 1)
                {
                    //This action will enable the user to click the "next" button to proceed to the next phase of setup 
                    Button homeButton = Home;
                    homeButton.IsEnabled = true;
                }

            }

        }

        private void Home_Clicked(object sender, EventArgs e)
        {
            ActiveUser user = new ActiveUser(Caregiver_ID, PLWD_ID, watchID);
            Navigation.PushAsync(new FindMyPLWD.MainPage(this.cdp,user));
        }
    }
}