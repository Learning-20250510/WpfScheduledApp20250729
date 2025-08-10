using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.SpecificRangeTable
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


                        command.CommandText = "INSERT INTO simpletasksystem.SpecificRange (taskstable_id, category) VALUES (@TTID, @Category)";
                        //command.ExecuteNonQuery();

                        var tasksRead = new TasksTable.Read();

                        //Steamの取得
                        var GetCollection = tasksRead.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=6 and kmn LIKE 'Steam%' ORDER BY RAND() LIMIT 5");

                        foreach (var ele in GetCollection)
                        {
                            command.Parameters.Add(new MySqlParameter("@TTID", ele.ID));
                            command.Parameters.Add(new MySqlParameter("@Category", "Steam"));
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }

                        //中身の初期化
                        GetCollection = new System.Collections.ObjectModel.ObservableCollection<TasksTable.DataClass>();
                        //GameRomで取得
                        GetCollection = tasksRead.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=6 and kmn LIKE 'GameRom%' ORDER BY RAND() LIMIT 5");

                        foreach (var ele in GetCollection)
                        {
                            command.Parameters.Add(new MySqlParameter("@TTID", ele.ID));
                            command.Parameters.Add(new MySqlParameter("@Category", "GameRom"));
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
                MessageBox.Show(me.Message);
            }

        }







    }
}
