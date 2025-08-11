using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfScheduledApp20250729;
using WpfScheduledApp20250729.Services;
using System.Windows;

namespace WpfScheduledApp20250729.ViewModels
{
    internal class AddTaskViewModel : NotificationObject
    {
        private readonly DataSeederService _dataSeederService;

        public AddTaskViewModel(DataSeederService dataSeederService)
        {
            _dataSeederService = dataSeederService;
        }

        public ICommand AutoInsertMMFilesCommand => new DelegateCommand(async () => await AutoInsertMMFilesAsync());
        public ICommand SynchronizeMMFilesCommand => new DelegateCommand(async () => await SynchronizeMMFilesAsync());

        private async Task AutoInsertMMFilesAsync()
        {
            try
            {
                var insertedCount = await _dataSeederService.AutoInsertMMFilesAsync();
                
                MessageBox.Show($"MMフォルダから {insertedCount} 個のタスクを自動作成しました。",
                    "MM自動作成完了", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MM自動作成エラー: {ex.Message}",
                    "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SynchronizeMMFilesAsync()
        {
            try
            {
                var (insertedCount, deletedCount) = await _dataSeederService.SynchronizeMMFilesAsync();
                
                MessageBox.Show($"MMファイル同期完了:\n" +
                    $"• 追加されたタスク: {insertedCount} 個\n" +
                    $"• 削除されたタスク: {deletedCount} 個",
                    "MM同期完了", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MM同期エラー: {ex.Message}",
                    "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
