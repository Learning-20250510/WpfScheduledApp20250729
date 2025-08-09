using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfScheduledApp20250729.Interfaces
{
    public interface IWindowService
    {
        // 汎用メソッド
        void ShowWindow<TWindow, TViewModel>()
            where TWindow : Window, new()
            where TViewModel : class, new();

        void ShowWindow<TViewModel>()
            where TViewModel : class, new();

        bool? ShowDialog<TWindow, TViewModel>()
            where TWindow : Window, new()
            where TViewModel : class, new();

        // 特定用途メソッド（オプション）
        void ShowAddTaskWindow();
        void ShowUpdateTaskWindow(int taskId);
        void ShowUpdateTaskWindow(object taskEntity);
        bool? ShowAddTaskDialog();
        bool? ShowUpdateTaskDialog(int taskId);
    }
}
