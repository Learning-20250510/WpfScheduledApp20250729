using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.DummyTasksTable
{
    class Read
    {
        private readonly string connectionString;

        public Read()
        {
            // MySQLへの接続情報
            InitialSettingsInformation initialSettingsInformation = new InitialSettingsInformation();
            this.connectionString = initialSettingsInformation.ConnectionString;
        }
        public ObservableCollection<DataClass> SelectDummyTasksTable(string commandtext)
        {


            // MySQLへの接続

            ObservableCollection<DataClass> TasksCollection = new ObservableCollection<DataClass>();

            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = commandtext;
                        command.ExecuteNonQuery();




                        using (var reader = command.ExecuteReader())
                        {
                            // コンソールに表示
                            while (reader.Read())
                            {
                                TasksCollection.Add(new DataClass { ID = int.Parse(reader["id"].ToString()), KMN = reader["kmn"].ToString(), PCP_ID = int.Parse(reader["pcp_id"].ToString()), Description = reader["description"].ToString(), EstimatedTime = int.Parse(reader["estimated_time"].ToString()), DueDate = reader["due_date"].ToString(), DueTime = reader["due_time"].ToString(), Priority = int.Parse(reader["priority"].ToString()), RepeatTimesPerDay = int.Parse(reader["repeat_times_per_day"].ToString()), RepeatTimesPerDayDummy = int.Parse(reader["repeat_times_per_day_dummy"].ToString()), CreatedAt = reader["created_at"].ToString(), LastClearedAt = reader["lastcleared_at"].ToString(), ClearTimesInTime = int.Parse(reader["clear_times_intime"].ToString()), ClearTimesOutOfTime = int.Parse(reader["clear_times_outoftime"].ToString()), Archived = Convert.ToBoolean(reader["archived"]), Postpone = Convert.ToBoolean(reader["postpone"]), RepeatDuration = int.Parse(reader["repeat_duration"].ToString()), PeriodicallyCycles = int.Parse(reader["periodically_cycles"].ToString()), SpecifiedDay = reader["specified_day"].ToString(), RelationalFile1 = reader["relational_file_1"].ToString(), RelationalFile2 = reader["relational_file_2"].ToString(), AutoCreateFirstVariableBranchesMMF = Convert.ToBoolean(reader["auto_create_first_variable_branches_mmfile"]) });
                            }
                        }

                        command.Parameters.Clear();



                    }

                    connection.Close();
                    return TasksCollection;

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + me.Message);
                MessageBox.Show(me.Message);
                return TasksCollection;
            }

        }

    }
}
