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
    public partial class EditInfoPage : ContentPage
    {
        private CurrentDevicePage cdp;
        private int deviceNum;

        public EditInfoPage(CurrentDevicePage cdp, int i)
        {
            InitializeComponent();
            this.cdp = cdp;
            this.deviceNum = i;
            DeviceLabel.Text = this.cdp.getData(i).getName();
            NameField.Text = this.cdp.getData(i).getName();
            AddressField.Text = this.cdp.getData(i).getAddress();
            if (this.cdp.getData(i).getAdditionalInfo() != null)
            {
                InfoField.Text = this.cdp.getData(i).getAdditionalInfo();
                //only add additional info if there is any
            }

        }

        public async void SaveChanges(Object sender, System.EventArgs e)
        {
            this.cdp.setNameAtIndex(NameField.Text, this.deviceNum);
            this.cdp.setAddressAtIndex(AddressField.Text, this.deviceNum);
            this.cdp.setAdditionalInfoAtIndex(InfoField.Text, this.deviceNum);
            //sets the info at the index without scanning for the device
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

    }
}