using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Views;
using WpfScheduledApp20250729.ViewModels;
using System.Windows;

namespace WpfScheduledApp20250729.Services
{
    class WindowService : IWindowService
    {
        // 汎用実装
        public void ShowWindow<TWindow, TViewModel>()
            where TWindow : Window, new()
            where TViewModel : class, new()
        {
            var window = new TWindow();
            var viewModel = new TViewModel();
            window.DataContext = viewModel;
            window.Show();
        }

        public void ShowWindow<TViewModel>()
            where TViewModel : class, new()
        {
            // ViewModelの型に基づいて適切なWindowを決定
            Window? window = null;
            var viewModel = new TViewModel();

            if (typeof(TViewModel) == typeof(ActionTaskViewModel))
            {
                window = new ActionTaskWindow();
            }
            else if (typeof(TViewModel) == typeof(AddTaskViewModel))
            {
                window = new AddTaskWindow();
            }
            else if (typeof(TViewModel) == typeof(UpdateTaskViewModel))
            {
                window = new UpdateTaskWindow();
            }

            if (window != null)
            {
                window.DataContext = viewModel;
                window.Show();
            }
        }

        public bool? ShowDialog<TWindow, TViewModel>()
            where TWindow : Window, new()
            where TViewModel : class, new()
        {
            var window = new TWindow();
            var viewModel = new TViewModel();
            window.DataContext = viewModel;
            return window.ShowDialog();
        }

        // 特定用途実装（パラメータが必要な場合）
        public void ShowAddTaskWindow()
        {
            ShowWindow<AddTaskWindow, AddTaskViewModel>();
        }

        public void ShowActionTaskWindow()
        {
            ShowWindow<ActionTaskWindow, ActionTaskViewModel>();
        }

        public void ShowUpdateTaskWindow(int taskId)
        {
            var window = new UpdateTaskWindow();
            var viewModel = new UpdateTaskViewModel(taskId); // パラメータ付き
            window.DataContext = viewModel;
            window.Show();
        }

        public void ShowUpdateTaskWindow(object taskEntity)
        {
            var window = new UpdateTaskWindow();
            var viewModel = new UpdateTaskViewModel(taskEntity); // オブジェクト型パラメータ付き
            window.DataContext = viewModel;
            window.Show();
        }

        public bool? ShowAddTaskDialog()
        {
            return ShowDialog<AddTaskWindow, AddTaskViewModel>();
        }

        public bool? ShowUpdateTaskDialog(int taskId)
        {
            var window = new UpdateTaskWindow();
            var viewModel = new UpdateTaskViewModel(taskId);
            window.DataContext = viewModel;
            return window.ShowDialog();
        }
    }
}
