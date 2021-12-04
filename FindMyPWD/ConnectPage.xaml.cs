using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FindMyPWD.Helper;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectPage : ContentPage
    {
        private readonly BLEScanneHelper BLEHelper;
        public ConnectPage()
        {
            InitializeComponent();
            BLEHelper = new BLEScanneHelper();
        }
        void Select(object sender, System.EventArgs e)
        {
            TempLbl.Text = ((Button)sender).Text;
            //send to setupscan if not setup or temp, else just scan
        }

        async void ScanBLE(object sender, System.EventArgs e) 
        {
            var test = await BLEHelper.ScanBLE(sender, e);
            TempLbl.Text = test.ToString();
        }

    }
}
