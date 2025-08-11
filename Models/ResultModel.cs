using System;

namespace WpfScheduledApp20250729.Models
{
    public class ResultModel
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string ResultText { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int EstimatedTime { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
        public ResultAction SelectedAction { get; set; }
        public int ClearTimesinTIme { get; set; }
        public int ClearTImesoutoftIme { get; set; }
    }

    public enum ResultAction
    {
        Clear,
        Again,
        AgainLater,
        UpdateColumn
    }
}