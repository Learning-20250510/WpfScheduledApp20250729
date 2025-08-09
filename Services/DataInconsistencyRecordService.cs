using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Models.Entities;
using WpfScheduledApp20250729.Auditing.Models;
using WpfScheduledApp20250729.Utils;

namespace WpfScheduledApp20250729.Services
{
    public class DataInconsistencyRecordService : BaseService<DataInconsistencyRecord>
    {
        public DataInconsistencyRecordService(BaseDbContext context) : base(context)
        {
        }

        protected override void UpdateEntityProperties(DataInconsistencyRecord existingEntity, DataInconsistencyRecord updatedEntity)
        {
            existingEntity.RuleName = updatedEntity.RuleName;
            existingEntity.ErrorDescription = updatedEntity.ErrorDescription;
            existingEntity.TargetTableName = updatedEntity.TargetTableName;
            existingEntity.TargetPrimaryKey = updatedEntity.TargetPrimaryKey;
            existingEntity.Severity = updatedEntity.Severity;
            existingEntity.Details = updatedEntity.Details;
            existingEntity.DetectedAt = updatedEntity.DetectedAt;
            existingEntity.IsResolved = updatedEntity.IsResolved;
            existingEntity.ResolvedAt = updatedEntity.ResolvedAt;
            existingEntity.ResolvedBy = updatedEntity.ResolvedBy;
            existingEntity.ResolutionNotes = updatedEntity.ResolutionNotes;
        }

        public async Task<DataInconsistencyRecord> UpdateAsync(DataInconsistencyRecord updatedEntity)
        {
            var result = await UpdateAsync(updatedEntity.Id, updatedEntity);
            return result ?? throw new InvalidOperationException($"Failed to update DataInconsistencyRecord with ID {updatedEntity.Id}");
        }

        public async Task<DataInconsistencyRecord> CreateFromAuditResultAsync(AuditResult auditResult)
        {
            try
            {
                var record = new DataInconsistencyRecord
                {
                    RuleName = auditResult.RuleName,
                    ErrorDescription = auditResult.Description,
                    TargetTableName = auditResult.EntityType,
                    TargetPrimaryKey = auditResult.EntityId,
                    Severity = auditResult.Severity.ToString(),
                    Details = auditResult.Details,
                    DetectedAt = auditResult.DetectedAt.ToUniversalTime(),
                    IsResolved = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    TouchedAt = DateTime.UtcNow,
                    LastUpdMethodName = nameof(CreateFromAuditResultAsync)
                };

                var createdRecord = await AddAsync(record);
                Logger.LogInfo($"データ不整合レコードを作成: ID={createdRecord.Id}, Rule={auditResult.RuleName}", 
                    nameof(CreateFromAuditResultAsync), nameof(DataInconsistencyRecordService));
                
                return createdRecord;
            }
            catch (Exception ex)
            {
                Logger.LogError($"データ不整合レコード作成エラー: {ex.Message}", 
                    nameof(CreateFromAuditResultAsync), nameof(DataInconsistencyRecordService));
                Logger.LogError(ex, nameof(CreateFromAuditResultAsync), nameof(DataInconsistencyRecordService));
                throw;
            }
        }

        public async Task<IEnumerable<DataInconsistencyRecord>> CreateBatchFromAuditResultsAsync(IEnumerable<AuditResult> auditResults)
        {
            var records = new List<DataInconsistencyRecord>();

            try
            {
                foreach (var auditResult in auditResults)
                {
                    // 既存の同じ不整合レコードが未解決で存在するかチェック
                    var existingRecord = await GetExistingUnresolvedRecordAsync(auditResult);
                    if (existingRecord != null)
                    {
                        // 既存レコードを更新
                        existingRecord.DetectedAt = auditResult.DetectedAt.ToUniversalTime();
                        existingRecord.Details = auditResult.Details;
                        existingRecord.UpdatedAt = DateTime.UtcNow;
                        existingRecord.TouchedAt = DateTime.UtcNow;
                        existingRecord.LastUpdMethodName = nameof(CreateBatchFromAuditResultsAsync);

                        await UpdateAsync(existingRecord);
                        records.Add(existingRecord);

                        Logger.LogInfo($"既存のデータ不整合レコードを更新: ID={existingRecord.Id}", 
                            nameof(CreateBatchFromAuditResultsAsync), nameof(DataInconsistencyRecordService));
                    }
                    else
                    {
                        // 新しいレコードを作成
                        var newRecord = await CreateFromAuditResultAsync(auditResult);
                        records.Add(newRecord);
                    }
                }

                Logger.LogInfo($"バッチ処理完了: {records.Count}件のデータ不整合レコードを処理", 
                    nameof(CreateBatchFromAuditResultsAsync), nameof(DataInconsistencyRecordService));

                return records;
            }
            catch (Exception ex)
            {
                Logger.LogError($"バッチ処理エラー: {ex.Message}", 
                    nameof(CreateBatchFromAuditResultsAsync), nameof(DataInconsistencyRecordService));
                Logger.LogError(ex, nameof(CreateBatchFromAuditResultsAsync), nameof(DataInconsistencyRecordService));
                throw;
            }
        }

