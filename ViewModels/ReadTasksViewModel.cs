using WpfScheduledApp20250729.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading.Tasks;
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

        // 図書館表示機能
        private bool _isLibraryView = false;
        public bool IsLibraryView
        {
            get => _isLibraryView;
            set => SetProperty(ref _isLibraryView, value);
        }

        // デザインテーマ管理
        public enum DesignTheme
        {
            Gaming = 0,
            Library = 1,
            Terminator = 2,
            Japanese = 3,
            Monochrome = 4
        }

        private DesignTheme _currentTheme = DesignTheme.Gaming;
        public DesignTheme CurrentTheme
        {
            get => _currentTheme;
            set => SetProperty(ref _currentTheme, value);
        }

        // テーマ切り替え用プロパティ
        public bool IsGamingTheme => CurrentTheme == DesignTheme.Gaming;
        public bool IsLibraryTheme => CurrentTheme == DesignTheme.Library;
        public bool IsTerminatorTheme => CurrentTheme == DesignTheme.Terminator;
        public bool IsJapaneseTheme => CurrentTheme == DesignTheme.Japanese;
        public bool IsMonochromeTheme => CurrentTheme == DesignTheme.Monochrome;

        private ObservableCollection<HTLSectionViewModel> _htlSections = new();
        public ObservableCollection<HTLSectionViewModel> HTLSections
        {
            get => _htlSections;
            set => SetProperty(ref _htlSections, value);
        }

        public DelegateCommand SearchCommand { get; }
        public DelegateCommand HighTaskCommand { get; }
        public DelegateCommand MiddleTaskCommand { get; }
        public DelegateCommand LowTaskCommand { get; }
        public DelegateCommand AddTaskCommand { get; }
        public DelegateCommand ActionTaskCommand { get; }
        public DelegateCommand AppSettingsCommand { get; }
        public DelegateCommand DatabaseSettingsCommand { get; }
        public DelegateCommand ToggleViewCommand { get; }
        public DelegateCommand SwitchThemeCommand { get; }

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
            ToggleViewCommand = new DelegateCommand(_ => ToggleView(), _ => true);
            SwitchThemeCommand = new DelegateCommand(_ => SwitchTheme(), _ => true);
            
            // 初期ビューを設定
            LoadInitialView();
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
        public event EventHandler<ThemeChangedEventArgs> ThemeChanged;

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

        public class ThemeChangedEventArgs : EventArgs
        {
            public DesignTheme NewTheme { get; }
            
            public ThemeChangedEventArgs(DesignTheme newTheme)
            {
                NewTheme = newTheme;
            }
        }

        #endregion

        #region 図書館表示機能

        private void LoadInitialView()
        {
            // 初期はテーブルビューを表示
            CurrentView = new ReadTaskControl();
            LoadLibraryData();
        }

        private void ToggleView()
        {
            IsLibraryView = !IsLibraryView;
            
            if (IsLibraryView)
            {
                // 図書館表示に切り替え
                var libraryControl = new LibraryTaskControl();
                libraryControl.DataContext = this;
                CurrentView = libraryControl;
                LoadLibraryData();
            }
            else
            {
                // テーブル表示に戻す
                CurrentView = new ReadTaskControl();
                LoadTableData();
            }
        }

        private void SwitchTheme()
        {
            // テーマを順番に切り替え: Gaming → Library → Terminator → Japanese → Monochrome → Gaming...
            CurrentTheme = (DesignTheme)(((int)CurrentTheme + 1) % 5);
            
            // プロパティ変更通知
            OnPropertyChanged(nameof(IsGamingTheme));
            OnPropertyChanged(nameof(IsLibraryTheme));
            OnPropertyChanged(nameof(IsTerminatorTheme));
            OnPropertyChanged(nameof(IsJapaneseTheme));
            OnPropertyChanged(nameof(IsMonochromeTheme));
            
            // テーマに応じたメッセージ表示
            string themeMessage = CurrentTheme switch
            {
                DesignTheme.Gaming => "🎮 GAMING MODE ACTIVATED!",
                DesignTheme.Library => "📚 LIBRARY MODE ACTIVATED!",
                DesignTheme.Terminator => "🤖 TERMINATOR MODE ACTIVATED! TARGET ACQUIRED.",
                DesignTheme.Japanese => "🌸 和風モード起動！雅な世界へようこそ。",
                DesignTheme.Monochrome => "⚫ MONOCHROME MODE ACTIVATED! SIMPLICITY IS BEAUTY.",
                _ => "UNKNOWN THEME"
            };
            
            MessageBox.Show(themeMessage, "THEME SWITCHED");
            
            // Windowのスタイルを更新するため、Windowへイベント通知
            ThemeChanged?.Invoke(this, new ThemeChangedEventArgs(CurrentTheme));
        }

        private async void LoadLibraryData()
        {
            try
            {
                // LowTaskからデータを取得
                var lowTasks = await GetTodayLowTasksAsync();
                
                // HTL別にグループ化
                var htlGroups = lowTasks.GroupBy(t => t.HowToLearnName).ToList();
                
                HTLSections.Clear();
                
                foreach (var htlGroup in htlGroups)
                {
                    var section = new HTLSectionViewModel
                    {
                        HTLName = htlGroup.Key,
                        HTLIcon = HTLStatistics.GetHTLIcon(htlGroup.Key),
                        ClearCount = await GetHTLClearCountAsync(htlGroup.Key)
                    };
                    
                    // タスクをLibraryTaskViewModelに変換
                    foreach (var task in htlGroup)
                    {
                        var libraryTask = LibraryTaskViewModel.FromLowTask(task);
                        section.Tasks.Add(libraryTask);
                    }
                    
                    section.UpdateProgress();
                    HTLSections.Add(section);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"図書館データ読み込みエラー: {ex.Message}", "エラー");
            }
        }

        private void LoadTableData()
        {
            // 既存のテーブルデータ読み込み処理
            ShowLowTasks();
        }

        private async Task<System.Collections.Generic.List<Models.Entities.LowTask>> GetTodayLowTasksAsync()
        {
            // 実際の実装ではLowTaskServiceを使用
            // 今日のタスクを取得する仮実装
            return await Task.FromResult(new System.Collections.Generic.List<Models.Entities.LowTask>
            {
                new Models.Entities.LowTask
                {
                    Id = 1,
                    Description = "FreePlane MMファイル作成",
                    EstimatedTime = 30,
                    ExecutionDate = DateOnly.FromDateTime(DateTime.Today),
                    ExecutionTime = new TimeOnly(9, 0),
                    LastClearedAt = DateTime.Today.AddHours(-1),
                    HowToLearnName = "FreePlane",
                    MiddleTaskMName = "学習計画",
                    ProjectName = "知識管理",
                    MiddleTaskId = 1,
                    ProjectId = 1
                },
                new Models.Entities.LowTask
                {
                    Id = 2,
                    Description = "PDF資料読み込み",
                    EstimatedTime = 45,
                    ExecutionDate = DateOnly.FromDateTime(DateTime.Today),
                    ExecutionTime = new TimeOnly(10, 0),
                    LastClearedAt = DateTime.Today.AddHours(2),
                    HowToLearnName = "PDF",
                    MiddleTaskMName = "資料研究",
                    ProjectName = "技術学習",
                    MiddleTaskId = 2,
                    ProjectId = 1
                },
                new Models.Entities.LowTask
                {
                    Id = 3,
                    Description = "動画教材視聴",
                    EstimatedTime = 60,
                    ExecutionDate = DateOnly.FromDateTime(DateTime.Today),
                    ExecutionTime = new TimeOnly(14, 0),
                    LastClearedAt = DateTime.Today.AddHours(-2),
                    HowToLearnName = "Movie",
                    MiddleTaskMName = "動画学習",
                    ProjectName = "スキルアップ",
                    MiddleTaskId = 3,
                    ProjectId = 2
                },
                new Models.Entities.LowTask
                {
                    Id = 4,
                    Description = "Webページ調査",
                    EstimatedTime = 25,
                    ExecutionDate = DateOnly.FromDateTime(DateTime.Today),
                    ExecutionTime = new TimeOnly(16, 0),
                    LastClearedAt = DateTime.Today.AddHours(-3),
                    HowToLearnName = "WebPage",
                    MiddleTaskMName = "情報収集",
                    ProjectName = "リサーチ",
                    MiddleTaskId = 4,
                    ProjectId = 2
                }
            });
        }

        private async Task<int> GetHTLClearCountAsync(string htlName)
        {
            // HTL別の累計クリア回数を取得（仮実装）
            return await Task.FromResult(htlName switch
            {
                "FreePlane" => 15,
                "PDF" => 23,
                "Movie" => 8,
                "WebPage" => 12,
                _ => 5
            });
        }

        #endregion
    }
}