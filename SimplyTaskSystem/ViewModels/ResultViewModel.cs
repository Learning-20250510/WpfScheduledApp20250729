using Microsoft.Toolkit.Uwp.Notifications;
using SimplyTaskSystem.Views;
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
    internal class ResultViewModel : NotificationObject, ICloseWindows
    {
        public ResultViewModel()
        {

        }

        private readonly int ID;
        private int KMT;
        private int HTL;
        private string DueDate;
        private int CTIT;
        private int CTOOT;
        private int RTPD;
        private int RTPDD;
        private int ET;
        private int Priority;
        private readonly bool IsCleared;
        private int TemporaryRepeatTaskCount;

        public ResultViewModel(int actionID, bool isCleared)
        {

            ResultWindow.StartedUp = true;

            Models.DBs.TasksTable.Read tasksTableRead = new Models.DBs.TasksTable.Read();
            TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(tasksTableRead.SelectTasksTemplate("select * from simpletasksystem.tasks where id=" + actionID));

            if (TasksCollection.Count != 1)
            {
                MessageBox.Show("id=" + actionID + " でクエリ発行したのに、なぜかレコード数が１ではなく、" + TasksCollection.Count + " になっています。");
            }
            foreach (var task in TasksCollection)
            {
                this.ID = task.ID;
                this.KMT = task.KMT;
                this.HTL = task.HTL;
                this.DueDate = task.DueDate;
                this.CTIT = task.ClearTimesInTime;
                this.CTOOT = task.ClearTimesOutOfTime;
                this.RTPD = task.RepeatTimesPerDay;
                this.RTPDD = task.RepeatTimesPerDayDummy;
                this.ET = task.EstimatedTime;
                this.Priority = task.Priority;

                this.EstimatedTimeTextBox = task.EstimatedTime;
                this.PriorityTextBox = task.Priority;
                this.TemporaryRepeatTaskCount = task.TemporaryRepeatTaskCount;

            }
            this.IsCleared = isCleared;

            //
            CulculateNextDueDate();
        }



        private ObservableCollection<Models.DBs.TasksTable.DataClass> TasksCollection;

        private void CulculateNextDueDate()
        {
            Debug.WriteLine("変更前: due_date: " + this.DueDate + ", ctit: " + this.CTIT + ", ctoot: " + this.CTOOT);

            //復習日計算処理

            //第２引数値によるどちらのカラムに結果値を追加するかの条件分岐
            if (this.IsCleared == true)
            {
                this.CTIT++;
            }
            else
            {
                this.CTOOT++;
            }
            Random rand = new Random();
            int r1 = rand.Next(1, 50);//復習日周期固定だと面白くないので、ランダム性取り入れた。
            int NextDueDate = this.CTIT * 5 + this.CTOOT * 2;
            Debug.WriteLine("乱数値： " + r1 + " 適用前のクリアタイム数の２つからの次の復習日： " + NextDueDate);

            NextDueDate = NextDueDate / r1;
            if (NextDueDate == 0)
            {
                //さすがに、復習日は０日後だと、一生今日なので翌日以降にしたい
                NextDueDate = this.CTIT * 5 + this.CTOOT * 2;
            }
            Debug.WriteLine("乱数値適用後の次の加算復習日値（最終的）： " + NextDueDate);

            string[] arr1 = this.DueDate.Split(' ');

            //削除する文字の配列
            char[] removeChars = new char[] { ' ', '/' };

            arr1[0] = removeChars.Aggregate(
                arr1[0], (s, c) => s.Replace(c.ToString(), ""));

            Debug.WriteLine("余計な文字列削除後：  " + arr1[0]);



            DateTime d1 = DateTime.ParseExact(arr1[0], "yyyyMMdd", null);
            Debug.WriteLine("変更前due_dateをDateTime型に変換したら・・・" + d1);
            d1 = d1.AddDays(NextDueDate);
            Debug.WriteLine("最終的な次の決定復習日： " + d1);
            var tasks = new Models.DBs.TasksTable.Update();
            string DateTimeNow = DateTime.Now.ToString("yyyy/MM/dd");
            Debug.WriteLine(DateTimeNow);
            string d1_string = d1.ToString("yyyy/MM/dd HH:mm:ss");
            Debug.WriteLine(d1_string);

            //tasks.UpdateTasksRecord($"UPDATE simpletasksystem.tasks SET due_date={d1_string}, clear_times_intime={CTIT}, clear_times_outoftime={CTOOT},lastcleared_at={DateTimeNow}, where id={this.ID}");
            tasks.UpdateTasksRecordOfResult(d1_string, CTIT, CTOOT, DateTimeNow, this.ID);


            if (this.KMT == 2)//LearnFromTheWorld
            {
                //Insert ReviewOfTry
                //InsertReviewOfTry();

            }
        }


        private string _resultText;
        public string ResultText
        {
            get
            {
                if (this.IsCleared == true)
                {
                    return this._resultText = "Congratulation!!!おめでとう！時間内クリアだよ！";

                }
                else
                {
                    return this._resultText = "もう少し！！！おしかったよ！！！失敗は成功のもと！";

                }
            }
  

        }


        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand
        {
            get
            {
                return this._clearCommand ?? (this._clearCommand = new DelegateCommand(
                    _ =>
                    {


                        /*
                        //トースター通知
                        new ToastContentBuilder()
                         .AddText("⏰ TimeRecorder ⏰")
                         .AddText("タスクをクリアしました！")
                        .Show();
                

                        */


                        Random rand = new Random();
                        int r1 = rand.Next(1, 50);//復習日周期固定だと面白くないので、ランダム性取り入れた。
                        int NextDueDate = this.CTIT * 5 + this.CTOOT * 2;
                        Debug.WriteLine("乱数値： " + r1 + " 適用前のクリアタイム数の２つからの次の復習日： " + NextDueDate);

                        NextDueDate = NextDueDate / r1;
                        if (NextDueDate == 0)
                        {
                            //さすがに、復習日は０日後だと、一生今日なので翌日以降にしたい
                            NextDueDate = this.CTIT * 5 + this.CTOOT * 2;
                        }
                        Debug.WriteLine("乱数値適用後の次の加算復習日値（最終的）： " + NextDueDate);

                        string[] arr1 = this.DueDate.Split(' ');

                        //削除する文字の配列
                        char[] removeChars = new char[] { ' ', '/' };

                        arr1[0] = removeChars.Aggregate(
                            arr1[0], (s, c) => s.Replace(c.ToString(), ""));

                        Debug.WriteLine("余計な文字列削除後：  " + arr1[0]);



                        DateTime d1 = DateTime.ParseExact(arr1[0], "yyyyMMdd", null);
                        Debug.WriteLine("変更前due_dateをDateTime型に変換したら・・・" + d1);
                        d1 = d1.AddDays(NextDueDate);
                        Debug.WriteLine("最終的な次の決定復習日： " + d1);
                        var tasks = new Models.DBs.TasksTable.Update();
                        string DateTimeNow = DateTime.Now.ToString("yyyy/MM/dd");
                        Debug.WriteLine(DateTimeNow);
                        string d1_string = d1.ToString("yyyy/MM/dd HH:mm:ss");
                        Debug.WriteLine(d1_string);

                        //tasks.UpdateTasksRecord($"UPDATE simpletasksystem.tasks SET due_date={d1_string}, clear_times_intime={CTIT}, clear_times_outoftime={CTOOT},lastcleared_at={DateTimeNow}, where id={this.ID}");


                        if (this.RTPD > 1)//RepeatTask（連続的断続的どちらも一色たんにする）
                        {
                            this.RTPDD--;

                            if (IsCleared == true)//時間内クリアの場合
                            {
                                TemporaryRepeatTaskCount++;
                            }
                            else
                            {
                                TemporaryRepeatTaskCount--;
                            }
                            
                            if (this.RTPDD == 1)//繰り返し終了⊂RepeatTask 評価（タスクのクリア度合い）とリセットをする
                            {
                                if (TemporaryRepeatTaskCount > 0)//1日のrepeatTimesのそれぞれのクリア度合いを反映した計算式である
                                {
                                    this.CTIT++;

                                }
                                else
                                {
                                    this.CTOOT++;
                                }
                                tasks.UpdateTasksRecordOfResult(d1_string, CTIT, CTOOT, DateTimeNow, this.ID);
                                //RTPDDとTemporaryRepeatTaskCountを初期化する
                                tasks.UpdateTasksRecord($"UPDATE simpletasksystem.tasks SET repeat_times_per_day_dummy={this.RTPD}, temporary_repeat_task_count=0, postpone=false where id={this.ID}");

                                //繰り返し終了したということのFlag
                                MainWindow.RTPDD_ClearFlag = true;

                            }
                            else if (this.RTPDD < 1)//これはバグなので一度リセット
                            {
                                this.CTIT++;
                                tasks.UpdateTasksRecordOfResult(default, CTIT, CTOOT, null, this.ID);
                                tasks.UpdateTasksRecord($"UPDATE simpletasksystem.tasks SET clear_times_intime={CTIT},clear_times_outoftime={CTOOT} where id={this.ID}");
                                tasks.UpdateTasksRecord($"UPDATE simpletasksystem.tasks SET repeat_times_per_day_dummy={this.RTPD}, postpone=false where id={this.ID}");
                            }
                            else//正常（１よりも大きい状態==まだ本日のrepeatTaskが終了していない）
                            {
                                //tasks.UpdateTasksRecord($"UPDATE simpletasksystem.tasks SET clear_times_intime={CTIT},clear_times_outoftime={CTOOT} where id={this.ID}");
                                //RTPDD==1 にならないとき、ctit,ctoot, duedate,lastclearedAtは更新しないことにする
                                tasks.UpdateTasksRecord($"UPDATE simpletasksystem.tasks SET repeat_times_per_day_dummy={this.RTPDD}, temporary_repeat_task_count={this.TemporaryRepeatTaskCount}, postpone=false where id={this.ID}");
                            }
                        }
                        //Not Repeat Task
                        else
                        {
                            tasks.UpdateTasksRecordOfResult(d1_string, CTIT, CTOOT, DateTimeNow, this.ID);

                        }
                        UpdateTaskTemplate();
                        CloseWindow();
                        FocusDG1();

                        //PlayTaskのガチャ
                        int maxValue = 1 + this.ET;
                        if (this.IsCleared == true)
                        {
                            maxValue = maxValue + 50;

                        }
                        else
                        {
                            //Nothing
                        }
                        for (int i=0; i<maxValue; i++)
                        {
                            int r = rand.Next(0, 1000);
                            if (r == 1)
                            {
                                MainWindow.permitPlay = true;
                                MainWindow.valueOfPermitPlay = maxValue;
                            }
                        }
                        //


                        ResultWindow.StartedUp = false;

                    },
                    _ =>
                    {
                        return true;
                    }
                    ));

            }



        }

        private DelegateCommand _againCommand;
        public DelegateCommand AgainCommand
        {
            get
            {
                return this._againCommand ?? (this._againCommand = new DelegateCommand(
                    _ =>
                    {
                        CloseWindow();
                        //トースター通知
                        new ToastContentBuilder()
                         .AddText("⏰ TimeRecorder ⏰")
                         .AddText("タスクをもう１度実行します！")
                         .Show();


                        TaskActionWindow taw = new TaskActionWindow(this.ID);
                        TaskActionViewModel tavm = new TaskActionViewModel(this.ID);
                        taw.DataContext = tavm;
                        taw.Show();



                    },
                    _ =>
                    {
                        return true;
                    }
                    ));

            }


        }
        

        private DelegateCommand _againLaterCommand;
        public DelegateCommand AgainLaterCommand
        {
            get
            {
                return this._againLaterCommand ?? (this._againLaterCommand = new DelegateCommand(
                    _ =>
                    {


                        //トースター通知
                        new ToastContentBuilder()
                         .AddText("⏰ TimeRecorder ⏰")
                         .AddText("タスクを後に伸ばします！")
                         .Show();

       
                        var tasks = new Models.DBs.TasksTable.Update();
                        tasks.UpdateTasksRecord($"UPDATE simpletasksystem.tasks SET postpone=true where id={this.ID}");

                        UpdateTaskTemplate();
                        CloseWindow();

                        FocusDG1();

                        ResultWindow.StartedUp = false;



                    },
                    _ =>
                    {
                        return true;
                    }
                    ));

            }


        }

        private DelegateCommand _updateTaskColumnCommad;
        public DelegateCommand UpdateTaskColumnCommad
        {
            get
            {
                return this._updateTaskColumnCommad ?? (this._updateTaskColumnCommad = new DelegateCommand(
                    _ =>
                    {




                        var updateWindow = new UpdateTaskRecordWindow();
                        var updateVM = new UpdateTaskRecordViewModel(this.ID);
                        updateWindow.DataContext = updateVM;
                        updateWindow.Show();

                        CloseWindow();

                        FocusDG1();

                        ResultWindow.StartedUp = false;



                    },
                    _ =>
                    {
                        return true;
                    }
                    ));

            }


        }


        private int _estimatedTimeTextBox;
        public int EstimatedTimeTextBox
        {
            get
            {
                return this._estimatedTimeTextBox;
            }
            set
            {
                if (SetProperty(ref this._estimatedTimeTextBox, value))
                {
                    var updateRecord = new Models.DBs.TasksTable.Update();
                    updateRecord.UpdateTasksRecord($"update simpletasksystem.tasks set estimated_time={EstimatedTimeTextBox} where id={ID}");
                    //MessageBox.Show("ID= " + ID + " のestimated_timeカラムValueを、" + EstimatedTimeTextBox + " に変更しました。");

                }
            }
        }

        private int _priorityTextBox;
        public int PriorityTextBox
        {
            get
            {
                return this._priorityTextBox;
            }
            set
            {
                if (SetProperty(ref this._priorityTextBox, value))
                {
                    var updateRecord = new Models.DBs.TasksTable.Update();
                    updateRecord.UpdateTasksRecord($"update simpletasksystem.tasks set priority={PriorityTextBox} where id={ID}");
                    //MessageBox.Show("ID= " + ID + " のpriorityカラムValueを、" + PriorityTextBox + " に変更しました。");

                }
            }
        }


        public static void FocusDG1()
        {
            DG1Focus?.Invoke();
        }

        public static Action DG1Focus { get; set; }

        void CloseWindow()
        {
            Close?.Invoke();
        }
        public Action Close { get; set; }

      
        public static void UpdateTaskTemplate()
        {
            TaskTemplate?.Invoke();
        }
        public static Action TaskTemplate { get; set; }

    }

    interface ICloseWindows
    {
        Action Close { get; set; }
    }

}
