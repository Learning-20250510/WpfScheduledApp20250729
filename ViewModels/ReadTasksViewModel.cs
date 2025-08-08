using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfScheduledApp20250729;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Views;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.Models.Entities;

namespace WpfScheduledApp20250729.ViewModels
{
    class ReadTasksViewModel : NotificationObject
    {
        private readonly IWindowService _windowService;
        private readonly HighTaskService _highTaskService;
        private readonly ArchitectureService _architectureService;

        public ReadTasksViewModel(IWindowService windowService, HighTaskService highTaskService, ArchitectureService architectureService)
        {
            _windowService = windowService;
            _highTaskService = highTaskService;
            _architectureService = architectureService;
        }

        private DelegateCommand? _addTaskCommand;
        public DelegateCommand AddTaskCommand
        {
            get
            {
                return this._addTaskCommand ?? (this._addTaskCommand = new DelegateCommand(
                async _ =>
                {
                    try
                    {
                        // 1. まずArchitectureを作成または取得
                        var architecture = await _architectureService.GetOrCreateDefaultArchitectureAsync();
                        
                        // 2. HighTaskを作成（有効なArchitectureIdを使用）
                        var newTask = new HighTask
                        {
                            TaskName = "テストタスク",
                            Description = "ボタンクリックで追加されたタスク",
                            ArchitectureId = architecture.Id,  // 有効なArchitectureIdを設定
                            ProjectId = 1,  // TODO: ProjectServiceも作成して対応
                            ClearTimesInTime = 0,
                            ClearTimesOutofTime = 0,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            TouchedAt = DateTime.UtcNow,
                            LastUpdMethodName = "AddTaskCommand"
                        };

                        await _highTaskService.AddHighTaskAsync(newTask);
                        // 成功時の処理
                        System.Diagnostics.Debug.WriteLine("HighTask追加成功");
                    }
                    catch (Exception ex)
                    {
                        // 詳細なエラー情報を出力
                        System.Diagnostics.Debug.WriteLine($"=== INSERT ERROR ===");
                        System.Diagnostics.Debug.WriteLine($"Message: {ex.Message}");
                        System.Diagnostics.Debug.WriteLine($"InnerException: {ex.InnerException?.Message}");
                        System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                        System.Diagnostics.Debug.WriteLine($"ex: {ex}");

                        
                        // MessageBoxでもエラーを表示
                        MessageBox.Show($"エラーが発生しました:\n{ex.Message}\n\nInnerException:\n{ex.InnerException?.Message}", 
                                      "データベースエラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    _windowService.ShowAddTaskWindow();
                },
                _ =>
                {
                    return true;
                }
                ));
            }
        }
    }
}
