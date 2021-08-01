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
    public partial class ConnectPage : ContentPage
    {
        public ConnectPage()
        {
            InitializeComponent();
        }
        void Select(object sender, System.EventArgs e)
        {
            TempLbl.Text = ((Button)sender).Text;
            //send to setupscan if not setup or temp, else just scan
        }
    }
}