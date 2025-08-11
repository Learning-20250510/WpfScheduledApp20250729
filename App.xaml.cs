using System.Configuration;
using System.Data;
using System.Windows;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.ViewModels;
using WpfScheduledApp20250729.Views;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Utils;
using WpfScheduledApp20250729.Auditing.Services;
using Npgsql;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace WpfScheduledApp20250729
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AuditManager? _auditManager;
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                Logger.LogWithContext("アプリケーション開始", Logger.LogLevel.Info);
                
                // データベースの自動作成
                await EnsureDatabasesExistAsync();
                
                // WindowServiceを作成
                var windowService = new WindowService();
                
                // DbContextとServicesを作成(ここのＤＢを切り替えるだけで本番と開発環境を切り替え可能)
                var dbContext = new DevelopmentContext();
                
                // データベーステーブルの自動作成（マイグレーション）
                await EnsureTablesExistAsync(dbContext);
                var architectureService = new ArchitectureService(dbContext);
                var projectService = new ProjectService(dbContext);
                var howToLearnService = new HowToLearnService(dbContext);
                var periodicallyCycleService = new PeriodicallyCycleService(dbContext);
                var relationExtensionAppService = new RelationExtensionAppService(dbContext);
                var highTaskService = new HighTaskService(dbContext);
                var middleTaskService = new MiddleTaskService(dbContext);
                var lowTaskService = new LowTaskService(dbContext);

                // 初期データ作成サービス
                var dataSeeder = new DataSeederService(
                    dbContext, 
                    architectureService, 
                    projectService, 
                    howToLearnService, 
                    periodicallyCycleService,
                    relationExtensionAppService);

                // データベース接続確認と初期データ作成
                if (await dataSeeder.CanConnectToDatabaseAsync())
                {
                    Logger.LogWithContext("データベース接続成功", Logger.LogLevel.Info);
                    await dataSeeder.SeedInitialDataAsync();
                    Logger.LogWithContext("初期データ作成完了", Logger.LogLevel.Info);
                }
                else
                {
                    Logger.LogWithContext("データベース接続失敗", Logger.LogLevel.Error);
                    System.Windows.MessageBox.Show(
                        "データベースに接続できません。接続設定を確認してください。", 
                        "データベースエラー", 
                        System.Windows.MessageBoxButton.OK, 
                        System.Windows.MessageBoxImage.Error);
                    Shutdown();
                    return;
                }

                // MainWindowとViewModelを作成
                var mainWindow = new ReadTasksWindow();
                var mainViewModel = new ReadTasksViewModel(windowService, highTaskService, middleTaskService, lowTaskService, architectureService, projectService);

                mainWindow.DataContext = mainViewModel;
                mainWindow.Show();
                MainWindow = mainWindow;

                // 監査システムを初期化して開始
                await InitializeAuditSystemAsync(dbContext);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "OnStartup", "App");
                System.Windows.MessageBox.Show(
                    $"アプリケーション初期化エラー:\n{ex.Message}", 
                    "起動エラー", 
                    System.Windows.MessageBoxButton.OK, 
                    System.Windows.MessageBoxImage.Error);
                Shutdown();
            }
        }

        private async Task EnsureDatabasesExistAsync()
        {
            try
            {
                Logger.LogWithContext("データベース存在確認・作成開始", Logger.LogLevel.Info);

                // appsettings.jsonから設定を読み取り
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
                
                var databases = new[]
                {
                    ("Development", configuration.GetConnectionString("DevelopmentConnection")),
                    ("Production", configuration.GetConnectionString("ProductionConnection")),
                    ("WebPage", configuration.GetConnectionString("WebPageConnection"))
                };

                foreach (var (name, connectionString) in databases)
                {
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        Logger.LogWithContext($"{name}の接続文字列が設定されていません", Logger.LogLevel.Error);
                        throw new InvalidOperationException($"{name}の接続文字列が設定されていません");
                    }
                    
                    await CreateDatabaseIfNotExistsAsync(name, connectionString);
                }

                Logger.LogWithContext("全データベースの存在確認・作成完了", Logger.LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "EnsureDatabasesExistAsync", "App");
                throw; // 重要なエラーなので再スロー
            }
        }

        private async Task CreateDatabaseIfNotExistsAsync(string dbName, string connectionString)
        {
            try
            {
                Logger.LogWithContext($"{dbName}データベースの確認開始", Logger.LogLevel.Info);

                // 接続文字列からデータベース名を抽出
                var builder = new NpgsqlConnectionStringBuilder(connectionString);
                var databaseName = builder.Database;
                
                if (string.IsNullOrEmpty(databaseName))
                {
                    Logger.LogWithContext($"{dbName}の接続文字列にデータベース名が含まれていません", Logger.LogLevel.Error);
                    throw new InvalidOperationException($"{dbName}の接続文字列にデータベース名が含まれていません");
                }
                
                var masterConnectionString = connectionString.Replace($"Database={databaseName}", "Database=postgres");

                using var masterConnection = new NpgsqlConnection(masterConnectionString);
                await masterConnection.OpenAsync();

                // データベースが存在するかチェック
                var checkDbSql = "SELECT 1 FROM pg_database WHERE datname = @dbName";
                using var checkCommand = new NpgsqlCommand(checkDbSql, masterConnection);
                checkCommand.Parameters.AddWithValue("dbName", databaseName);

                var exists = await checkCommand.ExecuteScalarAsync() != null;

                if (!exists)
                {
                    Logger.LogWithContext($"{dbName}データベースが存在しないため作成します", Logger.LogLevel.Info);

                    // テンプレートデータベースの照合順序を取得
                    var getCollationSql = "SELECT datcollate, datctype FROM pg_database WHERE datname = 'template1'";
                    using var collationCommand = new NpgsqlCommand(getCollationSql, masterConnection);
                    using var reader = await collationCommand.ExecuteReaderAsync();
                    
                    string collate = "C";
                    string ctype = "C";
                    
                    if (await reader.ReadAsync())
                    {
                        collate = reader.GetString("datcollate");
                        ctype = reader.GetString("datctype");
                        Logger.LogWithContext($"テンプレート照合順序: COLLATE={collate}, CTYPE={ctype}", Logger.LogLevel.Info);
                    }
                    reader.Close();

                    // データベースを作成（テンプレートと同じ照合順序を使用）
                    var createDbSql = $@"
                        CREATE DATABASE ""{databaseName}""
                        WITH 
                        OWNER = postgres
                        ENCODING = 'UTF8'
                        LC_COLLATE = '{collate}'
                        LC_CTYPE = '{ctype}'
                        TABLESPACE = pg_default
                        CONNECTION LIMIT = -1
                        IS_TEMPLATE = False";

                    using var createCommand = new NpgsqlCommand(createDbSql, masterConnection);
                    await createCommand.ExecuteNonQueryAsync();

                    Logger.LogWithContext($"{dbName}データベースを作成しました", Logger.LogLevel.Info);
                }
                else
                {
                    Logger.LogWithContext($"{dbName}データベースは既に存在します", Logger.LogLevel.Info);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"CreateDatabaseIfNotExistsAsync({dbName})", "App");
                throw; // データベース作成は必須なので再スロー
            }
        }

        private async Task EnsureTablesExistAsync(BaseDbContext context)
        {
            try
            {
                Logger.LogWithContext("データベーステーブルの作成・確認開始", Logger.LogLevel.Info);

                // データベースが存在し、テーブル構造を作成
                await context.Database.EnsureCreatedAsync();

                Logger.LogWithContext("データベーステーブルの作成・確認完了", Logger.LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "EnsureTablesExistAsync", "App");
                throw;
            }
        }

        private async Task InitializeAuditSystemAsync(BaseDbContext dbContext)
        {
            try
            {
                Logger.LogWithContext("データ不整合監査システム初期化開始", Logger.LogLevel.Info);
                
                _auditManager = new AuditManager(dbContext);
                await _auditManager.InitializeAsync();
                await _auditManager.StartAsync();

                Logger.LogWithContext("データ不整合監査システム初期化完了", Logger.LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "InitializeAuditSystemAsync", "App");
                Logger.LogWithContext("監査システムの初期化に失敗しましたが、アプリケーションは続行されます", Logger.LogLevel.Warning);
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            try
            {
                Logger.LogWithContext("アプリケーション終了処理開始", Logger.LogLevel.Info);
                
                if (_auditManager != null)
                {
                    await _auditManager.StopAsync();
                    _auditManager.Dispose();
                    _auditManager = null;
                    Logger.LogWithContext("監査システム終了完了", Logger.LogLevel.Info);
                }

                Logger.LogWithContext("アプリケーション終了", Logger.LogLevel.Info);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "OnExit", "App");
            }
            finally
            {
                base.OnExit(e);
            }
        }

    }

}
