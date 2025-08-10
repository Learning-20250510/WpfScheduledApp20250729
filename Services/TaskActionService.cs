using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.Services
{
    public class TaskActionService : ITaskActionService
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public event EventHandler<TaskActionCompletedEventArgs> TaskActionCompleted;

        public TaskActionModel GetTaskAction(int taskId)
        {
            try
            {
                // 既存のTasksTableからデータを取得
                // TODO: 実際のデータベース接続処理を実装
                // 今は仮のデータを返す
                return new TaskActionModel
                {
                    ID = taskId,
                    KMN = $"Task_{taskId}",
                    KMT = "Sample KMT",
                    HTL = 1,
                    EstimatedTime = 30,
                    Description = "Sample Description",
                    ScrollValue = 0,
                    RelationalFile1 = "",
                    RelationalFile2 = "",
                    StartTime = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"タスク取得エラー: {ex.Message}");
                return null;
            }
        }

        public void ExecuteTaskAction(TaskActionModel task)
        {
            if (task == null) return;

            try
            {
                // HTL別の処理を実行
                switch (task.HTL)
                {
                    case 1:
                        MessageBox.Show($"{task.HTL} = 1です。Unclassifiedは実行できません。終了してください");
                        break;
                    case 2:
                        ExecuteMovieTask(task);
                        break;
                    case 3:
                        ExecuteStillImageTask(task);
                        break;
                    case 4:
                        ExecutePDFContextTask(task);
                        break;
                    case 5:
                        ExecutePDFDesignTask(task);
                        break;
                    case 6:
                        ExecuteWorldTask(task);
                        break;
                    case 7:
                        ExecuteWebPageContextTask(task);
                        break;
                    case 8:
                        ExecuteWebPageDesignTask(task);
                        break;
                    case 9:
                        ExecuteWebPageMMTask(task);
                        break;
                    case 10:
                        ExecuteWebPageScrapingTask(task);
                        break;
                    case 11:
                        ExecutePDFOCRTask(task);
                        break;
                    case 12:
                        ExecuteBrainTask(task);
                        break;
                    case 13:
                        ExecuteResearchTask(task);
                        break;
                    case 14:
                        ExecuteSoundTask(task);
                        break;
                    case 15:
                        ExecuteFreePlaneSpeedTask(task);
                        break;
                    case 16:
                        ExecuteFreePlaneContextTask(task);
                        break;
                    case 17:
                        ExecuteFreePlaneTasksTask(task);
                        break;
                    case 18:
                        ExecuteVariablesTask(task);
                        break;
                    default:
                        MessageBox.Show($"未対応のHTL: {task.HTL}");
                        break;
                }

                // Freeplaneにフォーカス
                FocusApplication("Freeplane");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"タスク実行エラー: {ex.Message}");
            }
        }

        public void UpdateDamageCount(TaskActionModel task)
        {
            if (task == null) return;

            // ダメージカウント計算ロジック
            int totalDamage = 0;
            
            totalDamage += task.VoiceMemo / 5;
            totalDamage += task.ManualMemoBySP * 5;
            totalDamage += task.ManualMemoNumberOfPagesByAnalog * 20;
            totalDamage += task.ConcentrateTime * 50;
            
            task.DamageCount = totalDamage;
        }

        public void GenerateMMFile(TaskActionModel task)
        {
            if (task == null) return;

            try
            {
                if (task.HTL == 9 || task.HTL == 11)
                {
                    // MM ファイル生成処理
                    // TODO: FreePlaneファイル操作の実装
                    MessageBox.Show($"MMファイルを生成しました: {task.KMTName}_{task.HTLName}_{task.KMN}");
                }
                else
                {
                    MessageBox.Show("MMFileGeneratorを使うHTLではないので、実行できません。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MMファイル生成エラー: {ex.Message}");
            }
        }

        public void FocusApplication(string applicationName)
        {
            try
            {
                foreach (Process process in Process.GetProcesses())
                {
                    if (process.MainWindowTitle.IndexOf(applicationName, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"アプリケーションフォーカスエラー: {ex.Message}");
            }
        }

        #region Private HTL Execution Methods

        private void ExecuteMovieTask(TaskActionModel task)
        {
            var movieTask = new Models.HowToLearn.Movie.FocusContextBetWeen10SecondsOfMovie(
                task.KMN, task.MovieAndSoundStartTimeOfTen);
            movieTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecuteStillImageTask(TaskActionModel task)
        {
            var stillImageTask = new Models.HowToLearn.Movie.FocusContextInStillImageOfMovie(
                task.KMN, task.MovieStartTimeOfTwo);
            stillImageTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecutePDFContextTask(TaskActionModel task)
        {
            var pdfTask = new Models.HowToLearn.PDF.FocusContextInSpecificPageOfPDF(
                task.KMN, task.SpecificPageOfPDF);
            pdfTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecutePDFDesignTask(TaskActionModel task)
        {
            var pdfDesignTask = new Models.HowToLearn.PDF.FocusDesignInSpecificPageOfPDF(
                task.KMN, task.SpecificPageOfPDF);
            pdfDesignTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecuteWorldTask(TaskActionModel task)
        {
            var worldTask = new Models.HowToLearn.TheWorld.AnywayOfTheWorld();
            worldTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID, 
                                task.RelationalFile1, task.RelationalFile2);
        }

        private void ExecuteWebPageContextTask(TaskActionModel task)
        {
            var webPageTask = new Models.HowToLearn.WebPage.FocusContextInScrollValueOfWebPage(
                task.KMN, task.ValueOfScrollOfWebPage);
            webPageTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecuteWebPageDesignTask(TaskActionModel task)
        {
            var webPageDesignTask = new Models.HowToLearn.WebPage.FocusDesignInScrollValueOfWebPage(
                task.KMN, task.ValueOfScrollOfWebPage);
            webPageDesignTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecuteWebPageMMTask(TaskActionModel task)
        {
            var webPageMMTask = new Models.HowToLearn.WebPage.CreateMMFileDirectlyFromBrowserByAutoMMFileGenerationOfWebPage(
                task.KMN);
            webPageMMTask.TaskAction();
        }

        private void ExecuteWebPageScrapingTask(TaskActionModel task)
        {
            var webPageScrapingTask = new Models.HowToLearn.WebPage.CreateMMFileAutomaticallyScrapingByAutoMMFileGenerationOfWebPage(
                task.KMN);
            webPageScrapingTask.TaskAction();
        }

        private void ExecutePDFOCRTask(TaskActionModel task)
        {
            var pdfOCRTask = new Models.HowToLearn.PDF.CreateMMFileFromContextWithOCRByAutoMMFileGenerationOfPDF(
                task.KMN, task.SpecificPageOfPDF);
            pdfOCRTask.TaskAction();
        }

        private void ExecuteBrainTask(TaskActionModel task)
        {
            MessageBox.Show($"🎮 Brain Task実行: {task.KMN}\n自由な発想で思考してください！", "BRAIN TASK");
        }

        private void ExecuteResearchTask(TaskActionModel task)
        {
            MessageBox.Show($"🎮 Research Task実行: {task.KMN}\n調査を開始してください！", "RESEARCH TASK");
        }

        private void ExecuteSoundTask(TaskActionModel task)
        {
            MessageBox.Show($"🎮 Sound Task実行: {task.KMN}\n音声に集中してください！", "SOUND TASK");
        }

        private void ExecuteFreePlaneSpeedTask(TaskActionModel task)
        {
            var freePlaneSpeedTask = new Models.HowToLearn.FreePlane.FocusTheMMFileWithHavingAnFreedomIdeaSpeedlyOfFreePlane(
                task.KMN);
            freePlaneSpeedTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecuteFreePlaneContextTask(TaskActionModel task)
        {
            var freePlaneContextTask = new Models.HowToLearn.FreePlane.FocusContextOfTheMMFileWithHavingAnFreedomIdeaOfFreePlane(
                task.KMN);
            freePlaneContextTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecuteFreePlaneTasksTask(TaskActionModel task)
        {
            var freePlaneTasksTask = new Models.HowToLearn.FreePlane.FindTasksNTimesFromTheMMFileOfFreePlane(
                task.KMN);
            freePlaneTasksTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        private void ExecuteVariablesTask(TaskActionModel task)
        {
            var variablesTask = new Models.HowToLearn.TheWorld.ReadyForVariablesListLearningOfTheWorld();
            variablesTask.TaskAction(task.KMTName, task.KMN, task.HTLName, task.ID);
        }

        #endregion
    }
}