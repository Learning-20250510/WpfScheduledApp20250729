using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Models;
using WpfScheduledApp20250729.Auditing.Services;
using WpfScheduledApp20250729.Models.Context;

namespace WpfScheduledApp20250729.Auditing.Examples
{
    public static class CustomRuleExamples
    {
        public static void AddCustomRulesToAuditManager(AuditManager auditManager)
        {
            // 例1: 長時間更新されていないタスクをチェック
            auditManager.AddCustomRule(
                "StaleTaskCheck",
                "30日以上更新されていないタスクをチェック",
                CheckStaleTasksAsync
            );

            // 例2: 重複する名前のタスクをチェック
            auditManager.AddCustomRule(
                "DuplicateTaskName",
                "同じ名前を持つタスクが複数存在するかチェック",
                CheckDuplicateTaskNamesAsync
            );

            // 例3: エラーメッセージが設定されているタスクをチェック
            auditManager.AddCustomRule(
                "TasksWithErrors",
                "エラーメッセージが設定されているタスクをチェック",
                CheckTasksWithErrorsAsync
            );

            // 例4: 将来の日付が設定されているタスクをチェック
            auditManager.AddCustomRule(
                "FutureDateTasks",
                "将来の更新日時が設定されているタスクをチェック",
                CheckFutureDateTasksAsync
            );

            // 例5: 複数テーブルにまたがる不整合チェック - 参照整合性
            auditManager.AddCustomRule(
                "ReferentialIntegrityCheck",
                "外部キー参照の整合性をチェック",
                CheckReferentialIntegrityAsync
            );

            // 例6: 複数テーブルにまたがる不整合チェック - タスク階層の整合性
            auditManager.AddCustomRule(
                "TaskHierarchyIntegrityCheck",
                "タスク階層の整合性をチェック（HighTask -> MiddleTask -> LowTask）",
                CheckTaskHierarchyIntegrityAsync
            );

            // 例7: 複数テーブルにまたがる不整合チェック - プロジェクトとタスクの整合性
            auditManager.AddCustomRule(
                "ProjectTaskConsistencyCheck",
                "プロジェクトとタスクの整合性をチェック",
                CheckProjectTaskConsistencyAsync
            );
        }

        private static async Task<IEnumerable<AuditResult>> CheckStaleTasksAsync(DbContext context)
        {
            var results = new List<AuditResult>();
            var cutoffDate = DateTime.UtcNow.AddDays(-30);

            if (context is BaseDbContext dbContext)
            {
                // HighTask
                var staleHighTasks = await dbContext.HighTasks
                    .Where(t => !t.Archived && !t.Disabled && t.UpdatedAt < cutoffDate)
                    .ToListAsync();

                foreach (var task in staleHighTasks)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "StaleTaskCheck",
                        EntityType = "HighTask",
                        EntityId = task.Id.ToString(),
                        Description = $"30日以上更新されていません (最終更新: {task.UpdatedAt:yyyy-MM-dd})",
                        Severity = AuditSeverity.Warning,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"タスク名: {task.TaskName}, 最終更新: {task.UpdatedAt}, 経過日数: {(DateTime.UtcNow - task.UpdatedAt).Days}日"
                    });
                }

                // MiddleTask
                var staleMiddleTasks = await dbContext.MiddleTasks
                    .Where(t => !t.Archived && !t.Disabled && t.UpdatedAt < cutoffDate)
                    .ToListAsync();

