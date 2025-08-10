using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using SimplyTaskSystem.Views;
using SimplyTaskSystem.ViewModels;

namespace SimplyTaskSystem.ViewModels
{
    internal class UpdateTaskRecordViewModel : NotificationObject
    {

        void CloseWindow()
        {
            Close?.Invoke();
        }
        public Action Close { get; set; }



        private string preKMTValue;
        private string preHTLValue;

        public static bool StartedUp = false;


        public UpdateTaskRecordViewModel(int actionID)
        {
            StartedUp = true;
            this.ID = actionID.ToString();

            Models.DBs.ParentChildrenProjectTable.Read read = new Models.DBs.ParentChildrenProjectTable.Read();
            var r = read.SelectPCPTemplate("select * from simpletasksystem.pcp");
            PCPCollection = new ObservableCollection<Models.DBs.ParentChildrenProjectTable.DataClass>(r);

            Models.DBs.PrioritiesTable.Read read1 = new Models.DBs.PrioritiesTable.Read();
            PrioritiesCollection = new ObservableCollection<Models.DBs.PrioritiesTable.DataClass>(read1.SelectPrioritiesCollection("select * from simpletasksystem.priorities"));


            Models.DBs.PeriodicallyCyclesTable.Read read2 = new Models.DBs.PeriodicallyCyclesTable.Read();
            PCCollection = new ObservableCollection<Models.DBs.PeriodicallyCyclesTable.DataClass>(read2.SelectPCCollection("select * from simpletasksystem.periodicallycycles"));

            Models.DBs.KMTTable.Read read4 = new Models.DBs.KMTTable.Read();
            KMTsCollection = new ObservableCollection<Models.DBs.KMTTable.DataClass>(read4.SelectKMTCollection("select * from simpletasksystem.kmt"));

            var read5 = new Models.DBs.HTLTable.Read();
            HTLsCollection = new ObservableCollection<Models.DBs.HTLTable.DataClass>(read5.SelectHTLCollection("select * from simpletasksystem.htl"));

            //渡された引数のレコードをtasksTableから取得
            var read3 = new Models.DBs.TasksTable.Read();
            string commandText = $"select * from simpletasksystem.tasks where id={this.ID}";
            Debug.WriteLine("AllTasksCommandText: " + commandText);
            var oneTask = read3.SelectTasksTemplate(commandText);

            //Debug
            if (!(oneTask.Count == 1))
            {
                //throw new Exception("id=" + this.ID + " のレコードが、tasksTableに、" + oneTask.Count + " 個存在しています。１つのみでなければいけません。");
            }
            //各コントローラに初期値を与える。

            //KMN初期値

            
            foreach (var task in oneTask)
            {
                this.KMT = task.KMT.ToString();
                this.KMN = task.KMN;

                this.SelectPCPValue = task.PCP_ID.ToString();

                this.Description = task.Description;

                this.SelectPriorityValue = task.Priority.ToString();

                this.EstimatedTime = task.EstimatedTime;

                this.RepeatPatterns = task.RepeatPatterns;

                this.RTPD = task.RepeatTimesPerDay;
                this.DueDate = task.DueDate;
                this.DueTime = task.DueTime;
                this.RepeatDuration = task.RepeatDuration;
                this.Archived_CheckBox = task.Archived;
                this.Postpone_CheckBox = task.Postpone;
                this.RepeatDuration = task.RepeatDuration;

                this.SelectPCValue = task.PeriodicallyCycles.ToString();

                this.SpecifiedDay = task.SpecifiedDay;
                this.RF1 = task.RelationalFile1;
                this.RF2 = task.RelationalFile2;
                this.ACFVBMMF_CheckBox = task.AutoCreateFirstVariableBranchesMMF;

                //要注意対象項目
                this.SelectKMTValue = task.KMT.ToString();
                this.SelectHTLValue = task.HTL.ToString();

                //BuckUp用
                this.preKMTValue = this.SelectKMTValue;
                this.preHTLValue = this.SelectHTLValue;
            }


        }

        private string _iD;
        public string ID
        {
            get
            {
                return this._iD;
            }
            set
            {
                if (SetProperty(ref this._iD, value))
                {

                }

            }
        }

