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
    class Validations
    {
        private ObservableCollection<TasksTable.DataClass> allTasksCollection;
        private TasksTable.Read allTasks;
        private TasksTable.Insert Insert;
        public Validations()
        {
            //既存の全クエリ取得
            allTasks = new TasksTable.Read();
            var allTasksCollection = allTasks.SelectTasksTemplate("select * from simpletasksystem.tasks");

            //定期的メンテナンス
            ValidationOfExistingOfFreePlane();
            //ValidationOfExistingOfWebPage();
        }
        private void ValidationOfExistingOfFreePlane()
        {
            var freePlaneTasks = allTasks.SelectTasksTemplate("select * from simpletasksystem.tasks where kmt=5");

            var kmnFreePlaneList = new List<string>();
            foreach (var li in freePlaneTasks)
            {
                kmnFreePlaneList.Add(li.KMN);
            }

            var CheckDuplicationlist = new List<string>();

            foreach (var li in kmnFreePlaneList)
            {
                for (int i = 15; i < 18; i++)
                {
                    var checkDuplicateFreePlaneTasks = allTasks.SelectTasksTemplate($"select * from simpletasksystem.tasks where kmt=5 and kmn={li} and htl={i}");
                    if (checkDuplicateFreePlaneTasks.Count == 0)//一部ないしはすべてのHTLが存在しない
                    {
                        Debug.WriteLine("kmn: " + li + " がFreePlane系なのにも関わらず、 HTL=" + i + " がInsertされていません。Insertしましょう。");
                        //MessageBox.Show("kmn: " + li + " がFreePlane系なのにも関わらず、 HTL=" + i + " がInsertされていません。Insertしましょう。");
                    }
                    else if (checkDuplicateFreePlaneTasks.Count == 1)//正常
                    {

                    }
                    else if (checkDuplicateFreePlaneTasks.Count > 1)//重複している
                    {
                        Debug.WriteLine("kmn: " + li + " がFreePlane系にて重複しています。どちらかを削除しましょう。");
                        //MessageBox.Show("kmn: " + li + " がFreePlane系にて重複しています。どちらかを削除しましょう。");
                    }
                }
            }




        }
        private void ValidationOfExistingOfWebPage()
        {
            var webPageTasks = allTasks.SelectTasksTemplate("select * from simpletasksystem.tasks where kmt=4");

            var kmnWebPageList = new List<string>();
            foreach (var li in webPageTasks)
            {
                kmnWebPageList.Add(li.KMN);
            }

            var CheckDuplicationlist = new List<string>();

            foreach (var li in kmnWebPageList)
            {
                for (int i = 7; i < 11; i++)//htl ID
                {
                    for (int k = 0; k < 11; k++)
                    {
                        if (i == 9 || i == 10)//ScrollValue関係のないHTL
                        {
                            k = 0;//DefautValue
                        }
                        var checkDuplicationWebPageTasks = allTasks.SelectTasksTemplate($"select * from simpletasksystem.tasks where kmt=4 and kmn={li} and htl={i} and specific_scrollvalue_as_webpage={k}");

                        if (checkDuplicationWebPageTasks.Count == 0)//一部ないしはすべてのHTLが存在しない
                        {
                            Debug.WriteLine("kmn: " + li + " がWebPage系なのにも関わらず、 HTL=" + i + " ScrollValue=" + k + " がInsertされていません。Insertしましょう。");
                            //MessageBox.Show("kmn: " + li + " がWebPage系なのにも関わらず、 HTL=" + i + " ScrollValue=" + k + " がInsertされていません。Insertしましょう。");
                        }
                        else if (checkDuplicationWebPageTasks.Count == 1)//正常
                        {

                        }
                        else if (checkDuplicationWebPageTasks.Count > 1)//重複している
                        {
                            Debug.WriteLine("kmn: " + li + " がWebPage系にて重複しています。どちらかを削除しましょう。");
                            //MessageBox.Show("kmn: " + li + " がWebPage系にて重複しています。どちらかを削除しましょう。");
                        }
                    }
   
                }
            }
        }

        public bool ValidationOfNewRecordOfFreePlane(string newMMFile)
        {

            var checkDuplicateFreePlaneTasks = allTasks.SelectTasksTemplate($"select * from simpletasksystem.tasks where kmt=5 and kmn='{newMMFile}'");//既にkmt=5として存在していないかどうか
            if (checkDuplicateFreePlaneTasks.Count == 0)
            {
                return true;
            }
            else
            {
                //MessageBox.Show("Insertしようとしている、 " + newMMFile + " が既にTasksTableにKMT=5として存在してます。");
                return false;

            }

        }
        public bool ValidationOfNewRecordOfWebPage(string newWebPage)
        {

            var checkDuplicateFreePlaneTasks = allTasks.SelectTasksTemplate($"select * from simpletasksystem.tasks where kmt=4 and kmn={newWebPage}");//既にkmt=4として存在していないかどうか
            if (checkDuplicateFreePlaneTasks.Count == 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Insertしようとしている、 " + newWebPage + " が既にTasksTableにKMT=4として存在してます。");
                return false;

            }
        }
    }
}