                foreach (var task in staleMiddleTasks)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "StaleTaskCheck",
                        EntityType = "MiddleTask",
                        EntityId = task.Id.ToString(),
                        Description = $"30日以上更新されていません (最終更新: {task.UpdatedAt:yyyy-MM-dd})",
                        Severity = AuditSeverity.Warning,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"説明: {task.Description ?? "N/A"}, 最終更新: {task.UpdatedAt}, 経過日数: {(DateTime.UtcNow - task.UpdatedAt).Days}日"
                    });
                }

                // LowTask
                var staleLowTasks = await dbContext.LowTasks
                    .Where(t => !t.Archived && !t.Disabled && t.UpdatedAt < cutoffDate)
                    .ToListAsync();

                foreach (var task in staleLowTasks)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "StaleTaskCheck",
                        EntityType = "LowTask",
                        EntityId = task.Id.ToString(),
                        Description = $"30日以上更新されていません (最終更新: {task.UpdatedAt:yyyy-MM-dd})",
                        Severity = AuditSeverity.Warning,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"説明: {task.Description}, 最終更新: {task.UpdatedAt}, 経過日数: {(DateTime.UtcNow - task.UpdatedAt).Days}日"
                    });
                }
            }

            return await Task.FromResult(results);
        }

        private static async Task<IEnumerable<AuditResult>> CheckDuplicateTaskNamesAsync(DbContext context)
        {
            var results = new List<AuditResult>();

            if (context is BaseDbContext dbContext)
            {
                // HighTask の重複チェック (TaskNameプロパティを使用)
                var highTaskDuplicates = await dbContext.HighTasks
                    .Where(t => !t.Archived && !string.IsNullOrWhiteSpace(t.TaskName))
                    .GroupBy(t => t.TaskName.ToLower())
                    .Where(g => g.Count() > 1)
                    .ToListAsync();

                foreach (var group in highTaskDuplicates)
                {
                    var tasks = group.ToList();
                    foreach (var task in tasks)
                    {
                        results.Add(new AuditResult
                        {
                            RuleName = "DuplicateTaskName",
                            EntityType = "HighTask",
                            EntityId = task.Id.ToString(),
                            Description = $"重複するタスク名 '{task.TaskName}' ({group.Count()}個)",
                            Severity = AuditSeverity.Warning,
                            DetectedAt = DateTime.UtcNow,
                            Details = $"重複するタスクID: {string.Join(", ", tasks.Select(t => t.Id))}"
                        });
                    }
                }

                // MiddleTask の重複チェック (Descriptionプロパティを使用)
                var middleTaskDuplicates = await dbContext.MiddleTasks
                    .Where(t => !t.Archived && !string.IsNullOrWhiteSpace(t.Description))
                    .GroupBy(t => t.Description.ToLower())
                    .Where(g => g.Count() > 1)
                    .ToListAsync();

                foreach (var group in middleTaskDuplicates)
                {
                    var tasks = group.ToList();
                    foreach (var task in tasks)
                    {
                        results.Add(new AuditResult
                        {
                            RuleName = "DuplicateTaskName",
                            EntityType = "MiddleTask",
                            EntityId = task.Id.ToString(),
                            Description = $"重複する説明 '{task.Description}' ({group.Count()}個)",
                            Severity = AuditSeverity.Warning,
                            DetectedAt = DateTime.UtcNow,
                            Details = $"重複するタスクID: {string.Join(", ", tasks.Select(t => t.Id))}"
                        });
                    }
                }

                // LowTask の重複チェック (Descriptionプロパティを使用)
                var lowTaskDuplicates = await dbContext.LowTasks
                    .Where(t => !t.Archived && !string.IsNullOrWhiteSpace(t.Description))
                    .GroupBy(t => t.Description.ToLower())
                    .Where(g => g.Count() > 1)
                    .ToListAsync();

                foreach (var group in lowTaskDuplicates)
                {
                    var tasks = group.ToList();
                    foreach (var task in tasks)
                    {
                        results.Add(new AuditResult
                        {
                            RuleName = "DuplicateTaskName",
                            EntityType = "LowTask",
                            EntityId = task.Id.ToString(),
                            Description = $"重複する説明 '{task.Description}' ({group.Count()}個)",
                            Severity = AuditSeverity.Warning,
                            DetectedAt = DateTime.UtcNow,
                            Details = $"重複するタスクID: {string.Join(", ", tasks.Select(t => t.Id))}"
                        });
                    }
                }
            }

            return await Task.FromResult(results);
        }

        private static async Task<IEnumerable<AuditResult>> CheckTasksWithErrorsAsync(DbContext context)
        {
            var results = new List<AuditResult>();

            if (context is BaseDbContext dbContext)
            {
                // エラーメッセージがあるタスクをチェック
                var tasksWithErrors = new List<(string EntityType, object Id, string ErrorMessage, string Name)>();

                var highTasksWithErrors = await dbContext.HighTasks
                    .Where(t => !string.IsNullOrWhiteSpace(t.ErrorMessage))
                    .Select(t => new { t.Id, ErrorMessage = t.ErrorMessage!, t.TaskName })
                    .ToListAsync();

                tasksWithErrors.AddRange(highTasksWithErrors.Select(t => ("HighTask", (object)t.Id, t.ErrorMessage, t.TaskName)));

                var middleTasksWithErrors = await dbContext.MiddleTasks
                    .Where(t => !string.IsNullOrWhiteSpace(t.ErrorMessage))
                    .Select(t => new { t.Id, ErrorMessage = t.ErrorMessage!, t.Description })
                    .ToListAsync();

                tasksWithErrors.AddRange(middleTasksWithErrors.Select(t => ("MiddleTask", (object)t.Id, t.ErrorMessage, t.Description ?? "N/A")));

                var lowTasksWithErrors = await dbContext.LowTasks
                    .Where(t => !string.IsNullOrWhiteSpace(t.ErrorMessage))
                    .Select(t => new { t.Id, ErrorMessage = t.ErrorMessage!, t.Description })
                    .ToListAsync();

                tasksWithErrors.AddRange(lowTasksWithErrors.Select(t => ("LowTask", (object)t.Id, t.ErrorMessage, t.Description)));

                foreach (var (entityType, id, errorMessage, name) in tasksWithErrors)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "TasksWithErrors",
                        EntityType = entityType,
                        EntityId = id.ToString() ?? "N/A",
                        Description = "エラーメッセージが設定されています",
                        Severity = AuditSeverity.Error,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"タスク名: {name}, エラー: {errorMessage}"
                    });
                }
            }

            return await Task.FromResult(results);
        }

        private static async Task<IEnumerable<AuditResult>> CheckFutureDateTasksAsync(DbContext context)
        {
            var results = new List<AuditResult>();
            var now = DateTime.UtcNow;

            if (context is BaseDbContext dbContext)
            {
                var futureTasks = new List<(string EntityType, object Id, DateTime UpdatedAt, string Name)>();

                var futureHighTasks = await dbContext.HighTasks
                    .Where(t => t.UpdatedAt > now || t.TouchedAt > now)
                    .Select(t => new { t.Id, t.UpdatedAt, t.TouchedAt, t.TaskName })
                    .ToListAsync();

                foreach (var task in futureHighTasks)
                {
                    if (task.UpdatedAt > now)
                    {
                        futureTasks.Add(("HighTask", (object)task.Id, task.UpdatedAt, task.TaskName));
                    }
                    if (task.TouchedAt > now)
                    {
                        futureTasks.Add(("HighTask", (object)task.Id, task.TouchedAt, task.TaskName));
                    }
                }

                var futureMiddleTasks = await dbContext.MiddleTasks
                    .Where(t => t.UpdatedAt > now || t.TouchedAt > now)
                    .Select(t => new { t.Id, t.UpdatedAt, t.TouchedAt, t.Description })
                    .ToListAsync();

                foreach (var task in futureMiddleTasks)
                {
                    if (task.UpdatedAt > now)
                    {
                        futureTasks.Add(("MiddleTask", (object)task.Id, task.UpdatedAt, task.Description ?? "N/A"));
                    }
                    if (task.TouchedAt > now)
                    {
                        futureTasks.Add(("MiddleTask", (object)task.Id, task.TouchedAt, task.Description ?? "N/A"));
                    }
                }

                var futureLowTasks = await dbContext.LowTasks
                    .Where(t => t.UpdatedAt > now || t.TouchedAt > now)
                    .Select(t => new { t.Id, t.UpdatedAt, t.TouchedAt, t.Description })
                    .ToListAsync();

                foreach (var task in futureLowTasks)
                {
                    if (task.UpdatedAt > now)
                    {
                        futureTasks.Add(("LowTask", (object)task.Id, task.UpdatedAt, task.Description));
                    }
                    if (task.TouchedAt > now)
                    {
                        futureTasks.Add(("LowTask", (object)task.Id, task.TouchedAt, task.Description));
                    }
                }

                foreach (var (entityType, id, futureDate, name) in futureTasks)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "FutureDateTasks",
                        EntityType = entityType,
                        EntityId = id.ToString() ?? "N/A",
                        Description = $"将来の日付が設定されています ({futureDate:yyyy-MM-dd HH:mm})",
                        Severity = AuditSeverity.Error,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"タスク名: {name}, 将来日時: {futureDate}, 現在: {now}"
                    });
                }
            }

            return await Task.FromResult(results);
        }

        private static async Task<IEnumerable<AuditResult>> CheckReferentialIntegrityAsync(DbContext context)
        {
            var results = new List<AuditResult>();

            if (context is BaseDbContext dbContext)
            {
                // MiddleTaskのHighTaskId参照をチェック
                var middleTasksWithInvalidHighTaskId = await dbContext.MiddleTasks
                    .Where(mt => !dbContext.HighTasks.Any(ht => ht.Id == mt.HighTaskId))
                    .ToListAsync();

                foreach (var middleTask in middleTasksWithInvalidHighTaskId)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ReferentialIntegrityCheck",
                        EntityType = "MiddleTask",
                        EntityId = middleTask.Id.ToString(),
                        Description = $"存在しないHighTaskId '{middleTask.HighTaskId}' を参照しています",
                        Severity = AuditSeverity.Critical,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"MiddleTask ID: {middleTask.Id}, 無効なHighTaskId: {middleTask.HighTaskId}"
                    });
                }

                // LowTaskのMiddleTaskId参照をチェック
                var lowTasksWithInvalidMiddleTaskId = await dbContext.LowTasks
                    .Where(lt => !dbContext.MiddleTasks.Any(mt => mt.Id == lt.MiddleTaskId))
                    .ToListAsync();

                foreach (var lowTask in lowTasksWithInvalidMiddleTaskId)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ReferentialIntegrityCheck",
                        EntityType = "LowTask",
                        EntityId = lowTask.Id.ToString(),
                        Description = $"存在しないMiddleTaskId '{lowTask.MiddleTaskId}' を参照しています",
                        Severity = AuditSeverity.Critical,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"LowTask ID: {lowTask.Id}, 無効なMiddleTaskId: {lowTask.MiddleTaskId}"
                    });
                }

                // HighTaskのArchitectureId参照をチェック
                var highTasksWithInvalidArchitectureId = await dbContext.HighTasks
                    .Where(ht => !dbContext.Architectures.Any(a => a.Id == ht.ArchitectureId))
                    .ToListAsync();

                foreach (var highTask in highTasksWithInvalidArchitectureId)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ReferentialIntegrityCheck",
                        EntityType = "HighTask",
                        EntityId = highTask.Id.ToString(),
                        Description = $"存在しないArchitectureId '{highTask.ArchitectureId}' を参照しています",
                        Severity = AuditSeverity.Critical,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"HighTask ID: {highTask.Id}, TaskName: {highTask.TaskName}, 無効なArchitectureId: {highTask.ArchitectureId}"
                    });
                }

                // プロジェクト参照の整合性チェック
                var tasksWithInvalidProjectId = new List<(string EntityType, object Id, int ProjectId, string Name)>();

                var highTasksWithInvalidProjectId = await dbContext.HighTasks
                    .Where(ht => !dbContext.Projects.Any(p => p.Id == ht.ProjectId))
                    .ToListAsync();
                tasksWithInvalidProjectId.AddRange(highTasksWithInvalidProjectId
                    .Select(t => ("HighTask", (object)t.Id, t.ProjectId, t.TaskName)));

                var middleTasksWithInvalidProjectId = await dbContext.MiddleTasks
                    .Where(mt => !dbContext.Projects.Any(p => p.Id == mt.ProjectId))
                    .ToListAsync();
                tasksWithInvalidProjectId.AddRange(middleTasksWithInvalidProjectId
                    .Select(t => ("MiddleTask", (object)t.Id, t.ProjectId, t.Description ?? "N/A")));

                var lowTasksWithInvalidProjectId = await dbContext.LowTasks
                    .Where(lt => !dbContext.Projects.Any(p => p.Id == lt.ProjectId))
                    .ToListAsync();
                tasksWithInvalidProjectId.AddRange(lowTasksWithInvalidProjectId
                    .Select(t => ("LowTask", (object)t.Id, t.ProjectId, t.Description)));

                foreach (var (entityType, id, projectId, name) in tasksWithInvalidProjectId)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ReferentialIntegrityCheck",
                        EntityType = entityType,
                        EntityId = id.ToString() ?? "N/A",
                        Description = $"存在しないProjectId '{projectId}' を参照しています",
                        Severity = AuditSeverity.Critical,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"{entityType} ID: {id}, Name: {name}, 無効なProjectId: {projectId}"
                    });
                }
            }

            return await Task.FromResult(results);
        }

        private static async Task<IEnumerable<AuditResult>> CheckTaskHierarchyIntegrityAsync(DbContext context)
        {
            var results = new List<AuditResult>();

            if (context is BaseDbContext dbContext)
            {
                // HighTask -> MiddleTask -> LowTask の階層整合性をチェック

                // 1. MiddleTaskが参照するHighTaskが同じプロジェクトに属しているかチェック
                var inconsistentMiddleTasks = await (from mt in dbContext.MiddleTasks
                                                   join ht in dbContext.HighTasks on mt.HighTaskId equals ht.Id
                                                   where mt.ProjectId != ht.ProjectId
                                                   select new { MiddleTask = mt, HighTask = ht })
                                                   .ToListAsync();

                foreach (var item in inconsistentMiddleTasks)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "TaskHierarchyIntegrityCheck",
                        EntityType = "MiddleTask",
                        EntityId = item.MiddleTask.Id.ToString(),
                        Description = $"MiddleTaskのプロジェクトID ({item.MiddleTask.ProjectId}) が親HighTaskのプロジェクトID ({item.HighTask.ProjectId}) と異なります",
                        Severity = AuditSeverity.Error,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"MiddleTask ID: {item.MiddleTask.Id}, HighTask ID: {item.HighTask.Id}, HighTask Name: {item.HighTask.TaskName}"
                    });
                }

                // 2. LowTaskが参照するMiddleTaskが同じプロジェクトに属しているかチェック
                var inconsistentLowTasks = await (from lt in dbContext.LowTasks
                                                join mt in dbContext.MiddleTasks on lt.MiddleTaskId equals mt.Id
                                                where lt.ProjectId != mt.ProjectId
                                                select new { LowTask = lt, MiddleTask = mt })
                                                .ToListAsync();

                foreach (var item in inconsistentLowTasks)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "TaskHierarchyIntegrityCheck",
                        EntityType = "LowTask",
                        EntityId = item.LowTask.Id.ToString(),
                        Description = $"LowTaskのプロジェクトID ({item.LowTask.ProjectId}) が親MiddleTaskのプロジェクトID ({item.MiddleTask.ProjectId}) と異なります",
                        Severity = AuditSeverity.Error,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"LowTask ID: {item.LowTask.Id}, Description: {item.LowTask.Description}, MiddleTask ID: {item.MiddleTask.Id}"
                    });
                }

                // 3. HighTaskの作成日時 <= MiddleTask作成日時 <= LowTask作成日時 の順序チェック
                var hierarchyDateInconsistencies = await (from lt in dbContext.LowTasks
                                                        join mt in dbContext.MiddleTasks on lt.MiddleTaskId equals mt.Id
                                                        join ht in dbContext.HighTasks on mt.HighTaskId equals ht.Id
                                                        where ht.CreatedAt > mt.CreatedAt || mt.CreatedAt > lt.CreatedAt
                                                        select new { LowTask = lt, MiddleTask = mt, HighTask = ht })
                                                        .ToListAsync();

                foreach (var item in hierarchyDateInconsistencies)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "TaskHierarchyIntegrityCheck",
                        EntityType = "TaskHierarchy",
                        EntityId = $"HT:{item.HighTask.Id},MT:{item.MiddleTask.Id},LT:{item.LowTask.Id}",
                        Description = "タスク階層の作成日時順序が不正です",
                        Severity = AuditSeverity.Warning,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"HighTask作成: {item.HighTask.CreatedAt}, MiddleTask作成: {item.MiddleTask.CreatedAt}, LowTask作成: {item.LowTask.CreatedAt}"
                    });
                }
            }

            return await Task.FromResult(results);
        }

        private static async Task<IEnumerable<AuditResult>> CheckProjectTaskConsistencyAsync(DbContext context)
        {
            var results = new List<AuditResult>();

            if (context is BaseDbContext dbContext)
            {
                // プロジェクトとタスクの整合性チェック

                // 1. 無効化されたプロジェクトに属する有効なタスクをチェック
                var tasksInDisabledProjects = await (from p in dbContext.Projects
                                                   where p.Disabled
                                                   from ht in dbContext.HighTasks
                                                   where ht.ProjectId == p.Id && !ht.Disabled
                                                   select new { Project = p, HighTask = ht })
                                                   .ToListAsync();

                foreach (var item in tasksInDisabledProjects)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ProjectTaskConsistencyCheck",
                        EntityType = "HighTask",
                        EntityId = item.HighTask.Id.ToString(),
                        Description = $"無効化されたプロジェクト '{item.Project.Id}' に属する有効なタスクが存在します",
                        Severity = AuditSeverity.Warning,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"HighTask: {item.HighTask.TaskName}, Project Disabled: {item.Project.Disabled}, Task Disabled: {item.HighTask.Disabled}"
                    });
                }

                // 2. アーカイブされたプロジェクトに属する非アーカイブタスクをチェック
                var tasksInArchivedProjects = await (from p in dbContext.Projects
                                                   where p.Archived
                                                   from ht in dbContext.HighTasks
                                                   where ht.ProjectId == p.Id && !ht.Archived
                                                   select new { Project = p, HighTask = ht })
                                                   .ToListAsync();

                foreach (var item in tasksInArchivedProjects)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ProjectTaskConsistencyCheck",
                        EntityType = "HighTask",
                        EntityId = item.HighTask.Id.ToString(),
                        Description = $"アーカイブされたプロジェクト '{item.Project.Id}' に属する非アーカイブタスクが存在します",
                        Severity = AuditSeverity.Warning,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"HighTask: {item.HighTask.TaskName}, Project Archived: {item.Project.Archived}, Task Archived: {item.HighTask.Archived}"
                    });
                }

                // 3. タスクに関連するカウンター整合性チェック
                var tasksWithInconsistentCounters = await dbContext.HighTasks
                    .Where(ht => ht.ClearTimesInTime < 0 || ht.ClearTimesOutofTime < 0)
                    .ToListAsync();

                foreach (var task in tasksWithInconsistentCounters)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ProjectTaskConsistencyCheck",
                        EntityType = "HighTask",
                        EntityId = task.Id.ToString(),
                        Description = "タスククリア回数が負の値になっています",
                        Severity = AuditSeverity.Error,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"TaskName: {task.TaskName}, InTime: {task.ClearTimesInTime}, OutOfTime: {task.ClearTimesOutofTime}"
                    });
                }

                // 4. MiddleTaskとLowTaskのカウンター整合性チェック
                var middleTasksWithInconsistentCounters = await dbContext.MiddleTasks
                    .Where(mt => mt.ClearTimesInTime < 0 || mt.ClearTimesOutofTime < 0 || mt.EstimatedTime < 0)
                    .ToListAsync();

                foreach (var task in middleTasksWithInconsistentCounters)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ProjectTaskConsistencyCheck",
                        EntityType = "MiddleTask",
                        EntityId = task.Id.ToString(),
                        Description = "タスクの数値フィールドが負の値になっています",
                        Severity = AuditSeverity.Error,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"InTime: {task.ClearTimesInTime}, OutOfTime: {task.ClearTimesOutofTime}, EstimatedTime: {task.EstimatedTime}"
                    });
                }

                var lowTasksWithInconsistentCounters = await dbContext.LowTasks
                    .Where(lt => lt.ClearTimesInTime < 0 || lt.ClearTimesOutofTime < 0 || lt.EstimatedTime < 0)
                    .ToListAsync();

                foreach (var task in lowTasksWithInconsistentCounters)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "ProjectTaskConsistencyCheck",
                        EntityType = "LowTask",
                        EntityId = task.Id.ToString(),
                        Description = "タスクの数値フィールドが負の値になっています",
                        Severity = AuditSeverity.Error,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"Description: {task.Description}, InTime: {task.ClearTimesInTime}, OutOfTime: {task.ClearTimesOutofTime}, EstimatedTime: {task.EstimatedTime}"
                    });
                }
            }

            return await Task.FromResult(results);
        }

        public static void DemonstrateCustomRuleUsage(AuditManager auditManager)
        {
            // 監査間隔を10分に変更
            auditManager.SetInterval(TimeSpan.FromMinutes(10));

            // 独自のビジネスルールを追加
            auditManager.AddCustomRule(
                "CustomBusinessRule",
                "カスタムビジネスルールの例",
                async (context) =>
                {
                    var results = new List<AuditResult>();
                    
                    // ここに独自のビジネスロジックを実装
                    // 例: 特定の条件に合致するデータの検証
                    if (context is BaseDbContext dbContext)
                    {
                        // 実際の検証ロジックをここに追加
                        await Task.CompletedTask; // awaitを使用するためのダミー
                    }
                    
                    return await Task.FromResult(results);
                }
            );

            // 不要になったルールを削除
            auditManager.RemoveRule("StaleTaskCheck");
        }
    }
}