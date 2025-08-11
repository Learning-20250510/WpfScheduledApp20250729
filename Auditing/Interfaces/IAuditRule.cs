using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Models;

namespace WpfScheduledApp20250729.Auditing.Interfaces
{
    public interface IAuditRule
    {
        string Name { get; }
        string Description { get; }
        Task<IEnumerable<AuditResult>> ExecuteAsync(DbContext context);
    }
}