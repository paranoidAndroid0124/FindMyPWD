//for Bluetooth connection

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanPage : ContentPage
    {
        private MainPage mp;
        private CurrentDevicePage cdp;
        public ScanPage(MainPage mp, CurrentDevicePage cdp)
        {
            this.mp = mp;
            this.cdp = cdp;
            InitializeComponent();
            //will set isConnected to true if found. Send to map once connected.
            //App.Current.MainPage.Navigation.PopAsync();
        }
        //this.cdp.setConnected(true);
        //this.mp.openSet();
    }
}