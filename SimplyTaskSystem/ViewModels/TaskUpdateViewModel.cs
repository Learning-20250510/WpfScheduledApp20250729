using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimplyTaskSystem.ViewModels
{
    internal class TaskUpdateViewModel : NotificationObject
    {
        public TaskUpdateViewModel()
        {
            SetUpDataBindingValue();
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
        public TaskUpdateViewModel(ObservableCollection<Models.DBs.TasksTable.DataClass> TasksCollection)
        {
            this.TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(TasksCollection);
        }

        private ObservableCollection<string> _kMTsCollection;
        public ObservableCollection<string> KMTsCollection
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

        private string _selectKMTItem;
        public string SelectKMTItem
        {
            get
            {
                return this._selectKMTItem;
            }
            set
            {
                if (SetProperty(ref this._selectKMTItem, value))
                {
                    //MessageBox.Show(SelectPriorityItem);
                }
            }
        }


        private ObservableCollection<string> _prioritiesCollection;
        public ObservableCollection<string> PrioritiesCollection
        {
            get
            {
                return this._prioritiesCollection;
            }
            set
            {
                if (SetProperty(ref this._prioritiesCollection, value))
                {

                }
            }
        }

        private string _selectPriorityItem;
        public string SelectPriorityItem
        {
            get
            {
                return this._selectPriorityItem;
            }
            set
            {
                if (SetProperty(ref this._selectPriorityItem, value))
                {
                    //MessageBox.Show(SelectPriorityItem);
                }
            }
        }

        private ObservableCollection<string> _periodicallyCyclesCollection;
        public ObservableCollection<string> PeriodicallyCyclesCollection
        {
            get
            {
                return this._periodicallyCyclesCollection;
            }
            set
            {
                if (SetProperty(ref this._periodicallyCyclesCollection, value))
                {

                }
            }
        }

        private string _selectPeriodicallyCycleItem;
        public string SelectPeriodicallyCycleItem
        {
            get
            {
                return this._selectPeriodicallyCycleItem;
            }
            set
            {
                if (SetProperty(ref this._selectPeriodicallyCycleItem, value))
                {
                    //MessageBox.Show(SelectPriorityItem);
                }
            }
        }



        private void SetUpDataBindingValue()
        {
            PrioritiesCollection = new ObservableCollection<string>
            {
                //PriorityTableからのすべての値をこのコレクションに代入する。
                "曇り",
                "貼れ",
                "貼れｆ",
                "再興",
            };

            SelectPriorityItem = "貼れｆ";//ID=1のはずのUnclassifiedヲ設定

        }
    }
}
