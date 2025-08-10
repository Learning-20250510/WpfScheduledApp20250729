using WpfScheduledApp20250729.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfScheduledApp20250729;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Services;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.ViewModels
{
    class ReadTasksViewModel : NotificationObject
    {
        private readonly IWindowService _windowService;
        private readonly IGlobalHotKeyService _globalHotKeyService;
        private readonly ITaskActionService _taskActionService;
        private readonly IResultService _resultService;
        private readonly HighTaskService _highTaskService;
        private readonly MiddleTaskService _middleTaskService;
        private readonly LowTaskService _lowTaskService;
        private readonly ArchitectureService _architectureService;
        private readonly ProjectService _projectService;

        private ObservableCollection<object> _tasks = new();
        public ObservableCollection<object> Tasks 
        { 
            get => _tasks; 
            set => SetProperty(ref _tasks, value); 
        }

        private UserControl _currentView = new ReadTaskControl();
        public UserControl CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        private bool _letsStartCheckBox = false;
        public bool LetsStartCheckBox
        {
            get => _letsStartCheckBox;
            set => SetProperty(ref _letsStartCheckBox, value);
        }

        public DelegateCommand SearchCommand { get; }
        public DelegateCommand HighTaskCommand { get; }
        public DelegateCommand MiddleTaskCommand { get; }
        public DelegateCommand LowTaskCommand { get; }
        public DelegateCommand AddTaskCommand { get; }
        public DelegateCommand ActionTaskCommand { get; }
        public DelegateCommand AppSettingsCommand { get; }
        public DelegateCommand DatabaseSettingsCommand { get; }

        public ReadTasksViewModel(IWindowService windowService, IGlobalHotKeyService globalHotKeyService, ITaskActionService taskActionService, IResultService resultService, HighTaskService highTaskService, MiddleTaskService middleTaskService, LowTaskService lowTaskService, ArchitectureService architectureService, ProjectService projectService)
        {
            _windowService = windowService;
            _globalHotKeyService = globalHotKeyService;
            _taskActionService = taskActionService;
            _resultService = resultService;
            _highTaskService = highTaskService;
            _middleTaskService = middleTaskService;
            _lowTaskService = lowTaskService;
            _architectureService = architectureService;
            _projectService = projectService;

            // ホットキーイベントの購読
            _globalHotKeyService.HotKeyPressed += OnHotKeyPressed;

            // コマンドの初期化
            SearchCommand = new DelegateCommand(_ => ExecuteSearch(), _ => true);
            HighTaskCommand = new DelegateCommand(_ => ShowHighTasks(), _ => true);
            MiddleTaskCommand = new DelegateCommand(_ => ShowMiddleTasks(), _ => true);
            LowTaskCommand = new DelegateCommand(_ => ShowLowTasks(), _ => true);
            AddTaskCommand = new DelegateCommand(_ => ShowTaskAction(), _ => true);
            ActionTaskCommand = new DelegateCommand(_ => ShowTaskAction(), _ => true);
            AppSettingsCommand = new DelegateCommand(_ => ShowAppSettings(), _ => true);
            DatabaseSettingsCommand = new DelegateCommand(_ => ShowDatabaseSettings(), _ => true);
        }

        private void ExecuteSearch()
        {
            // 検索処理の実装
        }

        private void ShowAppSettings()
        {
            // アプリ設定画面を表示
        }

        private void ShowDatabaseSettings()
        {
            // データベース設定画面を表示
        }

        private async void ShowHighTasks()
        {
            var highTasks = await _highTaskService.GetAllAsync();
            Tasks.Clear();
            foreach (var task in highTasks)
            {
                Tasks.Add(task);
            }
            CurrentView = new HighTaskControl();
        }

        private async void ShowMiddleTasks()
        {
            var middleTasks = await _middleTaskService.GetAllAsync();
            Tasks.Clear();
            foreach (var task in middleTasks)
            {
                Tasks.Add(task);
            }
            CurrentView = new ReadTaskControl();
        }

        private async void ShowLowTasks()
        {
            var lowTasks = await _lowTaskService.GetAllAsync();
            Tasks.Clear();
            foreach (var task in lowTasks)
            {
                Tasks.Add(task);
            }
            CurrentView = new LowTaskControl();
        }

        private void OnHotKeyPressed(object sender, HotKeyPressedEventArgs e)
        {
            switch (e.HotKeyName)
            {
                case "ActivateWindow":
                    // ウィンドウをアクティベート
                    WindowActivateRequested?.Invoke(this, EventArgs.Empty);
                    break;
                case "AddTask":
                    // タスク追加ウィンドウを表示
                    _windowService.ShowAddTaskWindow();
                    break;
                case "Motivation1":
                case "Motivation2":
                case "Motivation3":
                case "Motivation4":
                case "Motivation5":
                case "Motivation6":
                case "Motivation7":
                case "Motivation8":
                case "Motivation9":
                case "Motivation0":
                    // モチベーション追加
                    HandleMotivation(e.HotKeyName);
                    break;
            }
        }

        private void HandleMotivation(string hotKeyName)
        {
            // モチベーション処理
            string motivationText = GetMotivationText(hotKeyName);
            MessageBox.Show($"{motivationText} レコードが追加されました。", "🎮 MOTIVATION BOOST!");
            
            if (hotKeyName == "Motivation1")
            {
                MessageBox.Show("motivationがないほうが、余計なこと考えなくて機械的にやりやすいから、今のお前は最高DA★", "⚡ GAMING MODE ACTIVATED!");
            }
        }

        private string GetMotivationText(string hotKeyName)
        {
            return hotKeyName switch
            {
                "Motivation1" => "🚀 BOOST LEVEL 1",
                "Motivation2" => "⚡ ENERGY SURGE",
                "Motivation3" => "🔥 FIRE MODE",
                "Motivation4" => "💎 DIAMOND FOCUS",
                "Motivation5" => "🌟 STAR POWER",
                "Motivation6" => "🎯 PRECISION TARGET",
                "Motivation7" => "⭐ LEGENDARY",
                "Motivation8" => "🏆 CHAMPION",
                "Motivation9" => "👑 MASTER",
                "Motivation0" => "🎮 ULTIMATE GAMER",
                _ => "UNKNOWN MOTIVATION"
            };
        }

        private void ShowTaskAction()
        {
            // ゲーミング仕様のTaskActionを表示
            var taskActionViewModel = new TaskActionViewModel(_taskActionService, 1); // サンプルタスクID
            taskActionViewModel.TaskCompleted += OnTaskCompleted;
            
            TaskActionRequested?.Invoke(this, new TaskActionRequestedEventArgs(taskActionViewModel));
        }

        private void OnTaskCompleted(object sender, TaskActionViewModel.TaskCompletedEventArgs e)
        {
            // 結果画面を表示
            var resultViewModel = new GamingResultViewModel(_resultService, e.Task, e.IsCompleted);
            resultViewModel.ResultProcessed += OnResultProcessed;
            
            ResultRequested?.Invoke(this, new ResultRequestedEventArgs(resultViewModel));
        }

        private void OnResultProcessed(object sender, GamingResultViewModel.ResultProcessedEventArgs e)
        {
            // 結果処理後の処理
            MessageBox.Show($"🎮 Action completed: {e.Action}", "GAMING RESULT");
        }

        #region Events

        public event EventHandler WindowActivateRequested;
        public event EventHandler<TaskActionRequestedEventArgs> TaskActionRequested;
        public event EventHandler<ResultRequestedEventArgs> ResultRequested;

        public class TaskActionRequestedEventArgs : EventArgs
        {
            public TaskActionViewModel ViewModel { get; }
            
            public TaskActionRequestedEventArgs(TaskActionViewModel viewModel)
            {
                ViewModel = viewModel;
            }
        }

        public class ResultRequestedEventArgs : EventArgs
        {
            public GamingResultViewModel ViewModel { get; }
            
            public ResultRequestedEventArgs(GamingResultViewModel viewModel)
            {
                ViewModel = viewModel;
            }
        }

        #endregion
    }
}