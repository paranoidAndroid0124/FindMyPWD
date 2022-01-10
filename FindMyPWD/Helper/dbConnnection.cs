using FindMyPLWD;
using FindMyPWD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Linq;
using System.Text.Json;

namespace FindMyPWD.Helper
{
    public class DBConnnection
    {
        public static ObservableCollection<DB_data> List { get; set; }
        //connection string is a security risk. Remove and use a azure function before release into production
        private const string stringConnection = @"Server=tcp:findmypwd-server.database.windows.net,1433;Initial Catalog=FindMyPWD_DB;Persist Security Info=False;User ID=paranoidAndroid;Password=7TyLM2EGw9sSyff;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string sqlQuery = null;
        /*This function is to read from the database*/
        public static void GetDB()
        {
            string sqlQuery = "READ_DATA";
            List = new ObservableCollection<DB_data>();
            using (SqlConnection con = new SqlConnection(stringConnection))
            {
                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    command.CommandText = @"SELECT [clocktime], [location], [device] FROM PWD";
                    con.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DB_data data = new DB_data()
                            {
                                clocktime = reader.GetString(0),
                                location = reader.GetString(1),
                                device = reader.GetString(2),
                            };
                            List.Add(data);
                        }
                    }
                }
            }
        }
        /*This function will add the seen device as a entry to the database*/
        public static bool WriteDB(string clocktime, string location, string device)
        {
            string sqlQuery = "insert into Main ([clocktime], [location], [device]) values(@clock,@loc,@device)"; //double check the values
            using (SqlConnection cnn = new SqlConnection(stringConnection))
            {
                try
                {
                    // Open the connection to the database. 
                    cnn.Open();

                    // Prepare the command to be executed on the db
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, cnn))
                    {
                        // Create and set the parameters values 
                        cmd.Parameters.Add("@clock", SqlDbType.NVarChar).Value = clocktime;
                        cmd.Parameters.Add("@loc", SqlDbType.NVarChar).Value = location;
                        cmd.Parameters.Add("@device", SqlDbType.NVarChar).Value = device;

                        // Let's ask the db to execute the query
                        int rowsAdded = cmd.ExecuteNonQuery();
                        if (rowsAdded > 0)
                            Console.WriteLine("Row inserted!!");//the row was inserted
                        else
                            // Well this should never really happen
                            Console.WriteLine("No row inserted");

                    }
                    return true; //it worked
                }
                catch (Exception)
                {
                    return false; //it failed
                }
            }
        }
    }
    public static class localDBConnnection //this is local sqlite db to store settings
    {
       
        public static List<BLEDevice> getPairedDevice()
        {
            //check if file exist
            //if (File.Exists(App.FilePath) && (File.ReadAllBytes(App.FilePath).Length > 0)) //check if the db file exist and if there is something in it
            if (File.Exists(App.FilePath))
            {
                var fd = File.OpenRead(App.FilePath);
                List<BLEDevice> results = JsonSerializer.Deserialize<List<BLEDevice>>(fd);
                fd.Close(); //closed the opened file

                return results;
            }
            else
            {
                return new List<BLEDevice>(); //empty
            }
        }
      

        public static void write(BLEDevice device) 
        {
            if (!File.Exists(App.FilePath)) //if the file doesn't exist than create it
            {
                createNewJson();
            }
            var fd = File.OpenRead(App.FilePath);
            List<BLEDevice> results = JsonSerializer.Deserialize<List<BLEDevice>>(fd);
            fd.Close();
            results.Add(device);
            string jsonString = JsonSerializer.Serialize<List<BLEDevice>>(results);
            File.WriteAllText(App.FilePath, jsonString);
        }

        public static void createNewJson() 
        {
            
            var fd = File.Create(App.FilePath);
            fd.Close();
            string jsonString = JsonSerializer.Serialize<List<BLEDevice>>(new List<BLEDevice>());
            File.WriteAllText(App.FilePath, jsonString);
        }
    }
}
