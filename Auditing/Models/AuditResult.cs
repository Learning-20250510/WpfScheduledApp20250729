using System;

namespace WpfScheduledApp20250729.Auditing.Models
{
    public class AuditResult
    {
        public string RuleName { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty;
        public string EntityId { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public AuditSeverity Severity { get; set; }
        public DateTime DetectedAt { get; set; }
        public string Details { get; set; } = string.Empty;
    }

    public enum AuditSeverity
    {
        Info = 0,
        Warning = 1,
        Error = 2,
        Critical = 3
    }
}