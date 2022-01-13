//For Bluetooth connection

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetUpScanPage : ContentPage
    {
        private Boolean Connect;
        private CurrentDevicePage cdp;
        public SetUpScanPage(Boolean connecting, CurrentDevicePage cdp)
        {
            this.Connect = connecting;
            InitializeComponent();
        }
        //if connecting (1), set info, send to map, set connected to true, else only send to main
        //this.cdp.setConnected(true);
    }
}