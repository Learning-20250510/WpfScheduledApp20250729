using System.Configuration;
using System.Data;
using System.Windows;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.ViewModels;

namespace WpfScheduledApp20250729
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // WindowServiceを作成
            var windowService = new WindowService();

            // MainWindowとViewModelを作成
            var mainWindow = new MainWindow();
            var mainViewModel = new ReadTasksViewModel(windowService);

            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
            MainWindow = mainWindow;

        }

    }

}
