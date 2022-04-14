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
using FindMyPWD.Model;

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

        string address;
        double radius;
        double long_coord;
        double lat_coord;

        ActiveUser user;



        public SetupSafeZones(CurrentDevicePage cdp, ActiveUser givenUser)
        {
            this.cdp = cdp;
            InitializeComponent();
            user = givenUser;
            PLWD_ID = user.getActivePlwdID(); 
        }

        async void saveZoneButton_Clicked(object sender, EventArgs e)
        {
            
            try
            {
                Console.WriteLine("INSIDE BUTTON METHOD ----------------------------------------");
                address = addressEntry.Text;
                radius = double.Parse(safeDistance.Text);
                Console.WriteLine("------------THIS IS THE ADDRESS: " + address);
                Console.WriteLine("INSIDE TRY BLOCK ----------------------------------------");
                var locations = await Geocoding.GetLocationsAsync(address);
                Console.WriteLine("-------------AFTER LOCATIONS, GIVES: " + locations.ToString());
                var location = locations?.FirstOrDefault();

                Console.WriteLine("This IS THE LOCATION !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + location.ToString());
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude},-" + $" Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    long_coord = location.Longitude;
                    lat_coord = location.Latitude;

                    var tem = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";

                    SqlCommand insertCommand = new SqlCommand("INSERT INTO SafeZones (address_name,longitude,latitude,radius,plwd_id) VALUES(@address, @longitude, @latitude, @radius, @plwd_id)");
                    insertCommand.Parameters.AddWithValue("@address", address.ToString());
                    insertCommand.Parameters.AddWithValue("@longitude", location.Longitude.ToString());
                    insertCommand.Parameters.AddWithValue("@latitude", location.Latitude.ToString());
                    insertCommand.Parameters.AddWithValue("@radius", radius.ToString());
                    insertCommand.Parameters.AddWithValue("@plwd_id", PLWD_ID.ToString());

                    //This will create a new row in the database with the values given 
                    int row = objdbaccess.executeQuery(insertCommand);

                    //This means the data entry was a sucess 
                    if (row == 1)
                    {
                        await DisplayAlert("Sucess", "New Zone Created", "OK");
                        Button saveButton = saveZoneButton;
                        saveButton.IsEnabled = false;

                        Button nextbutton = nextButton;
                        nextButton.IsEnabled = true;

                    }

                }
                else
                {
                    DisplayAlert("Error","Address entered was not found. Please try again.", "Ok");
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

        private void nextButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindMyPLWD.PostSafeZoneCreationPage(this.cdp,long_coord, lat_coord, address, radius,user));
        }
    }
}