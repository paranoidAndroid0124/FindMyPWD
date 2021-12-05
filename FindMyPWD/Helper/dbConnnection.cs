using FindMyPWD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FindMyPWD.Helper
{
    public class dbConnnection
    {
        public static ObservableCollection<DB_data> List { get; set; }
        private const string stringConnection = @"Server=tcp:findmypwd-server.database.windows.net,1433;Initial Catalog=FindMyPWD_DB;Persist Security Info=False;User ID=paranoidAndroid;Password=7TyLM2EGw9sSyff;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string sqlQuery = "READ_DATA";
        public static void GetDB()
        {
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
    }
}
