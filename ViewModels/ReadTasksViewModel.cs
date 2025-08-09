using WpfScheduledApp20250729.Controls;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WpfScheduledApp20250729;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Services;

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

        private bool _letsStartCheckBox;
        public bool LetsStartCheckBox
        {
            get => _letsStartCheckBox;
            set => SetProperty(ref _letsStartCheckBox, value);
        }

        public ReadTasksViewModel(IWindowService windowService, HighTaskService highTaskService, MiddleTaskService middleTaskService, LowTaskService lowTaskService, ArchitectureService architectureService, ProjectService projectService)
        {
            _windowService = windowService;
            _highTaskService = highTaskService;
            _middleTaskService = middleTaskService;
            _lowTaskService = lowTaskService;
            _architectureService = architectureService;
            _projectService = projectService;
        }

        private DelegateCommand? _highTaskCommand;
        public DelegateCommand HighTaskCommand
        {
            get
            {
                return _highTaskCommand ?? (_highTaskCommand = new DelegateCommand(
                async _ =>
                {
                    try
                    {
                        var highTasks = await _highTaskService.GetAllAsync();
                        Tasks.Clear();
                        foreach (var task in highTasks)
                        {
                            Tasks.Add(task);
                        }
                        CurrentView = new HighTaskControl();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"HighTask取得エラー: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                },
                _ => true));
            }
        }

        private DelegateCommand? _middleTaskCommand;
        public DelegateCommand MiddleTaskCommand
        {
            get
            {
                return _middleTaskCommand ?? (_middleTaskCommand = new DelegateCommand(
                async _ =>
                {
                    try
                    {
                        var middleTasks = await _middleTaskService.GetAllAsync();
                        Tasks.Clear();
                        foreach (var task in middleTasks)
                        {
                            Tasks.Add(task);
                        }
                        CurrentView = new ReadTaskControl();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"MiddleTask取得エラー: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                },
                _ => true));
            }
        }

        private DelegateCommand? _lowTaskCommand;
        public DelegateCommand LowTaskCommand
        {
            get
            {
                return _lowTaskCommand ?? (_lowTaskCommand = new DelegateCommand(
                async _ =>
                {
                    try
                    {
                        var lowTasks = await _lowTaskService.GetAllAsync();
                        Tasks.Clear();
                        foreach (var task in lowTasks)
                        {
                            Tasks.Add(task);
                        }
                        CurrentView = new LowTaskControl();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"LowTask取得エラー: {ex.Message}", "エラー", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                },
                _ => true));
            }
        }

        private DelegateCommand? _searchCommand;
        public DelegateCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new DelegateCommand(
                _ =>
                {
                    MessageBox.Show("検索機能は実装中です。", "情報", MessageBoxButton.OK, MessageBoxImage.Information);
                },
                _ => true));
            }
        }

        private DelegateCommand? _showDetailCommand;
        public DelegateCommand ShowDetailCommand
        {
            get
            {
                return _showDetailCommand ?? (_showDetailCommand = new DelegateCommand(
                param =>
                {
                    if (param != null)
                    {
                        // 編集ウィンドウを開く
                        _windowService.ShowUpdateTaskWindow(param);
                    }
                },
                param => param != null));
            }
        }

    }
}