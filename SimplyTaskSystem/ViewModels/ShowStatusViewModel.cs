using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem.ViewModels
{
    internal class ShowStatusViewModel : NotificationObject
    {
        public ShowStatusViewModel()
        {
            var read = new Models.DBs.TasksTable.Read();
            read.SelectTasksTemplate("select * from simpletasksystem.tasks where (  due_time is null and repeat_times_per_day in (1,2)");
        }



        private string _nonRepeatTask_TextBlock;
        public string NonRepeatTask_TextBlock
        {
            get
            {
                return this._nonRepeatTask_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._nonRepeatTask_TextBlock, value))
                {

                }
            }
        }

        private string _repeatTask_TextBlock;
        public string RepeatTask_TextBlock
        {
            get
            {
                return this._repeatTask_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._repeatTask_TextBlock, value))
                {

                }
            }
        }
        private string _pR_Today_TextBlock;
        public string PR_Today_TextBlock
        {
            get
            {
                return this._pR_Today_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_Today_TextBlock, value))
                {

                }
            }
        }
        private string _pR_InAFewDays_TextBlock;
        public string PR_InAFewDays_TextBlock
        {
            get
            {
                return this._pR_InAFewDays_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_InAFewDays_TextBlock, value))
                {

                }
            }
        }
        private string _pR_Want1st_TextBlock;
        public string PR_Want1st_TextBlock
        {
            get
            {
                return this._pR_Want1st_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_Want1st_TextBlock, value))
                {

                }
            }
        }
        private string _pR_Want2nd_TextBlock;
        public string PR_Want2nd_TextBlock
        {
            get
            {
                return this._pR_Want2nd_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_Want2nd_TextBlock, value))
                {

                }
            }
        }
        private string _pR_Play_TextBlock;
        public string PR_Play_TextBlock
        {
            get
            {
                return this._pR_Play_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_Play_TextBlock, value))
                {

                }
            }
        }
        private string _pR_JustNow_TextBlock;
        public string PR_JustNow_TextBlock
        {
            get
            {
                return this._pR_JustNow_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_JustNow_TextBlock, value))
                {

                }
            }
        }
        private string _pR_WithinAWeek_TextBlock;
        public string PR_WithinAWeek_TextBlock
        {
            get
            {
                return this._pR_WithinAWeek_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinAWeek_TextBlock, value))
                {

                }
            }
        }
        private string _pR_WithinTwoWeeks_TextBlock;
        public string PR_WithinTwoWeeks_TextBlock
        {
            get
            {
                return this._pR_WithinTwoWeeks_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinTwoWeeks_TextBlock, value))
                {

                }
            }
        }
        private string _pR_WithinAMonth_TextBlock;
        public string PR_WithinAMonth_TextBlock
        {
            get
            {
                return this._pR_WithinAMonth_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinAMonth_TextBlock, value))
                {

                }
            }
        }
        private string _pR_WithinThreeMonthes_TextBlock;
        public string PR_WithinThreeMonthes_TextBlock
        {
            get
            {
                return this._pR_WithinThreeMonthes_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinThreeMonthes_TextBlock, value))
                {

                }
            }
        }
        private string _pR_WithinHalfAYear_TextBlock;
        public string PR_WithinHalfAYear_TextBlock
        {
            get
            {
                return this._pR_WithinHalfAYear_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinHalfAYear_TextBlock, value))
                {

                }
            }
        }
        private string _pR_WithinAYear_TextBlock;
        public string PR_WithinAYear_TextBlock
        {
            get
            {
                return this._pR_WithinAYear_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_WithinAYear_TextBlock, value))
                {

                }
            }
        }
        private string _pR_Exercise_TextBlock;
        public string PR_Exercise_TextBlock
        {
            get
            {
                return this._pR_Exercise_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._pR_Exercise_TextBlock, value))
                {

                }
            }
        }

        private string _nonRepeatTaskOfTomorrow_TextBlock;
        public string NonRepeatTaskOfTomorrow_TextBlock
        {
            get
            {
                return this._nonRepeatTaskOfTomorrow_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._nonRepeatTaskOfTomorrow_TextBlock, value))
                {

                }
            }
        }

        private string _repeatTaskOfTomorrow_TextBlock;
        public string RepeatTaskOfTomorrow_TextBlock
        {
            get
            {
                return this._repeatTaskOfTomorrow_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._repeatTaskOfTomorrow_TextBlock, value))
                {

                }
            }
        }

        private string _canTodayTask_TextBlock;
        public string CanTodayTask_TextBlock
        {
            get
            {
                return this._canTodayTask_TextBlock;
            }
            set
            {
                if (SetProperty(ref this._canTodayTask_TextBlock, value))
                {

                }
            }
        }

    }
}
