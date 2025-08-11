using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using WpfScheduledApp20250729.Models.Entities;
using WpfScheduledApp20250729.Services;

namespace WpfScheduledApp20250729.ViewModels
{
    public class ResultViewModel : NotificationObject
    {
        private readonly LowTaskService _lowTaskService;
        private readonly DispatcherTimer _animationTimer;
        
        // 統計データ
        private int _totalTasks;
        private int _completedTasks;
        private int _remainingTasks;
        private int _totalEstimatedTime;
        private int _remainingEstimatedTime;
        private int _clearTimesinTIme;
        private int _clearTImesoutoftIme;
        
        // EXPシステム
        private int _currentExp;
        private int _maxExp = 10000;
        private int _gainedExp;
        private double _expProgressWidth;
        
        // アニメーション用
        private int _animatedExp = 0;
        private bool _isAnimating = false;

        public ResultViewModel(LowTaskService lowTaskService)
        {
            _lowTaskService = lowTaskService;
            _animationTimer = new DispatcherTimer(DispatcherPriority.Normal)
            {
                Interval = TimeSpan.FromMilliseconds(16) // ~60fps
            };
            _animationTimer.Tick += AnimationTimer_Tick;
            
            LoadTodayStatistics();
        }

        #region Properties

        public DateTime TodayDate => DateTime.Today;

        public int TotalTasks
        {
            get => _totalTasks;
            set => SetProperty(ref _totalTasks, value);
        }

        public int CompletedTasks
        {
            get => _completedTasks;
            set => SetProperty(ref _completedTasks, value);
        }

        public int RemainingTasks
        {
            get => _remainingTasks;
            set => SetProperty(ref _remainingTasks, value);
        }

        public int TotalEstimatedTime
        {
            get => _totalEstimatedTime;
            set => SetProperty(ref _totalEstimatedTime, value);
        }

        public int RemainingEstimatedTime
        {
            get => _remainingEstimatedTime;
            set => SetProperty(ref _remainingEstimatedTime, value);
        }

        public int CompletionRate
        {
            get
            {
                if (_totalTasks == 0) return 0;
                return (int)Math.Round(((double)_completedTasks / _totalTasks) * 100);
            }
        }

        public int CurrentExp
        {
            get => _animatedExp;
            set => SetProperty(ref _animatedExp, value);
        }

        public int MaxExp => _maxExp;

        public int GainedExp
        {
            get => _gainedExp;
            set => SetProperty(ref _gainedExp, value);
        }

        public double ExpProgressWidth
        {
            get => _expProgressWidth;
            set => SetProperty(ref _expProgressWidth, value);
        }

        public int ClearTimesinTIme
        {
            get => _clearTimesinTIme;
            set => SetProperty(ref _clearTimesinTIme, value);
        }

        public int ClearTImesoutoftIme
        {
            get => _clearTImesoutoftIme;
            set => SetProperty(ref _clearTImesoutoftIme, value);
        }

        #endregion

        #region Commands

        public ICommand StartAnimationCommand => new DelegateCommand(StartExpAnimation);

        #endregion

        #region Methods

        private async void LoadTodayStatistics()
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var allTodayTasks = await GetTodayTasksAsync(today);

                // 今日全体のタスク数
                TotalTasks = allTodayTasks.Count;

                // 今日完了したタスク（ExecutionTimeがnullでない かつ LastClearedAtが今日）
                CompletedTasks = allTodayTasks.Count(t => 
                    t.ExecutionTime != TimeOnly.MinValue && 
                    t.LastClearedAt.Date == DateTime.Today);

                // 今日の残りタスク（ExecutionDateが今日 かつ ExecutionTimeがnull でない かつ LastClearedAtが今日でない）
                RemainingTasks = allTodayTasks.Count(t => 
                    t.ExecutionDate == today && 
                    t.ExecutionTime != TimeOnly.MinValue && 
                    t.LastClearedAt.Date != DateTime.Today);

                // 今日全体のタスク数の推定時間合計値
                TotalEstimatedTime = allTodayTasks.Sum(t => t.EstimatedTime);

                // 今日残り時間タスク率（残りタスクの推定時間合計）
                var remainingTasks = allTodayTasks.Where(t => 
                    t.ExecutionDate == today && 
                    t.ExecutionTime != TimeOnly.MinValue && 
                    t.LastClearedAt.Date != DateTime.Today);
                RemainingEstimatedTime = remainingTasks.Sum(t => t.EstimatedTime);

                // ClearTimesinTIme と ClearTImesoutoftIme を計算
                CalculateClearTimes(allTodayTasks, today);

                // EXP計算（完了タスクベース）
                CalculateExp();

