using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Models;

namespace WpfScheduledApp20250729.Auditing.Rules
{
    public class DynamicAuditRule : BaseAuditRule
    {
        private readonly Func<DbContext, Task<IEnumerable<AuditResult>>> _ruleLogic;

        public DynamicAuditRule(string name, string description, Func<DbContext, Task<IEnumerable<AuditResult>>> ruleLogic) 
            : base(name, description)
        {
            _ruleLogic = ruleLogic ?? throw new ArgumentNullException(nameof(ruleLogic));
        }

        public override async Task<IEnumerable<AuditResult>> ExecuteAsync(DbContext context)
        {
            return await _ruleLogic(context);
        }
    }
}