using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Interfaces;
using WpfScheduledApp20250729.Auditing.Models;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.Utils;

namespace WpfScheduledApp20250729.Auditing.Services
{
    public class AuditService : IAuditService, IDisposable
    {
        private readonly DbContext _context;
        private readonly ConcurrentDictionary<string, IAuditRule> _rules;
        private Timer? _timer;
        private TimeSpan _interval = TimeSpan.FromMinutes(30); // デフォルト30分間隔
        private bool _isRunning = false;
        private readonly DataInconsistencyRecordService? _inconsistencyService;

        public event EventHandler<IEnumerable<AuditResult>>? AuditCompleted;

        public AuditService(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _rules = new ConcurrentDictionary<string, IAuditRule>();

            // DataInconsistencyRecordServiceを初期化（BaseDbContextの場合のみ）
            if (context is BaseDbContext baseContext)
            {
                _inconsistencyService = new DataInconsistencyRecordService(baseContext);
            }

            Logger.LogInfo("AuditService initialized", nameof(AuditService), nameof(AuditService));
        }

        public Task StartAsync()
        {
            if (_isRunning)
            {
                Logger.LogWarning("AuditService is already running", nameof(StartAsync), nameof(AuditService));
                return Task.CompletedTask;
            }

            _isRunning = true;
            Logger.LogInfo($"Starting AuditService with interval: {_interval}", nameof(StartAsync), nameof(AuditService));

            // 即座に一度実行
            _ = Task.Run(async () => await ExecuteAuditAsync());

            // 定期実行タイマーを設定
            _timer = new Timer(async _ => await ExecuteAuditAsync(), null, _interval, _interval);
            
            return Task.CompletedTask;
        }

        public Task StopAsync()
        {
            if (!_isRunning)
            {
                Logger.LogWarning("AuditService is not running", nameof(StopAsync), nameof(AuditService));
                return Task.CompletedTask;
            }

            _isRunning = false;
            _timer?.Dispose();
            _timer = null;
            
            Logger.LogInfo("AuditService stopped", nameof(StopAsync), nameof(AuditService));
            return Task.CompletedTask;
        }

        public void AddRule(IAuditRule rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            _rules.AddOrUpdate(rule.Name, rule, (key, oldRule) => rule);
            Logger.LogInfo($"Audit rule added: {rule.Name}", nameof(AddRule), nameof(AuditService));
        }

        public void RemoveRule(string ruleName)
        {
            if (string.IsNullOrWhiteSpace(ruleName))
                return;

            if (_rules.TryRemove(ruleName, out var removedRule))
            {
                Logger.LogInfo($"Audit rule removed: {ruleName}", nameof(RemoveRule), nameof(AuditService));
            }
            else
            {
                Logger.LogWarning($"Audit rule not found for removal: {ruleName}", nameof(RemoveRule), nameof(AuditService));
            }
        }

        public IEnumerable<IAuditRule> GetRules()
        {
            return _rules.Values.ToList();
        }

        public void SetInterval(TimeSpan interval)
        {
            if (interval <= TimeSpan.Zero)
                throw new ArgumentException("Interval must be positive", nameof(interval));

            var oldInterval = _interval;
            _interval = interval;
            
            Logger.LogInfo($"Audit interval changed from {oldInterval} to {_interval}", nameof(SetInterval), nameof(AuditService));

            // 実行中の場合、タイマーを再設定
            if (_isRunning && _timer != null)
            {
                _timer.Dispose();
                _timer = new Timer(async _ => await ExecuteAuditAsync(), null, _interval, _interval);
            }
        }

