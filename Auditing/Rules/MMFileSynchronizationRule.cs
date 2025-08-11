using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Interfaces;
using WpfScheduledApp20250729.Auditing.Models;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.Utils;

namespace WpfScheduledApp20250729.Auditing.Rules
{
    public class MMFileSynchronizationRule : IAuditRule
    {
        private readonly DataSeederService _dataSeederService;

        public string Name => "MMFileSynchronization";
        public string Description => "MMフォルダとHighTaskテーブルの整合性を保つための自動同期";
        public bool IsEnabled { get; set; } = true;

        public MMFileSynchronizationRule(DataSeederService dataSeederService)
        {
            _dataSeederService = dataSeederService ?? throw new ArgumentNullException(nameof(dataSeederService));
        }

        public async Task<IEnumerable<AuditResult>> ExecuteAsync()
        {
            var results = new List<AuditResult>();

            try
            {
                Logger.LogInfo("MMファイル同期ルール実行開始", nameof(ExecuteAsync), nameof(MMFileSynchronizationRule));

                // MMファイル同期を実行
                var (insertedCount, deletedCount) = await _dataSeederService.SynchronizeMMFilesAsync();

                // 同期結果に基づいて監査結果を作成
                if (insertedCount > 0 || deletedCount > 0)
                {
                    var auditResult = new AuditResult
                    {
                        RuleName = Name,
                        EntityType = "HighTask",
                        EntityId = "MMSync",
                        Description = $"MMファイル同期実行: {insertedCount}個追加, {deletedCount}個削除",
                        Severity = AuditSeverity.Info,
                        Details = $"同期詳細:\n" +
                                $"- 追加されたタスク: {insertedCount}個\n" +
                                $"- 削除されたタスク: {deletedCount}個\n" +
                                $"- 実行時刻: {DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                        DetectedAt = DateTime.Now
                    };

                    results.Add(auditResult);
                    Logger.LogInfo($"MMファイル同期完了: {insertedCount}個追加, {deletedCount}個削除", 
                        nameof(ExecuteAsync), nameof(MMFileSynchronizationRule));
                }
                else
                {
                    Logger.LogInfo("MMファイル同期完了: 変更なし", nameof(ExecuteAsync), nameof(MMFileSynchronizationRule));
                }
            }
            catch (Exception ex)
            {
                var errorResult = new AuditResult
                {
                    RuleName = Name,
                    EntityType = "HighTask",
                    EntityId = "MMSyncError",
                    Description = "MMファイル同期エラー",
                    Severity = AuditSeverity.Error,
                    Details = $"エラー詳細: {ex.Message}\nStackTrace: {ex.StackTrace}",
                    DetectedAt = DateTime.Now
                };

                results.Add(errorResult);
                Logger.LogError($"MMファイル同期エラー: {ex.Message}", nameof(ExecuteAsync), nameof(MMFileSynchronizationRule));
                Logger.LogError(ex, nameof(ExecuteAsync), nameof(MMFileSynchronizationRule));
            }

            return results;
        }
    }
}