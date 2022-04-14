using FindMyPWD.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostSafeZoneCreationPage : ContentPage
    {
        private CurrentDevicePage cdp;

        private double long_coord;
        private double lat_coord;
        private string addr_coord;
        private double rad_coord;

        ActiveUser user; 
        public PostSafeZoneCreationPage(CurrentDevicePage cdp, double lon, double lat, string addr, double rad, ActiveUser givenUser)
        {
            this.cdp = cdp;
            InitializeComponent();

            user = givenUser;

            long_coord = lon;
            lat_coord = lat;
            addr_coord = addr;
            rad_coord = rad;

            Console.WriteLine("----------------------INSIDE POST SAFE ZONE--------------------------------------");
            Console.WriteLine(long_coord);
            Console.WriteLine(lat_coord);
            Console.WriteLine(addr_coord);
            Console.WriteLine(rad_coord);
            Console.WriteLine("----------------------INSIDE POST SAFE ZONE--------------------------------------");

            Map map = thisMap;
            //Create a position on the map with these coordinates 
            Position position = new Position(lat_coord, long_coord );
            MapSpan ms = new MapSpan(position, 0.01, 0.01);
            map.MoveToRegion(ms);
            //Create a pin for the map with this label, adress, type, using the position from above 
            Pin pin = new Pin
            {
                Label = addr_coord,
                Address = addr_coord,
                Type = PinType.Place,
                Position = position
            };
            //Add the pin to the map 
            map.Pins.Add(pin);


            //Create a circle with the following properties 
            Circle circle = new Circle
            {
                Center = position,
                //gets the user inputed value and converst to string then to double to then create a Distance object which is in meteres
                Radius = new Distance(rad_coord),
                StrokeColor = Color.FromHex("#88FF0000"),
                StrokeWidth = 8,
                FillColor = Color.FromHex("#88FFC0CB")
            };
            //Add to the map elements 
            map.MapElements.Add(circle);

            
        }

        private void createMoreButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindMyPLWD.SetupSafeZones(this.cdp,user));
        }

        private void homeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FindMyPLWD.MainPage(this.cdp, user));
        }
    }
}