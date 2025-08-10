using System;
using System.Windows;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.Services
{
    public class ResultService : IResultService
    {
        public event EventHandler<ResultProcessedEventArgs> ResultProcessed;

        public ResultModel CreateResult(TaskActionModel task, bool isCompleted)
        {
            if (task == null)
                return null;

            var endTime = DateTime.Now;
            var duration = endTime - task.StartTime;

            return new ResultModel
            {
                TaskId = task.ID,
                TaskName = task.KMN,
                ResultText = isCompleted ? "🎉 MISSION COMPLETE! 🎉" : "⚠️ MISSION INCOMPLETE ⚠️",
                StartTime = task.StartTime,
                EndTime = endTime,
                Duration = duration,
                EstimatedTime = task.EstimatedTime,
                Priority = 2, // デフォルト値
                IsCompleted = isCompleted
            };
        }

        public void ProcessResultAction(ResultModel result)
        {
            if (result == null) return;

            try
            {
                switch (result.SelectedAction)
                {
                    case ResultAction.Clear:
                        ClearTask(result.TaskId);
                        break;
                    case ResultAction.Again:
                        RepeatTask(result.TaskId);
                        break;
                    case ResultAction.AgainLater:
                        RepeatTaskLater(result.TaskId);
                        break;
                    case ResultAction.UpdateColumn:
                        UpdateTaskColumns(result.TaskId, result.EstimatedTime, result.Priority);
                        break;
                }

                ResultProcessed?.Invoke(this, new ResultProcessedEventArgs(result, true));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"結果処理エラー: {ex.Message}");
                ResultProcessed?.Invoke(this, new ResultProcessedEventArgs(result, false));
            }
        }

        public void ClearTask(int taskId)
        {
            try
            {
                // TODO: データベースのタスクをクリア（完了）状態に更新
                // 現在は仮実装
                MessageBox.Show($"🎉 タスク {taskId} をクリアしました！");
                
                // リピートパターンに応じた処理
                // TODO: RepeatPatterns の処理を実装
                
                // MainWindow.RTPDD_ClearFlag の処理
                // TODO: 必要に応じてフラグ処理を実装
            }
            catch (Exception ex)
            {
                MessageBox.Show($"タスククリアエラー: {ex.Message}");
            }
        }

        public void RepeatTask(int taskId)
        {
            try
            {
                // TODO: タスクを即座に再実行キューに追加
                MessageBox.Show($"🔄 タスク {taskId} をもう一度実行します");
                
                // TaskActionWindowを再表示する処理
                // TODO: TaskActionWindowの再表示処理を実装
            }
            catch (Exception ex)
            {
                MessageBox.Show($"タスク再実行エラー: {ex.Message}");
            }
        }

        public void RepeatTaskLater(int taskId)
        {
            try
            {
                // TODO: タスクを後で実行するキューに追加
                MessageBox.Show($"⏰ タスク {taskId} を後で実行するように設定しました");
                
                // DueDateを更新する処理
                // TODO: DueDateの更新処理を実装
            }
            catch (Exception ex)
            {
                MessageBox.Show($"タスク後回しエラー: {ex.Message}");
            }
        }

        public void UpdateTaskColumns(int taskId, int estimatedTime, int priority)
        {
            try
            {
                // TODO: データベースのタスクカラムを更新
                MessageBox.Show($"📝 タスク {taskId} のカラム値を更新しました\n" +
                              $"推定時間: {estimatedTime}分\n" +
                              $"優先度: {priority}");
                
                // データベース更新処理
                // TODO: TasksTableの更新処理を実装
            }
            catch (Exception ex)
            {
                MessageBox.Show($"タスク更新エラー: {ex.Message}");
            }
        }
    }
}