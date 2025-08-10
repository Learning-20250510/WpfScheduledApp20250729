using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.TaskCommonElementsTable
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


                        command.CommandText = "INSERT INTO simpletasksystem.taskcommonelements (kmn, kmt, htl) VALUES (@kmn, @kmt, @htl)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {
                           
                        };

                        for (int i = 1; i <= list.Count; i++)
                        {
                            command.Parameters.Add(new MySqlParameter("@kmn", i));
                            command.Parameters.Add(new MySqlParameter("@kmt", list[i]));
                            command.Parameters.Add(new MySqlParameter("@htl", list[i]));
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
            }

        }

    }
}
