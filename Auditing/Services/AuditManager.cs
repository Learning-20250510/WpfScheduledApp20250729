using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Auditing.Interfaces;
using WpfScheduledApp20250729.Auditing.Models;
using WpfScheduledApp20250729.Auditing.Rules;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.Utils;

namespace WpfScheduledApp20250729.Auditing.Services
{
    public class AuditManager : IDisposable
    {
        private readonly IAuditService _auditService;
        private readonly IAuditRuleFactory _ruleFactory;
        private readonly DataSeederService? _dataSeederService;
        private bool _disposed = false;

        public AuditManager(DbContext dbContext, DataSeederService? dataSeederService = null)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));

            _auditService = new AuditService(dbContext);
            _ruleFactory = new AuditRuleFactory();
            _dataSeederService = dataSeederService;

            // イベントハンドラーを設定
            _auditService.AuditCompleted += OnAuditCompleted;
        }

        public Task InitializeAsync()
        {
            try
            {
                Logger.LogInfo("初期化中...", nameof(InitializeAsync), nameof(AuditManager));

                // 組み込みルールを追加
                var builtInRules = _ruleFactory.GetBuiltInRules();
                foreach (var rule in builtInRules)
                {
                    _auditService.AddRule(rule);
                    Logger.LogInfo($"監査ルールを追加: {rule.Name}", nameof(InitializeAsync), nameof(AuditManager));
                }

                // MMファイル同期ルールを追加（DataSeederServiceが利用可能な場合）
                if (_dataSeederService != null)
                {
                    var mmSyncRule = new MMFileSynchronizationRule(_dataSeederService);
                    _auditService.AddRule(mmSyncRule);
                    Logger.LogInfo($"MMファイル同期ルールを追加: {mmSyncRule.Name}", nameof(InitializeAsync), nameof(AuditManager));
                }

                // 監査間隔を設定（デフォルト30分）
                _auditService.SetInterval(TimeSpan.FromMinutes(30));

                Logger.LogInfo("AuditManager初期化完了", nameof(InitializeAsync), nameof(AuditManager));
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Logger.LogError($"AuditManager初期化エラー: {ex.Message}", nameof(InitializeAsync), nameof(AuditManager));
                Logger.LogError(ex, nameof(InitializeAsync), nameof(AuditManager));
                throw;
            }
        }

        public async Task StartAsync()
        {
            try
            {
                Logger.LogInfo("監査サービス開始", nameof(StartAsync), nameof(AuditManager));
                await _auditService.StartAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"監査サービス開始エラー: {ex.Message}", nameof(StartAsync), nameof(AuditManager));
                Logger.LogError(ex, nameof(StartAsync), nameof(AuditManager));
            }
        }

        public async Task StopAsync()
        {
            try
            {
                Logger.LogInfo("監査サービス停止", nameof(StopAsync), nameof(AuditManager));
                await _auditService.StopAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError($"監査サービス停止エラー: {ex.Message}", nameof(StopAsync), nameof(AuditManager));
                Logger.LogError(ex, nameof(StopAsync), nameof(AuditManager));
            }
        }

        public void AddCustomRule(string name, string description, Func<DbContext, Task<IEnumerable<AuditResult>>> ruleLogic)
        {
            try
            {
                var customRule = _ruleFactory.CreateRule(name, description, ruleLogic);
                _auditService.AddRule(customRule);
                Logger.LogInfo($"カスタム監査ルールを追加: {name}", nameof(AddCustomRule), nameof(AuditManager));
            }
            catch (Exception ex)
            {
                Logger.LogError($"カスタムルール追加エラー ({name}): {ex.Message}", nameof(AddCustomRule), nameof(AuditManager));
                Logger.LogError(ex, nameof(AddCustomRule), nameof(AuditManager));
            }
        }

        public void RemoveRule(string ruleName)
        {
            try
            {
                _auditService.RemoveRule(ruleName);
                Logger.LogInfo($"監査ルールを削除: {ruleName}", nameof(RemoveRule), nameof(AuditManager));
            }
            catch (Exception ex)
            {
                Logger.LogError($"ルール削除エラー ({ruleName}): {ex.Message}", nameof(RemoveRule), nameof(AuditManager));
                Logger.LogError(ex, nameof(RemoveRule), nameof(AuditManager));
            }
        }

        public void SetInterval(TimeSpan interval)
        {
            try
            {
                _auditService.SetInterval(interval);
                Logger.LogInfo($"監査間隔を変更: {interval}", nameof(SetInterval), nameof(AuditManager));
            }
            catch (Exception ex)
            {
                Logger.LogError($"間隔変更エラー: {ex.Message}", nameof(SetInterval), nameof(AuditManager));
                Logger.LogError(ex, nameof(SetInterval), nameof(AuditManager));
            }
        }

        public IEnumerable<IAuditRule> GetCurrentRules()
        {
            return _auditService.GetRules();
        }

        private void OnAuditCompleted(object? sender, IEnumerable<AuditResult> results)
        {
            // 監査完了時の処理
            // すでにAuditServiceでログ出力されているため、追加処理が必要な場合はここで実装
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    Logger.LogInfo("AuditManagerをシャットダウンしています...", nameof(Dispose), nameof(AuditManager));
                    
                    // イベントハンドラーを解除
                    if (_auditService != null)
                    {
                        _auditService.AuditCompleted -= OnAuditCompleted;
                    }

                    // 監査サービスを停止
                    _auditService?.StopAsync().GetAwaiter().GetResult();

                    // AuditServiceがIDisposableを実装している場合
                    if (_auditService is IDisposable disposableService)
                    {
                        disposableService.Dispose();
                    }

                    Logger.LogInfo("AuditManagerシャットダウン完了", nameof(Dispose), nameof(AuditManager));
                }
                catch (Exception ex)
                {
                    Logger.LogError($"AuditManagerシャットダウンエラー: {ex.Message}", nameof(Dispose), nameof(AuditManager));
                }
                finally
                {
                    _disposed = true;
                }
            }
        }
    }
}