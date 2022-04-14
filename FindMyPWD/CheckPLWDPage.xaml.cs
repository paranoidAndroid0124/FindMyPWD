using FindMyPWD.Model;
using System;
using System.Data;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using FindMyPWD.Helper;
using System.Threading;
using System.Threading.Tasks;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckPLWDPage : ContentPage
    {
        private CurrentDevicePage cdp;
        ActiveUser user;
        DBAccess objdbaccess = new DBAccess();
        //This is a data table that will store data retunred from read operations to the database
        DataTable dtLocations = new DataTable();
        DataTable dtDistance= new DataTable();
        DataTable dtSafeZones = new DataTable();
        bool[] testResults = new bool[100];

        public CheckPLWDPage(CurrentDevicePage cdp, ActiveUser userGiven)
        {
            this.cdp = cdp;
            InitializeComponent();
            user = userGiven;
        }

        private void checkOnPlwdButton_Clicked(object sender, System.EventArgs e)
        {
            label1.IsVisible = true;
            label3.IsVisible = true;
            label5.IsVisible = true;
            //First pull lastest location update from database

            string query = "SELECT TOP 1 * FROM LocationLogs ORDER BY ID DESC";
            objdbaccess.readDatathroughAdapter(query, dtLocations);
            //If the query from the database was sucessful and returned the caregiver ID
            if (dtLocations.Rows.Count == 1)
            {
                string temp1 = dtLocations.Rows[0]["longitude"].ToString();
                string temp2 = dtLocations.Rows[0]["latitude"].ToString();
                label2.Text = dtLocations.Rows[0]["address_name"].ToString();

                double long_coord = double.Parse(temp1);
                double lat_coord = double.Parse(temp2);

                //This is the latest location of the PLWD taken from the watch
                Location logPosition = new Location(lat_coord, long_coord);
                Console.WriteLine("------------------------ LOG POSITION :" + logPosition);

                //Calling this function would use geolocation to get the devices current location. However this doesn't work for testing with the phone simulator becuase it is set to a Microsft Office building
                //GetCurrentLocation();

                //Going to use Carleton University as the testing position
                //Location caregiverCurrentPosition = new Location(45.3876, -75.6960);
                Location caregiverCurrentPosition = new Location(45.3858, -75.6775);
                label3_1.Text = "Latitude: " + caregiverCurrentPosition.Latitude.ToString() + " Longitude: "+ caregiverCurrentPosition.Longitude.ToString();
                Console.WriteLine("------------------------ PHONE POSITION :" + caregiverCurrentPosition);

                //This is going to calculate the distance between the PLWD and the caregiver current location 
                //Note this doesn't take roads into account. It is simply the shortest distance between the two points. 
                double distanceFromCaregiver = Location.CalculateDistance(logPosition, caregiverCurrentPosition, DistanceUnits.Kilometers);
                Console.WriteLine("------------------------ DISTANCE FROM CAREGIVER " + distanceFromCaregiver);

                string plwd_id = user.getActivePlwdID();
                string caregiver_id = user.getActiveCaregiverID();

                string allowedDistanceQuery = "SELECT distance_from_caregiver FROM PLWD WHERE id='" + plwd_id + "' AND caregiver_id='" + caregiver_id + "'";
                string alloweedDistance;
                objdbaccess.readDatathroughAdapter(allowedDistanceQuery, dtDistance);
                if (dtDistance.Rows.Count == 1)
                {
                    alloweedDistance = dtDistance.Rows[0]["distance_from_caregiver"].ToString();
                    double alloweedDistanceValue = double.Parse(alloweedDistance) / 1000; 
                    Console.WriteLine("------------------------ ALLOWED DISTANCE " + alloweedDistanceValue);
                    if (distanceFromCaregiver <= alloweedDistanceValue)
                    {
                        resultLabel.Text = "PLWD Near Caregiver";
                        resultLabel.TextColor = Color.Green; 
                    }
                    else 
                    {
                        //Since the PLWD is not near the caregiver we now have to check if they are in any of the predefined safe zones

                        string allSafeZonesQuery = "Select * FROM SafeZones where plwd_id='" + plwd_id + "'";
                        objdbaccess.readDatathroughAdapter(allSafeZonesQuery, dtSafeZones);
                        int rows = dtSafeZones.Rows.Count;
                        if (rows >= 1)
                        {
                            for (int i=0; i < rows; i++)
                            {
                                Console.WriteLine("-------------------------HERE" + dtSafeZones.Rows[i]["id"].ToString());

                                Location temp = new Location(double.Parse(dtSafeZones.Rows[i]["latitude"].ToString()), double.Parse(dtSafeZones.Rows[i]["longitude"].ToString()));
                                double tempDistanceBetweenPoints = Location.CalculateDistance(logPosition, temp, DistanceUnits.Kilometers);
                                Console.WriteLine("-------------------------HERE tempDistanceBetweenPoints" + tempDistanceBetweenPoints);
                                double tempalloweedDistanceValue = double.Parse(dtSafeZones.Rows[i]["radius"].ToString()) / 1000;
                                Console.WriteLine("-------------------------HERE tempalloweedDistanceValue" + tempalloweedDistanceValue);

                                if (tempDistanceBetweenPoints <= tempalloweedDistanceValue)
                                {
                                    testResults[i] = true;
                                    Console.WriteLine("-------------------------PASS");
                                    Console.WriteLine("-------------------------PLWD IS IN SAFE ZOME");

                                    resultLabel.Text = "PLWD away from Caregiver but in SafeZones!";
                                    resultLabel.TextColor = Color.Green;

                                    return;
                                }
                                else
                                {
                                    testResults[i] = true;
                                    Console.WriteLine("-------------------------Fail");
                                }

                                
                            }

                            resultLabel.Text = "PLWD is away from Caregvier and not in SafeZones. LOST";
                            resultLabel.TextColor = Color.Red;

                            Console.WriteLine("-------------------------OUTSIDE FOR LOOP NOW PLWD IS LOST!!!!!!!!!!!!!!");

                        }
                    }

                }

                Console.WriteLine("------------------------ longitude: " + long_coord);
                Console.WriteLine("------------------------ longitude: " + lat_coord);
            }
        }

        CancellationTokenSource cts;

        //This is the way to get the device current location
        async Task GetCurrentLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }
    }
}