using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Interfaces;
using WpfScheduledApp20250729.Auditing.Models;

namespace WpfScheduledApp20250729.Auditing.Rules
{
    public abstract class BaseAuditRule : IAuditRule
    {
        public string Name { get; }
        public string Description { get; }

        protected BaseAuditRule(string name, string description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public abstract Task<IEnumerable<AuditResult>> ExecuteAsync(DbContext context);

        protected AuditResult CreateResult(string entityType, string entityId, string description, 
            AuditSeverity severity = AuditSeverity.Warning, string details = "")
        {
            return new AuditResult
            {
                RuleName = Name,
                EntityType = entityType,
                EntityId = entityId,
                Description = description,
                Severity = severity,
                DetectedAt = DateTime.UtcNow,
                Details = details
            };
        }
    }
}