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
    internal class ParentChildProjectAddViewModel : NotificationObject
    {
        public ParentChildProjectAddViewModel()
        {
            RefleshParentProjectsCollection();
        }

        private void RefleshParentProjectsCollection()
        {
            ParentProjectsCollection = new ObservableCollection<string>();

            Models.DBs.ParentChildrenProjectTable.Read read = new Models.DBs.ParentChildrenProjectTable.Read();

            foreach (var li in read.SelectPCPTemplate("select * from simpletasksystem.pcp"))
            {
                Debug.WriteLine(li.ParentProjectName);


                ParentProjectsCollection.Add(li.ParentProjectName);
            }
        }

        private string _addParentProject_TextBox;
        public string AddParentProject_TextBox
        {
            get
            {
                return this._addParentProject_TextBox;
            }
            set
            {
                if (SetProperty(ref this._addParentProject_TextBox, value))
                {

                }
            }
        }

        private DelegateCommand _addParentProjectBtnCommand;
        public DelegateCommand AddParentProjectBtnCommand
        {
            get
            {
                return this._addParentProjectBtnCommand ?? (this._addParentProjectBtnCommand = new DelegateCommand(
                _ =>
                {
                    Models.DBs.ParentChildrenProjectTable.Insert insert = new Models.DBs.ParentChildrenProjectTable.Insert();
                    insert.InsertParentRecord(AddParentProject_TextBox);
                    MessageBox.Show(AddParentProject_TextBox + " を追加しました。");
                    RefleshParentProjectsCollection();
                },
                _ =>
                {
                    if (AddParentProject_TextBox == "")
                    {
                        return false;

                    }
                    else
                    {
                        return true;
                    }
                }
                ));
            }
        }


        private ObservableCollection<string> _parentProjectsCollection;
        public ObservableCollection<string> ParentProjectsCollection
        {
            get
            {
                return this._parentProjectsCollection;
            }
            set
            {
                if (SetProperty(ref this._parentProjectsCollection, value))
                {

                }
            }
        }

        private string _selectParentProjectItem;
        public string SelectParentProjectItem
        {
            get
            {
                return this._selectParentProjectItem;
            }
            set
            {
                if (SetProperty(ref this._selectParentProjectItem, value))
                {
                }
            }
        }

        private string _addChildProject_TextBox;
        public string AddChildProject_TextBox
        {
            get
            {
                return this._addChildProject_TextBox;
            }
            set
            {
                if (SetProperty(ref this._addChildProject_TextBox, value))
                {

                }
            }
        }

        private DelegateCommand _addChildProjectBtnCommand;
        public DelegateCommand AddChildProjectBtnCommand
        {
            get
            {
                return this._addChildProjectBtnCommand ?? (this._addChildProjectBtnCommand = new DelegateCommand(
                _ =>
                {
                    Models.DBs.ParentChildrenProjectTable.Insert insert = new Models.DBs.ParentChildrenProjectTable.Insert();
                    insert.InsertChildRecord(SelectParentProjectItem, AddChildProject_TextBox);
                    MessageBox.Show(AddChildProject_TextBox + " を" + SelectParentProjectItem + " に追加しました。");
                    RefleshParentProjectsCollection();

                },
                _ =>
                {
                    if (SelectParentProjectItem == null || AddChildProject_TextBox == null)
                    {
                        return true;
                    }
                    else
                    {
                        return true;
                    }
                }
                ));
            }
        }



    }
}
