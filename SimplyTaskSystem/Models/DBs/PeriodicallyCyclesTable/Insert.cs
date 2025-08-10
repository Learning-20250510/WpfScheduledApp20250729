using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.PeriodicallyCyclesTable
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


                        command.CommandText = "INSERT INTO simpletasksystem.periodicallycycles (id, pc) VALUES (@ID, @PC)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {
                            "Nothing",
                            "Everyday",
                            "EveryMonday",
                            "EveryTuesday",
                            "EveryWednesday",
                            "EveryThursday",
                            "EveryFriday",
                            "EverySaturday",
                            "EverySunday",
                            "EveryTwoWeeksOnAMonday",
                            "EveryTwoWeeksOnATuesday",
                            "EveryTwoWeeksOnAWednesday",
                            "EveryTwoWeeksOnAThursday",
                            "EveryTwoWeeksOnAFriday",
                            "EveryTwoWeeksOnASaturday",
                            "EveryTwoWeeksOnSunday",
                            "Weekday",
                            "Weekend",
                            "EveryMonthOnRandomDay",
                            "EveryYearOnRandomDay",
                            "EveryMonthOnSpecifiedDay",
                            "EveryYearOnSpecifiedDay",
                  
                        };

                        for (int i = 1; i <= list.Count; i++)
                        {
                            command.Parameters.Add(new MySqlParameter("@ID", i));
                            command.Parameters.Add(new MySqlParameter("@PC", list[i - 1]));
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
