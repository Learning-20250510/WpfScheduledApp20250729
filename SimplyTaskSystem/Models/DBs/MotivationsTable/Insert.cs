using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.DBs.MotivationsTable
{
    class Insert
    {
        private readonly string connectionString;
        public static readonly List<string>  motivationList = new List<string>() 
        {
        "Lack motivation", 
        "Escape from reality",
        "Fail to prioritize",
        "Make an excuse",
        "Postpone",
        "Lazy",
        "Frustrating",
        "Time pollution", 
        "Feel difficultly", 
        "Loss of concentration",
        "Sleepy",
        "Binging", 
        "Envy", 
        "Be worried",
        "Lonely",
        "Angry", 
        "Regret", 
        "Want to die",
        "Cry", 
        "Loss self-confidence", 
        "Feel inferiority complex",
        "Be scolded", 
        "Suffer from duty",
            
         };
        public Insert()
        {
            // MySQLへの接続情報
            InitialSettingsInformation initialSettingsInformation = new InitialSettingsInformation();
            this.connectionString = initialSettingsInformation.ConnectionString;

        }
        public void InsertInitialRecords(string motivationName)
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.motivations (motivation_name) VALUES (@MOT)";
                        //command.ExecuteNonQuery();

           

              

               
                        command.Parameters.Add(new MySqlParameter("@MOT", motivationName));
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
                //MessageBox.Show(me.Message);
            }

        }
    }
}
