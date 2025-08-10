using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;

namespace SimplyTaskSystem.Models.DBs.TasksTable
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
        public void InsertTasksTemplate(string commandtext)
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

        public void InsertRecordsFromAddTaskWindow(Models.DBs.TasksTable.DataClass tasksDataClass)
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.tasks (kmn,kmt,htl,pcp_id,description,estimated_time,due_date,due_time,priority,repeat_times_per_day,archived,postpone,repeat_duration,periodically_cycles,specified_day,relational_file_1,relational_file_2,auto_create_first_variable_branches_mmfile, repeat_patterns, temporary_repeat_task_count) VALUES (@kmn, @kmt, @htl,@PCPID,@Description,@EstimatedTime,@DueDate,@DueTime,@Priority,@RTPD,@Archived,@PP,@RepeatDuration,@PC,@SD,@RF1,@RF2,@ACFVBMMF, @RP, @TRTC)";
                        //command.ExecuteNonQuery();

                        var list = new List<string>()
                        {

                        };

                   
                        command.Parameters.Add(new MySqlParameter("@kmn", tasksDataClass.KMN));
                        command.Parameters.Add(new MySqlParameter("@kmt", tasksDataClass.KMT));
                        command.Parameters.Add(new MySqlParameter("@htl", tasksDataClass.HTL));
                        command.Parameters.Add(new MySqlParameter("@PCPID", tasksDataClass.PCP_ID));
                        command.Parameters.Add(new MySqlParameter("@Description", tasksDataClass.Description));
                        command.Parameters.Add(new MySqlParameter("@EstimatedTime", tasksDataClass.EstimatedTime));
                        command.Parameters.Add(new MySqlParameter("@DueDate", tasksDataClass.DueDate));
                        command.Parameters.Add(new MySqlParameter("@DueTime", tasksDataClass.DueTime));
                        command.Parameters.Add(new MySqlParameter("@Priority", tasksDataClass.Priority));
                        command.Parameters.Add(new MySqlParameter("@RTPD", tasksDataClass.RepeatTimesPerDay));
                        command.Parameters.Add(new MySqlParameter("@Archived", tasksDataClass.Archived));
                        command.Parameters.Add(new MySqlParameter("@PP", tasksDataClass.Postpone));
                        command.Parameters.Add(new MySqlParameter("@RepeatDuration", tasksDataClass.RepeatDuration));
                        command.Parameters.Add(new MySqlParameter("@PC", tasksDataClass.PeriodicallyCycles));
                        command.Parameters.Add(new MySqlParameter("@SD", tasksDataClass.SpecifiedDay));
                        command.Parameters.Add(new MySqlParameter("@RF1", tasksDataClass.RelationalFile1));
                        command.Parameters.Add(new MySqlParameter("@RF2", tasksDataClass.RelationalFile2));
                        command.Parameters.Add(new MySqlParameter("@ACFVBMMF", tasksDataClass.AutoCreateFirstVariableBranchesMMF));
                        command.Parameters.Add(new MySqlParameter("@RP", tasksDataClass.RepeatPatterns));
                        command.Parameters.Add(new MySqlParameter("@TRTC", tasksDataClass.TemporaryRepeatTaskCount));
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
                MessageBox.Show("InsertTaskAddingに失敗しました。" + me.Message);
            }

        }



        
        public void InsertRecordsFromMemoURLs()
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.tasks (kmn,kmt,htl,priority,periodically_cycles,specific_scrollvalue_as_webpage,auto_create_first_variable_branches_mmfile) VALUES (@KMN,@KMT,@HTL,@PR,@PC,@SSAW,@ACFVBMMF)";

                        //メモ帳から行ごとにList<string>でURLを取得
                        var memoOperation = new FilesOperation.MemoOperation();
                        var list = memoOperation.ReadToEndFile(3);

                        var select = new Read();
                        var existingWebPage = select.SelectTasksTemplate("select * from simpletasksystem.tasks where kmt=4");
                        //検査用URLのみ（重複あり）リスト
                        var existingWebPageList = new List<string>();

                        foreach (var w in existingWebPage)
                        {
                            existingWebPageList.Add(w.KMN);
                        }

                        foreach (var newURL in list)
                        {
                            if (!(existingWebPageList.Contains(newURL)))
                            {
                                Debug.WriteLine(newURL + " はまだtasksTableにないのでこれから追加していきます。");

                                for (int i = 7; i < 9; i++)
                                {
                                    for (int k=0; k < 11; k++)
                                    {
                                        command.Parameters.Add(new MySqlParameter("@KMN", newURL));
                                        command.Parameters.Add(new MySqlParameter("@KMT", 4));
                                        command.Parameters.Add(new MySqlParameter("@HTL", i));
                                        command.Parameters.Add(new MySqlParameter("@PR", 3));
                                        command.Parameters.Add(new MySqlParameter("@PC", 2));
                                        command.Parameters.Add(new MySqlParameter("@SSAW", k));
                                        command.Parameters.Add(new MySqlParameter("@ACFVBMMF", true));
                                        command.ExecuteNonQuery();
                                        command.Parameters.Clear();
                                    }
  
                                }
                                for (int i = 9; i < 11; i++)
                                {
                                    int j = 0;
                                    command.Parameters.Add(new MySqlParameter("@KMN", newURL));
                                    command.Parameters.Add(new MySqlParameter("@KMT", 4));
                                    command.Parameters.Add(new MySqlParameter("@HTL", i));
                                    command.Parameters.Add(new MySqlParameter("@PR", 3));
                                    command.Parameters.Add(new MySqlParameter("@PC", 2));
                                    command.Parameters.Add(new MySqlParameter("@SSAW", j));
                                    command.Parameters.Add(new MySqlParameter("@ACFVBMMF", true));
                                    command.ExecuteNonQuery();
                                    command.Parameters.Clear();
                                }
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

        public void InsertRecordsFromPriorityWant1stMemoURLs()
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.tasks (kmn,kmt,htl,priority,periodically_cycles,specific_scrollvalue_as_webpage,auto_create_first_variable_branches_mmfile) VALUES (@KMN,@KMT,@HTL,@PR,@PC,@SSAW,@ACFVBMMF)";

                        //メモ帳から行ごとにList<string>でURLを取得
                        var memoOperation = new FilesOperation.MemoOperation();
                        var list = memoOperation.ReadToEndFile(4);

                        var select = new Read();
                        var existingWebPage = select.SelectTasksTemplate("select * from simpletasksystem.tasks where kmt=4");
                        //検査用URLのみ（重複あり）リスト
                        var existingWebPageList = new List<string>();

                        foreach (var w in existingWebPage)
                        {
                            existingWebPageList.Add(w.KMN);
                        }

                        foreach (var newURL in list)
                        {
                            if (!(existingWebPageList.Contains(newURL)))
                            {
                                Debug.WriteLine(newURL + " はまだtasksTableにないのでこれから追加していきます。");

                                for (int i = 7; i < 9; i++)
                                {
                                    for (int k = 0; k < 11; k++)
                                    {
                                        command.Parameters.Add(new MySqlParameter("@KMN", newURL));
                                        command.Parameters.Add(new MySqlParameter("@KMT", 4));
                                        command.Parameters.Add(new MySqlParameter("@HTL", i));
                                        command.Parameters.Add(new MySqlParameter("@PR", 4));
                                        command.Parameters.Add(new MySqlParameter("@PC", 1));
                                        command.Parameters.Add(new MySqlParameter("@SSAW", k));
                                        command.Parameters.Add(new MySqlParameter("@ACFVBMMF", false));
                                        command.ExecuteNonQuery();
                                        command.Parameters.Clear();
                                    }

                                }
                                for (int i = 9; i < 11; i++)
                                {
                                    int j = 0;
                                    command.Parameters.Add(new MySqlParameter("@KMN", newURL));
                                    command.Parameters.Add(new MySqlParameter("@KMT", 4));
                                    command.Parameters.Add(new MySqlParameter("@HTL", i));
                                    command.Parameters.Add(new MySqlParameter("@PR", 4));
                                    command.Parameters.Add(new MySqlParameter("@PC", 1));
                                    command.Parameters.Add(new MySqlParameter("@SSAW", j));
                                    command.Parameters.Add(new MySqlParameter("@ACFVBMMF", true));
                                    command.ExecuteNonQuery();
                                    command.Parameters.Clear();
                                }
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

        public void InsertRecordsFromPriorityWant2ndMemoURLs()
        {



            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();


                        command.CommandText = "INSERT INTO simpletasksystem.tasks (kmn,kmt,htl,priority,periodically_cycles,specific_scrollvalue_as_webpage,auto_create_first_variable_branches_mmfile) VALUES (@KMN,@KMT,@HTL,@PR,@PC,@SSAW,@ACFVBMMF)";

                        //メモ帳から行ごとにList<string>でURLを取得
                        var memoOperation = new FilesOperation.MemoOperation();
                        var list = memoOperation.ReadToEndFile(5);

                        var select = new Read();
                        var existingWebPage = select.SelectTasksTemplate("select * from simpletasksystem.tasks where kmt=4");
                        //検査用URLのみ（重複あり）リスト
                        var existingWebPageList = new List<string>();

                        foreach (var w in existingWebPage)
                        {
                            existingWebPageList.Add(w.KMN);
                        }

                        foreach (var newURL in list)
                        {
                            if (!(existingWebPageList.Contains(newURL)))
                            {
                                Debug.WriteLine(newURL + " はまだtasksTableにないのでこれから追加していきます。");

                                for (int i = 7; i < 9; i++)
                                {
                                    for (int k = 0; k < 11; k++)
                                    {
                                        command.Parameters.Add(new MySqlParameter("@KMN", newURL));
                                        command.Parameters.Add(new MySqlParameter("@KMT", 4));
                                        command.Parameters.Add(new MySqlParameter("@HTL", i));
                                        command.Parameters.Add(new MySqlParameter("@PR", 5));
                                        command.Parameters.Add(new MySqlParameter("@PC", 1));
                                        command.Parameters.Add(new MySqlParameter("@SSAW", k));
                                        command.Parameters.Add(new MySqlParameter("@ACFVBMMF", false));
                                        command.ExecuteNonQuery();
                                        command.Parameters.Clear();
                                    }

                                }
                                for (int i = 9; i < 11; i++)
                                {
                                    int j = 0;
                                    command.Parameters.Add(new MySqlParameter("@KMN", newURL));
                                    command.Parameters.Add(new MySqlParameter("@KMT", 4));
                                    command.Parameters.Add(new MySqlParameter("@HTL", i));
                                    command.Parameters.Add(new MySqlParameter("@PR", 5));
                                    command.Parameters.Add(new MySqlParameter("@PC", 1));
                                    command.Parameters.Add(new MySqlParameter("@SSAW", j));
                                    command.Parameters.Add(new MySqlParameter("@ACFVBMMF", true));
                                    command.ExecuteNonQuery();
                                    command.Parameters.Clear();
                                }
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




        public void InsertRecordFromDummyTasksTable()
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

                        for (int i = 1; i <= list.Count; i++)
                        {
                            command.Parameters.Add(new MySqlParameter("@ID", i));
                            command.Parameters.Add(new MySqlParameter("@HTL", list[i - 1]));
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


        public void InsertTasksTheWorldMyBrainResearch(int ID, bool TheWorld, bool MyBrain, bool Research)
        {
            /*method使い方一例
             *  あるレコードをテンプレートとしてこのmethodデInsert => そのもとになったテンプレートレコードは別メソッドでDeleteするのを忘れずに？
             * 
             */

            // MySQLへの接続

            ObservableCollection<DataClass> TasksCollection = new ObservableCollection<DataClass>();

            try
            {



                using (var connection = new MySqlConnection(connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        //第1引数のIDのレコード情報の取得
                        var Select = new Models.DBs.TasksTable.Read();
                        var tasks = Select.SelectTasksTemplate($"select * from simpletasksystem.tasks where id={ID}");

                        //整合性Check
                        if (tasks.Count != 1)
                        {
                            MessageBox.Show("ID= " + ID + "  のレコードが" + tasks.Count + " 個ありました。おかしな話です。");
                        }

                        

                        command.CommandText = "INSERT INTO simpletasksystem.tasks (kmn,kmt,htl,pcp_id,description,estimated_time,due_date,due_time,priority,repeat_times_per_day,archived,postpone,repeat_duration,periodically_cycles,specified_day,relational_file_1,relational_file_2,auto_create_first_variable_branches_mmfile) VALUES (@kmn, @kmt, @htl,@PCPID,@Description,@EstimatedTime,@DueDate,@DueTime,@Priority,@RTPD,@Archived,@PP,@RepeatDuration,@PC,@SD,@RF1,@RF2,@ACFVBMMF)";
                        //command.ExecuteNonQuery();


                        foreach (var tasksDataClass in tasks)
                        {

                            if (TheWorld == true)
                            {
                                //theWorld
                                command.Parameters.Add(new MySqlParameter("@kmn", tasksDataClass.KMN));
                                command.Parameters.Add(new MySqlParameter("@kmt", 2));
                                command.Parameters.Add(new MySqlParameter("@htl", 6));
                                command.Parameters.Add(new MySqlParameter("@PCPID", tasksDataClass.PCP_ID));
                                command.Parameters.Add(new MySqlParameter("@Description", tasksDataClass.Description));
                                command.Parameters.Add(new MySqlParameter("@EstimatedTime", tasksDataClass.EstimatedTime));
                                command.Parameters.Add(new MySqlParameter("@DueDate", tasksDataClass.DueDate));
                                if (tasksDataClass.DueTime == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@DueTime", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@DueTime", tasksDataClass.DueTime));

                                }
                                if (tasksDataClass.Priority == 1)
                                {
                                    command.Parameters.Add(new MySqlParameter("@Priority", 2));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@Priority", tasksDataClass.Priority));

                                }
                                command.Parameters.Add(new MySqlParameter("@RTPD", tasksDataClass.RepeatTimesPerDay));
                                command.Parameters.Add(new MySqlParameter("@Archived", tasksDataClass.Archived));
                                command.Parameters.Add(new MySqlParameter("@PP", tasksDataClass.Postpone));
                                command.Parameters.Add(new MySqlParameter("@RepeatDuration", tasksDataClass.RepeatDuration));
                                command.Parameters.Add(new MySqlParameter("@PC", tasksDataClass.PeriodicallyCycles));
                                if (tasksDataClass.SpecifiedDay == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@SD", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@SD", tasksDataClass.SpecifiedDay));

                                }
                                if (tasksDataClass.RelationalFile1 == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF1", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF1", tasksDataClass.RelationalFile1));

                                }

                                if (tasksDataClass.RelationalFile2 == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF2", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF2", tasksDataClass.RelationalFile2));

                                }
                                command.Parameters.Add(new MySqlParameter("@ACFVBMMF", tasksDataClass.AutoCreateFirstVariableBranchesMMF));
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                            }
 
                            if (MyBrain == true)
                            {

                                //MyBrain
                                command.Parameters.Add(new MySqlParameter("@kmn", tasksDataClass.KMN));
                                command.Parameters.Add(new MySqlParameter("@kmt", 3));
                                command.Parameters.Add(new MySqlParameter("@htl", 12));
                                command.Parameters.Add(new MySqlParameter("@PCPID", tasksDataClass.PCP_ID));
                                command.Parameters.Add(new MySqlParameter("@Description", tasksDataClass.Description));

                                //MyBrain系はE.T.に上限の設定
                                if (tasksDataClass.EstimatedTime > 15)
                                {
                                    command.Parameters.Add(new MySqlParameter("@EstimatedTime", 15));
                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@EstimatedTime", tasksDataClass.EstimatedTime));
                                }
                                command.Parameters.Add(new MySqlParameter("@DueDate", tasksDataClass.DueDate));
                                if (tasksDataClass.DueTime == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@DueTime", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@DueTime", tasksDataClass.DueTime));

                                }
                                if (tasksDataClass.Priority == 1)
                                {
                                    command.Parameters.Add(new MySqlParameter("@Priority", 4));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@Priority", tasksDataClass.Priority));

                                }
                                command.Parameters.Add(new MySqlParameter("@RTPD", tasksDataClass.RepeatTimesPerDay));
                                command.Parameters.Add(new MySqlParameter("@Archived", tasksDataClass.Archived));
                                command.Parameters.Add(new MySqlParameter("@PP", tasksDataClass.Postpone));
                                command.Parameters.Add(new MySqlParameter("@RepeatDuration", tasksDataClass.RepeatDuration));
                                command.Parameters.Add(new MySqlParameter("@PC", tasksDataClass.PeriodicallyCycles));
                                if (tasksDataClass.SpecifiedDay == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@SD", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@SD", tasksDataClass.SpecifiedDay));

                                }
                                if (tasksDataClass.RelationalFile1 == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF1", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF1", tasksDataClass.RelationalFile1));

                                }

                                if (tasksDataClass.RelationalFile2 == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF2", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF2", tasksDataClass.RelationalFile2));

                                }
                                command.Parameters.Add(new MySqlParameter("@ACFVBMMF", tasksDataClass.AutoCreateFirstVariableBranchesMMF));
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();

                            }


                            if (Research == true)
                            {
                                //Research
                                command.Parameters.Add(new MySqlParameter("@kmn", tasksDataClass.KMN));
                                command.Parameters.Add(new MySqlParameter("@kmt", 7));
                                command.Parameters.Add(new MySqlParameter("@htl", 13));
                                command.Parameters.Add(new MySqlParameter("@PCPID", tasksDataClass.PCP_ID));
                                command.Parameters.Add(new MySqlParameter("@Description", tasksDataClass.Description));
                                command.Parameters.Add(new MySqlParameter("@EstimatedTime", 2));
                                command.Parameters.Add(new MySqlParameter("@DueDate", tasksDataClass.DueDate));
                                if (tasksDataClass.DueTime == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@DueTime", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@DueTime", tasksDataClass.DueTime));

                                }
                                if (tasksDataClass.Priority == 1)
                                {
                                    command.Parameters.Add(new MySqlParameter("@Priority", 4));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@Priority", tasksDataClass.Priority));

                                }
                                command.Parameters.Add(new MySqlParameter("@RTPD", tasksDataClass.RepeatTimesPerDay));
                                command.Parameters.Add(new MySqlParameter("@Archived", tasksDataClass.Archived));
                                command.Parameters.Add(new MySqlParameter("@PP", tasksDataClass.Postpone));
                                command.Parameters.Add(new MySqlParameter("@RepeatDuration", tasksDataClass.RepeatDuration));
                                command.Parameters.Add(new MySqlParameter("@PC", tasksDataClass.PeriodicallyCycles));
                                if (tasksDataClass.SpecifiedDay == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@SD", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@SD", tasksDataClass.SpecifiedDay));

                                }
                                if (tasksDataClass.RelationalFile1 == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF1", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF1", tasksDataClass.RelationalFile1));

                                }

                                if (tasksDataClass.RelationalFile2 == "")
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF2", default));

                                }
                                else
                                {
                                    command.Parameters.Add(new MySqlParameter("@RF2", tasksDataClass.RelationalFile2));

                                }
                                command.Parameters.Add(new MySqlParameter("@ACFVBMMF", tasksDataClass.AutoCreateFirstVariableBranchesMMF));
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();

                            }

                        }




                    }

                    connection.Close();

                }



            }
            catch (MySqlException me)
            {
                Debug.WriteLine("ERROR: " + MethodBase.GetCurrentMethod().Name + me.Message);
                MessageBox.Show(MethodBase.GetCurrentMethod().Name + me.Message);
            }




        }



    }
}
