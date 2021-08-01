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
    public partial class ViewPage : ContentPage
    {
        private CurrentDevicePage cdp;
        public ViewPage(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();
            if (this.cdp.getData(0).getName() != null)
            {
                D1Name.Text = this.cdp.getData(0).getName();
            }
            else
            {
                D1Name.Text = "N/A";
            }

            if (this.cdp.getData(0).getAddress() != null)
            {
                D1Address.Text = this.cdp.getData(0).getAddress();
            }
            else
            {
                D1Address.Text = "N/A";
            }

            if (this.cdp.getData(0).getAdditionalInfo() != null)
            {
                D1Info.Text = this.cdp.getData(0).getAdditionalInfo();
            }
            else
            {
                D1Info.Text = "N/A";
            }

            if (this.cdp.getData(1).getName() != null)
            {
                D2Name.Text = this.cdp.getData(1).getName();
            }
            else
            {
                D2Name.Text = "N/A";
            }

            if (this.cdp.getData(1).getAddress() != null)
            {
                D2Address.Text = this.cdp.getData(1).getAddress();
            }
            else
            {
                D2Address.Text = "N/A";
            }

            if (this.cdp.getData(1).getAdditionalInfo() != null)
            {
                D2Info.Text = this.cdp.getData(1).getAdditionalInfo();
            }
            else
            {
                D2Info.Text = "N/A";
            }

            if (this.cdp.getData(2).getName() != null)
            {
                D3Name.Text = this.cdp.getData(2).getName();
            }
            else
            {
                D3Name.Text = "N/A";
            }

            if (this.cdp.getData(2).getAddress() != null)
            {
                D3Address.Text = this.cdp.getData(2).getAddress();
            }
            else
            {
                D3Address.Text = "N/A";
            }

            if (this.cdp.getData(2).getAdditionalInfo() != null)
            {
                D3Info.Text = this.cdp.getData(2).getAdditionalInfo();
            }
            else
            {
                D3Info.Text = "N/A";
            }

        }
    }
}