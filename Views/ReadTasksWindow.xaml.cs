using System;
using System.Windows;
using System.Windows.Interop;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.ViewModels;

namespace WpfScheduledApp20250729.Views
{
    /// <summary>
    /// ReadTasksWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ReadTasksWindow : Window
    {
        private IGlobalHotKeyService _globalHotKeyService;
        private IntPtr _windowHandle;
        private const int WM_HOTKEY = 0x0312;

        public ReadTasksWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
        }

        public ReadTasksWindow(ReadTasksViewModel viewModel, IGlobalHotKeyService globalHotKeyService) : this()
        {
            DataContext = viewModel;
            _globalHotKeyService = globalHotKeyService;
            
            // ViewModelのイベント購読
            viewModel.WindowActivateRequested += OnWindowActivateRequested;
            viewModel.TaskActionRequested += OnTaskActionRequested;
            viewModel.ResultRequested += OnResultRequested;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            
            // ウィンドウハンドルを取得
            var helper = new WindowInteropHelper(this);
            _windowHandle = helper.Handle;
            
            // ホットキーを設定
            if (_globalHotKeyService != null)
            {
                _globalHotKeyService.SetupHotKeys(_windowHandle);
                
                // メッセージフックを追加
                HwndSource source = HwndSource.FromHwnd(_windowHandle);
                source.AddHook(WndProc);
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_HOTKEY)
            {
                _globalHotKeyService?.ProcessHotKeyMessage(wParam);
                handled = true;
            }
            return IntPtr.Zero;
        }

        protected override void OnClosed(EventArgs e)
        {
            // ホットキーのクリーンアップ
            if (_globalHotKeyService != null && _windowHandle != IntPtr.Zero)
            {
                _globalHotKeyService.Cleanup(_windowHandle);
            }
            
            base.OnClosed(e);
        }

        private void OnWindowActivateRequested(object sender, EventArgs e)
        {
            Activate();
            Focus();
        }

        private void OnTaskActionRequested(object sender, ReadTasksViewModel.TaskActionRequestedEventArgs e)
        {
            var taskActionWindow = new GamingTaskActionWindow(e.ViewModel);
            taskActionWindow.Show();
        }

        private void OnResultRequested(object sender, ReadTasksViewModel.ResultRequestedEventArgs e)
        {
            var resultWindow = new GamingResultWindow(e.ViewModel);
            resultWindow.Show();
        }
    }
}