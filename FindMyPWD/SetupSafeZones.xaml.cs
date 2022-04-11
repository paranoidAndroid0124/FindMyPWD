using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using System.Data.SqlClient;


namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupSafeZones : ContentPage
    {
        private CurrentDevicePage cdp;
        //This is an object for accessing the SQL database
        DBAccess objdbaccess = new DBAccess();
        //This is a data table that will store data retunred from read operations to the database
        DataTable dtPLWD = new DataTable();
        string PLWD_ID;
        public SetupSafeZones(CurrentDevicePage cdp, string ID)
        {
            this.cdp = cdp;
            InitializeComponent();
            PLWD_ID = ID; 
        }

        private async void saveZoneButton_Clicked(object sender, EventArgs e)
        {
            string address = addressEntry.Text;
            double radius = double.Parse(safeDistance.Text);

            try
            {
                var locations = await Geocoding.GetLocationsAsync(address);

                var location = locations?.FirstOrDefault();
                Console.WriteLine("This IS THE LOCATION !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + location);
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude},-" + $" Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                                        
                    var tem = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                    Temp1.Text = location.ToString();
                    Temp2.Text = tem; 

                    SqlCommand insertCommand = new SqlCommand("INSERT INTO SafeZones (address_name,longitude,latitude,radius,plwd_id) VALUES(@address, @longitude, @latitude, @radius, @plwd_id)");
                    insertCommand.Parameters.AddWithValue("@address", address);
                    insertCommand.Parameters.AddWithValue("@longitude", location.Longitude);
                    insertCommand.Parameters.AddWithValue("@latitude", location.Latitude);
                    insertCommand.Parameters.AddWithValue("@radius", radius);
                    insertCommand.Parameters.AddWithValue("@plwd_id", PLWD_ID);

                    //This will create a new row in the database with the values given 
                    int row = objdbaccess.executeQuery(insertCommand);

                    //This means the data entry was a sucess 
                    if (row == 1)
                    {
                        DisplayAlert("Sucess", "New Zone Created", "OK");

                    }

                }
                else
                {
                    Temp1.Text = "Address not found";
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }

        }
    }
}