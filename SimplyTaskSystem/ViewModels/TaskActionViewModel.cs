using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SimplyTaskSystem.Models;
using SimplyTaskSystem.Views;

namespace SimplyTaskSystem.ViewModels
{
    internal class TaskActionViewModel : NotificationObject
    {
        public TaskActionViewModel()
        {

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
        public TaskActionViewModel(int actionID)
        {
            Models.DBs.TasksTable.Read tasksTableRead = new Models.DBs.TasksTable.Read();
            TasksCollection = new ObservableCollection<Models.DBs.TasksTable.DataClass>(tasksTableRead.SelectTasksTemplate("select * from simpletasksystem.tasks where id=" + actionID));

            if (TasksCollection.Count != 1)
            {
                MessageBox.Show("id=" + actionID + " でクエリ発行したのに、なぜかレコード数が１ではなく、" + TasksCollection.Count + " になっています。");
            }
            foreach (var task in TasksCollection)
            {
                this.ID = task.ID.ToString();
                this.KMN = task.KMN;
                this.KMT = task.KMT.ToString();
                this.HTL = task.HTL;
                this.ET = task.EstimatedTime;
                this.Description = task.Description;
                this.SCrollValue = task.specificScrollValueAsWebPage;
                this.RF1 = task.RelationalFile1;
                this.RF2 = task.RelationalFile2;
            }

            this.chooseHTL(this.HTL);

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

        private int _hTL;
        public int HTL
        {
            get
            {
                return this._hTL;
            }
            set
            {
                if (SetProperty(ref this._hTL, value))
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

        private int _eT;
        public int ET
        {
            get
            {
                return this._eT;
            }
            set
            {
                if (SetProperty(ref this._eT, value))
                {

                }
            }

        }

        private int _sCrollValue;
        public int SCrollValue
        {
            get
            {
                return this._sCrollValue;
            }
            set
            {
                if (SetProperty(ref this._sCrollValue, value))
                {

                }
            }

        }

        private int _voiceMemo;
        public int VoiceMemo
        {
            get
            {
                return this._voiceMemo;
            }
            set
            {
                if (SetProperty(ref this._voiceMemo, value))
                {
                    TaskActionWindow.damageCount = this._voiceMemo / 5;

                }
            }
        }

        private int _manualMemoBySP;
        public int ManualMemoBySP
        {
            get
            {
                return this._manualMemoBySP;
            }

            set
            {
                if (SetProperty(ref this._manualMemoBySP, value))
                {
                    TaskActionWindow.damageCount = this._manualMemoBySP * 5;

                }
            }
        }

        private int _manualMemoNumberOfPagesByAnalog;
        public int ManualMemoNumberOfPagesByAnalog
        {
            get
            {
                return this._manualMemoNumberOfPagesByAnalog;
            }
            set
            {
                if (SetProperty(ref this._manualMemoNumberOfPagesByAnalog, value))
                {
                    TaskActionWindow.damageCount = this._manualMemoNumberOfPagesByAnalog * 20;

                }
            }
        }

        private int _concentrateTime;
        public int ConcentrateTime
        {
            get
            {
                return this._concentrateTime;
            }
            set
            {
                if (SetProperty(ref this._concentrateTime, value))
                {
                    TaskActionWindow.damageCount = this._concentrateTime * 50;

                }
            }
        }

        private string _autoCreateMMFileGenerator
;
        public string AutoCreateMMFileGenerator

        {
            get
            {
                return this._autoCreateMMFileGenerator;
            }
            set
            {
                if (SetProperty(ref this._autoCreateMMFileGenerator, value))
                {

                }
            }
        }


        private DelegateCommand _generatorBtnCommand;
        public DelegateCommand GeneratorBtnCommand
        {
            get
            {
                return this._generatorBtnCommand ?? (this._generatorBtnCommand = new DelegateCommand(
                _ =>
                {
                    if (this.HTL == 9 || this.HTL == 11 )
                    {
                        var freePlane = new Models.FilesOperation.FreePlaneFileOperation();
                        var file = freePlane.createXmlFileWithContextAnylysis(int.Parse(this.ID), this.AutoCreateMMFileGenerator, this.KMTName+"_"+this.HTLName+"_"+this.KMN);

                        //一部ファイルだけ開く感じになってる？
                        var open = new Models.FilesOperation.FreePlaneFileOperation();
                        open.OpenSpecificFreePlaneFile(file);

                    }
                    else
                    {
                        MessageBox.Show("MMFileGeneratorを使うHTLではないので、実行できません。");
                    }
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }

        private string KMTName;
        private string HTLName;

        private string RF1;
        private string RF2;

        private void chooseHTL(int HTL)
        {
            int movieAndSoundStartTimeOfTen = 0;
            int movieStartTimeOfTwo = 0;
            int specificPageOfPDF = 0;
            double valueOfScrollOfWebPage = 0;
            foreach (var task in TasksCollection)
            {
                movieAndSoundStartTimeOfTen = task.TenSecondsIncrementAsSoundsAndMovie;
                movieStartTimeOfTwo = task.TwoSecondsIncrementAsMovie;
                specificPageOfPDF = task.SpecificPageAsPDF;
                valueOfScrollOfWebPage = task.specificScrollValueAsWebPage;
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
    
            if (HTL == 1)//Unclassified
            {
                MessageBox.Show(HTL + " =1です。Unclassifiedは実行できません。終了して下さい");
            }
            else if (HTL == 2)
            {
                Models.HowToLearn.Movie.FocusContextBetWeen10SecondsOfMovie movie = new Models.HowToLearn.Movie.FocusContextBetWeen10SecondsOfMovie(this.KMN, movieAndSoundStartTimeOfTen);
                movie.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else if (HTL == 3)
            {
                var movie = new Models.HowToLearn.Movie.FocusContextInStillImageOfMovie(this.KMN, movieStartTimeOfTwo);
                movie.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else if (HTL == 4)
            {
                var pdf = new Models.HowToLearn.PDF.FocusContextInSpecificPageOfPDF(this.KMN, specificPageOfPDF);
                pdf.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else if (HTL == 5)
            {
                var pdf = new Models.HowToLearn.PDF.FocusDesignInSpecificPageOfPDF(this.KMN, specificPageOfPDF);
                pdf.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else if (HTL == 6)
            {
                var theWorld = new Models.HowToLearn.TheWorld.AnywayOfTheWorld();
                theWorld.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID), this.RF1, this.RF2);
            }
            else if (HTL == 7)
            {
                var webPage = new Models.HowToLearn.WebPage.FocusContextInScrollValueOfWebPage(this.KMN, valueOfScrollOfWebPage);
                webPage.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else if (HTL == 8)
            {
                var webPage = new Models.HowToLearn.WebPage.FocusDesignInScrollValueOfWebPage(this.KMN, valueOfScrollOfWebPage);
                webPage.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else if (HTL == 9)
            {
                var webPage = new Models.HowToLearn.WebPage.CreateMMFileDirectlyFromBrowserByAutoMMFileGenerationOfWebPage(this.KMN);
                webPage.TaskAction();
            }
            else if (HTL == 10)
            {
                var webPage = new Models.HowToLearn.WebPage.CreateMMFileAutomaticallyScrapingByAutoMMFileGenerationOfWebPage(this.KMN);
                webPage.TaskAction();
            }
            else if (HTL == 11)
            {
                var pdf = new Models.HowToLearn.PDF.CreateMMFileFromContextWithOCRByAutoMMFileGenerationOfPDF(this.KMN, specificPageOfPDF);
                pdf.TaskAction();
            }
            else if (HTL == 12)
            {
                var myBrain = new Models.HowToLearn.MyBrain.FocusTheKMNWithHavingAnIdeaMyBrain();
                myBrain.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else if (HTL == 13)
            {
                var research = new Models.HowToLearn.Research.FindKMFromSomeURLAutomaticallyOfResearch();
                research.TaskAction();
            }
            else if (HTL == 14)
            {
                var sound = new Models.HowToLearn.Sound.FocusContextBetWeen10SecondsOfSound(this.KMN, movieAndSoundStartTimeOfTen);
                sound.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else if (HTL == 15)
            {
                var freePlane = new Models.HowToLearn.FreePlane.FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane(this.KMN);
                freePlane.TaskAction();
            }
            else if (HTL == 16)
            {
                var freePlane = new Models.HowToLearn.FreePlane.FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane(this.KMN);
                freePlane.TaskAction();
            }
            else if (HTL == 17)
            {
                var freePlane = new Models.HowToLearn.FreePlane.FindTasksNTimesFromTheMMFileOfFreePlane(this.KMN);
                freePlane.TaskAction();
            }
            else if (HTL == 18)
            {
                var theWorld = new Models.HowToLearn.TheWorld.ReadyForVariablesListLearningOfTheWorld();
                theWorld.TaskAction(this.KMTName, this.KMN, this.HTLName, int.Parse(this.ID));
            }
            else
            {
                //随時HTLが追加されるたびに更新
            }

            //すべてのプロセスを列挙する
            foreach (System.Diagnostics.Process p
                in System.Diagnostics.Process.GetProcesses())
            {
                //"メモ帳"がメインウィンドウのタイトルに含まれているか調べる
                if (0 <= p.MainWindowTitle.IndexOf("Freeplane"))
                {
                    //ウィンドウをアクティブにする
                    SetForegroundWindow(p.MainWindowHandle);
                    break;

                }
            }

        }



        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);



    }
}
