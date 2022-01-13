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
    public partial class SetUpSafetyZone : ContentPage
    {
        private CurrentDevicePage cdp;
        public SetUpSafetyZone(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();
        }
    }
}