using System;
using System.Collections.Generic;

namespace WpfScheduledApp20250729.Auditing.Interfaces
{
    public interface IAuditRuleFactory
    {
        IAuditRule CreateRule(string ruleName, string description, Func<Microsoft.EntityFrameworkCore.DbContext, System.Threading.Tasks.Task<IEnumerable<Models.AuditResult>>> ruleLogic);
        IEnumerable<IAuditRule> GetBuiltInRules();
    }
}