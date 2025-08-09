using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Models;

namespace WpfScheduledApp20250729.Auditing.Interfaces
{
    public interface IAuditService
    {
        Task StartAsync();
        Task StopAsync();
        void AddRule(IAuditRule rule);
        void RemoveRule(string ruleName);
        IEnumerable<IAuditRule> GetRules();
        event EventHandler<IEnumerable<AuditResult>> AuditCompleted;
        void SetInterval(TimeSpan interval);
    }
}