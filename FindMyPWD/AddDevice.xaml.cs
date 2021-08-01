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
    public partial class AddDevice : ContentPage
    {
        private CurrentDevicePage cdp;
        private int deviceNum;

        public AddDevice(CurrentDevicePage cdp, int i)
        {
            this.cdp = cdp;
            this.deviceNum = i;
            if (this.cdp.getConnected() == true)
            {
                ((Button)SetAndScan).IsVisible = false;
            }
            InitializeComponent();
        }

        public async void SetDevice(Object sender, System.EventArgs e)
        {
            if (NameField.Text != null && AddressField.Text != null)
            {
                this.cdp.setNameAtIndex(NameField.Text, this.deviceNum);
                this.cdp.setAddressAtIndex(AddressField.Text, this.deviceNum);
                this.cdp.setAdditionalInfoAtIndex(InfoField.Text, this.deviceNum);
                //sets the info at the index without scanning for the device
                await App.Current.MainPage.Navigation.PopToRootAsync();
            }
            //oly set values if they are changed
        }

        public async void SetDeviceScan(Object sender, System.EventArgs e)
        {
            this.cdp.setNameAtIndex(Name.Text, this.deviceNum);
            this.cdp.setAddressAtIndex(Address.Text, this.deviceNum);
            this.cdp.setAdditionalInfoAtIndex(AdditionalInformation.Text, this.deviceNum);
            //sets the info at index before scanning for the device
            //await Navigation.PushAsync(new App1.SetUpScanPage(true, this.cdp));
        }
    }
}