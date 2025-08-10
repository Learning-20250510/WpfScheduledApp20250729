using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.KMTTable
{
    class Insert
    {
        private readonly string connectionString;
        public Insert()
        {
            // MySQLへの接続情報
            InitialSettingsInformation initialSettingsInformation = new InitialSettingsInformation();
            this.connectionString = initialSettingsInformation.ConnectionString;
        }
        public void InsertInitialRecords()
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.kmt (id, kmt) VALUES (@ID, @KMT)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {
                            "Unclassified",
                            "TheWorld",
                            "MyBrain",
                            "WebPage",
                            "FreePlane",
                            "PDF",
                            "Research",
                        };

                        for (int i = 1; i <= list.Count; i++)
                        {
                            command.Parameters.Add(new MySqlParameter("@ID", i));
                            command.Parameters.Add(new MySqlParameter("@KMT", list[i - 1]));
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }




                        command.Parameters.Clear();



                    }

                    connection.Close();

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + me.Message);
                //MessageBox.Show(me.Message);
            }

        }

    }
}
