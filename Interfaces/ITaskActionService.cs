using System;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.Interfaces
{
    public interface ITaskActionService
    {
        TaskActionModel GetTaskAction(int taskId);
        void ExecuteTaskAction(TaskActionModel task);
        void UpdateDamageCount(TaskActionModel task);
        void GenerateMMFile(TaskActionModel task);
        void FocusApplication(string applicationName);
        event EventHandler<TaskActionCompletedEventArgs> TaskActionCompleted;
    }

    public class TaskActionCompletedEventArgs : EventArgs
    {
        public TaskActionModel Task { get; set; }
        public bool IsCompleted { get; set; }
        
        public TaskActionCompletedEventArgs(TaskActionModel task, bool isCompleted)
        {
            Task = task;
            IsCompleted = isCompleted;
        }
    }
}