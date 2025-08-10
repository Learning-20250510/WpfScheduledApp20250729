using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.Models.DBs.TasksTable
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
        public ObservableCollection<DataClass> SelectTasksTemplate(string commandtext)
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
                                //Debug.WriteLine($"TaskCategory:{reader["taskcategory"]} TaskName:{reader["taskname"]} Description:{reader["description"]} EstimatedTime:{reader["estimated_time"]} DueDate:{reader["due_date"]} DueTime:{reader["due_time"]} Priority{reader["priority"]} RepeatTimesPerDay:{reader["repeat_times_per_day"]}");

                                TasksCollection.Add(new DataClass { ID = int.Parse(reader["id"].ToString()), KMN = reader["kmn"].ToString(), KMT = int.Parse(reader["kmt"].ToString()), HTL = int.Parse(reader["htl"].ToString()), PCP_ID = int.Parse(reader["pcp_id"].ToString()), Description = reader["description"].ToString(), EstimatedTime = int.Parse(reader["estimated_time"].ToString()), DueDate = reader["due_date"].ToString(), DueTime = reader["due_time"].ToString(), Priority = int.Parse(reader["priority"].ToString()), RepeatTimesPerDay = int.Parse(reader["repeat_times_per_day"].ToString()), RepeatTimesPerDayDummy = int.Parse(reader["repeat_times_per_day_dummy"].ToString()), CreatedAt = reader["created_at"].ToString(), LastClearedAt = reader["lastcleared_at"].ToString(), ClearTimesInTime = int.Parse(reader["clear_times_intime"].ToString()), ClearTimesOutOfTime = int.Parse(reader["clear_times_outoftime"].ToString()), Archived = Convert.ToBoolean(reader["archived"]), Postpone = Convert.ToBoolean(reader["postpone"]), RepeatDuration = int.Parse(reader["repeat_duration"].ToString()), PeriodicallyCycles = int.Parse(reader["periodically_cycles"].ToString()), SpecifiedDay = reader["specified_day"].ToString(), SpecificPageAsPDF = int.Parse(reader["specific_page_as_pdf"].ToString()), specificScrollValueAsWebPage = int.Parse(reader["specific_scrollvalue_as_webpage"].ToString()), TenSecondsIncrementAsSoundsAndMovie = int.Parse(reader["ten_seconds_increment_as_sounds_and_movie"].ToString()), TwoSecondsIncrementAsMovie = int.Parse(reader["two_seconds_increment_as_movie"].ToString()), RelationalFile1 = reader["relational_file_1"].ToString(), RelationalFile2 = reader["relational_file_2"].ToString(), AutoCreateFirstVariableBranchesMMF = Convert.ToBoolean(reader["auto_create_first_variable_branches_mmfile"]), RepeatPatterns = int.Parse(reader["repeat_patterns"].ToString()), TemporaryRepeatTaskCount = int.Parse(reader["temporary_repeat_task_count"].ToString())});


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
                return TasksCollection;
            }

        }
        public ObservableCollection<DataClass> GetTasksOfTodayBtn()
        {
            ObservableCollection<DataClass> TasksCollection = new ObservableCollection<DataClass>();
            try
            {



                using (var connection = new MySqlConnection(this.connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        //command.CommandText = "select * from simpletasksystem.tasks inner join simpletasksystem.taskcommonelements on tasks.kmn_kmt_htl_id = taskcommonelements.id where (due_date<=CURDATE() and due_time is null and repeat_times_per_day=1 and priority in(1,2)) or (periodically_cycles=2 and lastcleared_at<CURDATE()) or (periodically_cycles=@DayOfWeek and lastcleared_at<CURDATE()) or (periodically_cycles=@DayOfTwoWeeks and lastcleared_at < @Duration2Weeks) or (periodically_cycles=@WeekdayORWeekend and lastcleared_at < CURDATE()) limit 100";
                        //command.CommandText = "select t.id as t_id, t.*, tce.id as tce_id, tce.kmn as tce_kmn, tce.kmt as tce_kmt, tce.htl as tce_htl, htl.id as htl_id, htl.htl as htl_htl, kmt.id as kmt_id, kmt.kmt as kmt_kmt, pr.id as pr_id, pr.name as pr_name, pc.id as pc_id, pc.name as pc_name from simpletasksystem.tasks as t inner join simpletasksystem.priorities as pr on t.priority=pr.id inner join simpletasksystem.taskcommonelements as tce on tce.id=t.kmn_kmt_htl_id inner join simpletasksystem.htl on htl.id=tce.htl inner join simpletasksystem.kmt on kmt.id=tce.kmt inner join simpletasksystem.periodicallycycles as pc on pc.id=t.periodically_cycles where (t.due_date<=CURDATE() and t.due_time is null and t.repeat_times_per_day=1 and t.priority in(1,2)) OR (t.periodically_cycles=2 and t.lastcleared_at<CURDATE()) OR (t.periodically_cycles=@DayOfWeek and t.lastcleared_at<CURDATE()) OR (t.periodically_cycles=@DayOfTwoWeeks and t.lastcleared_at < @Duration2Weeks) OR (t.periodically_cycles=@WeekdayORWeekend and t.lastcleared_at < CURDATE())";
                        //command.CommandText = "select t.id as t_id, t.*, tce.id as tce_id, tce.kmn as tce_kmn, tce.kmt as tce_kmt, tce.htl as tce_htl, htl.id as htl_id, htl.htl as htl_htl, kmt.id as kmt_id, kmt.kmt as kmt_kmt, pr.id as pr_id, pr.name as pr_name, pc.id as pc_id, pc.name as pc_name FROM simpletasksystem.tasks as t inner join simpletasksystem.periodicallycycles as pc on pc.id=t.periodically_cycles inner join simpletasksystem.priorities as pr on t.priority=pr.id inner join simpletasksystem.taskcommonelements as tce on tce.id=t.kmn_kmt_htl_id inner join simpletasksystem.htl on htl.id=tce.htl inner join simpletasksystem.kmt on kmt.id=tce.kmt";
                        //command.CommandText = "select t.id as t_id, t.*, kmn.kmn as kmn_kmn, pcp.id as pcp_id, htl.id as htl_id, htl.htl as htl_htl, kmt.id as kmt_id, kmt.kmt as kmt_kmt, pr.id as pr_id, pr.name as pr_name, pc.id as pc_id, pc.name as pc_name FROM simpletasksystem.tasks as t inner join simpletasksystem.periodicallycycles as pc on pc.id=t.periodically_cycles inner join simpletasksystem.priorities as pr on t.priority=pr.id inner join simpletasksystem.pcp on t.pcp_id=pcp.id inner join simpletasksystem.htl on htl.id=t.htl inner join simpletasksystem.kmt on kmt.id=t.kmt inner join simpletasksystem.kmn on kmn.id=t.kmn";
                        command.CommandText = "select * from simpletasksystem.tasks where (due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) and due_date >= CURDATE()  and due_time is null and repeat_patterns in (1,2) and priority in(1,2) )";


                        UtilityForCreatingCommandText utilityForCreatingCommandText = new UtilityForCreatingCommandText();
                        command.Parameters.Add(new MySqlParameter("@DayOfWeek", utilityForCreatingCommandText.GetTodayIsValueOfPCCorrespondenceTable()));
                        command.Parameters.Add(new MySqlParameter("@DayOfTwoWeeks", utilityForCreatingCommandText.GetTodayIsValueOfPCCorrespondenceTable(2)));

                        DateTime dateTime = DateTime.Now;

                        command.Parameters.Add(new MySqlParameter("@Duration2Weeks", dateTime.AddDays(-14)));
                        command.Parameters.Add(new MySqlParameter("@WeekdayORWeekend", utilityForCreatingCommandText.GetTodayIsWeekdayORWeekendFromCorrespondenceTable()));

                        command.ExecuteNonQuery();




                        using (var reader = command.ExecuteReader())
                        {
                            // コンソールに表示
                            while (reader.Read())
                            {
                                Debug.WriteLine("始まる");
                                //Debug.WriteLine(reader["t_id"]);
                                //Debug.WriteLine(reader["kmt_kmt"]);
                                //TasksCollection.Add(new DataClass { ID = int.Parse(reader["t_id"].ToString()), KMN = int.Parse(reader["kmn"].ToString()), PCP_ID = int.Parse(reader["pcp_id"].ToString()), Description = reader["description"].ToString(), EstimatedTime = int.Parse(reader["estimated_time"].ToString()), DueDate = reader["due_date"].ToString(), DueTime = reader["due_time"].ToString(), Priority = int.Parse(reader["priority"].ToString()), RepeatTimesPerDay = int.Parse(reader["repeat_times_per_day"].ToString()), RepeatTimesPerDayDummy = int.Parse(reader["repeat_times_per_day_dummy"].ToString()), CreatedAt = reader["created_at"].ToString(), LastClearedAt = reader["lastcleared_at"].ToString(), ClearTimesInTime = int.Parse(reader["clear_times_intime"].ToString()), ClearTimesOutOfTime = int.Parse(reader["clear_times_outoftime"].ToString()), Archived = Convert.ToBoolean(reader["archived"]), Postpone = Convert.ToBoolean(reader["postpone"]), RepeatDuration = int.Parse(reader["repeat_duration"].ToString()), PeriodicallyCycles = int.Parse(reader["periodically_cycles"].ToString()), SpecifiedDay = reader["specified_day"].ToString(), SpecificPageAsPDF = int.Parse(reader["specific_page_as_pdf"].ToString()), specificScrollValueAsWebPage = int.Parse(reader["specific_scrollvalue_as_webpage"].ToString()), TenSecondsIncrementAsSoundsAndMovie = int.Parse(reader["ten_seconds_increment_as_sounds_and_movie"].ToString()), TwoSecondsIncrementAsMovie = int.Parse(reader["two_seconds_increment_as_movie"].ToString()), RelationalFile1 = reader["relational_file_1"].ToString(), RelationalFile2 = reader["relational_file_2"].ToString(), HTL_HTL = reader["htl_htl"].ToString(), HTL_ID = int.Parse(reader["htl_id"].ToString()), KMT_ID = int.Parse(reader["kmt_id"].ToString()), KMT_KMT = reader["kmt_kmt"].ToString(), PC_ID = int.Parse(reader["pc_id"].ToString()), PC_Name = reader["pc_name"].ToString(), PR_ID = int.Parse(reader["pr_id"].ToString()), PR_Name = reader["pr_name"].ToString(), KMT = int.Parse(reader["kmt"].ToString()), KMN_KMN = reader["kmn"].ToString(), HTL = int.Parse(reader["htl"].ToString()), ParentProjectName = reader["parent"].ToString(), ChildProjectName = reader["child"].ToString()});
                                TasksCollection.Add(new DataClass { ID = int.Parse(reader["id"].ToString()), KMN = reader["kmn"].ToString(), PCP_ID = int.Parse(reader["pcp_id"].ToString()), Description = reader["description"].ToString(), EstimatedTime = int.Parse(reader["estimated_time"].ToString()), DueDate = reader["due_date"].ToString(), DueTime = reader["due_time"].ToString(), Priority = int.Parse(reader["priority"].ToString()), RepeatTimesPerDay = int.Parse(reader["repeat_times_per_day"].ToString()), RepeatTimesPerDayDummy = int.Parse(reader["repeat_times_per_day_dummy"].ToString()), CreatedAt = reader["created_at"].ToString(), LastClearedAt = reader["lastcleared_at"].ToString(), ClearTimesInTime = int.Parse(reader["clear_times_intime"].ToString()), ClearTimesOutOfTime = int.Parse(reader["clear_times_outoftime"].ToString()), Archived = Convert.ToBoolean(reader["archived"]), Postpone = Convert.ToBoolean(reader["postpone"]), RepeatDuration = int.Parse(reader["repeat_duration"].ToString()), PeriodicallyCycles = int.Parse(reader["periodically_cycles"].ToString()), SpecifiedDay = reader["specified_day"].ToString(), SpecificPageAsPDF = int.Parse(reader["specific_page_as_pdf"].ToString()), specificScrollValueAsWebPage = int.Parse(reader["specific_scrollvalue_as_webpage"].ToString()), TenSecondsIncrementAsSoundsAndMovie = int.Parse(reader["ten_seconds_increment_as_sounds_and_movie"].ToString()), TwoSecondsIncrementAsMovie = int.Parse(reader["two_seconds_increment_as_movie"].ToString()), RelationalFile1 = reader["relational_file_1"].ToString(), RelationalFile2 = reader["relational_file_2"].ToString(), KMT = int.Parse(reader["kmt"].ToString()),  HTL = int.Parse(reader["htl"].ToString())});
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


        public ObservableCollection<DataClass> GetTasksOfTodayMUST_PC()
        {
            ObservableCollection<DataClass> TasksCollection = new ObservableCollection<DataClass>();
            try
            {


                DayOfWeek dateWeek = DateTime.Now.DayOfWeek;//今日の曜日を取得

                using (var connection = new MySqlConnection(this.connectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();

                        if (dateWeek == DayOfWeek.Sunday)
                        {
                            Debug.WriteLine("今日は日曜日です。");
                            command.CommandText = "select * from simpletasksystem.tasks where (due_time is null and repeat_times_per_day in(1,2) and archived=0 AND ( periodically_cycles in(2,9,18) OR (periodically_cycles=16 and lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -7 day) ) OR (periodically_cycles=19 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -31 day) ) OR (periodically_cycles=20 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -365 day) ) )  )";

                        }
                        else if (dateWeek == DayOfWeek.Monday)
                        {
                            Debug.WriteLine("今日は月曜日です。");
                            command.CommandText = "select * from simpletasksystem.tasks where (due_time is null and repeat_times_per_day in (1,2) and archived=0 AND ( periodically_cycles in(2,3,17) OR (periodically_cycles=10 and lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -7 day) ) OR (periodically_cycles=19 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -31 day) ) OR (periodically_cycles=20 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -365 day) ) )  )";
                        }
                        else if (dateWeek == DayOfWeek.Tuesday)
                        {
                            Debug.WriteLine("今日は火曜日です。");
                            command.CommandText = "select * from simpletasksystem.tasks where (due_time is null and repeat_times_per_day in (1,2) and archived=0 AND ( periodically_cycles in(2,4,17) OR (periodically_cycles=11 and lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -7 day) ) OR (periodically_cycles=19 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -31 day) ) OR (periodically_cycles=20 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -365 day) ) )  )";

                        }
                        else if (dateWeek == DayOfWeek.Wednesday)
                        {
                            Debug.WriteLine("今日は水曜日です。");
                            command.CommandText = "select * from simpletasksystem.tasks where (due_time is null and repeat_times_per_day in (1,2) and archived=0 AND ( periodically_cycles in(2,5,17) OR (periodically_cycles=12 and lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -7 day) ) OR (periodically_cycles=19 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -31 day) ) OR (periodically_cycles=20 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -365 day) ) )  )";

                        }
                        else if (dateWeek == DayOfWeek.Thursday)
                        {
                            Debug.WriteLine("今日は木曜日です。");
                            command.CommandText = "select * from simpletasksystem.tasks where (due_time is null and repeat_times_per_day in (1,2) and archived=0 AND ( periodically_cycles in(2,6,17) OR (periodically_cycles=13 and lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -7 day) ) OR (periodically_cycles=19 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -31 day) ) OR (periodically_cycles=20 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -365 day) ) )  )";

                        }
                        else if (dateWeek == DayOfWeek.Friday)
                        {
                            Debug.WriteLine("今日は金曜日です。");
                            command.CommandText = "select * from simpletasksystem.tasks where (due_time is null and repeat_times_per_day in (1,2) and archived=0 AND ( periodically_cycles in(2,7,17) OR (periodically_cycles=14 and lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -7 day) ) OR (periodically_cycles=19 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -31 day) ) OR (periodically_cycles=20 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -365 day) ) )  )";
                        }
                        else if (dateWeek == DayOfWeek.Saturday)
                        {
                            Debug.WriteLine("今日は土曜日です。");
                            command.CommandText = "select * from simpletasksystem.tasks where (due_time is null and repeat_times_per_day in (1,2) and archived=0 AND ( periodically_cycles in(2,8,18) OR (periodically_cycles=15 and lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -7 day) ) OR (periodically_cycles=19 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -31 day) ) OR (periodically_cycles=20 AND lastcleared_at < DATE_ADD(CURRENT_DATE, INTERVAL -365 day) ) )  )";

                        }

                        command.ExecuteNonQuery();


                        using (var reader = command.ExecuteReader())
                        {
                            // コンソールに表示
                            while (reader.Read())
                            {
                                Debug.WriteLine("始まる");
                                TasksCollection.Add(new DataClass { ID = int.Parse(reader["id"].ToString()), KMN = reader["kmn"].ToString(), PCP_ID = int.Parse(reader["pcp_id"].ToString()), Description = reader["description"].ToString(), EstimatedTime = int.Parse(reader["estimated_time"].ToString()), DueDate = reader["due_date"].ToString(), DueTime = reader["due_time"].ToString(), Priority = int.Parse(reader["priority"].ToString()), RepeatTimesPerDay = int.Parse(reader["repeat_times_per_day"].ToString()), RepeatTimesPerDayDummy = int.Parse(reader["repeat_times_per_day_dummy"].ToString()), CreatedAt = reader["created_at"].ToString(), LastClearedAt = reader["lastcleared_at"].ToString(), ClearTimesInTime = int.Parse(reader["clear_times_intime"].ToString()), ClearTimesOutOfTime = int.Parse(reader["clear_times_outoftime"].ToString()), Archived = Convert.ToBoolean(reader["archived"]), Postpone = Convert.ToBoolean(reader["postpone"]), RepeatDuration = int.Parse(reader["repeat_duration"].ToString()), PeriodicallyCycles = int.Parse(reader["periodically_cycles"].ToString()), SpecifiedDay = reader["specified_day"].ToString(), SpecificPageAsPDF = int.Parse(reader["specific_page_as_pdf"].ToString()), specificScrollValueAsWebPage = int.Parse(reader["specific_scrollvalue_as_webpage"].ToString()), TenSecondsIncrementAsSoundsAndMovie = int.Parse(reader["ten_seconds_increment_as_sounds_and_movie"].ToString()), TwoSecondsIncrementAsMovie = int.Parse(reader["two_seconds_increment_as_movie"].ToString()), RelationalFile1 = reader["relational_file_1"].ToString(), RelationalFile2 = reader["relational_file_2"].ToString(), KMT = int.Parse(reader["kmt"].ToString()), HTL = int.Parse(reader["htl"].ToString()) });
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