        public async Task<DataInconsistencyRecord?> GetExistingUnresolvedRecordAsync(AuditResult auditResult)
        {
            try
            {
                return await _context.Set<DataInconsistencyRecord>()
                    .FirstOrDefaultAsync(r => 
                        r.RuleName == auditResult.RuleName &&
                        r.TargetTableName == auditResult.EntityType &&
                        r.TargetPrimaryKey == auditResult.EntityId &&
                        !r.IsResolved);
            }
            catch (Exception ex)
            {
                Logger.LogError($"既存レコード検索エラー: {ex.Message}", 
                    nameof(GetExistingUnresolvedRecordAsync), nameof(DataInconsistencyRecordService));
                return null;
            }
        }

        public async Task<DataInconsistencyRecord> MarkAsResolvedAsync(int recordId, string resolvedBy, string? resolutionNotes = null)
        {
            try
            {
                var record = await GetByIdAsync(recordId);
                if (record == null)
                    throw new ArgumentException($"Record with ID {recordId} not found");

                record.IsResolved = true;
                record.ResolvedAt = DateTime.UtcNow;
                record.ResolvedBy = resolvedBy;
                record.ResolutionNotes = resolutionNotes;
                record.UpdatedAt = DateTime.UtcNow;
                record.TouchedAt = DateTime.UtcNow;
                record.LastUpdMethodName = nameof(MarkAsResolvedAsync);

                var updatedRecord = await UpdateAsync(record);
                Logger.LogInfo($"データ不整合レコードを解決済みに更新: ID={recordId}, ResolvedBy={resolvedBy}", 
                    nameof(MarkAsResolvedAsync), nameof(DataInconsistencyRecordService));

                return updatedRecord;
            }
            catch (Exception ex)
            {
                Logger.LogError($"レコード解決処理エラー: {ex.Message}", 
                    nameof(MarkAsResolvedAsync), nameof(DataInconsistencyRecordService));
                Logger.LogError(ex, nameof(MarkAsResolvedAsync), nameof(DataInconsistencyRecordService));
                throw;
            }
        }

        public async Task<IEnumerable<DataInconsistencyRecord>> GetUnresolvedRecordsAsync()
        {
            try
            {
                return await _context.Set<DataInconsistencyRecord>()
                    .Where(r => !r.IsResolved)
                    .OrderByDescending(r => r.DetectedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"未解決レコード取得エラー: {ex.Message}", 
                    nameof(GetUnresolvedRecordsAsync), nameof(DataInconsistencyRecordService));
                Logger.LogError(ex, nameof(GetUnresolvedRecordsAsync), nameof(DataInconsistencyRecordService));
                return Enumerable.Empty<DataInconsistencyRecord>();
            }
        }

        public async Task<IEnumerable<DataInconsistencyRecord>> GetRecordsByRuleNameAsync(string ruleName)
        {
            try
            {
                return await _context.Set<DataInconsistencyRecord>()
                    .Where(r => r.RuleName == ruleName)
                    .OrderByDescending(r => r.DetectedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"ルール別レコード取得エラー: {ex.Message}", 
                    nameof(GetRecordsByRuleNameAsync), nameof(DataInconsistencyRecordService));
                Logger.LogError(ex, nameof(GetRecordsByRuleNameAsync), nameof(DataInconsistencyRecordService));
                return Enumerable.Empty<DataInconsistencyRecord>();
            }
        }

        public async Task<IEnumerable<DataInconsistencyRecord>> GetRecordsBySeverityAsync(AuditSeverity severity)
        {
            try
            {
                return await _context.Set<DataInconsistencyRecord>()
                    .Where(r => r.Severity == severity.ToString())
                    .OrderByDescending(r => r.DetectedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"重要度別レコード取得エラー: {ex.Message}", 
                    nameof(GetRecordsBySeverityAsync), nameof(DataInconsistencyRecordService));
                Logger.LogError(ex, nameof(GetRecordsBySeverityAsync), nameof(DataInconsistencyRecordService));
                return Enumerable.Empty<DataInconsistencyRecord>();
            }
        }

        public async Task<int> GetUnresolvedCountAsync()
        {
            try
            {
                return await _context.Set<DataInconsistencyRecord>()
                    .CountAsync(r => !r.IsResolved);
            }
            catch (Exception ex)
            {
                Logger.LogError($"未解決カウント取得エラー: {ex.Message}", 
                    nameof(GetUnresolvedCountAsync), nameof(DataInconsistencyRecordService));
                Logger.LogError(ex, nameof(GetUnresolvedCountAsync), nameof(DataInconsistencyRecordService));
                return 0;
            }
        }

        public async Task CleanupOldResolvedRecordsAsync(int daysOld = 30)
        {
            try
            {
                var cutoffDate = DateTime.Now.AddDays(-daysOld);
                var oldRecords = await _context.Set<DataInconsistencyRecord>()
                    .Where(r => r.IsResolved && r.ResolvedAt < cutoffDate)
                    .ToListAsync();

                if (oldRecords.Any())
                {
                    foreach (var record in oldRecords)
                    {
                        await DeleteAsync(record.Id);
                    }

                    Logger.LogInfo($"古い解決済みレコード {oldRecords.Count}件を削除", 
                        nameof(CleanupOldResolvedRecordsAsync), nameof(DataInconsistencyRecordService));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"古いレコードクリーンアップエラー: {ex.Message}", 
                    nameof(CleanupOldResolvedRecordsAsync), nameof(DataInconsistencyRecordService));
                Logger.LogError(ex, nameof(CleanupOldResolvedRecordsAsync), nameof(DataInconsistencyRecordService));
            }
        }
    }
}