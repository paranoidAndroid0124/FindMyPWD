using FindMyPWD.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        private CurrentDevicePage cdp;

        DBAccess objdbaccess = new DBAccess();
        //This is a data table that will store data retunred from read operations to the database
        DataTable dtCaregivers = new DataTable();

        DataTable dtPLWD = new DataTable();

        DataTable dtWatch = new DataTable();

        string CaregiverID;
        string plwdID;
        string watchID;

        ActiveUser user; 

        public SignInPage(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();
            //user = new ActiveUser("","","");
        }

        private void enterButton_Clicked(object sender, EventArgs e)
        {
            string caregiverEmail = emailEntry.Text;
            string caregiverPassword = passwordEntry.Text;

            //SELECT id FROM Caregiver WHERE email='John' AND password='Doe'; 

            //Get the id of the caregiver just created to pass to the next part of the set up
            string query = "SELECT id FROM Caregiver WHERE email='" + caregiverEmail + "' AND caregiver_password='" + caregiverPassword + "'";
            objdbaccess.readDatathroughAdapter(query, dtCaregivers);

            //If the query from the database was sucessful and returned the caregiver ID
            if (dtCaregivers.Rows.Count == 1)
            {
                CaregiverID = dtCaregivers.Rows[0]["id"].ToString();

                string query2 = "SELECT id FROM PLWD WHERE caregiver_id='" + CaregiverID + "'";
                objdbaccess.readDatathroughAdapter(query2, dtPLWD);

                //If the query from the database was sucessful and returned the caregiver ID
                if (dtPLWD.Rows.Count == 1)
                {
                    plwdID = dtPLWD.Rows[0]["id"].ToString();

                }

                string query3 = "SELECT id FROM Watch WHERE plwd_id='" + plwdID + "'";
                objdbaccess.readDatathroughAdapter(query3, dtWatch);

                //If the query from the database was sucessful and returned the caregiver ID
                if (dtWatch.Rows.Count == 1)
                {
                    watchID = dtWatch.Rows[0]["id"].ToString();

                }

                user = new ActiveUser(CaregiverID, plwdID, watchID);

                //user.setActiveCaregiverID = CaregiverID;
                //user.setActivePlwdID = plwdID;
                //user.setActiveWatchID = watchID;

                Console.WriteLine("------------------------" + CaregiverID);
                Console.WriteLine("------------------------" + plwdID);
                Console.WriteLine("------------------------" + watchID);

                //CaregiverLabel.Text = CaregiverID;
                //PWLDLabel.Text = plwdID;
                //WatchLabel.Text = watchID;

                Navigation.PushAsync(new FindMyPLWD.MainPage(this.cdp,user));

            }
            else
            {
                DisplayAlert("Error", "Login not found. Please create an account if you do not have one.", "Ok");
                emailEntry.Text = "";
                passwordEntry.Text = "";

            }

        }
    }
}