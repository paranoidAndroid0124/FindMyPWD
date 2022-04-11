using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace FindMyPWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            // Create a new map 
            Map map = new Map();
            //Create a position on the map with these coordinates 
            Position position = new Position(37.79752, -122.40183);
            //Create a pin for the map with this label, adress, type, using the position from above 
            Pin pin = new Pin
            {
                Label = "Microsoft San Francisco",
                Address = "1355 Market St, San Francisco CA",
                Type = PinType.Place,
                Position = position
            };
            //Add the pin to the map 
            map.Pins.Add(pin);

            //Create a circle with the following properties 
            Circle circle = new Circle
            {
                Center = position,
                Radius = new Distance(250),
                StrokeColor = Color.FromHex("#88FF0000"),
                StrokeWidth = 8,
                FillColor = Color.FromHex("#88FFC0CB")
            };
            //Add to the map elements 
            map.MapElements.Add(circle);
            //Title for the map page 
            Title = "Circle demo";
            //Show the map on the page 
            Content = map;
            //Open the map at the location specified by position 
            map.MoveToRegion(new MapSpan(position, 0.01, 0.01));

            InitializeComponent();
        }
    }
}