        private string _kMN;
        public string KMN
        {
            get
            {
                return this._kMN;
            }
            set
            {
                if (SetProperty(ref this._kMN, value))
                {

                }
            }
        }


        private string _kMT;
        public string KMT
        {
            get
            {
                return this._kMT;
            }
            set
            {
                if (SetProperty(ref this._kMT, value))
                {

                }
            }
        }

        private ObservableCollection<Models.DBs.ParentChildrenProjectTable.DataClass> _pCPCollection;
        public ObservableCollection<Models.DBs.ParentChildrenProjectTable.DataClass> PCPCollection
        {
            get
            {
                return this._pCPCollection;
            }
            set
            {
                if (SetProperty(ref this._pCPCollection, value))
                {

                }
            }
        }

        private string _selectPCPValue;
        public string SelectPCPValue
        {
            get
            {
                return this._selectPCPValue;
            }
            set
            {
                if (SetProperty(ref this._selectPCPValue, value))
                {

                }
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (SetProperty(ref this._description, value))
                {
                }
            }
        }

        private int _estimatedTime;
        public int EstimatedTime
        {
            get
            {
                return this._estimatedTime;
            }
            set
            {
                if (SetProperty(ref this._estimatedTime, value))
                {
                }
            }
        }

        private int _repeatPatterns;
        public int RepeatPatterns
        {
            get
            {
                return this._repeatPatterns;
            }
            set
            {
                if (SetProperty(ref this._repeatPatterns, value))
                {
                    MessageBox.Show("RepeatPatternsの値が変更されました。確定する前に本当によろしいか確認しましょう。");

                }
            }
        }

        private ObservableCollection<Models.DBs.PrioritiesTable.DataClass> _pRioritiesCollection;
        public ObservableCollection<Models.DBs.PrioritiesTable.DataClass> PrioritiesCollection
        {
            get
            {
                return this._pRioritiesCollection;
            }
            set
            {
                if (SetProperty(ref this._pRioritiesCollection, value))
                {

                }
            }
        }

        private int _selectPriorityItem;
        public int SelectPriorityItem
        {
            get
            {
                return this._selectPriorityItem;
            }
            set
            {
                if (SetProperty(ref this._selectPriorityItem, value))
                {

                }
            }
        }


        private string _selectPriorityValue;
        public string SelectPriorityValue
        {
            get
            {
                return this._selectPriorityValue;
            }
            set
            {
                if (SetProperty(ref this._selectPriorityValue, value))
                {

                }
            }
        }

        private string _dueDate;
        public string DueDate
        {
            get
            {
                return this._dueDate;
            }
            set
            {
                if (SetProperty(ref this._dueDate, value))
                {

                }
            }
        }

        private string _dueTime;
        public string DueTime
        {
            get
            {
                return this._dueTime;
            }
            set
            {
                if (SetProperty(ref this._dueTime, value))
                {

                }
            }
        }

        private int _rTPD;
        public int RTPD
        {
            get
            {
                return this._rTPD;
            }
            set
            {
                if (SetProperty(ref this._rTPD, value))
                {

                }
            }
        }

