using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.ViewModels
{
    internal class AddTaskViewModel : NotificationObject, ICloseWindows
    {

        public static bool StartedUp = false;


        public AddTaskViewModel()
        {
            StartedUp = true;
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

            //各コントローラに初期値を与える。

            //KMN初期値



            this.KMT = "1";

            this.SelectPCPValue = "1";

            this.SelectPriorityValue = "1";

            this.EstimatedTime = 1;
            this.RepeatPatterns = 1;
            this.RTPD = 1;
            this.RepeatDuration = 1;
            this.Archived_CheckBox = false;
            this.Postpone_CheckBox = false;

            this.SelectPCValue = "1";

            this.ACFVBMMF_CheckBox = false;

            //要注意対象項目
            this.SelectKMTValue = "1";
            this.SelectHTLValue = "1";

         

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
                    else if (this.KMN == "" || this.KMN == null)
                    {
                        MessageBox.Show("this.KMN: " + this.KMN + " となり、nullOR空文字になってしまっています。何かを入力してください。");
                    }
                    else if (!(SelectKMTValue == "1" && SelectHTLValue == "1" || SelectKMTValue == "2" && SelectHTLValue == "6" || SelectKMTValue == "2" && SelectHTLValue == "18" || SelectKMTValue == "3" && SelectHTLValue == "12"))
                    {
                        MessageBox.Show("SelectKMTValue: " + SelectKMTValue + " ,SelectHTLValue: " + SelectHTLValue + " となっており、ＫＭＴとＨＴＬの組み合わせが正しくありません。");
                    }
                    else
                    {
                        //DBに値を挿入
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
                         
                        tasksDataClass.KMN = this.KMN;
                        tasksDataClass.KMT = int.Parse(this.SelectKMTValue);
                        tasksDataClass.HTL = int.Parse(this.SelectHTLValue);
                        tasksDataClass.PCP_ID = int.Parse(this.SelectPCPValue);
                        tasksDataClass.Description = this.Description;
                        tasksDataClass.EstimatedTime = this.EstimatedTime;
                        tasksDataClass.RepeatPatterns = this.RepeatPatterns;
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

                    



                        //string commandText = $"INSERT INTO simpletasksystem.tasks pcp_id={SelectPCPValue}, estimated_time={EstimatedTime}, priority={SelectPriorityValue},  repeat_times_per_day={RTPD}, archived={Archived_CheckBox}, repeat_duration={RepeatDuration}, periodically_cycles={SelectPCValue},  auto_create_first_variable_branches_mmfile={ACFVBMMF_CheckBox}, kmt={SelectKMTValue}, htl={SelectHTLValue} ";
                        /*
                        string commandText = $"INSERT INTO simpletasksystem.tasks (pcp_id,estimated_time, priority, repeat_times_per_day,archived,repeat_duration,periodically_cycles, auto_create_first_variable_branches_mmfile, kmt, htl, description, due_time, specified_day, relational_file_1, relational_file_2, due_date) VALUES ({SelectPCPValue}, {EstimatedTime},{SelectPriorityValue},{RTPD}, {Archived_CheckBox}, {RepeatDuration}, {SelectPCValue},{ACFVBMMF_CheckBox}, {SelectKMTValue}, {SelectHTLValue},{Description},{DueTime},{SpecifiedDay},{RF1},{RF2},{DueDate}) ";

                 
                        
                        Debug.WriteLine("Insert予定commnadText: " + commandText);
                        insert.InsertTasksTemplate(commandText);
                        */

                        insert.InsertRecordsFromAddTaskWindow(tasksDataClass);
                        if (tasksDataClass.KMT == 3)//myBrain
                        {
                            //Reserch系としてもInsert
                            tasksDataClass.KMT = 7;
                            tasksDataClass.HTL = 13;
                            insert.InsertRecordsFromAddTaskWindow(tasksDataClass);


                        }

                        MessageBox.Show("無事に、taskのUpdateが完了しました。");
                        CloseWindow();
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

        void CloseWindow()
        {
            Close?.Invoke();
        }
        public Action Close { get; set; }
    }
}
