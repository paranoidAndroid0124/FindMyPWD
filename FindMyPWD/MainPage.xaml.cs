﻿using System.Linq;
using System.Threading.Tasks;
using static System.Math;
using Xamarin.Forms;
using Xamarin.Essentials;
using FindMyPWD.Helper;
using FindMyPWD.Model;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace FindMyPLWD
{
    public partial class MainPage : ContentPage
    {

        private CurrentDevicePage cdp;
        private readonly BLEScanneHelper BLEHelper;
        public MainPage(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();
            BLEHelper = new BLEScanneHelper();
        }

        public async void Handle_Clicked_Pair(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.ViewPairedDevices()));
        }
        public async void Handle_Clicked_Set_Up(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.SetUpPage(this.cdp)));
        }
        public async void Handle_Clicked_Manage(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(this.cdp));
        }
        public async void Handle_Clicked_Range(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.GeoFencingPage(this.cdp)));
        }
        public async void Open_Map(object sender, System.EventArgs e)
        {
            await Map.OpenAsync(this.cdp.getLatitude(), this.cdp.getLongitude(), new MapLaunchOptions
            {
                NavigationMode = NavigationMode.None
            });
            checkLocation();
            //start making sure device is in range as soon as the map opens
        }
        public async void BLEConfig(object sender, System.EventArgs e)
        {
            string BLE = await BLEHelper.CheckBLE();
            TempLbl.Text = BLE;
        }

        public void DB_Reading(object sender, EventArgs e) 
        {
            DBConnnection.GetDB(); 
            var db = DBConnnection.List;
            TempLbl.Text = db.Count().ToString();
        }
        public void resetJson(object sender, EventArgs e)
        {
            StreamWriter strm = File.CreateText(App.FilePath);
            strm.Flush();
            strm.Close();
            string jsonString = JsonSerializer.Serialize<List<BLEDevice>>(new List<BLEDevice>());
            File.WriteAllText(App.FilePath, jsonString);
            
        }
        public async void setupSafetyZone(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.SetUpSafetyZone(this.cdp)));
        }

        public async void checkLocation()
        {
            while(this.cdp.getConnected() == true) //checks location while connected
            {
                var userLocation = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.High, Timeout = TimeSpan.FromSeconds(60)
                }) ;
                if(this.cdp.getInRange() == true)
                {
                    double Long = userLocation.Longitude;
                    double Lat = userLocation.Latitude;
                    var longDif = userLocation.Longitude - Long;
                    var latDif = userLocation.Latitude - Lat;
                    double range;
                    if(this.cdp.getRange() == 22)
                    {
                        range = .0002;
                    }
                    else if(this.cdp.getRange() == 33)
                    {
                        range = .0003;
                    }
                    else
                    {
                        range = .0004;
                    }

                    if (Sqrt(latDif*latDif + longDif*longDif) > range)
                    {
                        this.cdp.setInRange(false);
                        await DisplayAlert("Warning", "PLWD has left the geofencing range.", "OK");
                    }
                    await Task.Delay(1200000);
                    //wait 20 minutes if in range
                }
                else
                {
                    double Long = userLocation.Longitude;
                    double Lat = userLocation.Latitude;
                    var longDif = userLocation.Longitude - Long;
                    var latDif = userLocation.Latitude - Lat;
                    double range;
                    if (this.cdp.getRange() == 22)
                    {
                        range = .0002;
                    }
                    else if (this.cdp.getRange() == 33)
                    {
                        range = .0003;
                    }
                    else
                    {
                        range = .0004;
                    }

                    if (Sqrt(latDif * latDif + longDif * longDif) < range)
                    {
                        this.cdp.setInRange(true);
                        await DisplayAlert("Alert", "PLWD has entered the geofencing range.", "OK");
                    }
                    await Task.Delay(180000);
                    //wait three minutes if out of range
                }
                //different time periods while in geofence or not
            }
        }

    }
}
