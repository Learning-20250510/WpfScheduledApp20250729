using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Toolkit.Uwp.Notifications;
using SimplyTaskSystem.Models;
using SimplyTaskSystem.ViewModels;

using SimplyTaskSystem.Models.DBs.MotivationsTable;
namespace SimplyTaskSystem.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            motivationInsert = new Models.DBs.MotivationsTable.Insert();

            InitializeComponent();

    
            // WindowHandleを取得
            var host = new WindowInteropHelper(this);
            WindowHandle = host.Handle;

            // HotKeyを設定
            SetUpHotKey();
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;

            //時間指定タスク
            var timer1 = CreateTimer(60000, TimerMethod);
            timer1.Start();
       

            var timer2 = CreateTimer(1000, TimerMethodForNextActionTask);
            timer2.Start();

            var timer3 = CreateTimer(200000, TimerMethodForCountOfExerciseTask);
            timer3.Start();




        }

        private void TimerMethodForRepeatDuration()
        {
        }

        private void TimerMethod()
        {

            var timeTasks = new Models.DBs.TasksTable.Read();
            var tasks = timeTasks.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) and due_date >= CURDATE() and due_time is not null and repeat_patterns in (1,2))");
            foreach (var tk in tasks)
            {
                Debug.WriteLine("due_time: " + tk.DueTime);
                string nowTime = DateTime.Now.ToString("HH:mm:ss");

                Debug.WriteLine(DateTime.Now);


                DateTime d1 = DateTime.ParseExact(tk.DueTime, "HH:mm:ss", null);
                Debug.WriteLine("tk.DueTime: " + tk.DueTime);

                if (DateTime.Now >= d1)
                {
                    Debug.WriteLine("比較していくよ！");
                    Debug.WriteLine(DateTime.Now);
                    Debug.WriteLine(d1);


                    //トースター通知
                    new ToastContentBuilder()
                     .AddText("⏰ TimeRecorder ⏰")
                     .AddText(tk.KMT + "_" + tk.KMN + "が本日" + tk.DueTime + " より開始の通知のお知らせです。今あるタスクを中断してこちらを実行しましょう。")
                     .Show();
                }
            }
            }

        public static bool permitPlay;
        private bool permitExercise;
        public static int valueOfPermitPlay=1;
        private bool permitRTPDTask;

        private ObservableCollection<Models.DBs.TasksTable.DataClass> actionTasks = new ObservableCollection<Models.DBs.TasksTable.DataClass>();

        internal static class ForTimerVariable
        {
            public static int DoingID = 0;
            public static DateTime nextDoingTime;//14:33:10(etc(Secondはどうでもいいけど）
            public static bool IsReservation = false;
        }
        private void TimerMethodForNextActionTask()
        {
            //RTPDのタスクの準備
            var shuffleList = new List<int>();
            var shuffleRepeatDurationList = new List<int>();

            //なんだっけこれ
            if (ForTimerVariable.DoingID == 0)
            {
                var Tasks = new Models.DBs.TasksTable.Read();
                //var repeatTasks = Tasks.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) and due_time is null and repeat_times_per_day>1)");
                var repeatTasks = Tasks.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_time is null and repeat_patterns in (3)) ORDER BY RAND()");//断続的タスク（1つもレコードがないとエラーになったなので、何かしらダミーレコードいれておくか

                foreach (var rp_task in repeatTasks)
                {

                    shuffleList.Add(rp_task.ID);
                    shuffleRepeatDurationList.Add(rp_task.RepeatDuration);
                }
            }



            var rand = new Random();
            if (shuffleList.Count == 0)//repeatTaskが存在しないならやる必要がないのでreturn
            {
                //return;
                Debug.WriteLine("shuffleList.Count=０でした。");

            }
            else
            {
                if (ForTimerVariable.DoingID == 0)
                {
                    var randed_i = rand.Next(0, shuffleList.Count);
                    ForTimerVariable.DoingID = shuffleList[randed_i];
                    ForTimerVariable.nextDoingTime = DateTime.Now.AddMinutes(shuffleRepeatDurationList[randed_i]);
                    Debug.WriteLine("DoingID: " + ForTimerVariable.DoingID + " nextDoingTime: " + ForTimerVariable.nextDoingTime + " が次の実行予定タスクです");
                }
                else
                {

           
                }



            }
            if (ForTimerVariable.nextDoingTime <= DateTime.Now)
            {
                Debug.WriteLine("DoingID: " + ForTimerVariable.DoingID + " nextDoingTime: " + ForTimerVariable.nextDoingTime + " の実行予約が完了しました。");
                ForTimerVariable.IsReservation = true;

            }


            //PlayTaskの準備
            if (permitPlay == true)
            {
                var wholeRand = rand.Next(100);

                var task = new Models.DBs.TasksTable.Read();

                /*
                var actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=6");
                var shuffeledActionTask = actionTask.OrderBy(a => Guid.NewGuid()).ToList();
                */

                ObservableCollection<Models.DBs.TasksTable.DataClass> actionTask = new ObservableCollection<Models.DBs.TasksTable.DataClass>();
                var specificRangeRead = new Models.DBs.SpecificRangeTable.Read();


                //全体
                if (wholeRand > 97)
                {

                    MessageBox.Show("全体、のPlayTaskが選ばれました。");

                    int r = 0;

                    for (int i = 0; i < valueOfPermitPlay; i++)
                    {
                        var dummy_r = rand.Next(0, 10000);
                        Debug.WriteLine("valueofPermitPlay: " + valueOfPermitPlay);

                        if (r < dummy_r)
                        {
                            r = dummy_r;
                        }
                    }

                    if (r > 9900)
                    {
                        actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=6 and estimated_time < 120 ORDER BY RAND() limit 1");

                    }
                    else if (r > 9700)
                    {
                        actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=6 and estimated_time < 60 ORDER BY RAND()limit 1");


                    }
                    else if (r > 9400)
                    {
                        actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=6 and estimated_time < 30 ORDER BY RAND() limit 1");

                    }
                    else if (r > 9000)
                    {
                        actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=6 and estimated_time < 10 ORDER BY RAND() limit 1");

                    }
                    else
                    {
                        //Nothing
                    }
                }

                //GameRom
                else if (wholeRand > 50)
                {
                    MessageBox.Show("GameRomのPlayTaskが選ばれました。");

                    //GameRomのタスクを全取得
                    var randomTasks = specificRangeRead.SelectSpecificRangeCollection("select * from simpletasksystem.SpecificRange where category='GameRom'");
                    var dummyPreActionTask = new ObservableCollection < Models.DBs.TasksTable.DataClass> ();
                    var preActionTaskCollection = new ObservableCollection < Models.DBs.TasksTable.DataClass> ();

                    //GameRomの全レコードをTasksTableへ移行している。
                    foreach (var ran in randomTasks)
                    {
                        //dummyとして受け取り
                        dummyPreActionTask =task.SelectTasksTemplate($"select * from simpletasksystem.tasks where id={ran.TasksTableID}");

                        //BuckUp
                        foreach (var li in dummyPreActionTask)
                        {
                            preActionTaskCollection.Add(li);
                        }
                        //初期化
                        dummyPreActionTask = new ObservableCollection<Models.DBs.TasksTable.DataClass>();

                    }


                    int r = 0;
                    int targetID = 0;

                    //試行回数を減らすため、１０で割り算をしている
                    Debug.WriteLine("valueofPermitPlay: " + valueOfPermitPlay);
                    for (int i = 0; i < valueOfPermitPlay / 10; i++)
                    {
                        //乱数の範囲
                        var dummy_r = rand.Next(0, preActionTaskCollection.Count);

                        if (r <= preActionTaskCollection[dummy_r].EstimatedTime)
                        {
                            r = preActionTaskCollection[dummy_r].EstimatedTime;
                            targetID = preActionTaskCollection[dummy_r].ID;
                        }
                    }

                    actionTask = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where id={targetID}");




                }
                //Steam
                else if (wholeRand > -1)
                {
                    MessageBox.Show("SteamのPlayTaskが選ばれました。");
                    //Steamのタスクを全取得
                    var randomTasks = specificRangeRead.SelectSpecificRangeCollection("select * from simpletasksystem.SpecificRange where category='Steam'");
                    var dummyPreActionTask = new ObservableCollection<Models.DBs.TasksTable.DataClass>();
                    var preActionTaskCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>();

                    //Steamの全レコードをTasksTableへ移行している。
                    foreach (var ran in randomTasks)
                    {
                        //dummyとして受け取り
                        dummyPreActionTask = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where id={ran.TasksTableID}");

                        //BuckUp
                        foreach (var li in dummyPreActionTask)
                        {
                            preActionTaskCollection.Add(li);
                        }
                        //初期化
                        dummyPreActionTask = new ObservableCollection<Models.DBs.TasksTable.DataClass>();

                    }


                    int r = 0;
                    int targetID = 0;

                    //試行回数を減らすため、１０で割り算をしている
                    Debug.WriteLine("valueofPermitPlay: " + valueOfPermitPlay);
                    for (int i = 0; i < valueOfPermitPlay / 10; i++)
                    {
                        //乱数の範囲
                        var dummy_r = rand.Next(0, preActionTaskCollection.Count);

                        if (r <= preActionTaskCollection[dummy_r].EstimatedTime)
                        {

                            Debug.WriteLine("dummy_r: " + dummy_r + "   r: " + r);
                            r = preActionTaskCollection[dummy_r].EstimatedTime;
                            Debug.WriteLine("dummy_r: " + dummy_r + "   r: " + r);
                            targetID = preActionTaskCollection[dummy_r].ID;
                        }
                    }

                    actionTask = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where id={targetID}");


                }


                //ここから、全体に対する処理（どのplayタスクに対してもする共通処理）

                foreach (var ac in actionTask)
                {
                    MessageBox.Show("ガチャにあたりました！" + ac.KMN + " タスクを実行します。");
                    var tavm = new TaskActionViewModel(ac.ID);
                    var taw = new TaskActionWindow(ac.ID);
                    taw.DataContext = tavm;
                    taw.Show();

                }

                /*

                for (int i=0; i < valueOfPermitPlay; i++)
                {
                    var r = rand.Next(0, 20000);

                    bool isExitLoop = false;
                    foreach (var ac in shuffeledActionTask)
                    {
                        Debug.WriteLine("ac.KMN: " + ac.KMN);
                        if (ac.EstimatedTime == r)
                        {
                            MessageBox.Show("ガチャにあたりました！" + ac.KMN + " タスクを実行します。");
                            var tavm = new TaskActionViewModel(ac.ID);
                            var taw = new TaskActionWindow(ac.ID);
                            taw.DataContext = tavm;
                            taw.Show();
                            isExitLoop = true;
                            break;
                            
                        }
                    }

                    if (isExitLoop == true)
                    {
                        break;
                    }
                }

                */

                permitPlay = false;//Reset
            }
            else if (permitExercise == true && ResultWindow.StartedUp == false && TaskActionWindow.StartedUp == false)
            {
                var task = new Models.DBs.TasksTable.Read();

                ObservableCollection<Models.DBs.TasksTable.DataClass> actionTask = new ObservableCollection<Models.DBs.TasksTable.DataClass>();


                int r = 0;
                var dummy_r = rand.Next(0, 10);


                //腰痛体操
                if (r < 7)
                {
                    actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=14 and kmn LIKE 'Exercise_LowBackPain%' ORDER BY RAND() limit 1");

                }
                //通常のエクササイズ全般
                else if (r >= 7)
                {
                    actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where priority=14 ORDER BY RAND()limit 1");


                }
                else
                {
                    //Nothing
                }

                foreach (var ac in actionTask)
                {
                    MessageBox.Show("Exerciseの時間です。" + ac.KMN + " タスクを実行します。");
                    var tavm = new TaskActionViewModel(ac.ID);
                    var taw = new TaskActionWindow(ac.ID);
                    taw.DataContext = tavm;
                    taw.Show();

                    permitExercise = false;

                }

            }
            else if (ForTimerVariable.IsReservation == true)
            {
                if (TaskActionWindow.StartedUp == false && ResultWindow.StartedUp == false)
                {
                    MessageBox.Show("予約タスクを実行します。");
                    Debug.WriteLine("予約タスクを実行します。");
                    TaskActionWindow taw = new TaskActionWindow(ForTimerVariable.DoingID);

                    TaskActionViewModel tav = new TaskActionViewModel(ForTimerVariable.DoingID);
                    taw.DataContext = tav;
                    taw.Show();
                    taw.Activate();

                    //すべてを初期化
                    ForTimerVariable.DoingID = 0;
                    ForTimerVariable.nextDoingTime = new DateTime();
                    ForTimerVariable.IsReservation = false;
                    Debug.WriteLine("DoingID: " + ForTimerVariable.DoingID + " nextDoingTime: " + ForTimerVariable.nextDoingTime + " IsReservation: " + ForTimerVariable.IsReservation);
                }
            }


            Debug.WriteLine("TAW: " + TaskActionWindow.StartedUp + " AddTaskViewModel: " + AddTaskViewModel.StartedUp + " UpdateTask: " + UpdateTaskRecordViewModel.StartedUp + ResultWindow.StartedUp);

            //Today以下のタスク自動実行（time is null and rtpd>1), today以下）
            if (this.AutoActMode_CheckBox.IsChecked == true && TaskActionWindow.StartedUp == false && ResultWindow.StartedUp == false && AddTaskViewModel.StartedUp == false && UpdateTaskRecordViewModel.StartedUp == false)
            {
                Debug.WriteLine("AutoActMode_CheckBox: " + "により実行されます。");


                
     

                //actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) and due_time is null and repeat_times_per_day=1 and priority in(1,2)) ORDER BY RAND() limit 1");

                /*
                foreach (var ac in actionTask)
                {
                    var taw = new TaskActionWindow(ac.ID);

                    var tavm = new TaskActionViewModel(ac.ID);
                    taw.DataContext = tavm;
                    taw.Show();
                }
                */

                //actionTaskの残りタスク数が０になったら再度取得していく。
                if (actionTasks.Count == 0)
                {
                    var task = new Models.DBs.TasksTable.Read();


                    /*【大前提】
                     * PCの値は、クエリ文に指定していないので、ＰＣがなんであれPriorityのみで評価しているので、
                     * PCの値を心配しなくても、ＰＲさえ注意しておけば、task(gameと呼ぼうかな）実行漏れ
                     * 回避できるはず。
                     * Q: e.g.: Priority=7(justnow)でDueDateが未来でかつPCが本日を示しているタスクについて
                     * 実行が遅れないか？
                     * A: 確かに。
                     *  Solution:
                     *      1:クエリに、論理和でPCのときの場合を追記
                     *      2:JustNowが全部終わったら＝＞PCのタスクに＝＞Today
                     *          n.b.:
                     *              Priorityがlow and PCの値が今日を示しているタスク＞PR=Today なことも起こり得ると思うけど
                     *              しかたないかな。それよりも、PR=7が実行遅れるほうが優先順位がlowerになるほうが問題かも。
                     * 
                     */

                    //「JustNow」 Taskが存在しなかったら(JustNowが存在しな場合のみ下位のPriorityTask実行権付与
                    actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) and due_time is null and repeat_patterns in (1,2) and priority in(7) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 5");
                    //actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where id=633974");
                    if (actionTasks.Count == 0)
                    {
                        
                        actionTasks = task.GetTasksOfTodayMUST_PC();

                        if (actionTasks.Count == 0)
                        {
                            //次の優先順位は"Today"
                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) and due_time is null and repeat_patterns in (1,2) and priority in(2) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 5");

                            if (actionTasks.Count == 0)
                            {
                                MessageBox.Show("今日やるべき周期的なタスクはありませんでした。");

                                var r = rand.Next(0, 1000);

                                //In a Few Days(3日以内）
                                if (r <= 600)
                                {
                                    do
                                    {

                                        int i = 1;

                                        //actionTask = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) and due_time is null and repeat_times_per_day=1 and priority in(2)) ORDER BY RAND() limit 1");


                                        if (i == 0)//期限切れタスク（本来あってはならない）
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 4 day) and due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (i == 1)//期限ぎりぎりタスク
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 3) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL 4 day) and due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (i == 2)//期限の2日前
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 2) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL 3 day) and due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (i == 3)//期限の1日前
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 1) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL 2 day) and due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (i == 4)//期限の0日前
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 0) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL 1 day) and due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (i == 5)//期限の前日
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL -1) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL 0 day) and due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (i == 6)//期限の2日前
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL -2) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL -1 day) and due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (i == 7)//期限の3日前より前全部
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where ( due_date < DATE_ADD(CURRENT_DATE, INTERVAL -2 day) and due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (i == 8)//すべてのInAFewDays
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_time is null and repeat_patterns in (1,2) and priority in(3) and htl NOT IN (1,9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                            break;
                                        }
                                        i++;
                                    }
                                    while (actionTasks.Count == 0);//もう少し期限を広げてタスク検索。


                                }
                                else if (r <= 800)//1週間以内
                                {

                                    int repeatCount = 1;

                                    int less = 7;
                                    int more = 8;

                                    while (true)
                                    {
                                        if (repeatCount == 1)//期限切れタスク（本来あってはならない）
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 8 day) and due_time is null and repeat_patterns in (1,2) and priority in(8) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (repeatCount < 12)
                                        {
                                            actionTasks = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL {less}) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL {more} day) and due_time is null and repeat_patterns in (1,2) and priority in(8) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (repeatCount == 12)
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_time is null and repeat_patterns in (1,2) and priority in(8) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                            break;
                                        }
                                        repeatCount++;
                                        less--;
                                        more--;
                                    }

                                }
                                else if (r <= 900)
                                {
                                    int repeatCount = 1;

                                    int less = 14;
                                    int more = 9;

                                    while (true)
                                    {
                                        if (repeatCount == 1)//期限切れタスク（本来あってはならない）
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 15 day) and due_time is null and repeat_patterns in (1,2) and priority in(9) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (repeatCount < 17)
                                        {
                                            actionTasks = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL {less}) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL {more} day) and due_time is null and repeat_patterns in (1,2) and priority in(9) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (repeatCount == 17)
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_time is null and repeat_patterns in (1,2) and priority in(9) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                            break;
                                        }
                                        repeatCount++;
                                        less--;
                                        more--;
                                    }

                                }
                                else if (r <= 950)
                                {
                                    int repeatCount = 1;

                                    int less = 31;
                                    int more = 32;

                                    while (true)
                                    {
                                        if (repeatCount == 1)//期限切れタスク（本来あってはならない）
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 32 day) and due_time is null and repeat_patterns in (1,2) and priority in(10) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (repeatCount < 35)
                                        {
                                            actionTasks = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL {less}) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL {more} day) and due_time is null and repeat_patterns in (1,2) and priority in(10) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (repeatCount == 35)
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_time is null and repeat_patterns in (1,2) and priority in(10) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                            break;
                                        }
                                        repeatCount++;
                                        less--;
                                        more--;
                                    }


                                }
                                else if (r <= 975)
                                {
                                    int repeatCount = 1;

                                    int less = 93;
                                    int more = 94;

                                    while (true)
                                    {
                                        if (repeatCount == 1)//期限切れタスク（本来あってはならない）
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 94 day) and due_time is null and repeat_patterns in (1,2) and priority in(11) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (repeatCount < 97)
                                        {
                                            actionTasks = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL {less}) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL {more} day) and due_time is null and repeat_patterns in (1,2) and priority in(11) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (repeatCount == 97)
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_time is null and repeat_patterns in (1,2) and priority in(11) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                            break;
                                        }
                                        repeatCount++;
                                        less--;
                                        more--;
                                    }
                                }
                                else if (r <= 995)
                                {
                                    int repeatCount = 1;

                                    int less = 182;
                                    int more = 183;

                                    while (true)
                                    {
                                        if (repeatCount == 1)//期限切れタスク（本来あってはならない）
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 183 day) and due_time is null and repeat_patterns in (1,2) and priority in(12) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (repeatCount < 186)
                                        {
                                            actionTasks = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL {less}) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL {more} day) and due_time is null and repeat_patterns in (1,2) and priority in(12) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (repeatCount == 186)
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_time is null and repeat_patterns in (1,2) and priority in(12) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                            break;
                                        }

                                        repeatCount++;
                                        less--;
                                        more--;
                                    }
                                }
                                else if (r <= 999)
                                {
                                    int repeatCount = 1;

                                    int less = 365;
                                    int more = 366;

                                    while (true)
                                    {
                                        if (repeatCount == 1)//期限切れタスク（本来あってはならない）
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL 366 day) and due_time is null and repeat_patterns in in(1,2) and priority in(13) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");

                                        }
                                        else if (repeatCount < 368)
                                        {
                                            actionTasks = task.SelectTasksTemplate($"select * from simpletasksystem.tasks where (due_date >= DATE_ADD(CURRENT_DATE, INTERVAL {less}) AND due_date < DATE_ADD(CURRENT_DATE, INTERVAL {more} day) and due_time is null and repeat_patterns in (1,2) and priority in(13) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                        }
                                        else if (repeatCount == 368)
                                        {
                                            actionTasks = task.SelectTasksTemplate("select * from simpletasksystem.tasks where (due_time is null and repeat_patterns in (1,2) and priority in(13) and htl NOT IN (9,10,13) and archived=0) ORDER BY RAND() limit 10");
                                            break;
                                        }
                                        repeatCount++;
                                        less--;
                                        more--;
                                    }
                                }
                                else
                                {
                                    //随時追加
                                }
                            }

                        }

                    }
         
           
                }
                else
                {
                    //実行した段階で、クリアのいかんにかかわらず、削除される機構
                    var taw = new TaskActionWindow(actionTasks[0].ID);

                    var tavm = new TaskActionViewModel(actionTasks[0].ID);
                    taw.DataContext = tavm;
                    taw.Show();
                    if (actionTasks[0].RepeatPatterns == 1)//NoRepeat
                    {
                        MessageBox.Show("削除しました。1" + actionTasks[0].KMN);
                        actionTasks.RemoveAt(0);

                    }
                    else if (actionTasks[0].RepeatPatterns == 2)//Repeat(連続的)
                    {
                        //RTPDD == 2の段階で、実行キュータスク群から削除してあるのは、ここで削除しておかないとResultViewModelのほうで、RTPDDがResetされて無限ループになる。
                        if (RTPDD_ClearFlag == true)
                        {
                            actionTasks.RemoveAt(0);
                            MessageBox.Show(actionTasks[0].KMN + " の本日のrepeatTimesは、残り1回で終了です。実行タスクキューから削除しました。");

                            //初期化
                            RTPDD_ClearFlag = false;
                        }

                    }
                    else if (actionTasks[0].RepeatPatterns == 3)//Repeat(断続的）（基本的に、AutoActModeでは現在のところ、実行する予定はない値だが一応記述しておくか
                    {
                        MessageBox.Show("削除しました。2" + actionTasks[0].KMN);

                        actionTasks.RemoveAt(0);

                    }
                }

        

     

            }
        }

        //条件式を、Result画面でCLEARボタンが押されると、RTPDD--;になる処理を記述しているので、この条件式を（＝＝２）で評価すると無限ループが発生したので、独自のフラグを用意した。
        public static bool RTPDD_ClearFlag = false;

        private void TimerMethodForCountOfExerciseTask()
        {

            var rand = new Random();

            if (rand.Next(10) == 0)
            {
                permitExercise = true;
            }

        }

        private DispatcherTimer CreateTimer(int interval, Action action)
        {
            // 優先順位を指定してタイマのインスタンスを生成
            DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);

            // インターバルを設定
            timer.Interval = new TimeSpan(0, 0, 0, 0, interval);

            // タイマメソッドを設定
            timer.Tick += (e, s) => { action(); };

            // 画面が閉じられるときに、タイマを停止
            this.Closing += (e, s) => { timer.Stop(); };

            return timer;
        }

        private void ActButton_Click(object sender, RoutedEventArgs e)
        {
            object ID = ((Button)sender).CommandParameter;

            System.Diagnostics.Debug.WriteLine("sender: " + sender);
            System.Diagnostics.Debug.WriteLine("e: " + e);
            //MessageBox.Show(ID.ToString());

            TaskActionWindow taw = new TaskActionWindow(int.Parse(ID.ToString()));
            TaskActionViewModel tavm = new TaskActionViewModel(int.Parse(ID.ToString()));
            taw.DataContext = tavm;
            taw.Show();


            //rowIndex++;
            focusActBtn();


        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            object ID = ((Button)sender).CommandParameter;
            MessageBox.Show(ID.ToString());

            UpdateTaskRecordWindow wtuaita = new UpdateTaskRecordWindow();
            UpdateTaskRecordViewModel wtuaitavm = new UpdateTaskRecordViewModel(int.Parse(ID.ToString()));
            wtuaita.DataContext = wtuaitavm;
            wtuaita.Show();
        }


        public int rowIndex = 0;

        public void focusActBtn()
        {

            //int rowIndex = 0;     // 0 ～
            int columnIndex = 0;  // 0 ～

            // データグリッドにフォーカスが必要
            DataGrid1.Focus();

            try
            {
                // DataGridCellInfo を生成
                DataGridCellInfo cellInfo = new DataGridCellInfo(DataGrid1.Items[rowIndex], DataGrid1.Columns[columnIndex]);

                // CurrentCell を設定
                DataGrid1.CurrentCell = cellInfo;

                // 選択中の行を設定
                DataGrid1.SelectedIndex = rowIndex;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("実行するタスクがありません。");
    
            }







        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }





        /*
     * HotKey登録時に指定するID。
     * アプリケーションの場合は、0x0000～0xbfffの間で指定すること。
     * （共有DLLの場合は、0xc000～0xffffの間を使用する。）
     */
        private const int HOTKEY_ID1 = 0x0001;
        private const int HOTKEY_ID2 = 0x0002;
        private const int HOTKEY_ID3 = 0x0003;
        private const int HOTKEY_ID4 = 0x0004;
        private const int HOTKEY_ID5 = 0x0005;
        private const int HOTKEY_ID6 = 0x0006;
        private const int HOTKEY_ID7 = 0x0007;
        private const int HOTKEY_ID8 = 0x0008;
        private const int HOTKEY_ID9 = 0x0009;
        private const int HOTKEY_ID10 = 0x00010;
        private const int HOTKEY_ID11 = 0x00011;
        private const int HOTKEY_ID12 = 0x00012;
        private const int HOTKEY_ID13 = 0x00013;
        private const int HOTKEY_ID14 = 0x00014;
        private const int HOTKEY_ID15 = 0x00015;
        private const int HOTKEY_ID16 = 0x00016;
        private const int HOTKEY_ID17 = 0x00017;
        private const int HOTKEY_ID18 = 0x00018;
        private const int HOTKEY_ID19 = 0x00019;
        private const int HOTKEY_ID20 = 0x00020;
        private const int HOTKEY_ID21 = 0x00021;
        private const int HOTKEY_ID22 = 0x00022;
        private const int HOTKEY_ID23 = 0x00023;
        private const int HOTKEY_ID24 = 0x00024;
        private const int HOTKEY_ID25 = 0x00025;
        private const int HOTKEY_ID26 = 0x00026;

        // HotKey Message ID
        private const int WM_HOTKEY = 0x0312;

        private IntPtr WindowHandle;

        /* 
         * HotKey登録を行う関数。
         * 失敗の場合は0（他で使用されている）、成功の場合は0以外の数値が返される。
         */
        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, int modKey, int vKey);

        /*
         * HotKey解除を行う関数。
         * 失敗の場合は0、成功の場合は0以外の数値が返される。
         */
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);




        // HotKey登録。
        private void SetUpHotKey()
        {
            // Alt + 1 をHotKeyとして登録。
            var result1 = RegisterHotKey(WindowHandle, HOTKEY_ID1, (int)ModifierKeys.Control, KeyInterop.VirtualKeyFromKey(Key.R));
            if (result1 == 0)
            {
                //MessageBox.Show("HotKey1の登録に失敗しました。");
            }

            // Alt + テンキーの1 をHotKeyとして登録。
            var result2 = RegisterHotKey(WindowHandle, HOTKEY_ID2, (int)ModifierKeys.Control + (int)ModifierKeys.Shift, KeyInterop.VirtualKeyFromKey(Key.T));
            if (result2 == 0)
            {
                //MessageBox.Show("HotKey2の登録に失敗しました。");
            }

            //未定
            var result3 = RegisterHotKey(WindowHandle, HOTKEY_ID3, (int)ModifierKeys.Alt, KeyInterop.VirtualKeyFromKey(Key.V));

            //未定
            var result4 = RegisterHotKey(WindowHandle, HOTKEY_ID4, (int)ModifierKeys.Alt, KeyInterop.VirtualKeyFromKey(Key.B));
    

            var result5 = RegisterHotKey(WindowHandle, HOTKEY_ID5, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D1));
            var result6 = RegisterHotKey(WindowHandle, HOTKEY_ID6, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D2));
            var result7 = RegisterHotKey(WindowHandle, HOTKEY_ID7, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D3));
            var result8 = RegisterHotKey(WindowHandle, HOTKEY_ID8, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D4));
            var result9 = RegisterHotKey(WindowHandle, HOTKEY_ID9, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D5));
            var result10 = RegisterHotKey(WindowHandle, HOTKEY_ID10, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D6));
            var result11 = RegisterHotKey(WindowHandle, HOTKEY_ID11, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D7));
            var result12 = RegisterHotKey(WindowHandle, HOTKEY_ID12, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D8));
            var result13 = RegisterHotKey(WindowHandle, HOTKEY_ID13, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D9));
            var result14 = RegisterHotKey(WindowHandle, HOTKEY_ID14, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift), KeyInterop.VirtualKeyFromKey(Key.D0));
            var result15 = RegisterHotKey(WindowHandle, HOTKEY_ID15, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D0));
            var result16 = RegisterHotKey(WindowHandle, HOTKEY_ID16, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D1));
            var result17 = RegisterHotKey(WindowHandle, HOTKEY_ID17, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D2));
            var result18 = RegisterHotKey(WindowHandle, HOTKEY_ID18, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D3));
            var result20 = RegisterHotKey(WindowHandle, HOTKEY_ID20, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D4));
            var result21 = RegisterHotKey(WindowHandle, HOTKEY_ID20, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D5));
            var result22 = RegisterHotKey(WindowHandle, HOTKEY_ID20, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D6));
            var result23 = RegisterHotKey(WindowHandle, HOTKEY_ID20, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D7));
            var result24 = RegisterHotKey(WindowHandle, HOTKEY_ID20, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D8));
            var result25 = RegisterHotKey(WindowHandle, HOTKEY_ID20, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.D9));
            var result26 = RegisterHotKey(WindowHandle, HOTKEY_ID20, ((int)ModifierKeys.Control + (int)ModifierKeys.Shift + (int)ModifierKeys.Alt), KeyInterop.VirtualKeyFromKey(Key.Q));



        }

        private Models.DBs.MotivationsTable.Insert motivationInsert;

        // ここでHotKeyが押された際の挙動を設定する。
        void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            // HotKeyが押されたかどうかを判定。
            if (msg.message != WM_HOTKEY) return;

            switch (msg.wParam.ToInt32())
            {
                case HOTKEY_ID1:
                    //MessageBox.Show("HotKey1(Alt + D1)");

                    //MainViewModel mvm = new MainViewModel();
                    //this.DataContext = mvm;
                    //DataGrid1.Items.Refresh();
                    //focusActBtn();
                    //rowIndex++;
                    this.Activate();

                    break;
                case HOTKEY_ID2:
                    //this.Activate();

                    //MessageBox.Show("MainWIndowがActiveになりました");

                    /*
                    MainViewModel mvm2 = new MainViewModel();
                    this.DataContext = mvm2;
                    */

                    AddTaskViewModel.StartedUp = true;
                    AddTaskViewModel wtavm = new AddTaskViewModel();
                    AddTaskWindow wta = new AddTaskWindow();

                    wta.DataContext = wtavm;
                    wta.Show();
                    wta.Activate();

                    TaskActionWindow.damageCount += 5;
                    break;

                case HOTKEY_ID3:
           
                    break;

                case HOTKEY_ID4:
              
                    break;
                case HOTKEY_ID5:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[0]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[0] + " レコードが追加されました。");
                    MessageBox.Show("motivationがないほうが、余計なこと考えなくて機械的にやりやすいから、今のお前は最高ＤＡ★");
                    break;
                case HOTKEY_ID6:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[1]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[1] + " レコードが追加されました。");


                    break;
                case HOTKEY_ID7:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[2]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[2] + " レコードが追加されました。");


                    break;
                case HOTKEY_ID8:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[3]);

                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[3] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID9:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[4]);

                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[4] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID10:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[5]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[5] + " レコードが追加されました。");


                    break;

                case HOTKEY_ID11:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[6]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[6] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID12:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[7]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[7] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID13:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[8]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[8] + " レコードが追加されました。");


                    break;

                case HOTKEY_ID14:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[9]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[9] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID15:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[10]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[10] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID16:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[11]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[11] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID17:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[12]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[12] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID18:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[13]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[13] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID19:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[14]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[14] + " レコードが追加されました。");


                    break;

                case HOTKEY_ID20:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[15]);

                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[15] + " レコードが追加されました。");

                    break;
                case HOTKEY_ID21:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[16]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[16] + " レコードが追加されました。");


                    break;
                case HOTKEY_ID22:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[17]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[17] + " レコードが追加されました。");


                    break;
                case HOTKEY_ID23:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[18]);

                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[18] + " レコードが追加されました。");

                    break;
                case HOTKEY_ID24:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[19]);

                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[19] + " レコードが追加されました。");

                    break;
                case HOTKEY_ID25:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[20]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[20] + " レコードが追加されました。");

                    break;

                case HOTKEY_ID26:
                    motivationInsert.InsertInitialRecords(Models.DBs.MotivationsTable.Insert.motivationList[21]);
                    MessageBox.Show(Models.DBs.MotivationsTable.Insert.motivationList[21] + " レコードが追加されました。");



                    break;












                default:
                    break;
            }
        }

        // Windowを閉じる際にHotKeyの解除を行う。
        private void Window_Closed(object sender, EventArgs e)
        {
            UnregisterHotKey(WindowHandle, HOTKEY_ID1);
            UnregisterHotKey(WindowHandle, HOTKEY_ID2);
  
            ComponentDispatcher.ThreadPreprocessMessage -= ComponentDispatcher_ThreadPreprocessMessage;
        }











    }
}
