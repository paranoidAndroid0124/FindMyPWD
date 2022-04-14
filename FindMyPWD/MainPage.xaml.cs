using System.Linq;
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
        ActiveUser user; 

        private CurrentDevicePage cdp;
        private readonly BLEScanneHelper BLEHelper;
        public MainPage(CurrentDevicePage cdp, ActiveUser usergiven)
        {
            this.cdp = cdp;
            InitializeComponent();
            BLEHelper = new BLEScanneHelper();
            user = usergiven;
            
        }

        public async void Handle_Clicked_Pair(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.ViewPairedDevices()));
        }
        public async void Handle_Clicked_Set_Up(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.SetUpPage(this.cdp,"","")));
        }
        public async void Handle_Clicked_UserSetup(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.UserSetup(this.cdp)));
        }
        private async void NotificationPage_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.NotificationTestPage(this.cdp)));
        }
        
        public async void Safe_Zones(object sender, System.EventArgs e)
        {
            
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.SetupSafeZones(this.cdp, user)));
        }

        public void resetJson(object sender, EventArgs e)
        {
            StreamWriter strm = File.CreateText(App.FilePath);
            strm.Flush();
            strm.Close();
            string jsonString = JsonSerializer.Serialize<List<BLEDevice>>(new List<BLEDevice>());
            File.WriteAllText(App.FilePath, jsonString);
            
        }

        private async void CheckPLWDButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new FindMyPLWD.CheckPLWDPage(this.cdp,user)));
        }
    }

}
