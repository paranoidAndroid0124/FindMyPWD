using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Data;
using System.Data.SqlClient;


namespace FindMyPLWD
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSetup : ContentPage
    {
        //This is an object for accessing the SQL database
        DBAccess objdbaccess = new DBAccess();
        //This is a data table that will store data retunred from read operations to the database
        DataTable dtCaregivers = new DataTable(); 

        public static string CaregiverID; 
        private CurrentDevicePage cdp;
        public UserSetup(CurrentDevicePage cdp)
        {
            this.cdp = cdp;
            InitializeComponent();
        }

        private void sendToDatabase_Clicked(object sender, EventArgs e)
        {
            //this is saving the user data from the fields 
            string caregiverFirstName = caregiverFirstNameEntry.Text;
            string caregiverLastName = caregiverLastNameEntry.Text;
            string caregiverEmail = caregiverEmailEntry.Text;
            string plwdFirstName = plwdFirstNameEntry.Text;
            string plwdLastName = plwdFirstNameEntry.Text;

            //Checks to ensure all the fields are filled out correctly
            if (caregiverFirstName.Equals(null))
            {
                DisplayAlert("Alert", "You must enter a Caregiver First Name", "OK");
            }
            else if (caregiverFirstName.Equals(null))
            {
                DisplayAlert("Alert", "You must enter a Caregiver Last Name", "OK");
            }
            else if (caregiverLastName.Equals(null))
            {
                DisplayAlert("Alert", "You must enter a Caregiver Email", "OK");
            }
            else 
            {
                //This means all the fields were filled out correctly 
                //Now it is time to send the data to the database

                //First we create a SQL command for inserting data

                SqlCommand insertCommand = new SqlCommand("INSERT INTO Caregiver (firstname,lastname,email) VALUES(@caregvierFirstName,@caregvierLastName, @caregiverEmail)");
                insertCommand.Parameters.AddWithValue("@caregvierFirstName", caregiverFirstName);
                insertCommand.Parameters.AddWithValue("@caregvierLastName", caregiverLastName);
                insertCommand.Parameters.AddWithValue("@caregiverEmail", caregiverEmail);

                //This will create a new row in the database with the values given 
                int row = objdbaccess.executeQuery(insertCommand);

                //This is mean the data entry was a sucess 
                if (row == 1)
                {
                    DisplayAlert("Sucess", "Caregiver " + caregiverFirstName + " " + caregiverLastName + " Created" , "OK");
                  
                    //This action will enable the user to click the "next" button to proceed to the next phase of setup 
                    Button nextbutton = nextSetupButton;
                    nextbutton.IsEnabled = true; 
                }
            }

            
        }

        private void nextSetupButton_Clicked(object sender, EventArgs e)
        {
            //This gets the caregiver email just created
            string caregiverEmail = caregiverEmailEntry.Text;


            //Console.WriteLine("THIS IS THE EMIAL BEING SENT!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!" + caregiverEmail);

            //Get the id of the caregiver just created to pass to the next part of the set up
            string query = "SELECT id FROM Caregiver WHERE email='" + caregiverEmail + "'";
            objdbaccess.readDatathroughAdapter(query,dtCaregivers);

            //Console.WriteLine(dtCaregivers.Rows[0]["id"].ToString());

            //If the query from the database was sucessful and returned the caregiver ID
            if (dtCaregivers.Rows.Count == 1)
            {
               CaregiverID = dtCaregivers.Rows[0]["id"].ToString();
               
            }
            //Button click will bring user to next page 
            Navigation.PushAsync(new FindMyPLWD.plwdSetUp(this.cdp, CaregiverID));
        }
    }
}