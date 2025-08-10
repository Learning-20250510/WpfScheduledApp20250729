using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.ViewModels
{
    internal class TaskAddViewModel : NotificationObject
    {

        private int _iD;
        public int ID
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

        private string _taskName;
        public string TaskName
        {
            get
            {
                return this._taskName;
            }
            set
            {
                if (SetProperty(ref this._taskName, value))
                {

                }
            }
        }

        private int _priorityRadioToday;
        public int PriorityRadioToday
        {
            get
            {
                return this._priorityRadioToday;
            }
            set
            {
                if (SetProperty(ref this._priorityRadioToday, value))
                {

                }
            }
        }

        private int _priorityRadioInAFewDays;
        public int PriorityRadioInAFewDays
        {
            get
            {
                return this._priorityRadioInAFewDays;
            }
            set
            {
                if (SetProperty(ref this._priorityRadioInAFewDays, value))
                {

                }
            }
        }

        private int _priorityRadioWant1st;
        public int PriorityRadioWant1st
        {
            get
            {
                return this._priorityRadioWant1st;
            }
            set
            {
                if (SetProperty(ref this._priorityRadioWant1st, value))
                {

                }
            }
        }

        private int _priorityRadioWant2nd;
        public int PriorityRadioWant2nd
        {
            get
            {
                return this._priorityRadioWant2nd;
            }
            set
            {
                if (SetProperty(ref this._priorityRadioWant2nd, value))
                {

                }
            }
        }

        private int _priorityRadioPlay;
        public int PriorityRadioPlay
        {
            get
            {
                return this._priorityRadioPlay;
            }
            set
            {
                if (SetProperty(ref this._priorityRadioPlay, value))
                {

                }
            }
        }

        private int _priorityRadioUnclassified;
        public int PriorityRadioUnclassified
        {
            get
            {
                return this._priorityRadioUnclassified;
            }
            set
            {
                if (SetProperty(ref this._priorityRadioUnclassified, value))
                {

                }
            }
        }

        private int _description;
        public int Description
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
        


        private DelegateCommand _enterCommand;
        public DelegateCommand EnterCommand
        {
            get
            {
                return this._enterCommand ?? (this._enterCommand = new DelegateCommand(
                    _ =>
                    {
                        System.Diagnostics.Debug.WriteLine("Taskを追加します。");

                        /*
                        dBs.InsertTasks(1, TaskName, TaskDescription, EstimatedTime, priorityInt, DueDate, DueTime, RepeatTimesPerDay, RepeatDuration, null, null, null, null, SelectItemValue);

                        dBs.UpdateLevelRecord(1, false, false, priorityInt, false);


                        CloseWindow();
                        Utility1.ActiveWindow(p);
     
                        ShowAnimation();
                        TaskActionWindow.damageCount += 5;
                        */

                    },
                    _ =>
                    {

                        if ((TaskName == null || TaskName == "") || EstimatedTime < 1)
                        {
                            System.Windows.MessageBox.Show("タスク名はnullや空文字以外の文字列、EstimatedTimeは自然数を指定してください。");
                            return false;
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("いける");
                            return true;
                        }

                    }));
            }
        }

    }
}
