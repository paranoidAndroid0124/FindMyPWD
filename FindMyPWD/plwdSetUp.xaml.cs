using System;
using System.Data;
using System.Data.SqlClient;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FindMyPLWD
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class plwdSetUp : ContentPage
    {
        private CurrentDevicePage cdp;
        //This is the caregiver associated with the PLWD that is going to be created 
        private string CaregiverID;
        //This is an object for accessing the SQL database
        DBAccess objdbaccess = new DBAccess();
        //This is a data table that will store data retunred from read operations to the database
        DataTable dtPLWD = new DataTable();
        
        String PLWD_ID;
        public plwdSetUp(CurrentDevicePage cdp, String ID)
        {
            this.cdp = cdp;
            InitializeComponent();

            //the caregiver ID is sent from the previous view where the caregiver was set up 
            CaregiverID = ID;
        }
       

        private void sendToDatabase_Clicked(object sender, EventArgs e)
        {
            //this is saving the user data from the fields 
            string plwdFirstName = plwdFirstNameEntry.Text;
            string plwdLastName = plwdLastNameEntry.Text;
            string plwdDistance = plwdDistanceEntry.Text;

            //Checks to ensure all the fields are filled out correctly
            if (plwdFirstName.Equals(null))
            {
                DisplayAlert("Alert", "You must enter a Caregiver First Name", "OK");
            }
            else if (plwdLastName.Equals(null))
            {
                DisplayAlert("Alert", "You must enter a Caregiver Last Name", "OK");
            }
            else if (plwdDistance.Equals(null))
            {
                DisplayAlert("Alert", "You must enter a safe distance from Caregiver", "OK");
            }
            else
            {
                //At this point all the fields are filled out correctly

                //Creating SQL command for accessing database to create an entry
                SqlCommand insertCommand = new SqlCommand("INSERT INTO PLWD (firstname,lastname,caregiver_id,watch_id,distance_from_caregiver) VALUES(@plwdFirstName,@plwdLastName,@caregiver_id,@watch_id,@distance_from_caregiver)");
                insertCommand.Parameters.AddWithValue("@plwdFirstName", plwdFirstName);
                insertCommand.Parameters.AddWithValue("@plwdLastName", plwdLastName);
                insertCommand.Parameters.AddWithValue("@caregiver_id", CaregiverID);
                insertCommand.Parameters.AddWithValue("@watch_id", 0);
                insertCommand.Parameters.AddWithValue("@distance_from_caregiver", plwdDistance);

                //This will create a new row in the database with the values given 
                int row = objdbaccess.executeQuery(insertCommand);

                //This means the data entry was a sucess 
                if (row == 1)
                {
                    DisplayAlert("Sucess", "PLWD " + plwdFirstName + " " + plwdLastName + " created", "OK");

                    //This action will enable the user to click the "next" button to proceed to the next phase of setup 
                    Button nextbutton = nextButton;
                    nextbutton.IsEnabled = true;
                }
            }


        }

        private void nextButton_Clicked(object sender, EventArgs e)
        {
            //the next phase of set up will pair a watch and link it to the PLWD and the caregiver account so it needs to be sent to the next view 

            string plwdFirstName = plwdFirstNameEntry.Text;
            string plwdLastName = plwdLastNameEntry.Text;

            string query = "SELECT id FROM PLWD WHERE firstname='" + plwdFirstName + "' AND lastname='" + plwdLastName + "' AND caregiver_id='" + CaregiverID + "'";
            objdbaccess.readDatathroughAdapter(query, dtPLWD);

            if (dtPLWD.Rows.Count == 1)
            {
                PLWD_ID = dtPLWD.Rows[0]["id"].ToString();

            }
            //Button click will bring user to next page 
            //Navigation.PushAsync(new NavigationPage(new MainPage(this.cdp)));

            Navigation.PushAsync(new FindMyPLWD.SetupSafeZones(this.cdp, PLWD_ID));

        }
    }
}