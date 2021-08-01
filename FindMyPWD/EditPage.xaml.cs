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
    public partial class EditPage : ContentPage
    {
        private CurrentDevicePage cdp;

        public EditPage(CurrentDevicePage cdp)
        {
            InitializeComponent();
            this.cdp = cdp;
            for (int i = 0; i < 3; i++)
            {
                if (this.cdp.getData(i).getName() == null && this.cdp.getData(i).getAddress() == null && this.cdp.getData(i).getAdditionalInfo() == null)
                {
                    if (i == 0)
                    {
                        ((Button)Device1).IsVisible = false;
                    }
                    else if (i == 1)
                    {
                        ((Button)Device2).IsVisible = false;
                    }
                    else
                    {
                        ((Button)Device3).IsVisible = false;
                    }
                    //checks if the device has been set up to be edited
                }
                else
                {
                    if (i == 0)
                    {
                        ((Button)Device1).Text = cdp.getData(i).getName();
                    }
                    else if (i == 1)
                    {
                        ((Button)Device2).Text = cdp.getData(i).getName();
                    }
                    else
                    {
                        ((Button)Device3).Text = cdp.getData(i).getName();
                    }
                }
            }
        }
        public async void Select(object sender, System.EventArgs e)
        {
            int i = 3;
            if (((Button)sender) == Device1)
            {
                i = 0;
            }
            else if (((Button)sender) == Device2)
            {
                i = 1;
            }
            else if (((Button)sender) == Device3)
            {
                i = 2;
            }
            await Navigation.PushAsync(new FindMyPLWD.EditInfoPage(this.cdp, i));
            //sends button name to editinfopage for header
        }
    }
}