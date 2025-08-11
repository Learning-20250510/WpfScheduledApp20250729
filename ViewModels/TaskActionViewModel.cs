using System;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Input;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Models;
using WpfScheduledApp20250729.Services;

namespace WpfScheduledApp20250729.ViewModels
{
    public class TaskActionViewModel : NotificationObject, IDisposable
    {
        private readonly ITaskActionService _taskActionService;
        private readonly DispatcherTimer _timer;
        private TaskActionModel _task;
        private bool _disposed = false;
        
        // タイピングゲーム関連
        private TypingGameSettings _typingSettings;
        private int _currentTypingCount;
        private int _maxTypingCount = 1000;
        private int _typingDecrementValue = 5;

        public TaskActionViewModel(ITaskActionService taskActionService)
        {
            _taskActionService = taskActionService;
            _timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            
            // タイピングゲーム初期化
            InitializeTypingGame();
            GlobalHotKeyService.KeyPressed += OnKeyPressed;
        }

        public TaskActionViewModel(ITaskActionService taskActionService, int taskId) : this(taskActionService)
        {
            LoadTask(taskId);
            _timer.Start();
        }

        #region Properties

        private string _id = string.Empty;
        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _kmn = string.Empty;
        public string KMN
        {
            get => _kmn;
            set => SetProperty(ref _kmn, value);
        }

        private string _kmt = string.Empty;
        public string KMT
        {
            get => _kmt;
            set => SetProperty(ref _kmt, value);
        }

        private int _htl;
        public int HTL
        {
            get => _htl;
            set => SetProperty(ref _htl, value);
        }

        private int _estimatedTime;
        public int EstimatedTime
        {
            get => _estimatedTime;
            set => SetProperty(ref _estimatedTime, value);
        }

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _scrollValue;
        public int ScrollValue
        {
            get => _scrollValue;
            set => SetProperty(ref _scrollValue, value);
        }

        private int _voiceMemo;
        public int VoiceMemo
        {
            get => _voiceMemo;
            set
            {
                if (SetProperty(ref _voiceMemo, value))
                {
                    UpdateDamageCount();
                }
            }
        }

        private int _manualMemoBySP;
        public int ManualMemoBySP
        {
            get => _manualMemoBySP;
            set
            {
                if (SetProperty(ref _manualMemoBySP, value))
                {
                    UpdateDamageCount();
                }
            }
        }

        private int _manualMemoNumberOfPagesByAnalog;
        public int ManualMemoNumberOfPagesByAnalog
        {
            get => _manualMemoNumberOfPagesByAnalog;
            set
            {
                if (SetProperty(ref _manualMemoNumberOfPagesByAnalog, value))
                {
                    UpdateDamageCount();
                }
            }
        }

        private int _concentrateTime;
        public int ConcentrateTime
        {
            get => _concentrateTime;
            set
            {
                if (SetProperty(ref _concentrateTime, value))
                {
                    UpdateDamageCount();
                }
            }
        }

        private string _autoCreateMMFileGenerator = string.Empty;
        public string AutoCreateMMFileGenerator
        {
            get => _autoCreateMMFileGenerator;
            set => SetProperty(ref _autoCreateMMFileGenerator, value);
        }

        private string _elapsedTimeText = "00:00:00";
        public string ElapsedTimeText
        {
            get => _elapsedTimeText;
            set => SetProperty(ref _elapsedTimeText, value);
        }

        private double _healthBarWidth = 400;
        public double HealthBarWidth
        {
            get => _healthBarWidth;
            set => SetProperty(ref _healthBarWidth, value);
        }

        private int _damageCount;
        public int DamageCount
        {
            get => _damageCount;
            set
            {
                if (SetProperty(ref _damageCount, value))
                {
                    UpdateHealthBar();
                }
            }
        }

        #endregion

        #region Commands

        private DelegateCommand _generateMMFileCommand;
        public DelegateCommand GenerateMMFileCommand
        {
            get
            {
                if (_generateMMFileCommand == null)
                {
                    _generateMMFileCommand = new DelegateCommand(
                        _ => GenerateMMFile(),
                        _ => _task?.HTL == 9 || _task?.HTL == 11
                    );
                }
                return _generateMMFileCommand;
            }
        }

        private DelegateCommand _completeTaskCommand;
        public DelegateCommand CompleteTaskCommand
        {
            get
            {
                if (_completeTaskCommand == null)
                {
                    _completeTaskCommand = new DelegateCommand(
                        _ => CompleteTask(),
                        _ => _task != null
                    );
                }
                return _completeTaskCommand;
            }
        }

        #endregion

        #region Methods

        private void LoadTask(int taskId)
        {
            _task = _taskActionService.GetTaskAction(taskId);
            if (_task != null)
            {
                ID = _task.ID.ToString();
                KMN = _task.KMN;
                KMT = _task.KMT;
                HTL = _task.HTL;
                EstimatedTime = _task.EstimatedTime;
                Description = _task.Description;
                ScrollValue = _task.ScrollValue;

                // タスク実行
                _taskActionService.ExecuteTaskAction(_task);
            }
        }

        private void UpdateDamageCount()
        {
            if (_task == null) return;

            _task.VoiceMemo = VoiceMemo;
            _task.ManualMemoBySP = ManualMemoBySP;
            _task.ManualMemoNumberOfPagesByAnalog = ManualMemoNumberOfPagesByAnalog;
            _task.ConcentrateTime = ConcentrateTime;

            _taskActionService.UpdateDamageCount(_task);
            DamageCount = _task.DamageCount;
        }

