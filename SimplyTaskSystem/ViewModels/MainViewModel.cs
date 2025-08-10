using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.ViewModels
{
    using SimplyTaskSystem.Views;
    using SimplyTaskSystem.ViewModels;
    using SimplyTaskSystem.Models;
    using SimplyTaskSystem.Models.DBs;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows;

    internal class MainViewModel : NotificationObject
    {
        public MainViewModel()
        {
            Debug.WriteLine("実験です。");

            ShowStatusViewModel showStatusViewModel = new ShowStatusViewModel();
            ShowStatusWindow showStatusWindow = new ShowStatusWindow();
            showStatusWindow.DataContext = showStatusViewModel;
            showStatusWindow.Show();

            Models.DBs.TasksTable.InitialSettingsInformation mdttisi = new Models.DBs.TasksTable.InitialSettingsInformation();
            System.Diagnostics.Debug.WriteLine(mdttisi.ConnectionString);
            Models.DBs.TasksTable.InitialSettingsInformation instance_a = new Models.DBs.TasksTable.InitialSettingsInformation();
            instance_a.CreateTasksTable();

            //ExperimentOpenAllWindowWithViewModel();
   
            Models.FilesOperation.PDFFileOperation pDFFileOperation = new Models.FilesOperation.PDFFileOperation();
            //pDFFileOperation.OpenPDFFSpecificPage("a", 0);

            
            Models.DBs.TasksTable.Read read = new Models.DBs.TasksTable.Read();
            TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(read.SelectTasksTemplate("select * from simpletasksystem.tasks limit 50"));


            Models.DBs.DBsSetup dBsSetup = new Models.DBs.DBsSetup();
            dBsSetup.createMustTables();
            dBsSetup.insertInitialRecords();

            
            /*
            Models.FilesOperation.FreePlaneFileOperation freePlaneFileOperation = new Models.FilesOperation.FreePlaneFileOperation();
            freePlaneFileOperation.newCreateXml("TheWorld", "桜を見に行く", "AnywayOfTheWorld");

            */


            /*
            var insert = new Models.DBs.TasksTable.Insert();
            insert.InsertRecordsFromMemoURLs();
            insert.InsertRecordsFromPriorityWant1stMemoURLs();
            insert.InsertRecordsFromPriorityWant2ndMemoURLs();

            */

        }


        private DelegateCommand _searchBtnCommand;
        public DelegateCommand SearchBtnCommand
        {
            get
            {
                return this._searchBtnCommand ?? (this._searchBtnCommand = new DelegateCommand(
                _ =>
                {
                    CommandText = "select * from simpletasksystem.tasks where ";
                    CheckValidationControllOfInt(this.ID_TextBox, "id");
                    //CheckValidationControllOfStringOfLikesearch(this.KMN_Text_Box, "kmn");
                    CheckValidationControllOfString(this.KMN_Text_Box, "kmn");

                    CheckValidationPROfBool();
                    CheckValidationPCOfBool();
                    CheckValidationHTLOfBool();
                    CheckValidationKMTOfBool();

                    CheckValidationControllOfInt(this.EstimatedTime_TextBox, "estimated_time");
                    CheckValidationControllOfString(this.DueDate_TextBox, "due_date");
                    CheckValidationControllOfString(this.DueTime_TextBox, "due_time");
                    CheckValidationControllOfString(this.RTPD_TextBox, "repeat_time_per_day");
                    CheckValidationControllOfString(this.RTPDD_TextBox, "repeat_time_per_day_dummy");
                    CheckValidationControllOfString(this.CreatedAt_TextBox, "created_at");
                    CheckValidationControllOfString(this.LastClearedAt_TextBox, "lastcleared_at");
                    CheckValidationControllOfString(this.Archived_TextBox, "archived");
                    CheckValidationControllOfString(this.Postpone_TextBox, "postpone");
                    CheckValidationControllOfInt(this.RepeatDuration_TextBox, "repeat_duration");
                    CheckValidationControllOfString(this.SpecifiedDay_TextBox, "specified_day");
                    CheckValidationControllOfInt(this.SpecificPageAsPDF_TextBox, "specific_page_as_pdf");
                    CheckValidationControllOfInt(this.SSAW_TextBox, "specific_scrollvalue_as_webpage");
                    CheckValidationControllOfInt(this.TSIASAM_TextBox, "ten_seconds_increment_as_sounds_and_movie");
                    CheckValidationControllOfInt(this.TSIAM_TextBox, "two_seconds_increment_as_movie");
                    CheckValidationControllOfString(this.RF1_TextBox, "relational_file_1");
                    CheckValidationControllOfString(this.RF2_TextBox, "relational_file_2");
                    //Remove "and"
                    if (CommandText.Length >= 3)
                    {
                        Debug.WriteLine("最終的なクエリ： " + CommandText);

                        CommandText = CommandText.Remove(CommandText.Length - 4);

                    }
                    CheckValidationControllOfIntOfLimit(LimitNumber_TextBox);

                    //Remove "and"
                    if (CommandText.Length >= 3)
                    {
                        Debug.WriteLine("最終的なクエリ： " + CommandText);

                        CommandText = CommandText.Remove(CommandText.Length - 4);

                    }



                    Debug.WriteLine("最終的なクエリ： " + CommandText);
                    MessageBox.Show("最終的なクエリ： " + CommandText);
                    var tasksRead = new Models.DBs.TasksTable.Read();
                    TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(tasksRead.SelectTasksTemplate(CommandText));
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }



        private bool _allCheckBoxCommand;
        public bool AllCheckBoxCommand
        {
            get
            {
                return this._allCheckBoxCommand;
            }
            set
            {
                if (SetProperty(ref this._allCheckBoxCommand, value))
                {
  
                    if (AllCheckBoxCommand == true)
                    {

                        foreach (var li in TasksCollection)
                        {
                            li.CheckedBox = true;
                        }

                    }
                    else
                    {

                        foreach (var li in TasksCollection)
                        {
                            li.CheckedBox = false;
                        }
                    }

                }
            }
        }


        private bool _autoActMode_CheckBox;
        public bool AutoActMode_CheckBox
        {
            get
            {
                return this._autoActMode_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._autoActMode_CheckBox, value))
                {

                }
            }
        }





        private DelegateCommand _checkAllBtnCommand;
        public DelegateCommand CheckAllBtnCommand
        {
            get
            {
                return this._checkAllBtnCommand ?? (this._checkAllBtnCommand = new DelegateCommand(
                _ =>
                {
                    if (! (KMT_Unclassified_CheckBox == true && KMT_TheWorld_CheckBox == true) )
                    {
                        KMT_Unclassified_CheckBox = true;
                        KMT_TheWorld_CheckBox = true;
                        KMT_MyBrain_ChecBox = true;
                        KMT_WebPage_CheckBox = true;
                        KMT_FreePlane_CheckBox = true;
                        KMT_PDF_CheckBox = true;
                        KMT_Research_CheckBox = true;

                        HTL_Unclassified_CheckBox = true;
                        HTL_FCBW10OM_CheckBox = true;
                        HTL_FCISIOM_CheckBox = true;
                        HTL_FCISPOPDF_CheckBox = true;
                        HTL_FDISPOPDF_CheckBox = true;
                        HTL_AnywayOfTheWorld_CheckBox = true;
                        HTL_FCISVOWP_CheckBox = true;
                        HTL_FDISVOWP_CheckBox = true;
                        HTL_CMMFASBAMMFGOW_CheckBox = true;
                        HTL_CMMFDFBBAMMFGOWP_CheckBox = true;
                        HTL_CMMFFCWOCRBAMMFGOPDF_CheckBox = true;
                        HTL_FocusTheKMNWithHavingAnIdeaMyBrain_CheckBox = true;
                        HTL_FindKMFromSomeURLAutomaticallyOfResearch_CheckBox = true;
                        HTL_FocusContextBetWeen10SecondsOfSound_CheckBox = true;
                        HTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox = true;
                        HTL_FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane_CheckBox = true;
                        HTL_FindTasksNTimesFromTheMMFileOfFreePlane_CheckBox = true;
                        HTL_ReadyForVariablesListLearningOfTheWorld_CheckBox = true;
                        HTL_ReviewOfTheWorldOfMyBrain_CheckBox = true;


                    }
                    else
                    {
                        //AllCheckBox = false;
                        KMT_Unclassified_CheckBox = false;
                        KMT_TheWorld_CheckBox = false;
                        KMT_MyBrain_ChecBox = false;
                        KMT_WebPage_CheckBox = false;
                        KMT_FreePlane_CheckBox = false;
                        KMT_PDF_CheckBox = false;
                        KMT_Research_CheckBox = false;

                        HTL_Unclassified_CheckBox = false;
                        HTL_FCBW10OM_CheckBox = false;
                        HTL_FCISIOM_CheckBox = false;
                        HTL_FCISPOPDF_CheckBox = false;
                        HTL_FDISPOPDF_CheckBox = false;
                        HTL_AnywayOfTheWorld_CheckBox = false;
                        HTL_FCISVOWP_CheckBox = false;
                        HTL_FDISVOWP_CheckBox = false;

                        HTL_CMMFASBAMMFGOW_CheckBox = false;
                        HTL_CMMFDFBBAMMFGOWP_CheckBox = false;
                        HTL_CMMFFCWOCRBAMMFGOPDF_CheckBox = false;

                        HTL_FocusTheKMNWithHavingAnIdeaMyBrain_CheckBox = false;
                        HTL_FindKMFromSomeURLAutomaticallyOfResearch_CheckBox = false;

                        HTL_FocusContextBetWeen10SecondsOfSound_CheckBox = false;
                        HTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox = false;
                        HTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox = false;
                        HTL_FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane_CheckBox = false;
                        HTL_FindTasksNTimesFromTheMMFileOfFreePlane_CheckBox = false;
                        HTL_ReadyForVariablesListLearningOfTheWorld_CheckBox = false;
                        HTL_ReviewOfTheWorldOfMyBrain_CheckBox = false;


                    }
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }

        private DelegateCommand _todayBtnCommand;
        public DelegateCommand TodayBtnCommand
        {
            get
            {
                return this._todayBtnCommand ?? (this._todayBtnCommand = new DelegateCommand(
                _ =>
                {
                    Models.DBs.TasksTable.Read read = new Models.DBs.TasksTable.Read();
                    TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(read.GetTasksOfTodayBtn());

                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }

        private DelegateCommand _todayTimeIsNotNullBtnCommand;
        public DelegateCommand TodayTimeIsNotNullBtnCommand
        {
            get
            {
                return this._todayTimeIsNotNullBtnCommand ?? (this._todayTimeIsNotNullBtnCommand = new DelegateCommand(
                _ =>
                {
                    Models.DBs.TasksTable.Read read = new Models.DBs.TasksTable.Read();
                    TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(read.SelectTasksTemplate("select * from simpletasksystem.tasks where due_time is not null"));
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }

        private DelegateCommand _unscheduledBtnCommand;
        public DelegateCommand UnscheduledBtnCommand
        {
            get
            {
                return this._unscheduledBtnCommand ?? (this._unscheduledBtnCommand = new DelegateCommand(
                _ =>
                {
                    Models.DBs.TasksTable.Read read = new Models.DBs.TasksTable.Read();
                    //TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(read.SelectTasksTemplate("select * from simpletasksystem.tasks where childrenproject_id=1"));
                    TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(read.SelectTasksTemplate("select * from simpletasksystem.tasks where due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) or kmt=1 or priority=1"));
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }

        private DelegateCommand _overdueBtnCommand;
        public DelegateCommand OverdueBtnCommand
        {
            get
            {
                return this._overdueBtnCommand ?? (this._overdueBtnCommand = new DelegateCommand(
                _ =>
                {
                    Models.DBs.TasksTable.Read read = new Models.DBs.TasksTable.Read();
                    TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(read.SelectTasksTemplate("select * from simpletasksystem.tasks where due_date < CURDATE()"));
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }

        private DelegateCommand _rTPDOTBtnCommand;
        public DelegateCommand RTPDOTBtnCommand
        {
            get
            {
                return this._rTPDOTBtnCommand ?? (this._rTPDOTBtnCommand = new DelegateCommand(
                _ =>
                {
                    Models.DBs.TasksTable.Read read = new Models.DBs.TasksTable.Read();
                    TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(read.SelectTasksTemplate("select * from simpletasksystem.tasks where due_date <= CURDATE() and repeat_times_per_day > 1"));
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }

        private DelegateCommand _updateAllCheckedBoxRecordsBtnCommnad;
        public DelegateCommand UpdateAllCheckedBoxRecordsBtnCommand
        {
            get
            {
                return this._updateAllCheckedBoxRecordsBtnCommnad ?? (this._updateAllCheckedBoxRecordsBtnCommnad = new DelegateCommand(
                    _ =>
                    {
                        var updateAllWindow = new UpdateAllCheckedRecordsWindow();

                        var intList = new List<int>();

                        foreach (var li in TasksCollection)
                        {
                            if (li.CheckedBox == true)
                            {
                                intList.Add(li.ID);
                            }
                        }
                        var updateAllWindowViewModel = new UpdateAllCheckedRecordsViewModel(intList);
                        updateAllWindow.DataContext = updateAllWindowViewModel;
                        updateAllWindow.Show();

                        MessageBox.Show("AllCheckeTaskを指定された値に更新しました。");
                    },
                    _ =>
                    {
                        return true;
                    }
                    ));
            }
        }

        private DelegateCommand _updateFromKMTIsUnclassifiedToMyBrainAndResearchBtnCommand;
        public DelegateCommand UpdateFromKMTIsUnclassifiedToMyBrainAndResearchBtnCommand
        {
            get
            {
                return this._updateFromKMTIsUnclassifiedToMyBrainAndResearchBtnCommand ?? (this._updateFromKMTIsUnclassifiedToMyBrainAndResearchBtnCommand = new DelegateCommand(
                    _ =>
                    {
                        MessageBoxResult result = MessageBox.Show("ほんとうに、ＫＭＴ=Unclassifiedのレコードすべてを'KMT = MyBrain, HTL = FocusTheKMNWithHavingAnIdeaMyBrain'、'KMT=Research, HTL=...'に変更しますか？", "メッセージボックス", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            MessageBoxResult result_2 = MessageBox.Show("やっぱりやめておきますか？", "メッセージボックス", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (result_2 == MessageBoxResult.No)
                            {
                                MessageBox.Show("無事に、動作が完了しました。");
                            }
                        }
                        else
                      {

                        }
                    },
                    _ =>
                    {
                        return true;
                    }
                    ));
            }
        }

        private DelegateCommand _insertFromDummyTasksTableBtnCommand;
        public DelegateCommand InsertFromDummyTasksTableBtnCommand
        {
            get
            {
                return this._insertFromDummyTasksTableBtnCommand ?? (this._insertFromDummyTasksTableBtnCommand = new DelegateCommand(
                    _ =>
                    {
                        MessageBoxResult result = MessageBox.Show("ほんとうにDummyTasksTableのレコードすべてをTasksTableにMM系（３つ）としてInsertしますか？", "メッセージボックス", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            MessageBoxResult result_2 = MessageBox.Show("やっぱりやめておきますか？", "メッセージボックス", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                            if (result_2 == MessageBoxResult.No)
                            {
                                //DummyTasksTableの全レコードをTasksTableへとInsertしていく。
                                var dummyTasks = new Models.DBs.DummyTasksTable.Insert();
                                dummyTasks.InsertRecordsToTasksTableOfFreePlane();

                                MessageBox.Show("無事に、動作が完了しました。");
                            }
                        }
                        else
                        {

                        }
                    },
                    _ =>
                    {
                        return true;
                    }
                    ));
            }
        }



        private DelegateCommand _openInsertPCPWindowBtnCommand;
        public DelegateCommand OpenInsertPCPWindowBtnCommand
        {
            get
            {
                return this._openInsertPCPWindowBtnCommand ?? (this._openInsertPCPWindowBtnCommand = new DelegateCommand(
                    _ =>
                    {
                        var pcpWindow = new ParentChildProjectAddWindow();
                        var pcpVM = new ParentChildProjectAddViewModel();
                        pcpWindow.DataContext = pcpVM;
                        pcpWindow.Show();
                    },
                    _ =>
                    {
                        return true;
                    }
                    ));
            }
        }


        private DelegateCommand _updateAllCheckedBoxRecordsToTheWorldMyBrainResearchBtnCommand;
        public DelegateCommand UpdateAllCheckedBoxRecordsToTheWorldMyBrainResearchBtnCommand
        {
            get
            {
                return this._updateAllCheckedBoxRecordsToTheWorldMyBrainResearchBtnCommand ?? (this._updateAllCheckedBoxRecordsToTheWorldMyBrainResearchBtnCommand = new DelegateCommand(
                    _ =>
                    {
                        var insert = new Models.DBs.TasksTable.Insert();
                        var delete = new Models.DBs.TasksTable.Delete();

                        foreach (var li in TasksCollection)
                        {
                            //渡されたIDすべてのレコードのKMT,HTLがUnClassifedともにそうなっているかCheckして、でなければエラー
                            if (li.CheckedBox == true)
                            {
                                if (!(li.KMT == 1 || li.HTL == 1))
                                {
                                    MessageBox.Show(li.ID + " は、KMT or HTLがUnclassifiedではないため、この一括変更機能はご利用いただけません。個別に変更してください。");
                                }
                                else
                                {
                                    //上記レコードをMyBrain, Research,TheWorld系としてInsertする。
                                    insert.InsertTasksTheWorldMyBrainResearch(li.ID, true, true, true);
                                    //Templateとなったレコードを削除
                                    delete.DeleteRecordFromTasksTableByID(li.ID);

                                }
                            }
                        }



                    },
                    _ =>
                    {
                        return true;
                    }
                    ));
            }
        }



        private DelegateCommand _updateAllCheckedBoxRecordsToMyBrainResearchBtnCommand;
        public DelegateCommand UpdateAllCheckedBoxRecordsToMyBrainResearchBtnCommand
        {
            get
            {
                return this._updateAllCheckedBoxRecordsToMyBrainResearchBtnCommand ?? (this._updateAllCheckedBoxRecordsToMyBrainResearchBtnCommand = new DelegateCommand(
                    _ =>
                    {
                        var insert = new Models.DBs.TasksTable.Insert();
                        var delete = new Models.DBs.TasksTable.Delete();

                        foreach (var li in TasksCollection)
                        {
                            //渡されたIDすべてのレコードのKMT,HTLがUnClassifedともにそうなっているかCheckして、でなければエラー
                            if (li.CheckedBox == true)
                            {
                                if (!(li.KMT == 1 || li.HTL == 1))
                                {
                                    MessageBox.Show(li.ID + " は、KMT or HTLがUnclassifiedではないため、この一括変更機能はご利用いただけません。個別に変更してください。");
                                }
                                else
                                {
                                    //上記レコードをMyBrain, Research,TheWorld系としてInsertする。
                                    insert.InsertTasksTheWorldMyBrainResearch(li.ID, false, true, true);
                                    //Templateとなったレコードを削除
                                    delete.DeleteRecordFromTasksTableByID(li.ID);

                                }
                            }
                        }
                    },
                    _ =>
                    {
                        return true;
                    }
                    ));
            }
        }


        private string CommandText = "";

        private void CheckValidationControllOfInt(string controller, string columnName, bool endPoint=false)
        {



            int dummyInt;
            if (int.TryParse(controller, out dummyInt))
            {
                if (endPoint == false)
                {
                    CommandText += columnName + "=" + dummyInt + " and ";
                }
                else
                {
                    CommandText += columnName + "=" + dummyInt;
                }
                Debug.WriteLine(CommandText);
            }
            else
            {
                if (controller == "NULL")
                {
                    CommandText += columnName + " IS NULL";
                    Debug.WriteLine(CommandText);
                }
                else if (controller == "NOTNULL")
                {
                    CommandText += columnName + " IS NOT NULL";
                    Debug.WriteLine(CommandText);
                }
                else if (controller == "" || controller == null)
                {
                    CommandText += "";
                }
                
                else
                {
                    MessageBox.Show(controller + " に不適切な値が代入されています。");
                    CommandText = "";//初期化
                }


            }
        }


        private void CheckValidationControllOfIntOfLimit(string controller, bool endPoint = false)
        {



            int dummyInt;
            string statementName = "limit ";
            if (int.TryParse(controller, out dummyInt))
            {
                if (endPoint == false)
                {
                    CommandText += statementName + dummyInt + " and ";
                }
                else
                {
                    CommandText += statementName + dummyInt;
                }
                Debug.WriteLine(CommandText);
            }
            else
            {
                MessageBox.Show(controller + " というLimit句に不適切な値が代入されています。");


            }
        }

        private void CheckValidationControllOfString(string controller, string columnName, bool endPoint=false)
        {

            if (controller == "NULL")
            {
                CommandText += columnName + " IS NULL";
                Debug.WriteLine(CommandText);
            }
            else if (controller == "NOTNULL")
            {
                CommandText += columnName + " IS NOT NULL";
                Debug.WriteLine(CommandText);
            }
            else if (controller == "" || controller == " " || controller == null)
            {
                CommandText += "";
            }
            else
            {
                CommandText += columnName + "=" + controller;
                Debug.WriteLine(CommandText);
            }

            if (endPoint == false && !((controller == "NULL" || controller == "NOTNULL" || controller == "" || controller == " " || controller == null)))
            {
                CommandText += " and ";
                Debug.WriteLine(CommandText);
            }
        }

        private void CheckValidationControllOfStringOfLikesearch(string controller, string columnName, bool endPoint = false)
        {

            if (controller == "NULL")
            {
                CommandText += columnName + " IS NULL";
                Debug.WriteLine(CommandText);
            }
            else if (controller == "NOTNULL")
            {
                CommandText += columnName + " IS NOT NULL";
                Debug.WriteLine(CommandText);
            }
            else if (controller == "" || controller == " " || controller == null)
            {
                CommandText += "";
            }
            else
            {
                CommandText += columnName + " '%" + controller + "%'";
                Debug.WriteLine(CommandText);
            }

            if (endPoint == false && !((controller == "NULL" || controller == "NOTNULL" || controller == "" || controller == " " || controller == null)))
            {
                CommandText += " and ";
                Debug.WriteLine(CommandText);
            }
        }


        private void CheckValidationControllOfBool(bool controller, string columnName)
        {
            if (controller == true)
            {
                CommandText += columnName + "=true";
            }
            else
            {
                CommandText += columnName + "=false";
            }
            Debug.WriteLine(CommandText);
            CommandText += ") ) and ";


        }

        private void CheckValidationPCOfBool()
        {
            if (!(PC_Nothing_CheckBox == false && PC_Everyday_CheckBox == false && PC_EveryMonday_CheckBox == false && PC_EveryTuesday_CheckBox == false && PC_EveryWednesday_CheckBox == false && PC_EveryThursday_CheckBox == false && PC_EveryFriday_CheckBox == false && PC_EverySaturday_CheckBox == false && PC_EverySunday_CheckBox == false && PC_EveryTwoWeeksOnAMonday_CheckBox == false && PC_EveryTwoWeeksOnATuesday_CheckBox == false && PC_EveryTwoWeeksOnAWednesday_CheckBox == false && PC_EveryTwoWeeksOnAThursday_CheckBox == false && PC_EveryTwoWeeksOnAFriday_CheckBox == false && PC_EveryTwoWeeksOnASaturday_CheckBox == false && PC_EveryTwoWeeksOnASunday_CheckBox == false && PC_Weekday_CheckBox == false && PC_Weekend_CheckBox == false && PC_EveryMonthOnRandomDay_CheckBox == false && PC_EveryYearOnRandomDay_CheckBox == false && PC_EveryMonthOnSpecifiedDay_CheckBox == false && PC_EveryYearOnSpecifiedDay_CheckBox == false))
            {
                CommandText += "( periodically_cycles in (";
                if (PC_Nothing_CheckBox == true)
                {
                    CommandText += "1, ";
                }
                if (PC_Everyday_CheckBox == true)
                {
                    CommandText += "2, ";
                }
                if (PC_EveryMonday_CheckBox == true)
                {
                    CommandText += "3, ";
                }
                if (PC_EveryTuesday_CheckBox == true)
                {
                    CommandText += "4, ";
                }
                if (PC_EveryWednesday_CheckBox == true)
                {
                    CommandText += "5, ";
                }
                if (PC_EveryThursday_CheckBox == true)
                {
                    CommandText += "6, ";
                }
                if (PC_EveryFriday_CheckBox == true)
                {
                    CommandText += "7, ";
                }
                if (PC_EverySaturday_CheckBox == true)
                {
                    CommandText += "8, ";
                }
                if (PC_EverySunday_CheckBox == true)
                {
                    CommandText += "9, ";
                }
                if (PC_EveryTwoWeeksOnAMonday_CheckBox == true)
                {
                    CommandText += "10, ";
                }
                if (PC_EveryTwoWeeksOnATuesday_CheckBox == true)
                {
                    CommandText += "11, ";
                }
                if (PC_EveryTwoWeeksOnAWednesday_CheckBox == true)
                {
                    CommandText += "12, ";
                }
                if (PC_EveryTwoWeeksOnAThursday_CheckBox == true)
                {
                    CommandText += "13, ";
                }
                if( PC_EveryTwoWeeksOnAFriday_CheckBox == true)
                {
                    CommandText += "14, ";
                }
                if (PC_EveryTwoWeeksOnASaturday_CheckBox == true)
                {
                    CommandText += "15, ";
                }
                if (PC_EveryTwoWeeksOnASunday_CheckBox == true)
                {
                    CommandText += "16, ";
                }
                if (PC_Weekday_CheckBox == true)
                {
                    CommandText += "17, ";
                }
                if (PC_Weekend_CheckBox == true)
                {
                    CommandText += "18, ";
                }
                if (PC_EveryMonthOnRandomDay_CheckBox == true)
                {
                    CommandText += "19, ";
                }
                if (PC_EveryYearOnRandomDay_CheckBox == true)
                {
                    CommandText += "20, ";
                }
                if (PC_EveryMonthOnSpecifiedDay_CheckBox == true)
                {
                    CommandText += "21, ";
                }
                if (PC_EveryYearOnSpecifiedDay_CheckBox == true)
                {
                    CommandText += "22, ";
                }
                CommandText = CommandText.Remove(CommandText.Length - 2);

                CommandText += ") ) and ";
            }
        }


        private void CheckValidationHTLOfBool()
        {
            if (!(HTL_Unclassified_CheckBox == false && HTL_FCBW10OM_CheckBox == false && HTL_FCISIOM_CheckBox == false && HTL_FCISPOPDF_CheckBox == false && HTL_FDISPOPDF_CheckBox == false && HTL_AnywayOfTheWorld_CheckBox == false && HTL_FCISVOWP_CheckBox == false && HTL_FDISVOWP_CheckBox == false && HTL_CMMFDFBBAMMFGOWP_CheckBox == false && HTL_CMMFASBAMMFGOW_CheckBox == false && HTL_CMMFFCWOCRBAMMFGOPDF_CheckBox == false && HTL_FocusTheKMNWithHavingAnIdeaMyBrain_CheckBox == false && HTL_FindKMFromSomeURLAutomaticallyOfResearch_CheckBox == false && HTL_FocusContextBetWeen10SecondsOfSound_CheckBox == false && HTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox == false && HTL_FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane_CheckBox == false && HTL_FindTasksNTimesFromTheMMFileOfFreePlane_CheckBox == false && HTL_ReadyForVariablesListLearningOfTheWorld_CheckBox == false && HTL_ReviewOfTheWorldOfMyBrain_CheckBox == false ))
            {
                CommandText += "( htl in (";
                if (HTL_Unclassified_CheckBox == true)
                {
                    CommandText += "1, ";
                }
                if (HTL_FCBW10OM_CheckBox == true)
                {
                    CommandText += "2, ";
                }
                if (HTL_FCISIOM_CheckBox == true)
                {
                    CommandText += "3, ";
                }
                if (HTL_FCISPOPDF_CheckBox == true)
                {
                    CommandText += "4, ";
                }
                if (HTL_FDISPOPDF_CheckBox == true)
                {
                    CommandText += "5, ";
                }
                if (HTL_AnywayOfTheWorld_CheckBox == true)
                {
                    CommandText += "6, ";
                }
                if (HTL_FCISVOWP_CheckBox == true)
                {
                    CommandText += "7, ";
                }
                if (HTL_FDISVOWP_CheckBox == true)
                {
                    CommandText += "8, ";
                }
                if (HTL_CMMFDFBBAMMFGOWP_CheckBox == true)
                {
                    CommandText += "9, ";
                }
                if (HTL_CMMFASBAMMFGOW_CheckBox == true)
                {
                    CommandText += "10, ";
                }
                if (HTL_CMMFFCWOCRBAMMFGOPDF_CheckBox == true)
                {
                    CommandText += "11, ";
                }
                if (HTL_FocusTheKMNWithHavingAnIdeaMyBrain_CheckBox == true)
                {
                    CommandText += "12, ";
                }
                if (HTL_FindKMFromSomeURLAutomaticallyOfResearch_CheckBox == true)
                {
                    CommandText += "13, ";
                }
                if (HTL_FocusContextBetWeen10SecondsOfSound_CheckBox == true)
                {
                    CommandText += "14, ";
                }
                if (HTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox == true)
                {
                    CommandText += "15, ";
                }
                if (HTL_FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane_CheckBox == true)
                {
                    CommandText += "16, ";
                }
                if (HTL_FindTasksNTimesFromTheMMFileOfFreePlane_CheckBox == true)
                {
                    CommandText += "17, ";
                }
                if (HTL_ReadyForVariablesListLearningOfTheWorld_CheckBox == true)
                {
                    CommandText += "18, ";
                }
                if (HTL_ReviewOfTheWorldOfMyBrain_CheckBox == true)
                {
                    CommandText += "19, ";
                }

                CommandText = CommandText.Remove(CommandText.Length - 2);

                CommandText += ") ) and ";
            }
        }

        private void CheckValidationPROfBool()
        {
            if ( !(PR_Unclassified_CheckBox== false && PR_Today_CheckBox == false && PR_InAFewDays_CheckBox == false && PR_Want1st_CheckBox == false && PR_Want2nd_CheckBox == false && PR_Play_CheckBox == false) )
            {
                CommandText += "( priority in (";
                if (PR_Unclassified_CheckBox == true)
                {
                    CommandText += "1, ";
                }
                if (PR_Today_CheckBox == true)
                {
                    CommandText += "2, ";
                }
                if (PR_InAFewDays_CheckBox == true)
                {
                    CommandText += "3, ";
                }
                if (PR_Want1st_CheckBox == true)
                {
                    CommandText += "4, ";
                }
                if (PR_Want2nd_CheckBox == true)
                {
                    CommandText += "5, ";
                }
                if (PR_Play_CheckBox == true)
                {
                    CommandText += "6, ";
                }

                if (PR_JustNow_CheckBox == true)
                {
                    CommandText += "7, ";
                }
                if (PR_WithinAWeek_CheckBox == true)
                {
                    CommandText += "8, ";
                }
                if (PR_WithinTwoWeeks_CheckBox == true)
                {
                    CommandText += "9, ";
                }
                if (PR_WithinAMonth_CheckBox == true)
                {
                    CommandText += "10, ";
                }
                if (PR_WithinThreeMonthes_CheckBox == true)
                {
                    CommandText += "11, ";
                }
                if (PR_WithinHalfAYear_CheckBox == true)
                {
                    CommandText += "12, ";
                }
                if (PR_WithinAYear_CheckBox == true)
                {
                    CommandText += "13, ";
                }
                if (PR_Exercise_CheckBox == true)
                {
                    CommandText += "14, ";
                }


                CommandText = CommandText.Remove(CommandText.Length - 2);
                Debug.WriteLine("途中  ；" + CommandText);
                CommandText += ") ) and ";
            }
        }

        private void CheckValidationKMTOfBool()
        {
            if (!(KMT_Unclassified_CheckBox == false && KMT_TheWorld_CheckBox == false && KMT_MyBrain_ChecBox == false && KMT_WebPage_CheckBox == false && KMT_FreePlane_CheckBox == false && KMT_PDF_CheckBox == false && KMT_Research_CheckBox == false))
            {
                CommandText += "( kmt in (";
                if (KMT_Unclassified_CheckBox == true)
                {
                    CommandText += "1, ";
                }
                if (KMT_TheWorld_CheckBox == true)
                {
                    CommandText += "2, ";
                }
                if (KMT_MyBrain_ChecBox == true)
                {
                    CommandText += "3, ";
                }
                if (KMT_WebPage_CheckBox == true)
                {
                    CommandText += "4, ";
                }
                if (KMT_FreePlane_CheckBox == true)
                {
                    CommandText += "5, ";
                }
                if (KMT_PDF_CheckBox == true)
                {
                    CommandText += "6, ";
                }
                if (KMT_Research_CheckBox == true)
                {
                    CommandText += "7, ";
                }

                CommandText = CommandText.Remove(CommandText.Length - 2);
                Debug.WriteLine("途中  ；" + CommandText);
                CommandText += ") ) and ";
            }
        }

    

        private string _iD_TextBox;
        public string ID_TextBox
        {
            get
            {
                return this._iD_TextBox;
            }
            set
            {
                if (SetProperty(ref this._iD_TextBox, value))
                {
          
                }
            }
        }

   

        private string _kMN_TextBox;
        public string KMN_Text_Box
        {
            get
            {
                return this._kMN_TextBox;
            }
            set
            {
                if (SetProperty(ref this._kMN_TextBox, value))
                {

                }
            }
        }

        private bool _kMT_Unclassified_CheckBox;
        public bool KMT_Unclassified_CheckBox
        {
            get
            {
                return this._kMT_Unclassified_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._kMT_Unclassified_CheckBox, value))
                {
                }
            }
        }

        private bool _kMT_TheWorld_CheckBox;
        public bool KMT_TheWorld_CheckBox
        {
            get
            {
                return this._kMT_TheWorld_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._kMT_TheWorld_CheckBox, value))
                {
                }
            }
        }

        private bool _kMT_MyBrain_ChecBox;
        public bool KMT_MyBrain_ChecBox
        {
            get
            {
                return this._kMT_MyBrain_ChecBox;
            }
            set
            {
                if (SetProperty(ref this._kMT_MyBrain_ChecBox, value))
                {

                }
            }
        }

        private bool _kMT_WebPage_CheckBox;
        public bool KMT_WebPage_CheckBox
        {
            get
            {
                return this._kMT_WebPage_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._kMT_WebPage_CheckBox, value))
                {

                }
            }
        }

        private bool _kMT_FreePlane_CheckBox;
        public bool KMT_FreePlane_CheckBox
        {
            get
            {
                return this._kMT_FreePlane_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._kMT_FreePlane_CheckBox, value))
                {

                }
            }
        }

        private bool _kMT_PDF_CheckBox;
        public bool KMT_PDF_CheckBox
        {
            get
            {
                return this._kMT_PDF_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._kMT_PDF_CheckBox, value))
                {

                }
            }
        }

        private bool _kMT_Research_CheckBox;
        public bool KMT_Research_CheckBox
        {
            get
            {
                return this._kMT_Research_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._kMT_Research_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_Unclassified_CheckBox;
        public bool HTL_Unclassified_CheckBox
        {
            get
            {
                return this._hTL_Unclassified_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_Unclassified_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_FCBW10OM_CheckBox;
        public bool HTL_FCBW10OM_CheckBox
        {
            get
            {
                return this._hTL_FCBW10OM_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FCBW10OM_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_FCISIOM_CheckBox;
        public bool HTL_FCISIOM_CheckBox
        {
            get
            {
                return this._hTL_FCISIOM_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FCISIOM_CheckBox, value))
                {

                }
            }
        }

        private bool _hTl_FCISPOPDF_CheckBox;
        public bool HTL_FCISPOPDF_CheckBox
        {
            get
            {
                return this._hTl_FCISPOPDF_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTl_FCISPOPDF_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_FDISPOPDF_CheckBox;
        public bool HTL_FDISPOPDF_CheckBox
        {
            get
            {
                return this._hTL_FDISPOPDF_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FDISPOPDF_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_AnywayOfTheWorld_CheckBox;
        public bool HTL_AnywayOfTheWorld_CheckBox
        {
            get
            {
                return this._hTL_AnywayOfTheWorld_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_AnywayOfTheWorld_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_FCISVOWP_CheckBox;
        public bool HTL_FCISVOWP_CheckBox
        {
            get
            {
                return this._hTL_FCISVOWP_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FCISVOWP_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_FDISVOWP_CheckBox;
        public bool HTL_FDISVOWP_CheckBox
        {
            get
            {
                return this._hTL_FDISVOWP_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FDISVOWP_CheckBox, value))
                {

                }
            }
        }


        private bool _hTL_CMMFDFBBAMMFGOWP_CheckBox;
        public bool HTL_CMMFDFBBAMMFGOWP_CheckBox
        {
            get
            {
                return this._hTL_CMMFDFBBAMMFGOWP_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_CMMFDFBBAMMFGOWP_CheckBox, value))
                {

                }
            }
        }


        private bool _hTL_CMMFASBAMMFGOW_CheckBox;
        public bool HTL_CMMFASBAMMFGOW_CheckBox
        {
            get
            {
                return this._hTL_CMMFASBAMMFGOW_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_CMMFASBAMMFGOW_CheckBox, value))
                {

                }
            }
        }


        private bool _hTL_CMMFFCWOCRBAMMFGOPDF_CheckBox;
        public bool HTL_CMMFFCWOCRBAMMFGOPDF_CheckBox
        {
            get
            {
                return this._hTL_CMMFFCWOCRBAMMFGOPDF_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_CMMFFCWOCRBAMMFGOPDF_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_FocusContextBetWeen10SecondsOfSound_CheckBox;
        public bool HTL_FocusContextBetWeen10SecondsOfSound_CheckBox
        {
            get
            {
                return this._hTL_FocusContextBetWeen10SecondsOfSound_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FocusContextBetWeen10SecondsOfSound_CheckBox, value))
                {

                }
            }
        }
        private bool _hTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox;
        public bool HTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox
        {
            get
            {
                return this._hTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane_CheckBox, value))
                {

                }
            }
        }
        private bool _hTL_FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane_CheckBox;
        public bool HTL_FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane_CheckBox
        {
            get
            {
                return this._hTL_FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane_CheckBox, value))
                {

                }
            }
        }
        private bool _hTL_FindTasksNTimesFromTheMMFileOfFreePlane_CheckBox;
        public bool HTL_FindTasksNTimesFromTheMMFileOfFreePlane_CheckBox
        {
            get
            {
                return this._hTL_FindTasksNTimesFromTheMMFileOfFreePlane_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FindTasksNTimesFromTheMMFileOfFreePlane_CheckBox, value))
                {

                }
            }
        }
        private bool _hTL_ReadyForVariablesListLearningOfTheWorld_CheckBox;
        public bool HTL_ReadyForVariablesListLearningOfTheWorld_CheckBox
        {
            get
            {
                return this._hTL_ReadyForVariablesListLearningOfTheWorld_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_ReadyForVariablesListLearningOfTheWorld_CheckBox, value))
                {

                }
            }
        }
        private bool _hTL_ReviewOfTheWorldOfMyBrain_CheckBox;
        public bool HTL_ReviewOfTheWorldOfMyBrain_CheckBox
        {
            get
            {
                return this._hTL_ReviewOfTheWorldOfMyBrain_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_ReviewOfTheWorldOfMyBrain_CheckBox, value))
                {

                }
            }
        }
     


        private string _kMN_KMT_HTL_ID_TextBox;
        public string KMN_KMT_HTL_ID_TextBox
        {
            get
            {
                return this._kMN_KMT_HTL_ID_TextBox;
            }
            set
            {
                if (SetProperty(ref this._kMN_KMT_HTL_ID_TextBox, value))
                {

                }
            }
        }

        private bool _hTL_FocusTheKMNWithHavingAnIdeaMyBrain_CheckBox
;
        public bool HTL_FocusTheKMNWithHavingAnIdeaMyBrain_CheckBox

        {
            get
            {
                return this._hTL_FocusTheKMNWithHavingAnIdeaMyBrain_CheckBox
;
            }
            set
            {
                if (SetProperty(ref this._hTL_FocusTheKMNWithHavingAnIdeaMyBrain_CheckBox, value))
                {

                }
            }
        }

        private bool _hTL_FindKMFromSomeURLAutomaticallyOfResearch_CheckBox;
        public bool HTL_FindKMFromSomeURLAutomaticallyOfResearch_CheckBox
        {
            get
            {
                return this._hTL_FindKMFromSomeURLAutomaticallyOfResearch_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._hTL_FindKMFromSomeURLAutomaticallyOfResearch_CheckBox, value))
                {

                }
            }
        }



        private bool _pR_Unclassified_CheckBox;
        public bool PR_Unclassified_CheckBox
        {
            get
            {
                return this._pR_Unclassified_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_Unclassified_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_Today_CheckBox;
        public bool PR_Today_CheckBox
        {
            get
            {
                return this._pR_Today_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_Today_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_InAFewDays_CheckBox;
        public bool PR_InAFewDays_CheckBox
        {
            get
            {
                return this._pR_InAFewDays_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_InAFewDays_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_Want1st_CheckBox;
        public bool PR_Want1st_CheckBox
        {
            get
            {
                return this._pR_Want1st_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_Want1st_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_Want2nd_CheckBox;
        public bool PR_Want2nd_CheckBox
        {
            get
            {
                return this._pR_Want2nd_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_Want2nd_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_Play_CheckBox;
        public bool PR_Play_CheckBox
        {
            get
            {
                return this._pR_Play_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_Play_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_JustNow_CheckBox;
        public bool PR_JustNow_CheckBox
        {
            get
            {
                return this._pR_JustNow_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_JustNow_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_WithinAWeek_CheckBox;
        public bool PR_WithinAWeek_CheckBox
        {
            get
            {
                return this._pR_WithinAWeek_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinAWeek_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_WithinTwoWeeks_CheckBox;
        public bool PR_WithinTwoWeeks_CheckBox
        {
            get
            {
                return this._pR_WithinTwoWeeks_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinTwoWeeks_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_WithinAMonth_CheckBox;
        public bool PR_WithinAMonth_CheckBox
        {
            get
            {
                return this._pR_WithinAMonth_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinAMonth_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_WithinThreeMonthes_CheckBox;
        public bool PR_WithinThreeMonthes_CheckBox
        {
            get
            {
                return this._pR_WithinThreeMonthes_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinThreeMonthes_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_WithinHalfAYear_CheckBox;
        public bool PR_WithinHalfAYear_CheckBox
        {
            get
            {
                return this._pR_WithinHalfAYear_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinHalfAYear_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_WithinAYear_CheckBox;
        public bool PR_WithinAYear_CheckBox
        {
            get
            {
                return this._pR_WithinAYear_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinAYear_CheckBox, value))
                {

                }
            }
        }

        private bool _pR_Exercise_CheckBox;
        public bool PR_Exercise_CheckBox
        {
            get
            {
                return this._pR_Exercise_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pR_Exercise_CheckBox, value))
                {

                }
            }
        }


        private string _estimatedTime_TextBox;
        public string EstimatedTime_TextBox
        {
            get
            {
                return this._estimatedTime_TextBox;
            }
            set
            {
                if (SetProperty(ref this._estimatedTime_TextBox, value))
                {

                }
            }
        }

        private string _dueDate_TextBox;
        public string DueDate_TextBox
        {
            get
            {
                return this._dueDate_TextBox;
            }
            set
            {
                if (SetProperty(ref this._dueDate_TextBox, value))
                {

                }
            }
        }

        private string _dueTime_TextBox;
        public string DueTime_TextBox
        {
            get
            {
                return this._dueTime_TextBox;
            }
            set
            {
                if (SetProperty(ref this._dueTime_TextBox, value))
                {

                }
            }
        }

        private string _rTPD_TextBox;
        public string RTPD_TextBox
        {
            get
            {
                return this._rTPD_TextBox;
            }
            set
            {
                if (SetProperty(ref this._rTPD_TextBox, value))
                {

                }
            }
        }

        private string _rTPDD_TextBox;
        public string RTPDD_TextBox
        {
            get
            {
                return this._rTPDD_TextBox;
            }
            set
            {
                if (SetProperty(ref this._rTPDD_TextBox, value))
                {

                }
            }
        }

        private string _createdAt_TextBox;
        public string CreatedAt_TextBox
        {
            get
            {
                return this._createdAt_TextBox;
            }
            set
            {
                if (SetProperty(ref this._createdAt_TextBox, value))
                {

                }
            }
        }

        private string _lastClearedAt_TextBox;
        public string LastClearedAt_TextBox
        {
            get
            {
                return this._lastClearedAt_TextBox;
            }
            set
            {
                if (SetProperty(ref this._lastClearedAt_TextBox, value))
                {

                }
            }
        }

        private string _archived_TextBox;
        public string Archived_TextBox
        {
            get
            {
                return this._archived_TextBox;
            }
            set
            {
                if (SetProperty(ref this._archived_TextBox, value))
                {

                }
            }
        }

        private string _postpone_TextBox;
        public string Postpone_TextBox
        {
            get
            {
                return this._postpone_TextBox;
            }
            set
            {
                if (SetProperty(ref this._postpone_TextBox, value))
                {

                }
            }
        }

        private string _repeatDuration_TextBox;
        public string RepeatDuration_TextBox
        {
            get
            {
                return this._repeatDuration_TextBox;
            }
            set
            {
                if (SetProperty(ref this._repeatDuration_TextBox, value))
                {

                }
            }
        }

        private bool _pC_Nothing_CheckBox;
        public bool PC_Nothing_CheckBox
        {
            get
            {
                return this._pC_Nothing_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_Nothing_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_Everyday_CheckBox;
        public bool PC_Everyday_CheckBox
        {
            get
            {
                return this._pC_Everyday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_Everyday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryMonday;
        public bool PC_EveryMonday_CheckBox
        {
            get
            {
                return this._pC_EveryMonday;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryMonday, value))
                {

                }
            }
        }

        private bool _pC_EveryTuesday_CheckBox;
        public bool PC_EveryTuesday_CheckBox
        {
            get
            {
                return this._pC_EveryTuesday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryTuesday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryWednesday_CheckBox;
        public bool PC_EveryWednesday_CheckBox
        {
            get
            {
                return this._pC_EveryWednesday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryWednesday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryThursday_CheckBox;
        public bool PC_EveryThursday_CheckBox
        {
            get
            {
                return this._pC_EveryThursday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryThursday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryFriday_CheckBox;
        public bool PC_EveryFriday_CheckBox
        {
            get
            {
                return this._pC_EveryFriday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryFriday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EverySaturday_CheckBox;
        public bool PC_EverySaturday_CheckBox
        {
            get
            {
                return this._pC_EverySaturday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EverySaturday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EverySunday_CheckBox;
        public bool PC_EverySunday_CheckBox
        {
            get
            {
                return this._pC_EverySunday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EverySunday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryTwoWeeksOnAMonday_CheckBox;
        public bool PC_EveryTwoWeeksOnAMonday_CheckBox
        {
            get
            {
                return this._pC_EveryTwoWeeksOnAMonday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryTwoWeeksOnAMonday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryTwoWeeksOnATuesday_CheckBox;
        public bool PC_EveryTwoWeeksOnATuesday_CheckBox
        {
            get
            {
                return this._pC_EveryTwoWeeksOnATuesday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryTwoWeeksOnATuesday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryTwoWeeksOnAWednesday_CheckBox;
        public bool PC_EveryTwoWeeksOnAWednesday_CheckBox
        {
            get
            {
                return this._pC_EveryTwoWeeksOnAWednesday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryTwoWeeksOnAWednesday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryTwoWeeksOnAThursday_CheckBox;
        public bool PC_EveryTwoWeeksOnAThursday_CheckBox
        {
            get
            {
                return this._pC_EveryTwoWeeksOnAThursday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryTwoWeeksOnAThursday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryTwoWeeksOnAFriday_CheckBox;
        public bool PC_EveryTwoWeeksOnAFriday_CheckBox
        {
            get
            {
                return this._pC_EveryTwoWeeksOnAFriday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryTwoWeeksOnAFriday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryTwoWeeksOnASaturday_CheckBox;
        public bool PC_EveryTwoWeeksOnASaturday_CheckBox
        {
            get
            {
                return this._pC_EveryTwoWeeksOnASaturday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryTwoWeeksOnASaturday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryTwoWeeksOnASunday_CheckBox;
        public bool PC_EveryTwoWeeksOnASunday_CheckBox
        {
            get
            {
                return this._pC_EveryTwoWeeksOnASunday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryTwoWeeksOnASunday_CheckBox, value))
                {

                }
            }
        }


        private bool _pC_Weekday_CheckBox;
        public bool PC_Weekday_CheckBox
        {
            get
            {
                return this._pC_Weekday_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_Weekday_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_Weekend_CheckBox;
        public bool PC_Weekend_CheckBox
        {
            get
            {
                return this._pC_Weekend_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_Weekend_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryMonthOnRandomDay_CheckBox;
        public bool PC_EveryMonthOnRandomDay_CheckBox
        {
            get
            {
                return this._pC_EveryMonthOnRandomDay_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryMonthOnRandomDay_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryYearOnRandomDay_CheckBox;
        public bool PC_EveryYearOnRandomDay_CheckBox
        {
            get
            {
                return this._pC_EveryYearOnRandomDay_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryYearOnRandomDay_CheckBox, value))
                {

                }
            }
        }


        private bool _pC_EveryMonthOnSpecifiedDay_CheckBox;
        public bool PC_EveryMonthOnSpecifiedDay_CheckBox
        {
            get
            {
                return this._pC_EveryMonthOnSpecifiedDay_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryMonthOnSpecifiedDay_CheckBox, value))
                {

                }
            }
        }

        private bool _pC_EveryYearOnSpecifiedDay_CheckBox;
        public bool PC_EveryYearOnSpecifiedDay_CheckBox
        {
            get
            {
                return this._pC_EveryYearOnSpecifiedDay_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._pC_EveryYearOnSpecifiedDay_CheckBox, value))
                {

                }
            }
        }

        private string _specifiedDay_TextBox;
        public string SpecifiedDay_TextBox
        {
            get
            {
                return this._specifiedDay_TextBox;
            }
            set
            {
                if (SetProperty(ref this._specifiedDay_TextBox, value))
                {

                }
            }
        }

        private string _specificPageAsPDF_TextBox;
        public string SpecificPageAsPDF_TextBox
        {
            get
            {
                return this._specificPageAsPDF_TextBox;
            }
            set
            {
                if (SetProperty(ref this._specificPageAsPDF_TextBox, value))
                {

                }
            }
        }


        private string _sSAW_TextBox;
        public string SSAW_TextBox
        {
            get
            {
                return this._sSAW_TextBox;
            }
            set
            {
                if (SetProperty(ref this._sSAW_TextBox, value))
                {

                }
            }
        }

        private string _tSIASAM_TextBox;
        public string TSIASAM_TextBox
        {
            get
            {
                return this._tSIASAM_TextBox;
            }
            set
            {
                if (SetProperty(ref this._tSIASAM_TextBox, value))
                {

                }
            }
        }

        private string _tSIAM_TextBox;
        public string TSIAM_TextBox
        {
            get
            {
                return this._tSIAM_TextBox;
            }
            set
            {
                if (SetProperty(ref this._tSIAM_TextBox, value))
                {

                }
            }
        }

        private string _rF1_TextBox;
        public string RF1_TextBox
        {
            get
            {
                return this._rF1_TextBox;
            }
            set
            {
                if (SetProperty(ref this._rF1_TextBox, value))
                {

                }
            }
        }

        private string _rF2_TextBox;
        public string RF2_TextBox
        {
            get
            {
                return this._rF2_TextBox;
            }
            set
            {
                if (SetProperty(ref this._rF2_TextBox, value))
                {

                }
            }
        }

        private string _limitNumber_TextBox = "100";
        public string LimitNumber_TextBox
        {
            get
            {
                return this._limitNumber_TextBox;
            }
            set
            {
                if (SetProperty(ref this._limitNumber_TextBox, value))
                {

                }
            }
        }



        private ObservableCollection<Models.DBs.TasksTable.DataClass> _tasksCollection;
        public ObservableCollection<Models.DBs.TasksTable.DataClass> TasksCollection
        {
            get
            {
                return this._tasksCollection;
            }
            set
            {
                if (SetProperty(ref this._tasksCollection, value))
                {

                }
            }
        }




        private void ExperimentOpenAllWindowWithViewModel()
        {
            TaskAddViewModel tavm = new TaskAddViewModel();
            TaskAddWindow taw = new TaskAddWindow();
            taw.DataContext = tavm;
            taw.Show();

            TaskUpdateViewModel tuvm = new TaskUpdateViewModel();
            TaskUpdateWindow tuw = new TaskUpdateWindow();
            tuw.DataContext = tuvm;
            tuw.Show();

            ResultViewModel rvm = new ResultViewModel();
            ResultWindow rw = new ResultWindow(false);
            rw.DataContext = rvm;
            rw.Show();

            TaskActionViewModel taskActionViewModel = new TaskActionViewModel();
            TaskActionWindow taskActionWindow = new TaskActionWindow();
            taskActionWindow.DataContext = taskActionViewModel;
            taskActionWindow.Show();

            ParentChildProjectAddViewModel pcp = new ParentChildProjectAddViewModel();
            ParentChildProjectAddWindow pcpw = new ParentChildProjectAddWindow();
            pcpw.DataContext = pcp;
            pcpw.Show();

            UpdateAllCheckedRecordsViewModel updateAll = new UpdateAllCheckedRecordsViewModel();
            UpdateAllCheckedRecordsWindow updateAllWindow = new UpdateAllCheckedRecordsWindow();
            updateAllWindow.DataContext = updateAll;
            updateAllWindow.Show();

            UpdateTaskRecordViewModel updateOne = new UpdateTaskRecordViewModel(3);
            UpdateTaskRecordWindow updateOneWindow = new UpdateTaskRecordWindow();
            updateOneWindow.DataContext = updateOne;
            updateOneWindow.Show();

            var addTaskViewModel = new AddTaskViewModel();
            var addTaskWindow = new AddTaskWindow();
            addTaskWindow.DataContext = addTaskViewModel;
            addTaskWindow.Show();



        }
    }
}
