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
    public partial class GeoFencingPage : ContentPage
    {
        private CurrentDevicePage cdp;
        public GeoFencingPage(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();
        }
        public async void Clicked_22(object sender, System.EventArgs e)
        {
            this.cdp.setRange(22);
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }
        public async void Clicked_33(object sender, System.EventArgs e)
        {
            this.cdp.setRange(33);
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }
        public async void Clicked_44(object sender, System.EventArgs e)
        {
            this.cdp.setRange(44);
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }
        //
    }
}