        private bool _archived_CheckBox;
        public bool Archived_CheckBox
        {
            get
            {
                return this._archived_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._archived_CheckBox, value))
                {
                    MessageBox.Show("Archivedの値が変更されました。確定する前に本当によろしいか確認しましょう。");
                }
            }
        }

        private bool _postpone_CheckBox;
        public bool Postpone_CheckBox
        {
            get
            {
                return this._postpone_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._postpone_CheckBox, value))
                {
                    MessageBox.Show("Postponeの値が変更されました。確定する前に本当によろしいか確認しましょう。");
                }
            }
        }


        private int _repeatDuration;
        public int RepeatDuration
        {
            get
            {
                return this._repeatDuration;
            }
            set
            {
                if (SetProperty(ref this._repeatDuration, value))
                {
                }
            }
        }

        private ObservableCollection<Models.DBs.PeriodicallyCyclesTable.DataClass> _pCCollection;
        public ObservableCollection<Models.DBs.PeriodicallyCyclesTable.DataClass> PCCollection
        {
            get
            {
                return this._pCCollection;
            }
            set
            {
                if (SetProperty(ref this._pCCollection, value))
                {

                }
            }
        }

        private string _selectPCValue;
        public string SelectPCValue
        {
            get
            {
                return this._selectPCValue;
            }
            set
            {
                if (SetProperty(ref this._selectPCValue, value))
                {

                }
            }
        }


        private string _specifiedDay;
        public string SpecifiedDay
        {
            get
            {
                return this._specifiedDay;
            }
            set
            {
                if (SetProperty(ref this._specifiedDay, value))
                {

                }
            }
        }


        private string _rF1;
        public string RF1
        {
            get
            {
                return this._rF1;
            }
            set
            {
                if (SetProperty(ref this._rF1, value))
                {

                }
            }
        }


        private string _rF2;
        public string RF2
        {
            get
            {
                return this._rF2;
            }
            set
            {
                if (SetProperty(ref this._rF2, value))
                {

                }
            }
        }


        private bool _aCFVBMMF_CheckBox;
        public bool ACFVBMMF_CheckBox
        {
            get
            {
                return this._aCFVBMMF_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._aCFVBMMF_CheckBox, value))
                {
                    MessageBox.Show("ACFVBMMFの値が変更されました。確定する前に本当によろしいか確認しましょう。");

                }
            }
        }

        private bool _changeWithRelationalOfAll_CheckBox;
        public bool ChangeWithRelationalOfAll_CheckBox
        {
            get
            {
                return this._changeWithRelationalOfAll_CheckBox;
            }
            set
            {
                if (SetProperty(ref this._changeWithRelationalOfAll_CheckBox, value))
                {
                    MessageBox.Show("ChangeWithRelationalOfAll_CheckBoxの値が変更されました。確定する前に本当によろしいか確認しましょう。");

                }
            }
        }


        private ObservableCollection<Models.DBs.KMTTable.DataClass> _kMTsCollection;
        public ObservableCollection<Models.DBs.KMTTable.DataClass> KMTsCollection
        {
            get
            {
                return this._kMTsCollection;
            }
            set
            {
                if (SetProperty(ref this._kMTsCollection, value))
                {

                }
            }
        }

        private string _selectKMTValue;
        public string SelectKMTValue
        {
            get
            {
                return this._selectKMTValue;
            }
            set
            {
                if (SetProperty(ref this._selectKMTValue, value))
                {

                }
            }
        }



        private ObservableCollection<Models.DBs.HTLTable.DataClass> _hTLsCollection;
        public ObservableCollection<Models.DBs.HTLTable.DataClass> HTLsCollection
        {
            get
            {
                return this._hTLsCollection;
            }
            set
            {
                if (SetProperty(ref this._hTLsCollection, value))
                {

                }
            }
        }

        private string _selectHTLValue;
        public string SelectHTLValue
        {
            get
            {
                return this._selectHTLValue;
            }
            set
            {
                if (SetProperty(ref this._selectHTLValue, value))
                {

                }
            }
        }















        private DelegateCommand _sendBtnCommand;
        public DelegateCommand SendBtnCommand
        {
            get
            {
                return this._sendBtnCommand ?? (this._sendBtnCommand = new DelegateCommand(
                _ =>
                {

                    if (SelectPriorityValue == null)
                    {
                        MessageBox.Show(" Priorityがnullです。何かを選択してください。");
                    }
                    else if (SelectPCValue == null)
                    {
                        MessageBox.Show("PCがnullです。何かを選択してください・");
                    }
                    else if (SelectPCPValue == null)
                    {
                        MessageBox.Show("PCPがnullです。何かを選択して下さい。");
                    }
                    else if (SelectPriorityValue == "1")
                    {
                        MessageBox.Show("PriorityがUnclassifiedです。ほかを選択してください。");
                    }
                    else if (!(preKMTValue == "1" || preKMTValue == "2" || preKMTValue == "3"))
                    {
                        MessageBox.Show("preKMTValue: " + preKMTValue + " です。このレコードはＫＭＴを変更できません。");
                    }
                    else if (!(preHTLValue == "1" || preHTLValue == "6" || preHTLValue == "12" || preHTLValue == "18"))
                    {
                        MessageBox.Show("preHTLValue: " + preHTLValue + " です。このレコードはＨＴＬを変更できません。");
                    }
                    else if (!(SelectKMTValue == "1" && SelectHTLValue=="1" || SelectKMTValue == "2" && SelectHTLValue=="6" || SelectKMTValue=="2" && SelectHTLValue=="18" || SelectKMTValue=="3" && SelectHTLValue=="12"))
                    {
                        MessageBox.Show("SelectKMTValue: " + SelectKMTValue + " ,SelectHTLValue: " + SelectHTLValue + " となっており、ＫＭＴとＨＴＬの組み合わせが正しくありません。");
                    }
                    else
                    {
                        //DBに値を挿入して、IDLiStのwhere句で指定
                        Models.DBs.TasksTable.Update update = new Models.DBs.TasksTable.Update();

                        string commandText = $"UPDATE simpletasksystem.tasks SET pcp_id={SelectPCPValue}, estimated_time={EstimatedTime}, priority={SelectPriorityValue},  repeat_times_per_day={RTPD}, repeat_patterns={RepeatPatterns}, archived={Archived_CheckBox}, postpone={Postpone_CheckBox},repeat_duration={RepeatDuration}, periodically_cycles={SelectPCValue},  auto_create_first_variable_branches_mmfile={ACFVBMMF_CheckBox}, kmt={SelectKMTValue}, htl={SelectHTLValue} ";
                        if (!(Description == null))
                        {
                            if (Description == "")
                            {
                                //nullを代入
                                commandText += ",description =null";

                            }
                            else
                            {
                                commandText += $",description = '{Description}'";
                            }
                        }
                        if (!(DueTime == null))
                        {
                            if (DueTime == "")
                            {
                                //nullを代入
                                commandText += ",due_time=null";

                            }
                            else
                            {
                                commandText += $",due_time='{DueTime}'";
                            }
                        }
                        if (!(SpecifiedDay == null))
                        {
                            if (SpecifiedDay == "")
                            {
                                //nullを代入
                                commandText += ",specified_day=null";

                            }
                            else
                            {
                                commandText += $",specified_day='{SpecifiedDay}'";

                            }
                        }
                        if (!(RF1 == null))
                        {
                            if (RF1 == "")
                            {
                                //nullを代入
                                commandText += ",relational_file_1=null";

                            }
                            else
                            {
                                commandText += $",relational_file_1='{RF1}'";

                            }
                        }
                        if (!(RF2 == null))
                        {
                            if (RF2 == "")
                            {
                                //nullを代入
                                commandText += ",relational_file_2=null";

                            }
                            else
                            {
                                commandText += $",relational_file_2='{RF2}'";
                            }
                        }

                        if (!(DueDate == null))
                        {
                            if (DueDate == "")
                            {
                                DueDate = DateTime.Now.ToString("yyyy:MM:dd");
                            }
                            commandText += $",due_date='{DueDate}'";
                        }





                        Models.DBs.TasksTable.Read read = new Models.DBs.TasksTable.Read();


                        if (ChangeWithRelationalOfAll_CheckBox == true)//関連レコード全部に対してUpdate
                        {
                            int intKMT = int.Parse(this.KMT);
                            if (intKMT == 1)
                            {
                                //Nothing Of Relation
                                string whereStatement = $" where id={ID}";
                                Debug.WriteLine("commnadText: " + commandText + whereStatement);
                                update.UpdateTasksRecord(commandText + whereStatement);
                            }
                            else if (intKMT == 2)//TheWorld
                            {
                                //Nothing Of Relation
                                string whereStatement = $" where id={ID}";
                                Debug.WriteLine("commnadText: " + commandText + whereStatement);
                                update.UpdateTasksRecord(commandText + whereStatement);
                            }
                            else if (intKMT == 3)//MyBrain
                            {
                                //Nothing Of Relation
                                string whereStatement = $" where id={ID}";
                                Debug.WriteLine("commnadText: " + commandText + whereStatement);
                                update.UpdateTasksRecord(commandText + whereStatement);
                            }
                            else if (intKMT == 4)//WebPage
                            {
                                string whereStatement = $" where id={this.ID} OR (kmn={this.KMN} and kmt={this.KMT}";
                                Debug.WriteLine("commnadText: " + commandText + whereStatement);
                                update.UpdateTasksRecord(commandText + whereStatement);

                            }
                            else if (intKMT == 5)//FreePlane
                            {
                                string whereStatement = $" where id={this.ID} OR (kmn={this.KMN} and kmt={this.KMT}";
                                Debug.WriteLine("commnadText: " + commandText + whereStatement);
                                update.UpdateTasksRecord(commandText + whereStatement);

                            }
                            else if (intKMT == 6)//PDF
                            {
                                string whereStatement = $" where id={this.ID} OR (kmn={this.KMN} and kmt={this.KMT}";
                                Debug.WriteLine("commnadText: " + commandText + whereStatement);
                                update.UpdateTasksRecord(commandText + whereStatement);

                            }
                            else if (intKMT == 7)//Research
                            {
                                string whereStatement = $" where id={this.ID} OR (kmn={this.KMN} and kmt={this.KMT}";
                                Debug.WriteLine("commnadText: " + commandText + whereStatement);
                                update.UpdateTasksRecord(commandText + whereStatement);
                            }
                            else
                            {
                                //随時更新（仮にレコードが増えたらelse ifとして追加していく。
                            }
                        }
                        else
                        {
                            string whereStatement = $" where id={ID}";
                            Debug.WriteLine("commnadText: " + commandText + whereStatement);
                            update.UpdateTasksRecord(commandText + whereStatement);
                        }

                        MessageBox.Show("無事に、taskのUpdateが完了しました。");
                        CloseWindow();


                        //DBにResearch値を挿入
                        Models.DBs.TasksTable.Insert insert = new Models.DBs.TasksTable.Insert();

                        //Insertする入れ物作成

                        if (Description == "")
                        {
                            Description = null;
                        }

                        if (DueTime == "" || DueTime == null)
                        {
                            DueTime = null;
                        }

                        if (SpecifiedDay == "" || SpecifiedDay == null)
                        {
                            SpecifiedDay = null;
                        }

                        if (RF1 == "" || RF1 == null)
                        {
                            RF1 = null;
                        }

                        if (RF2 == "" || RF2 == null)
                        {
                            RF2 = null;
                        }

                        if (DueDate == "" || DueDate == null)
                        {
                            DueDate = DateTime.Now.ToString("yyyy:MM:dd");
                        }


                        Models.DBs.TasksTable.DataClass tasksDataClass = new Models.DBs.TasksTable.DataClass();

       




                        if (SelectKMTValue == "3")//myBrain
                        {
                            //Reserch系としてもInsert
                            tasksDataClass.KMN = this.KMN;
                            tasksDataClass.KMT = 7;
                            tasksDataClass.HTL = 13;
                            tasksDataClass.PCP_ID = int.Parse(this.SelectPCPValue);
                            tasksDataClass.Description = this.Description;
                            tasksDataClass.EstimatedTime = this.EstimatedTime;
                            tasksDataClass.DueDate = this.DueDate;
                            tasksDataClass.DueTime = this.DueTime;
                            tasksDataClass.Priority = int.Parse(this.SelectPriorityValue);
                            tasksDataClass.RepeatTimesPerDay = this.RTPD;
                            tasksDataClass.Archived = this.Archived_CheckBox;
                            tasksDataClass.Postpone = this.Postpone_CheckBox;
                            tasksDataClass.RepeatDuration = this.RepeatDuration;
                            tasksDataClass.PeriodicallyCycles = int.Parse(SelectPCValue);
                            tasksDataClass.SpecifiedDay = this.SpecifiedDay;
                            tasksDataClass.RelationalFile1 = this.RF1;
                            tasksDataClass.RelationalFile2 = this.RF2;
                            tasksDataClass.AutoCreateFirstVariableBranchesMMF = this.ACFVBMMF_CheckBox;
                            insert.InsertRecordsFromAddTaskWindow(tasksDataClass);


                        }


                        StartedUp = false;

                    }
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }


    }
}
