using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfScheduledApp20250729.Models.Context;

namespace WpfScheduledApp20250729.Views
{
    /// <summary>
    /// ReadTasksWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ReadTasksWindow : Window
    {
        public ReadTasksWindow()
        {
            InitializeComponent();
            // 非同期でDB初期化
            _ = InitializeDatabaseAsync();
        }
        // 非同期でDB初期化
        private async Task InitializeDatabaseAsync()
        {
            try
            {
                using (var context = new ProductionContext())
                {
                    await context.Database.EnsureCreatedAsync();
                }
                using (var context = new DevelopmentContext())
                {
                    await context.Database.EnsureCreatedAsync();
                }
                using (var context = new WebPageContext())
                {
                    await context.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("DB初期化ex: " + ex);
                MessageBox.Show($"DB初期化エラー: {ex.Message}");
            }
        }
    }
}



