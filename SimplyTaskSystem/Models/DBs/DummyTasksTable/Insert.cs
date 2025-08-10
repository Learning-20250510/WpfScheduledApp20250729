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
    class Insert
    {
        private readonly string connectionString;
        public Insert()
        {
            // MySQLへの接続情報
            InitialSettingsInformation initialSettingsInformation = new InitialSettingsInformation();
            this.connectionString = initialSettingsInformation.ConnectionString;
        }
        public void InsertDummyTasksTemplate(string commandtext)
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


        public void InsertRecordFromAction(int actionID, string mmFileNameAsKMN)
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.dummytaskstable (kmn,pcp_id,description,estimated_time,due_date,due_time,priority,repeat_times_per_day,repeat_times_per_day_dummy,repeat_duration,periodically_cycles,specified_day,relational_file_1,relational_file_2,auto_create_first_variable_branches_mmfile) VALUES (@KMN,@PCPID,@Description,@ET,@DD,@DT,@PR,@RTPD,@RTPDD,@RD,@PC,@SD,@RF1,@RF2,@ACFVBMMF)";

                        var tasks = new Models.DBs.TasksTable.Read();
                        var tasksCollection = tasks.SelectTasksTemplate($"select * from simpletasksystem.tasks where id={actionID}");

                        if (tasksCollection.Count != 1)
                        {
                            MessageBox.Show("tasksCollection: " + tasksCollection + " というおかしなことが起こっています。値が１でなければいけません");
                        }


                        command.Parameters.Add(new MySqlParameter("@KMN", mmFileNameAsKMN));

                        foreach (var li in tasksCollection)
                        {
                            command.Parameters.Add(new MySqlParameter("@PCPID", li.PCP_ID));
                            command.Parameters.Add(new MySqlParameter("@Description", li.Description));
                            command.Parameters.Add(new MySqlParameter("@ET", li.EstimatedTime));
                            command.Parameters.Add(new MySqlParameter("@DD", li.DueDate));
                            if (li.DueTime == "" || li.DueTime == null)
                            {
                                li.DueTime = null;
                            }
                            command.Parameters.Add(new MySqlParameter("@DT", li.DueTime));
                            command.Parameters.Add(new MySqlParameter("@PR", li.Priority));
                            command.Parameters.Add(new MySqlParameter("@RTPD", li.RepeatTimesPerDay));
                            command.Parameters.Add(new MySqlParameter("@RTPDD", li.RepeatTimesPerDayDummy));
                            command.Parameters.Add(new MySqlParameter("@RD", li.RepeatDuration));
                            command.Parameters.Add(new MySqlParameter("@PC", li.PeriodicallyCycles));
                            if (li.SpecifiedDay == "" || li.SpecifiedDay == null)
                            {
                                li.SpecifiedDay = null;
                            }
                            command.Parameters.Add(new MySqlParameter("@SD", li.SpecifiedDay));
                            command.Parameters.Add(new MySqlParameter("@RF1", li.RelationalFile1));
                            command.Parameters.Add(new MySqlParameter("@RF2", li.RelationalFile2));
                            command.Parameters.Add(new MySqlParameter("@ACFVBMMF", li.AutoCreateFirstVariableBranchesMMF));
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
                MessageBox.Show("ERROR: " + me.Message);
            }

        }


        public void InsertRecordsToTasksTableOfFreePlane()
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        //DummyTasksTableの全レコードの取得
                        var selectAllDummyTasksTable = new Read();
                        var selectAllDummyTasksRecords = selectAllDummyTasksTable.SelectDummyTasksTable("select * from simpletasksystem.dummytaskstable");

                        command.CommandText = "INSERT INTO simpletasksystem.tasks (kmn,kmt,htl,pcp_id,description,estimated_time,due_date,due_time,priority,repeat_times_per_day,archived,postpone,repeat_duration,periodically_cycles,specified_day,relational_file_1,relational_file_2,auto_create_first_variable_branches_mmfile) VALUES (@kmn, @kmt, @htl,@PCPID,@Description,@EstimatedTime,@DueDate,@DueTime,@Priority,@RTPD,@Archived,@PP,@RepeatDuration,@PC,@SD,@RF1,@RF2,@ACFVBMMF)";
                        //command.ExecuteNonQuery();

                        foreach (var li in selectAllDummyTasksRecords)
                        {
                            var validations = new TasksTable.Validations();
                            if (validations.ValidationOfNewRecordOfFreePlane(li.KMN))
                            {
                                for (int i = 15; i < 18; i++)
                                {
                                    command.Parameters.Add(new MySqlParameter("@kmn", li.KMN));
                                    command.Parameters.Add(new MySqlParameter("@kmt", 5));
                                    command.Parameters.Add(new MySqlParameter("@htl", i));
                                    command.Parameters.Add(new MySqlParameter("@PCPID", li.PCP_ID));
                                    command.Parameters.Add(new MySqlParameter("@Description", li.Description));
                                    command.Parameters.Add(new MySqlParameter("@EstimatedTime", li.EstimatedTime));
                                    command.Parameters.Add(new MySqlParameter("@DueDate", li.DueDate));
                                    if (li.DueTime == "" || li.DueTime == null)
                                    {
                                        li.DueTime = null;
                                    }
                                    command.Parameters.Add(new MySqlParameter("@DueTime", li.DueTime));
                                    command.Parameters.Add(new MySqlParameter("@Priority", li.Priority));
                                    command.Parameters.Add(new MySqlParameter("@RTPD", li.RepeatTimesPerDay));
                                    command.Parameters.Add(new MySqlParameter("@Archived", li.Archived));
                                    command.Parameters.Add(new MySqlParameter("@PP", li.Postpone));
                                    command.Parameters.Add(new MySqlParameter("@RepeatDuration", li.RepeatDuration));
                                    command.Parameters.Add(new MySqlParameter("@PC", li.PeriodicallyCycles));
                                    if (li.SpecifiedDay == "")
                                    {
                                        li.SpecifiedDay = default;
                                    }
                                    command.Parameters.Add(new MySqlParameter("@SD", li.SpecifiedDay));
                                    command.Parameters.Add(new MySqlParameter("@RF1", li.RelationalFile1));
                                    command.Parameters.Add(new MySqlParameter("@RF2", li.RelationalFile2));
                                    command.Parameters.Add(new MySqlParameter("@ACFVBMMF", li.AutoCreateFirstVariableBranchesMMF));
                                    command.ExecuteNonQuery();
                                    command.Parameters.Clear();
                                }
                            }
                            else
                            {
                                //MessageBox.Show(li.KMN + " は、既にTasksTableにInsertされているため、今回Insertしません。");
                                Debug.WriteLine(li.KMN + " は、既にTasksTableにInsertされているため、今回Insertしません。");
                            }
            
   
                        }





                        command.Parameters.Clear();



                    }

                    connection.Close();

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + me.Message);
                MessageBox.Show("InsertTaskAddingに失敗しました。" + me.Message);
            }

        }




    }
}
