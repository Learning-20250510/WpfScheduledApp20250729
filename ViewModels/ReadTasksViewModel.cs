using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Views;

namespace WpfScheduledApp20250729.ViewModels
{
    class ReadTasksViewModel
    {
        private readonly IWindowService _windowService;

        public ReadTasksViewModel(IWindowService windowService)
        {
            _windowService = windowService;
        }

        private void AddTask()
        {
            // 汎用メソッド使用
            _windowService.ShowWindow<AddTaskWindow, AddTaskViewModel>();
        }

        private void UpdateTask(int taskId)
        {
            // パラメータが必要な場合は特定メソッド
            _windowService.ShowUpdateTaskWindow(taskId);
        }
    }
}
