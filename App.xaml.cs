using System.Configuration;
using System.Data;
using System.Windows;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.ViewModels;
using WpfScheduledApp20250729.Views;
using WpfScheduledApp20250729.Models.Context;
using WpfScheduledApp20250729.Utils;

namespace WpfScheduledApp20250729
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                Logger.LogWithContext("アプリケーション開始", Logger.LogLevel.Info);
                
                // WindowServiceを作成
                var windowService = new WindowService();
                
                // DbContextとServicesを作成(ここのＤＢを切り替えるだけで本番と開発環境を切り替え可能)
                var dbContext = new DevelopmentContext();
                var architectureService = new ArchitectureService(dbContext);
                var projectService = new ProjectService(dbContext);
                var howToLearnService = new HowToLearnService(dbContext);
                var periodicallyCycleService = new PeriodicallyCycleService(dbContext);
                var highTaskService = new HighTaskService(dbContext);

                // 初期データ作成サービス
                var dataSeeder = new DataSeederService(
                    dbContext, 
                    architectureService, 
                    projectService, 
                    howToLearnService, 
                    periodicallyCycleService);

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
                var mainViewModel = new ReadTasksViewModel(windowService, highTaskService, architectureService, projectService);

                mainWindow.DataContext = mainViewModel;
                mainWindow.Show();
                MainWindow = mainWindow;
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

    }

}
