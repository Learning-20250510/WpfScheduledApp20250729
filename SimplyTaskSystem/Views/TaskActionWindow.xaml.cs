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
using SimplyTaskSystem.ViewModels;
namespace SimplyTaskSystem.Views
{
    /// <summary>
    /// TaskActionWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskActionWindow : Window
    {

        private ObservableCollection<Models.DBs.TasksTable.DataClass> TasksCollection;

        private DateTime GoalTime;
        private double branchAmount;
        private double GoalBranchAmount;
        public static double damageCount;


        private bool clearflag = false;

        public static bool StartedUp = false;


        //base information of actionID(実行中タスク）
        private readonly int ID;
        private string KMN;
        private int KMT;
        private int HTL;
        private int ET;
        private int Priority;

        private string KMTName;
        private string HTLName;

        private string Description;

        public TaskActionWindow()
        {
            InitializeComponent();
        }
        private DispatcherTimer timer1;
        private DispatcherTimer timer2;
        private DispatcherTimer timer3;
        public TaskActionWindow(int actionID)
        {
            StartedUp = true;

            this.Left = 0;
            this.Top = 0;

            InitializeComponent();
            //this.WindowState = WindowState.Maximized;//最大化
            Models.DBs.TasksTable.Read tasksTableRead = new Models.DBs.TasksTable.Read();
            TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(tasksTableRead.SelectTasksTemplate("select * from simpletasksystem.tasks where id=" + actionID));

            if (TasksCollection.Count != 1)
            {
                MessageBox.Show("id=" + actionID + " でクエリ発行したのに、なぜかレコード数が１ではなく、" + TasksCollection.Count + " になっています。");
            }
            foreach (var task in TasksCollection)
            {
                this.ID = task.ID;
                this.KMN = task.KMN;
                this.KMT = task.KMT;
                this.HTL = task.HTL;
                this.ET = task.EstimatedTime;
                this.Description = task.Description;
                this.Priority = task.Priority;
            }

            //KMT_KMN_HTL_
            var htl = new Models.DBs.HTLTable.Read();
            var htlCollection = htl.SelectHTLCollection($"select * from simpletasksystem.htl where id={this.HTL}");
            foreach (var li in htlCollection)
            {
                this.HTLName = li.HTLName;
            }

            var kmt = new Models.DBs.KMTTable.Read();
            var kmtCollection = kmt.SelectKMTCollection($"select * from simpletasksystem.kmt where id={this.KMT}");
            foreach (var li in kmtCollection)
            {
                this.KMTName = li.KMTName;
            }
            this.SetGoalTimer();

            timer1 = CreateTimer(1000, TimerMethod);
            timer1.Start();
            timer2 = CreateTimer(100, TimerMethod2);
            timer2.Start();

            timer3 = CreateTimer(3000, TimerMethod3);
            timer3.Start();

            // WindowHandleを取得（for what?)
            var host = new WindowInteropHelper(this);
            WindowHandle = host.Handle;


            // HotKeyを設定
            SetUpHotKey();
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;

        }






        private void SetGoalTimer()//１回限りの実行
        {
   
            //ゲーム開始ときの時刻を保持しておき、その差分で残り時間の表現
  
            GoalTime = DateTime.Now.AddMinutes(this.ET);
   

            if (this.HTL == 2 || this.HTL == 3 || this.HTL == 4 || this.HTL == 5 || this.HTL == 7 || this.HTL == 8 || this.HTL == 12 || this.HTL == 14)//W.O. OR R.A.,Input,ROT
            {
                GoalBranchAmount = this.ET * 40;
            }
            else if (this.HTL == 15 || this.HTL == 16)//R, S.L
            {
                GoalBranchAmount = this.ET * 30;
            }
            else if (this.HTL == 17)//ToTask
            {
                GoalBranchAmount = this.ET * 40;
            }
            else if (this.HTL == 6 || this.HTL == 18)//TheWolrd
            {
                GoalBranchAmount = this.ET * 40;
            }
            else if (this.HTL == 9 || this.HTL == 11)//形態素解析でのゴール
            {
                GoalBranchAmount = this.ET * 1000;//branchでのゴールを目標としないので適当な値を入れてる。
            }
            else if (this.HTL == 10 || this.HTL == 13)//自動実行タスク関連
            {
                GoalBranchAmount = this.ET * 1000;//手動で実行されることはないので適当な値（一応思考した証として明示的記述）

            }
            else
            {

            }

            Debug.WriteLine("GoalBranchAmout: " + GoalBranchAmount);

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

        private void TimerMethod()
        {

            uxClock.Text = DateTime.Now.ToString("hh:mm:ss");
            TimeSpan remain_time = GoalTime - DateTime.Now;
            System.Diagnostics.Debug.WriteLine("GoalTime: " + GoalTime + " DateTime.Now: " + DateTime.Now);
            System.Diagnostics.Debug.WriteLine(remain_time);
            uxClock.Text = remain_time.ToString();

            /*
            if (remain_time.Seconds < 5  && remain_time.Seconds > 0)
            {
                //トースター通知
                new ToastContentBuilder()
                    .AddText("残り時間5秒を切りました。")
                    .Show();
            }
            */

            if (remain_time.Seconds < 0)//E.T.時間切れ（強制フォーカスしていったん終了し、againORClearORPostPone
            {
                this.Activate();
                //Utility1.PlayTaskFailedSound();


            }

            //if (remain_time.Seconds < 10 && remain_time.Seconds > 0)
            TimeSpan time1 = new TimeSpan(0, 0, 0);
            TimeSpan time2 = new TimeSpan(0, 0, 10);

            /*
             * 別のTimerMethodを作成してそっちでやる
            if (remain_time < time2 && remain_time > time1)

            {
                System.Diagnostics.Debug.WriteLine("remain_time: " + remain_time + " remain_time.Seconds: " + remain_time.Seconds);
                //トースター通知
                new ToastContentBuilder()
                    .AddText("残り時間10秒を切りました。")
                    .Show();
            }
            */


            if ( (branchAmount > GoalBranchAmount || clearflag == true ) && AddTaskViewModel.StartedUp == false)
            {

                if (remain_time.Seconds >= 0 && AddTaskViewModel.StartedUp == false && UpdateTaskRecordViewModel.StartedUp == false)
                {
                    Debug.WriteLine("時間うちにくりあできたよ！");
                    //Utility1.PlaySuccessSound();

                    this.Close();



                    var rw = new ResultWindow(true);
                    ResultViewModel rvm = new ResultViewModel(this.ID, true);
                    rw.DataContext = rvm;
                    rw.Show();
                    rw.Activate();

                    TaskActionWindow.StartedUp = false;


                    /*
                    //トースター通知
                    new ToastContentBuilder()
                        .AddText("⏰ TimeRecorder ⏰")
                        .AddText("タスクをクリアしました！")
                        .Show();

                    */
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("時間外クリアだよ！おしかった！");
                    MessageBox.Show("実行するために知識を入手するのではなく、知識を入手するために実行する");
                    this.Close();


                    var rw = new ResultWindow(false);
                    var rvm = new ResultViewModel(this.ID, false);
                    rw.DataContext = rvm;
                    rw.Show();
                    rw.Activate();
                    TaskActionWindow.StartedUp = false;//これおそらく、taskAction画面が起動しているかどうかで、this.Close()したから、falseにしたんだと思う。repeat_time_per_dayの起動などに評価に使うためとか

                }
            }




            for (int i = 1; i <= 256; i++)
            {
                int rtn = GetAsyncKeyState(i);
                if ((rtn & 1) != 0)
                {
                    Debug.WriteLine(
                        "キーボード押下:" + i);
               
                    if (i == 13 || i == 45 || i == 9)//新規ブランチ作成
                    {
                        branchAmount++;
                        Debug.WriteLine("新規ブランチが作成されました。");
                        //Utility1.PlayBranchNormalSound();
                    }
               
                    if (i == 164 || i == 18)//新規task作成
                    {
                        branchAmount += 10;
                        //Utility1.PlayTaskAddingSound();
                        Debug.WriteLine("新規タスクを追加します");
                    }
      
                }
            }

        }
        public static bool motivationDamage = false;

        private void TimerMethod2()//ProgeressBar処理
        {

            if (motivationDamage == true)
            {
                branchAmount += 10;
                motivationDamage = false;//初期化
            }

            if (damageCount != 0)
            {
                branchAmount += damageCount;
                damageCount = 0;//初期化
            }



            double dummyRatio = branchAmount / GoalBranchAmount;
            double dummyInnerWidth = outerHealthBar.Width - (outerHealthBar.Width * (dummyRatio));
            if (dummyInnerWidth < 0)
            {
                innerHealthBar.Width = 0;//数秒沖に実行しているため、minusになることがありコンパイルエラーがでたので。
            }
            else
            {
                innerHealthBar.Width = dummyInnerWidth;
            }
            //System.Diagnostics.Debug.WriteLine("branchAmout: " + branchAmount + " innerHealthBar.Width" + innerHealthBar.Width);

            if (dummyRatio > 0.8)
            {
                innerHealthBar.Fill = Brushes.Violet;
            }
            else if (dummyRatio > 0.6)
            {
                innerHealthBar.Fill = Brushes.Yellow;
            }
            else
            {
                innerHealthBar.Fill = Brushes.Green;
            }
        }

        private void TimerMethod3()
        {
            TimeSpan remain_time = GoalTime - DateTime.Now;
            TimeSpan time1 = new TimeSpan(0, 0, 0);
            TimeSpan time2;
           

            //ETが大きいタスクは、ダメージ数値を入力する可能性が高いので、１５秒ではなくもう少し余裕を持ったかつ余裕すぎない値として25秒を選定
            if (this.ET > 5)
            {
                time2 = new TimeSpan(0, 0, 25);
                if (remain_time < time2 && remain_time > time1)

                {
                    System.Diagnostics.Debug.WriteLine("remain_time: " + remain_time + " remain_time.Seconds: " + remain_time.Seconds);
                    //トースター通知
                    new ToastContentBuilder()
                        .AddText("残り時間25秒を切りました。")
                        .Show();
                }

            }
            else
            {
                time2 = new TimeSpan(0, 0, 18);
                if (remain_time < time2 && remain_time > time1)

                {
                    System.Diagnostics.Debug.WriteLine("remain_time: " + remain_time + " remain_time.Seconds: " + remain_time.Seconds);
                    //トースター通知
                    new ToastContentBuilder()
                        .AddText("残り時間18秒を切りました。")
                        .Show();
                }
            }


        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            //e.Cancel = true;
        }

        /*
      * HotKey登録時に指定するID。
      * アプリケーションの場合は、0x0000～0xbfffの間で指定すること。
      * （共有DLLの場合は、0xc000～0xffffの間を使用する。）
      */
        private const int HOTKEY_ID1 = 0x0100;
        private const int HOTKEY_ID2 = 0x0101;
        private const int HOTKEY_ID3 = 0x0102;
        private const int HOTKEY_ID4 = 0x0103;


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
            var result1 = RegisterHotKey(WindowHandle, HOTKEY_ID1, (int)ModifierKeys.Alt, KeyInterop.VirtualKeyFromKey(Key.N));
            if (result1 == 0)
            {
                //MessageBox.Show("HotKey1の登録に失敗しました。");
            }

            // Alt + テンキーの1 をHotKeyとして登録。
            var result2 = RegisterHotKey(WindowHandle, HOTKEY_ID2, (int)ModifierKeys.Alt, KeyInterop.VirtualKeyFromKey(Key.Y));
            if (result2 == 0)
            {
                //MessageBox.Show("HotKey2の登録に失敗しました。");
            }

            //タスク中断
            var result3 = RegisterHotKey(WindowHandle, HOTKEY_ID3, (int)ModifierKeys.Alt, KeyInterop.VirtualKeyFromKey(Key.W));
            if (result3 == 0)
            {
                //MessageBox.Show("HotKey3の登録に失敗しました。");
            }


            //タスククリア（try,researchのような数値化でクリア判定できない手動的クリア）
            var result4 = RegisterHotKey(WindowHandle, HOTKEY_ID4, (int)ModifierKeys.Control, KeyInterop.VirtualKeyFromKey(Key.Down));
            if (result4 == 0)
            {
                //MessageBox.Show("HotKey4の登録に失敗しました。");
            }



        }

        // ここでHotKeyが押された際の挙動を設定する。
        void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            // HotKeyが押されたかどうかを判定。
            if (msg.message != WM_HOTKEY) return;

            switch (msg.wParam.ToInt32())
            {
                case HOTKEY_ID1:
                    //MessageBox.Show("新たしいFreePlaneFileを作成して開きます");

                    //theWorldにしているのは特に意味はない。ただＭＭＦＩＬＥ生成methodが、どれでもいいというかインターフェースとして実装するのが面倒だから適当に同じ処理内容であろうものにしている。
                    var theWorld = new Models.HowToLearn.TheWorld.ReadyForVariablesListLearningOfTheWorld();
                    theWorld.TaskAction(this.KMTName, this.KMN, this.HTLName, this.ID);

                    break;
                case HOTKEY_ID2:
                    this.Activate();
                    break;
                case HOTKEY_ID3:
                    this.Close();
                    //MainWindow.rowIndex++;
                    break;
                case HOTKEY_ID4:

                    clearflag = true;
                    Debug.WriteLine("タスククリア！！！！");
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
            UnregisterHotKey(WindowHandle, HOTKEY_ID3);
            UnregisterHotKey(WindowHandle, HOTKEY_ID4);

            ComponentDispatcher.ThreadPreprocessMessage -= ComponentDispatcher_ThreadPreprocessMessage;
        }






        // キーボードの押下を監視するサンプル(C#.NET/VS2005)
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(int vKey);

    }
}
