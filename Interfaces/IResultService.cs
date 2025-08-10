using System;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.Interfaces
{
    public interface IResultService
    {
        ResultModel CreateResult(TaskActionModel task, bool isCompleted);
        void ProcessResultAction(ResultModel result);
        void ClearTask(int taskId);
        void RepeatTask(int taskId);
        void RepeatTaskLater(int taskId);
        void UpdateTaskColumns(int taskId, int estimatedTime, int priority);
        event EventHandler<ResultProcessedEventArgs> ResultProcessed;
    }

    public class ResultProcessedEventArgs : EventArgs
    {
        public ResultModel Result { get; set; }
        public bool Success { get; set; }
        
        public ResultProcessedEventArgs(ResultModel result, bool success)
        {
            Result = result;
            Success = success;
        }
    }
}