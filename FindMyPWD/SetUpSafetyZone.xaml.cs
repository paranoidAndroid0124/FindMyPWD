using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Forms.Maps.Map;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetUpSafetyZone : ContentPage
    {
        private CurrentDevicePage cdp;
        public SetUpSafetyZone(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();

        }

        public bool IsShowingUser { get; set; }


        async void enterButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var address = addressEntry.Text;
                var locations = await Geocoding.GetLocationsAsync(address);

                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude},-" +
                        $" Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    var tem = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                    addressLabel.Text = address;
                    coordLabel.Text = tem;

                    
                    ShowOnMap(location.Longitude,location.Latitude, address);

                    
                    //if (WihtinRange(x, y, location.Latitude, location.Longitude) == true)
                    //{
                    //    Console.WriteLine("In Range");
                    //}
                    //else { Console.WriteLine("Not In Range"); }

                }
                else
                {
                    addressLabel.Text = "Address not found";
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

        public void ShowOnMap(double lon, double lat, string addr)
        {
            //Create a position on the map with these coordinates 
            Position position = new Position(lon, lat);
            MapSpan ms = new MapSpan(position, 0.1, 0.1);
            Map.MoveToRegion(ms);
            //Create a pin for the map with this label, adress, type, using the position from above 
            Pin pin = new Pin
            {
                Label = addr,
                Address = "Carleton Univerity Ottawa Canada",
                Type = PinType.Place,
                Position = position
            };
            //Add the pin to the map 
            Map.Pins.Add(pin);


            //Create a circle with the following properties 
            Circle circle = new Circle
            {
                Center = position,
                //gets the user inputed value and converst to string then to double to then create a Distance object which is in meteres
                Radius = new Distance(double.Parse(radiusFromPoint.Text)),
                StrokeColor = Color.FromHex("#88FF0000"),
                StrokeWidth = 8,
                FillColor = Color.FromHex("#88FFC0CB")
            };
            //Add to the map elements 
            Map.MapElements.Add(circle);
            //Title for the map page 
            //Title = "Circle demo";
            //Show the map on the page 
            //Content = Map;
            //Open the map at the location specified by position 
            return;
       
        }

        public void WihtinRange(double lon, double lat, double lon2, double lat2)
        {
            Position p = new Position(lon, lat);
            Position p2 = new Position(lon2, lat2);
            Distance diff = Distance.BetweenPositions(p2,p);
            string t = diff.ToString();
            double t2 = double.Parse(t);

            double raduius = double.Parse(radiusFromPoint.Text);
            if (t2 <= raduius)
            {
                Console.WriteLine("In Range");
                //return true;
            }
            else
            {
                Console.WriteLine("Not In Range");
                //return false;
            }
            Console.WriteLine("Not Working");
            //return false; 
            return;
        }

        private void checkButton_Clicked(object sender, EventArgs e)
        {
            double x = 43.642481;
            double y = 79.38605;
            //WihtinRange(x, y, location.Latitude, location.Longitude);
            //double x = 43.642481;
            //double y = 79.38605;
            //bool answer = WihtinRange(x, y);

            //if (WihtinRange(x, y))
            //{
            //    Console.WriteLine("In Range");
            //}
            //else { Console.WriteLine("Not In Range"); }


        }
    }
}