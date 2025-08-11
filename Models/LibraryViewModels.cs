using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Models
{
    /// <summary>
    /// HTL別セクション表示用のViewModel
    /// </summary>
    public class HTLSectionViewModel : NotificationObject
    {
        private int _completedTasks;
        private int _totalTasks;
        private double _progressWidth;

        public string HTLName { get; set; } = string.Empty;
        public string HTLIcon { get; set; } = "📚";
        public int ClearCount { get; set; }
        
        public ObservableCollection<LibraryTaskViewModel> Tasks { get; set; } = new();

        public int CompletedTasks
        {
            get => _completedTasks;
            set => SetProperty(ref _completedTasks, value);
        }

        public int TotalTasks
        {
            get => _totalTasks;
            set 
            { 
                SetProperty(ref _totalTasks, value);
                UpdateProgressWidth();
            }
        }

        public int ProgressPercentage
        {
            get
            {
                if (TotalTasks == 0) return 0;
                return (int)Math.Round(((double)CompletedTasks / TotalTasks) * 100);
            }
        }

        public double ProgressWidth
        {
            get => _progressWidth;
            private set => SetProperty(ref _progressWidth, value);
        }

        private void UpdateProgressWidth()
        {
            if (TotalTasks == 0)
            {
                ProgressWidth = 0;
            }
            else
            {
                ProgressWidth = ((double)CompletedTasks / TotalTasks) * 400; // 400px max width
            }
            OnPropertyChanged(nameof(ProgressPercentage));
        }

        public void UpdateProgress()
        {
            CompletedTasks = Tasks.Count(t => t.IsCompleted);
            TotalTasks = Tasks.Count;
            UpdateProgressWidth();
        }
    }

    /// <summary>
    /// 図書館風タスク表示用のViewModel
    /// </summary>
    public class LibraryTaskViewModel : NotificationObject
    {
        private bool _isCompleted;
        
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int EstimatedTime { get; set; }
        public DateTime ExecutionDate { get; set; }
        public TimeSpan ExecutionTime { get; set; }
        public DateTime LastClearedAt { get; set; }
        public string MiddleTaskName { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string HTLName { get; set; } = string.Empty;

        public bool IsCompleted
        {
            get => _isCompleted;
            set 
            { 
                SetProperty(ref _isCompleted, value);
                OnPropertyChanged(nameof(StatusIcon));
            }
        }

        public string StatusIcon
        {
            get
            {
                if (IsCompleted) return "✅";
                if (DateTime.Now.Date > ExecutionDate.Date) return "⚠️";
                if (DateTime.Now.TimeOfDay > ExecutionTime) return "🔄";
                return "📋";
            }
        }

        public string ExecutionTimeDisplay
        {
            get
            {
                return ExecutionTime != TimeSpan.Zero 
                    ? ExecutionTime.ToString(@"hh\:mm") 
                    : "Any time";
            }
        }

        public static LibraryTaskViewModel FromLowTask(LowTask lowTask)
        {
            return new LibraryTaskViewModel
            {
                Id = lowTask.Id,
                Description = lowTask.Description,
                EstimatedTime = lowTask.EstimatedTime,
                ExecutionDate = lowTask.ExecutionDate.ToDateTime(TimeOnly.MinValue),
                ExecutionTime = lowTask.ExecutionTime.ToTimeSpan(),
                LastClearedAt = lowTask.LastClearedAt,
                MiddleTaskName = lowTask.MiddleTaskMName,
                ProjectName = lowTask.ProjectName,
                HTLName = lowTask.HowToLearnName,
                IsCompleted = lowTask.LastClearedAt.Date == DateTime.Today && 
                            lowTask.ExecutionTime != TimeOnly.MinValue
            };
        }
    }

    /// <summary>
    /// HTL別統計情報
    /// </summary>
    public class HTLStatistics
    {
        public string HTLName { get; set; } = string.Empty;
        public string HTLIcon { get; set; } = "📚";
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int ClearCount { get; set; } // 累計クリア回数
        
        public static string GetHTLIcon(string htlName)
        {
            return htlName?.ToLower() switch
            {
                var name when name.Contains("freeplane") => "🧠",
                var name when name.Contains("pdf") => "📄",
                var name when name.Contains("movie") => "🎬",
                var name when name.Contains("webpage") => "🌐",
                var name when name.Contains("world") => "🌍",
                _ => "📚"
            };
        }
    }
}