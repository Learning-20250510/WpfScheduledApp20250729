using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Interfaces;
using WpfScheduledApp20250729.Auditing.Models;
using WpfScheduledApp20250729.Auditing.Rules;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.Auditing.Services
{
    public class AuditRuleFactory : IAuditRuleFactory
    {
        public IAuditRule CreateRule(string ruleName, string description, Func<DbContext, Task<IEnumerable<AuditResult>>> ruleLogic)
        {
            return new DynamicAuditRule(ruleName, description, ruleLogic);
        }

        public IEnumerable<IAuditRule> GetBuiltInRules()
        {
            return new List<IAuditRule>
            {
                CreateTaskDateConsistencyRule(),
                CreateArchivedButNotDisabledRule(),
                CreateEmptyTaskNameRule(),
                CreateFutureCreatedAtRule(),
                CreateUpdateBeforeCreateRule()
            };
        }

        private IAuditRule CreateTaskDateConsistencyRule()
        {
            return CreateRule(
                "TaskDateConsistency",
                "タスクの日付の整合性をチェック（CreatedAt <= UpdatedAt <= TouchedAt）",
                async (context) =>
                {
                    var results = new List<AuditResult>();

                    if (context is BaseDbContext dbContext)
                    {
                        // HighTask
                        var highTasks = await dbContext.HighTasks.ToListAsync();
                        results.AddRange(CheckDateConsistency(highTasks.Cast<BaseEntity>(), "HighTask"));

                        // MiddleTask
                        var middleTasks = await dbContext.MiddleTasks.ToListAsync();
                        results.AddRange(CheckDateConsistency(middleTasks.Cast<BaseEntity>(), "MiddleTask"));

                        // LowTask
                        var lowTasks = await dbContext.LowTasks.ToListAsync();
                        results.AddRange(CheckDateConsistency(lowTasks.Cast<BaseEntity>(), "LowTask"));
                    }

                    return await Task.FromResult(results);
                }
            );
        }

        private IAuditRule CreateArchivedButNotDisabledRule()
        {
            return CreateRule(
                "ArchivedButNotDisabled",
                "アーカイブされているのに無効化されていないエンティティをチェック",
                async (context) =>
                {
                    var results = new List<AuditResult>();

                    if (context is BaseDbContext dbContext)
                    {
                        var inconsistentEntities = new List<(BaseEntity entity, string type, object id)>();

                        // 各エンティティタイプをチェック
                        var highTasks = await dbContext.HighTasks.Where(t => t.Archived && !t.Disabled).ToListAsync();
                        inconsistentEntities.AddRange(highTasks.Select(t => ((BaseEntity)t, "HighTask", (object)t.Id)));

                        var middleTasks = await dbContext.MiddleTasks.Where(t => t.Archived && !t.Disabled).ToListAsync();
                        inconsistentEntities.AddRange(middleTasks.Select(t => ((BaseEntity)t, "MiddleTask", (object)t.Id)));

                        var lowTasks = await dbContext.LowTasks.Where(t => t.Archived && !t.Disabled).ToListAsync();
                        inconsistentEntities.AddRange(lowTasks.Select(t => ((BaseEntity)t, "LowTask", (object)t.Id)));

                        foreach (var (entity, type, id) in inconsistentEntities)
                        {
                            results.Add(new AuditResult
                            {
                                RuleName = "ArchivedButNotDisabled",
                                EntityType = type,
                                EntityId = id.ToString() ?? "N/A",
                                Description = $"{type} がアーカイブされていますが無効化されていません",
                                Severity = AuditSeverity.Warning,
                                DetectedAt = DateTime.UtcNow,
                                Details = $"Archived=true, Disabled=false at {entity.UpdatedAt}"
                            });
                        }
                    }

                    return await Task.FromResult(results);
                }
            );
        }

        private IAuditRule CreateEmptyTaskNameRule()
        {
            return CreateRule(
                "EmptyTaskName",
                "空のタスク名または説明を持つタスクをチェック",
                async (context) =>
                {
                    var results = new List<AuditResult>();

                    if (context is BaseDbContext dbContext)
                    {
                        // HighTask - TaskNameプロパティをチェック
                        var highTasksWithEmptyNames = await dbContext.HighTasks
                            .Where(t => string.IsNullOrWhiteSpace(t.TaskName))
                            .ToListAsync();

                        foreach (var task in highTasksWithEmptyNames)
                        {
                            results.Add(new AuditResult
                            {
                                RuleName = "EmptyTaskName",
                                EntityType = "HighTask",
                                EntityId = task.Id.ToString(),
                                Description = "タスク名が空です",
                                Severity = AuditSeverity.Error,
                                DetectedAt = DateTime.UtcNow,
                                Details = $"CreatedAt: {task.CreatedAt}, UpdatedAt: {task.UpdatedAt}"
                            });
                        }

                        // MiddleTask - Descriptionプロパティをチェック
                        var middleTasksWithEmptyDesc = await dbContext.MiddleTasks
                            .Where(t => string.IsNullOrWhiteSpace(t.Description))
                            .ToListAsync();

                        foreach (var task in middleTasksWithEmptyDesc)
                        {
                            results.Add(new AuditResult
                            {
                                RuleName = "EmptyTaskName",
                                EntityType = "MiddleTask",
                                EntityId = task.Id.ToString(),
                                Description = "タスクの説明が空です",
                                Severity = AuditSeverity.Warning,
                                DetectedAt = DateTime.UtcNow,
                                Details = $"CreatedAt: {task.CreatedAt}, UpdatedAt: {task.UpdatedAt}"
                            });
                        }

                        // LowTask - Descriptionプロパティをチェック
                        var lowTasksWithEmptyDesc = await dbContext.LowTasks
                            .Where(t => string.IsNullOrWhiteSpace(t.Description))
                            .ToListAsync();

                        foreach (var task in lowTasksWithEmptyDesc)
                        {
                            results.Add(new AuditResult
                            {
                                RuleName = "EmptyTaskName",
                                EntityType = "LowTask",
                                EntityId = task.Id.ToString(),
                                Description = "タスクの説明が空です",
                                Severity = AuditSeverity.Warning,
                                DetectedAt = DateTime.UtcNow,
                                Details = $"CreatedAt: {task.CreatedAt}, UpdatedAt: {task.UpdatedAt}"
                            });
                        }
                    }

                    return await Task.FromResult(results);
                }
            );
        }

        private IAuditRule CreateFutureCreatedAtRule()
        {
            return CreateRule(
                "FutureCreatedAt",
                "未来の作成日時を持つエンティティをチェック",
                async (context) =>
                {
                    var results = new List<AuditResult>();
                    var now = DateTime.UtcNow;

                    if (context is BaseDbContext dbContext)
                    {
                        var futureEntities = new List<(BaseEntity entity, string type, object id)>();

                        // 各エンティティタイプをチェック
                        var futureHighTasks = await dbContext.HighTasks.Where(t => t.CreatedAt > now).ToListAsync();
                        futureEntities.AddRange(futureHighTasks.Select(t => ((BaseEntity)t, "HighTask", (object)t.Id)));

                        var futureMiddleTasks = await dbContext.MiddleTasks.Where(t => t.CreatedAt > now).ToListAsync();
                        futureEntities.AddRange(futureMiddleTasks.Select(t => ((BaseEntity)t, "MiddleTask", (object)t.Id)));

                        var futureLowTasks = await dbContext.LowTasks.Where(t => t.CreatedAt > now).ToListAsync();
                        futureEntities.AddRange(futureLowTasks.Select(t => ((BaseEntity)t, "LowTask", (object)t.Id)));

                        foreach (var (entity, type, id) in futureEntities)
                        {
                            results.Add(new AuditResult
                            {
                                RuleName = "FutureCreatedAt",
                                EntityType = type,
                                EntityId = id.ToString() ?? "N/A",
                                Description = $"{type} の作成日時が未来になっています",
                                Severity = AuditSeverity.Error,
                                DetectedAt = DateTime.UtcNow,
                                Details = $"CreatedAt: {entity.CreatedAt}, Current: {now}"
                            });
                        }
                    }

                    return await Task.FromResult(results);
                }
            );
        }

        private IAuditRule CreateUpdateBeforeCreateRule()
        {
            return CreateRule(
                "UpdateBeforeCreate",
                "更新日時が作成日時より前のエンティティをチェック",
                async (context) =>
                {
                    var results = new List<AuditResult>();

                    if (context is BaseDbContext dbContext)
                    {
                        var inconsistentEntities = new List<(BaseEntity entity, string type, object id)>();

                        // 各エンティティタイプをチェック
                        var highTasks = await dbContext.HighTasks.Where(t => t.UpdatedAt < t.CreatedAt).ToListAsync();
                        inconsistentEntities.AddRange(highTasks.Select(t => ((BaseEntity)t, "HighTask", (object)t.Id)));

                        var middleTasks = await dbContext.MiddleTasks.Where(t => t.UpdatedAt < t.CreatedAt).ToListAsync();
                        inconsistentEntities.AddRange(middleTasks.Select(t => ((BaseEntity)t, "MiddleTask", (object)t.Id)));

                        var lowTasks = await dbContext.LowTasks.Where(t => t.UpdatedAt < t.CreatedAt).ToListAsync();
                        inconsistentEntities.AddRange(lowTasks.Select(t => ((BaseEntity)t, "LowTask", (object)t.Id)));

                        foreach (var (entity, type, id) in inconsistentEntities)
                        {
                            results.Add(new AuditResult
                            {
                                RuleName = "UpdateBeforeCreate",
                                EntityType = type,
                                EntityId = id.ToString() ?? "N/A",
                                Description = $"{type} の更新日時が作成日時より前になっています",
                                Severity = AuditSeverity.Critical,
                                DetectedAt = DateTime.UtcNow,
                                Details = $"CreatedAt: {entity.CreatedAt}, UpdatedAt: {entity.UpdatedAt}"
                            });
                        }
                    }

                    return await Task.FromResult(results);
                }
            );
        }

        private static IEnumerable<AuditResult> CheckDateConsistency(IEnumerable<BaseEntity> entities, string entityType)
        {
            var results = new List<AuditResult>();

            foreach (var entity in entities)
            {
                // CreatedAt <= UpdatedAt <= TouchedAt の順序をチェック
                if (entity.CreatedAt > entity.UpdatedAt)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "TaskDateConsistency",
                        EntityType = entityType,
                        EntityId = GetEntityId(entity),
                        Description = $"{entityType} の作成日時が更新日時より後になっています",
                        Severity = AuditSeverity.Critical,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"CreatedAt: {entity.CreatedAt}, UpdatedAt: {entity.UpdatedAt}"
                    });
                }

                if (entity.UpdatedAt > entity.TouchedAt)
                {
                    results.Add(new AuditResult
                    {
                        RuleName = "TaskDateConsistency",
                        EntityType = entityType,
                        EntityId = GetEntityId(entity),
                        Description = $"{entityType} の更新日時がタッチ日時より後になっています",
                        Severity = AuditSeverity.Warning,
                        DetectedAt = DateTime.UtcNow,
                        Details = $"UpdatedAt: {entity.UpdatedAt}, TouchedAt: {entity.TouchedAt}"
                    });
                }
            }

            return results;
        }

        private static string GetEntityId(BaseEntity entity)
        {
            if (entity == null)
                return "N/A";

            // リフレクションを使用してIdプロパティを取得
            var idProperty = entity.GetType().GetProperty("Id");
            return idProperty?.GetValue(entity)?.ToString() ?? "N/A";
        }
    }
}