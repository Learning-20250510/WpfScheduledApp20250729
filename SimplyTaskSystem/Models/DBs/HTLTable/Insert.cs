using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.HTLTable
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


                        command.CommandText = "INSERT INTO simpletasksystem.htl (id, htl) VALUES (@ID, @HTL)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {
                            "Unclassified",
                            "FocusContextBetWeen10SecondsOfMovie",
                            "FocusContextInStillImageOfMovie",
                            "FocusContextInSpecificPageOfPDF",
                            "FocusDesignInSpecificPageOfPDF",
                            "AnywayOfTheWorld",
                            "FocusContextInScrollValueOfWebPage",
                            "FocusDesignInScrollValueOfWebPage",

                            "CreateMMFileDirectlyFromBrowserByAutoMMFileGenerationOfWebPage",
                            "CreateMMFileAutomaticallyScrapingByAutoMMFileGenerationOfWebPage",

                            "CreateMMFileFromContextWithOCRByAutoMMFileGenerationOfPDF",
                            "FocusTheKMNWithHavingAnIdeaMyBrain",
                            "FindKMFromSomeURLAutomaticallyOfResearch",

                            "FocusContextBetWeen10SecondsOfSound",
                            "FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane",
                            "FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane",
                            "FindTasksNTimesFromTheMMFileOfFreePlane",
                            "ReadyForVariablesListLearningOfTheWorld",
                            "ReviewOfTheWorldOfMyBrain",

                        };

                        for (int i=1; i<=list.Count; i++)
                        {
                            command.Parameters.Add(new MySqlParameter("@ID", i));
                            command.Parameters.Add(new MySqlParameter("@HTL", list[i-1]));
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