        private async Task ExecuteAuditAsync()
        {
            if (!_rules.Any())
            {
                Logger.LogInfo("No audit rules configured, skipping audit", nameof(ExecuteAuditAsync), nameof(AuditService));
                return;
            }

            try
            {
                Logger.LogInfo($"Starting audit execution with {_rules.Count} rules", nameof(ExecuteAuditAsync), nameof(AuditService));
                var allResults = new List<AuditResult>();

                foreach (var rule in _rules.Values)
                {
                    try
                    {
                        var results = await rule.ExecuteAsync(_context);
                        if (results != null && results.Any())
                        {
                            allResults.AddRange(results);
                            Logger.LogInfo($"Rule '{rule.Name}' found {results.Count()} issues", nameof(ExecuteAuditAsync), nameof(AuditService));
                        }
                        else
                        {
                            Logger.LogInfo($"Rule '{rule.Name}' found no issues", nameof(ExecuteAuditAsync), nameof(AuditService));
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError($"Error executing audit rule '{rule?.Name ?? "Unknown"}': {ex.Message}", nameof(ExecuteAuditAsync), nameof(AuditService));
                        
                        // エラー自体を監査結果として記録
                        allResults.Add(new AuditResult
                        {
                            RuleName = rule?.Name ?? "Unknown",
                            EntityType = "System",
                            EntityId = "N/A",
                            Description = $"Audit rule execution failed: {ex.Message}",
                            Severity = AuditSeverity.Critical,
                            DetectedAt = DateTime.UtcNow,
                            Details = ex.ToString()
                        });
                    }
                }

                // 結果をログ出力
                LogAuditResults(allResults);

                // 不整合レコードをデータベースに保存
                await SaveInconsistenciesToDatabaseAsync(allResults);

                // イベントを発火
                AuditCompleted?.Invoke(this, allResults);

                Logger.LogInfo($"Audit execution completed. Total issues found: {allResults.Count}", nameof(ExecuteAuditAsync), nameof(AuditService));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Critical error in audit execution: {ex.Message}", nameof(ExecuteAuditAsync), nameof(AuditService));
                Logger.LogError(ex, nameof(ExecuteAuditAsync), nameof(AuditService));
            }
        }

        private void LogAuditResults(IEnumerable<AuditResult> results)
        {
            if (!results.Any())
                return;

            Logger.LogInfo("=== AUDIT RESULTS ===", nameof(LogAuditResults), nameof(AuditService));
            
            var groupedResults = results.GroupBy(r => r.Severity).OrderByDescending(g => g.Key);
            
            foreach (var group in groupedResults)
            {
                var logLevel = group.Key switch
                {
                    AuditSeverity.Critical => Logger.LogLevel.Error,
                    AuditSeverity.Error => Logger.LogLevel.Error,
                    AuditSeverity.Warning => Logger.LogLevel.Warning,
                    _ => Logger.LogLevel.Info
                };

                foreach (var result in group)
                {
                    var message = $"[{result.Severity}] {result.RuleName}: {result.Description} " +
                                $"(Entity: {result.EntityType}#{result.EntityId})";
                    
                    Logger.Log(message, logLevel, nameof(LogAuditResults), nameof(AuditService));
                    
                    if (!string.IsNullOrEmpty(result.Details))
                    {
                        Logger.Log($"Details: {result.Details}", logLevel, nameof(LogAuditResults), nameof(AuditService));
                    }
                }
            }
            
            Logger.LogInfo("=== END AUDIT RESULTS ===", nameof(LogAuditResults), nameof(AuditService));
        }

        private async Task SaveInconsistenciesToDatabaseAsync(IEnumerable<AuditResult> results)
        {
            if (_inconsistencyService == null || !results.Any())
            {
                Logger.LogInfo("不整合レコード保存をスキップ: サービスが利用できないか結果が空です", 
                    nameof(SaveInconsistenciesToDatabaseAsync), nameof(AuditService));
                return;
            }

            try
            {
                Logger.LogInfo($"不整合レコードをデータベースに保存中: {results.Count()}件", 
                    nameof(SaveInconsistenciesToDatabaseAsync), nameof(AuditService));

                // バッチで不整合レコードを保存
                var savedRecords = await _inconsistencyService.CreateBatchFromAuditResultsAsync(results);

                Logger.LogInfo($"不整合レコード保存完了: {savedRecords.Count()}件", 
                    nameof(SaveInconsistenciesToDatabaseAsync), nameof(AuditService));

                // 重要度別の統計をログ出力
                var statisticsByGroup = savedRecords.GroupBy(r => r.Severity)
                    .Select(g => new { Severity = g.Key, Count = g.Count() })
                    .OrderByDescending(s => s.Severity);

                foreach (var stat in statisticsByGroup)
                {
                    Logger.LogInfo($"  - {stat.Severity}: {stat.Count}件", 
                        nameof(SaveInconsistenciesToDatabaseAsync), nameof(AuditService));
                }

                // 未解決レコード総数をログ出力
                var unresolvedCount = await _inconsistencyService.GetUnresolvedCountAsync();
                Logger.LogInfo($"現在の未解決不整合レコード総数: {unresolvedCount}件", 
                    nameof(SaveInconsistenciesToDatabaseAsync), nameof(AuditService));
            }
            catch (Exception ex)
            {
                Logger.LogError($"不整合レコード保存エラー: {ex.Message}", 
                    nameof(SaveInconsistenciesToDatabaseAsync), nameof(AuditService));
                Logger.LogError(ex, nameof(SaveInconsistenciesToDatabaseAsync), nameof(AuditService));
                
                // データベース保存に失敗してもログ出力は続行
            }
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _isRunning = false;
        }
    }
}