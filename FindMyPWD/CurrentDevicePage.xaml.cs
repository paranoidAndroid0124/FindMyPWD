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
    public partial class CurrentDevicePage : ContentPage
    {
        private DeviceData[] dd;
        private Boolean Connected = false; //default false
        private int batteryLife;
        private int geoFenceRange;
        private double lat; //latitude
        private double lon; //longitude
        private Boolean inRange;
        public CurrentDevicePage()
        {
            InitializeComponent();
            this.dd = new DeviceData[3];
            this.dd[0] = new DeviceData();
            this.dd[1] = new DeviceData();
            this.dd[2] = new DeviceData();
            //Doesn't have to be 3
            //sets each data to the null set of data
            ((Button)Remove).IsVisible = true;
            ((Button)Edit).IsVisible = true;
            ((Button)Disconnect).IsVisible = false;
            this.inRange = true; //assume in range when first set up
            this.geoFenceRange = 22; //set default of 22 m, or .0002 coordinate degrees
            this.lat = 45.3876;
            this.lon = -75.6960; // default coordinates are Carleton University
        }

        public DeviceData getData(int index)
        {
            return this.dd[index];
        }
        public DeviceData[] getData()
        {
            return this.dd;
        }

        public int getRange()
        {
            return this.geoFenceRange;
        }
        public double getLongitude()
        {
            return this.lon;
        }

        public double getLatitude()
        {
            return this.lat;
        }

        public void setNameAtIndex(String name, int index)
        {
            if (index < 0 || index > 2) { return; }
            this.dd[index].setName(name);
        }
        public void setAddressAtIndex(String address, int index)
        {
            if (index < 0 || index > 2) { return; }
            this.dd[index].setAddress(address);
        }
        public void setAdditionalInfoAtIndex(String additionalInfo, int index)
        {
            if (index < 0 || index > 2) { return; }
            this.dd[index].setAdditionalInfo(additionalInfo);
        }

        public void setRange(int range)
        {
            this.geoFenceRange = range;
        }
        /*
         * public void setCellNumAtIndex(int cellNum, int index)
         * {
         * this.dd[index].setCellNum(cellNum);
         * }
         * 
         * public void setBluetoothNumAtIndex(int bluetoothNum, int index)
         * {
         * this.dd[index].setBluetoothNum(bluetoothNum);
         * }
         */

        public void setConnected(Boolean isConnected)
        {
            this.Connected = isConnected;
            if (this.Connected == true)
            {
                ((Button)Remove).IsVisible = false;
                ((Button)Edit).IsVisible = false;
                ((Button)Disconnect).IsVisible = true;
            }
            else if (this.Connected == false)
            {
                ((Button)Remove).IsVisible = true;
                ((Button)Edit).IsVisible = true;
                ((Button)Disconnect).IsVisible = false;
            }
        }
        public Boolean getConnected()
        {
            return this.Connected;
        }

        public Boolean  getInRange()
        {
            return this.inRange;
        }
        public void setInRange(Boolean InRange)
        {
            this.inRange = InRange;
        }


        public async void Edit_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FindMyPLWD.EditPage(this));
        }
        public async void Remove_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FindMyPLWD.RemovePage(this));
        }

        public async void Disconnect_From_Device(object sender, System.EventArgs e)
        {
            this.setConnected(false);
            await App.Current.MainPage.Navigation.PopToRootAsync();
        }

        public async void View_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new FindMyPLWD.ViewPage(this));
        }
    }
}