using System;
using System.ComponentModel;
using System.Windows.Threading;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.ViewModels
{
    public class TaskActionViewModel : NotificationObject, IDisposable
    {
        private readonly ITaskActionService _taskActionService;
        private readonly DispatcherTimer _timer;
        private TaskActionModel _task;
        private bool _disposed = false;

        public TaskActionViewModel(ITaskActionService taskActionService)
        {
            _taskActionService = taskActionService;
            _timer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
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
                return _generateMMFileCommand ??= new DelegateCommand(
                    _ => GenerateMMFile(),
                    _ => _task?.HTL == 9 || _task?.HTL == 11
                );
            }
        }

        private DelegateCommand _completeTaskCommand;
        public DelegateCommand CompleteTaskCommand
        {
            get
            {
                return _completeTaskCommand ??= new DelegateCommand(
                    _ => CompleteTask(),
                    _ => _task != null
                );
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
                _timer?.Stop();
                _timer?.Tick -= Timer_Tick;
                _disposed = true;
            }
        }

        #endregion
    }
}