                // プロパティ更新通知
                OnPropertyChanged(nameof(CompletionRate));
            }
            catch (Exception ex)
            {
                // エラーハンドリング
                System.Windows.MessageBox.Show($"統計データの読み込みエラー: {ex.Message}", "エラー");
            }
        }

        private async Task<List<LowTask>> GetTodayTasksAsync(DateOnly today)
        {
            // 実際の実装ではLowTaskServiceを使用してデータベースから取得
            // ここでは仮実装
            return await Task.FromResult(new List<LowTask>
            {
                // サンプルデータ
                new LowTask
                {
                    Id = 1,
                    ExecutionDate = today,
                    ExecutionTime = new TimeOnly(9, 0),
                    EstimatedTime = 30,
                    LastClearedAt = DateTime.Today.AddHours(9.5),
                    Description = "Sample Task 1",
                    MiddleTaskMName = "Sample Middle",
                    HowToLearnName = "Sample HTL",
                    ProjectName = "Sample Project"
                },
                new LowTask
                {
                    Id = 2,
                    ExecutionDate = today,
                    ExecutionTime = new TimeOnly(10, 0),
                    EstimatedTime = 45,
                    LastClearedAt = DateTime.Today.AddHours(-1), // 昨日
                    Description = "Sample Task 2",
                    MiddleTaskMName = "Sample Middle 2",
                    HowToLearnName = "Sample HTL 2",
                    ProjectName = "Sample Project 2"
                },
                new LowTask
                {
                    Id = 3,
                    ExecutionDate = today,
                    ExecutionTime = new TimeOnly(14, 0),
                    EstimatedTime = 60,
                    LastClearedAt = DateTime.Today.AddHours(14.5),
                    Description = "Sample Task 3",
                    MiddleTaskMName = "Sample Middle 3",
                    HowToLearnName = "Sample HTL 3",
                    ProjectName = "Sample Project 3"
                }
            });
        }

        private void CalculateClearTimes(List<LowTask> allTodayTasks, DateOnly today)
        {
            // 今日完了したタスクのみを対象とする
            var completedTodayTasks = allTodayTasks.Where(t => 
                t.ExecutionTime != TimeOnly.MinValue && 
                t.LastClearedAt.Date == DateTime.Today).ToList();

            // 時間内クリア回数：実行予定時間以内にクリアされたタスク
            ClearTimesinTIme = completedTodayTasks.Count(t => 
            {
                var scheduledDateTime = today.ToDateTime(t.ExecutionTime);
                var estimatedEndTime = scheduledDateTime.AddMinutes(t.EstimatedTime);
                return t.LastClearedAt <= estimatedEndTime;
            });

            // 時間外クリア回数：実行予定時間を過ぎてクリアされたタスク
            ClearTImesoutoftIme = completedTodayTasks.Count(t => 
            {
                var scheduledDateTime = today.ToDateTime(t.ExecutionTime);
                var estimatedEndTime = scheduledDateTime.AddMinutes(t.EstimatedTime);
                return t.LastClearedAt > estimatedEndTime;
            });
        }

        private void CalculateExp()
        {
            // 完了タスクベースでEXP計算
            _currentExp = CompletedTasks * 100; // 1タスク = 100 EXP
            _gainedExp = _currentExp; // 今回獲得分として表示
            
            // プログレスバーの幅計算（400pxが最大）
            ExpProgressWidth = (_currentExp / (double)_maxExp) * 400;
            
            OnPropertyChanged(nameof(GainedExp));
        }

        private void StartExpAnimation()
        {
            if (_isAnimating) return;
            
            _isAnimating = true;
            _animatedExp = 0;
            CurrentExp = 0;
            ExpProgressWidth = 0;
            
            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            const int incrementStep = 15; // アニメーション速度
            
            if (_animatedExp < _currentExp)
            {
                _animatedExp = Math.Min(_animatedExp + incrementStep, _currentExp);
                CurrentExp = _animatedExp;
                
                // プログレスバーも同時にアニメーション
                ExpProgressWidth = (_animatedExp / (double)_maxExp) * 400;
                
                OnPropertyChanged(nameof(ExpProgressWidth));
            }
            else
            {
                // アニメーション完了
                _animationTimer.Stop();
                _isAnimating = false;
                
                // フローティングテキストアニメーションをトリガー
                TriggerFloatingExpAnimation();
            }
        }

        private void TriggerFloatingExpAnimation()
        {
            // XAMLのStoryboardを開始する処理
            // 実際の実装ではViewからViewModelにイベントを送信するか、
            // ViewModelからViewに通知する仕組みが必要
            
            // ここでは効果音の代わりにシステムサウンドを再生
            System.Media.SystemSounds.Asterisk.Play();
        }

        #endregion
    }
}