        private void UpdateHealthBar()
        {
            // ダメージに基づいてヘルスバーの幅を更新（ゲーミング風）
            double maxWidth = 400;
            double damageRatio = Math.Min(DamageCount / 1000.0, 1.0); // 1000を最大ダメージとする
            HealthBarWidth = maxWidth * (1.0 - damageRatio);
        }

        private void GenerateMMFile()
        {
            if (_task == null) return;

            _task.AutoCreateMMFileGenerator = AutoCreateMMFileGenerator;
            _taskActionService.GenerateMMFile(_task);
        }

        private void CompleteTask()
        {
            _timer.Stop();
            
            // 結果画面への遷移イベント発火
            TaskCompleted?.Invoke(this, new TaskCompletedEventArgs(_task, true));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_task != null)
            {
                var elapsed = DateTime.Now - _task.StartTime;
                ElapsedTimeText = elapsed.ToString(@"hh\:mm\:ss");
            }
        }

        #endregion

        #region Events

        public event EventHandler<TaskCompletedEventArgs> TaskCompleted;

        public class TaskCompletedEventArgs : EventArgs
        {
            public TaskActionModel Task { get; }
            public bool IsCompleted { get; }

            public TaskCompletedEventArgs(TaskActionModel task, bool isCompleted)
            {
                Task = task;
                IsCompleted = isCompleted;
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Tick -= Timer_Tick;
                }
                GlobalHotKeyService.KeyPressed -= OnKeyPressed;
                _disposed = true;
            }
        }

        #endregion

        #region タイピングゲーム機能

        private void InitializeTypingGame()
        {
            _typingSettings = new TypingGameSettings();
            _currentTypingCount = _maxTypingCount;
        }

        private void OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            // 任意のキー入力でプログレスバーを減少
            if (_currentTypingCount > 0)
            {
                _currentTypingCount = Math.Max(0, _currentTypingCount - _typingDecrementValue);
                RaisePropertyChanged(nameof(CurrentTypingCount));
                RaisePropertyChanged(nameof(TypingProgressWidth));
                RaisePropertyChanged(nameof(TypingProgressText));
            }
        }

        public void LoadTypingSettings(int? architectureId, int? howToLearnId)
        {
            // Architecture/HowToLearnに基づく設定
            _typingSettings = new TypingGameSettings
            {
                ArchitectureId = architectureId,
                HowToLearnId = howToLearnId,
                MaxProgressValue = GetMaxValueByType(architectureId, howToLearnId),
                DecrementPerKey = GetDecrementByType(architectureId, howToLearnId),
                ProgressBarColor = GetColorByType(architectureId, howToLearnId)
            };

            _maxTypingCount = _typingSettings.MaxProgressValue;
            _typingDecrementValue = _typingSettings.DecrementPerKey;
            _currentTypingCount = _maxTypingCount;

            RaisePropertyChanged(nameof(MaxTypingCount));
            RaisePropertyChanged(nameof(TypingDecrementValue));
            RaisePropertyChanged(nameof(CurrentTypingCount));
            RaisePropertyChanged(nameof(TypingProgressWidth));
            RaisePropertyChanged(nameof(TypingProgressText));
        }

        private int GetMaxValueByType(int? architectureId, int? howToLearnId)
        {
            // HowToLearnに基づく最大値設定
            if (howToLearnId.HasValue)
            {
                return howToLearnId.Value switch
                {
                    1 => 500,  // FreePlane
                    2 => 1500, // PDF
                    3 => 800,  // Movie
                    4 => 1200, // WebPage
                    _ => 1000
                };
            }
            // Architectureに基づく設定
            return architectureId switch
            {
                1 => 600,  // 学習系
                2 => 1000, // 作業系
                3 => 1500, // プロジェクト系
                _ => 1000
            };
        }

        private int GetDecrementByType(int? architectureId, int? howToLearnId)
        {
            // HowToLearnに基づく減少値設定
            if (howToLearnId.HasValue)
            {
                return howToLearnId.Value switch
                {
                    1 => 3,  // FreePlane - 軽い
                    2 => 8,  // PDF - 重い
                    3 => 5,  // Movie - 中程度
                    4 => 6,  // WebPage - 中程度
                    _ => 5
                };
            }
            // Architectureに基づく設定
            return architectureId switch
            {
                1 => 4,  // 学習系 - 軽い
                2 => 6,  // 作業系 - 中程度
                3 => 8,  // プロジェクト系 - 重い
                _ => 5
            };
        }

        private string GetColorByType(int? architectureId, int? howToLearnId)
        {
            // HowToLearnに基づく色設定
            if (howToLearnId.HasValue)
            {
                return howToLearnId.Value switch
                {
                    1 => "#FF00FF00", // FreePlane - 緑
                    2 => "#FFFF8000", // PDF - オレンジ
                    3 => "#FFFF0080", // Movie - マゼンタ
                    4 => "#FF0080FF", // WebPage - 青
                    _ => "#FF00FFFF"
                };
            }
            return "#FF00FFFF"; // デフォルト：シアン
        }

        // タイピングゲーム用プロパティ
        public int CurrentTypingCount
        {
            get => _currentTypingCount;
            set => SetProperty(ref _currentTypingCount, value);
        }

        public int MaxTypingCount => _maxTypingCount;

        public int TypingDecrementValue => _typingDecrementValue;

        public double TypingProgressWidth
        {
            get
            {
                if (_maxTypingCount <= 0) return 0;
                return ((double)_currentTypingCount / _maxTypingCount) * 200; // 200はProgressBarの幅
            }
        }

        public string TypingProgressText
        {
            get
            {
                var percentage = _maxTypingCount > 0 ? (_currentTypingCount * 100) / _maxTypingCount : 0;
                return $"{percentage}% POWER REMAINING";
            }
        }

        #endregion
    }
}