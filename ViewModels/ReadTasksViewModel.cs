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

        public ReadTasksViewModel(IWindowService windowService, HighTaskService highTaskService, MiddleTaskService middleTaskService, LowTaskService lowTaskService, ArchitectureService architectureService, ProjectService projectService)
        {
            _windowService = windowService;
            _highTaskService = highTaskService;
            _middleTaskService = middleTaskService;
            _lowTaskService = lowTaskService;
            _architectureService = architectureService;
            _projectService = projectService;

            // コマンドの初期化
            SearchCommand = new DelegateCommand(_ => ExecuteSearch(), _ => true);
            HighTaskCommand = new DelegateCommand(_ => ShowHighTasks(), _ => true);
            MiddleTaskCommand = new DelegateCommand(_ => ShowMiddleTasks(), _ => true);
            LowTaskCommand = new DelegateCommand(_ => ShowLowTasks(), _ => true);
            AddTaskCommand = new DelegateCommand(_ => _windowService.ShowAddTaskWindow(), _ => true);
            ActionTaskCommand = new DelegateCommand(_ => _windowService.ShowActionTaskWindow(), _ => true);
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
    }
}