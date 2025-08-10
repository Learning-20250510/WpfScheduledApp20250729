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
    internal class UpdateAllCheckedRecordsViewModel : NotificationObject
    {
        public UpdateAllCheckedRecordsViewModel()
        {
            Models.DBs.ParentChildrenProjectTable.Read read = new Models.DBs.ParentChildrenProjectTable.Read();
            var r = read.SelectPCPTemplate("select * from simpletasksystem.pcp");
            PCPCollection = new ObservableCollection<Models.DBs.ParentChildrenProjectTable.DataClass>(r);

            Models.DBs.PrioritiesTable.Read read1 = new Models.DBs.PrioritiesTable.Read();
            PrioritiesCollection = new ObservableCollection<Models.DBs.PrioritiesTable.DataClass>(read1.SelectPrioritiesCollection("select * from simpletasksystem.priorities"));


            Models.DBs.PeriodicallyCyclesTable.Read read2 = new Models.DBs.PeriodicallyCyclesTable.Read();
            PCCollection = new ObservableCollection<Models.DBs.PeriodicallyCyclesTable.DataClass>(read2.SelectPCCollection("select * from simpletasksystem.periodicallycycles"));

 

            //KMN初期値

            this.EstimatedTime = 1;
            this.RTPD = 1;
            this.RepeatDuration = 1;
            this.DueDate = null;
            this.Archived_CheckBox = false;
            this.ACFVBMMF_CheckBox = false;

            ActionIDsList = new List<int>
            {
                1,
                5,
                3,
            };

            //各コントローラに初期値を与える。
            foreach (var li in ActionIDsList)
            {
                ID += li + ", ";
            }


        }

        private List<int> ActionIDsList;
        public UpdateAllCheckedRecordsViewModel(List<int> actionIDs)
        {
            this.ActionIDsList = new List<int>(actionIDs);//変更対象IDリスト

            Models.DBs.ParentChildrenProjectTable.Read read = new Models.DBs.ParentChildrenProjectTable.Read();
            var r = read.SelectPCPTemplate("select * from simpletasksystem.pcp");
            PCPCollection = new ObservableCollection<Models.DBs.ParentChildrenProjectTable.DataClass>(r);

            Models.DBs.PrioritiesTable.Read read1 = new Models.DBs.PrioritiesTable.Read();
            PrioritiesCollection = new ObservableCollection<Models.DBs.PrioritiesTable.DataClass>(read1.SelectPrioritiesCollection("select * from simpletasksystem.priorities"));


            Models.DBs.PeriodicallyCyclesTable.Read read2 = new Models.DBs.PeriodicallyCyclesTable.Read();
            PCCollection = new ObservableCollection<Models.DBs.PeriodicallyCyclesTable.DataClass>(read2.SelectPCCollection("select * from simpletasksystem.periodicallycycles"));

            //各コントローラに初期値を与える。
            foreach (var li in ActionIDsList)
            {
                ID += li + ", ";
            }

            //KMN初期値

            this.EstimatedTime = 1;
            this.RTPD = 1;
            this.DueDate = null;
            this.RepeatDuration = 1;
            this.Archived_CheckBox = false;
            this.ACFVBMMF_CheckBox = false;
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

        private Models.DBs.ParentChildrenProjectTable.DataClass _selectPCPItem;
        public Models.DBs.ParentChildrenProjectTable.DataClass SelectPCPItem
        {
            get
            {
                return this._selectPCPItem;
            }
            set
            {
                if (SetProperty(ref this._selectPCPItem, value))
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

        private Models.DBs.PrioritiesTable.DataClass _selectPriorityItem;
        public Models.DBs.PrioritiesTable.DataClass SelectPriorityItem
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

        private Models.DBs.PeriodicallyCyclesTable.DataClass _selectPCItem;
        public Models.DBs.PeriodicallyCyclesTable.DataClass SelectPCItem
        {
            get
            {
                return this._selectPCItem;
            }
            set
            {
                if (SetProperty(ref this._selectPCItem, value))
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


        private DelegateCommand _sendBtnCommand;
        public DelegateCommand SendBtnCommand
        {
            get
            {
                return this._sendBtnCommand ?? (this._sendBtnCommand = new DelegateCommand(
                _ =>
                {
                    MessageBox.Show("GFd");

                    if (SelectPriorityItem == null)
                    {
                        MessageBox.Show(" Priorityがnullです。何かを選択してください。");
                    }
                    else if (SelectPCItem == null)
                    {
                        MessageBox.Show("PCがnullです。何かを選択してください・");
                    }
                    else if (SelectPCPItem == null)
                    {
                        MessageBox.Show("PCPがnullです。何かを選択して下さい。");
                    }
                    else
                    {
                        //DBに値を挿入して、IDLiStのwhere句で指定
                        Models.DBs.TasksTable.Update update = new Models.DBs.TasksTable.Update();

                        string commandText = $"UPDATE simpletasksystem.tasks SET pcp_id={SelectPCPItem.ID}, estimated_time={EstimatedTime}, priority={SelectPriorityItem.ID},  repeat_times_per_day={RTPD}, archived={Archived_CheckBox}, repeat_duration={RepeatDuration}, periodically_cycles={SelectPCItem.ID},  auto_create_first_variable_branches_mmfile={ACFVBMMF_CheckBox} ";
                        if ( !(Description == null) )
                        {
                            commandText += $",description = '{Description}'";
                        }
                        if ( !(DueTime == null))
                        {
                            commandText += $",due_time='{DueTime}'";
                        }
                        if( !(SpecifiedDay == null))
                        {
                            commandText += $",specified_day='{SpecifiedDay}'";
                        }
                        if (!(RF1 == null))
                        {
                            commandText += $",relational_file_1='{RF1}'";
                        }
                        if (!(RF2 == null))
                        {
                            commandText += $",relational_file_2='{RF2}'";
                        }

                        if (!(DueDate == null))
                        {
                            commandText += $",due_date='{DueDate}'";
                        }


                        foreach (var id in ActionIDsList)
                        {
                            string whereStatement = $" where id={id}";
                            Debug.WriteLine("commnadText: " + commandText + whereStatement);

                            update.UpdateTasksRecord(commandText + whereStatement);
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


    }
}
