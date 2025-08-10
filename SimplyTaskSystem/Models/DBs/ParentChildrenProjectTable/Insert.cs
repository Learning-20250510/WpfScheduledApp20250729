using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.ParentChildrenProjectTable
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


                        command.CommandText = "INSERT INTO simpletasksystem.pcp (parent,child) VALUES (@P,@C)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {

                        };


                        command.Parameters.Add(new MySqlParameter("@P", "Unclassified"));
                        command.Parameters.Add(new MySqlParameter("@C", "General"));
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();






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
        public void InsertParentRecord(string parentProjectName)
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.pcp (parent,child) VALUES (@P,@C)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {

                        };

              
                        command.Parameters.Add(new MySqlParameter("@P", parentProjectName));
                        command.Parameters.Add(new MySqlParameter("@C", "General"));
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();




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


        public void InsertChildRecord(string parentProjectName, string childProjectName)
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.pcp (parent,child) VALUES (@P,@C)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {

                        };


                        command.Parameters.Add(new MySqlParameter("@P", parentProjectName));
                        command.Parameters.Add(new MySqlParameter("@C", childProjectName));
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();




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
