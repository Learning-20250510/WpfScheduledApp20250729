using System;
using WpfScheduledApp20250729.Interfaces;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.ViewModels
{
    public class GamingResultViewModel : NotificationObject
    {
        private readonly IResultService _resultService;
        private ResultModel _result;

        public GamingResultViewModel(IResultService resultService)
        {
            _resultService = resultService;
        }

        public GamingResultViewModel(IResultService resultService, TaskActionModel task, bool isCompleted) : this(resultService)
        {
            LoadResult(task, isCompleted);
        }

        #region Properties

        private string _resultText = string.Empty;
        public string ResultText
        {
            get => _resultText;
            set => SetProperty(ref _resultText, value);
        }

        private string _taskName = string.Empty;
        public string TaskName
        {
            get => _taskName;
            set => SetProperty(ref _taskName, value);
        }

        private string _durationText = string.Empty;
        public string DurationText
        {
            get => _durationText;
            set => SetProperty(ref _durationText, value);
        }

        private int _estimatedTimeTextBox;
        public int EstimatedTimeTextBox
        {
            get => _estimatedTimeTextBox;
            set => SetProperty(ref _estimatedTimeTextBox, value);
        }

        private int _priorityTextBox;
        public int PriorityTextBox
        {
            get => _priorityTextBox;
            set => SetProperty(ref _priorityTextBox, value);
        }

        private bool _showUpdateFields;
        public bool ShowUpdateFields
        {
            get => _showUpdateFields;
            set => SetProperty(ref _showUpdateFields, value);
        }

        #endregion

        #region Commands

        private DelegateCommand _clearCommand;
        public DelegateCommand ClearCommand
        {
            get
            {
                return _clearCommand ??= new DelegateCommand(
                    _ => ExecuteAction(ResultAction.Clear),
                    _ => _result != null
                );
            }
        }

        private DelegateCommand _againCommand;
        public DelegateCommand AgainCommand
        {
            get
            {
                return _againCommand ??= new DelegateCommand(
                    _ => ExecuteAction(ResultAction.Again),
                    _ => _result != null
                );
            }
        }

        private DelegateCommand _againLaterCommand;
        public DelegateCommand AgainLaterCommand
        {
            get
            {
                return _againLaterCommand ??= new DelegateCommand(
                    _ => ExecuteAction(ResultAction.AgainLater),
                    _ => _result != null
                );
            }
        }

        private DelegateCommand _updateTaskColumnCommand;
        public DelegateCommand UpdateTaskColumnCommand
        {
            get
            {
                return _updateTaskColumnCommand ??= new DelegateCommand(
                    _ => ExecuteAction(ResultAction.UpdateColumn),
                    _ => _result != null && EstimatedTimeTextBox > 0 && PriorityTextBox > 0
                );
            }
        }

        private DelegateCommand _showUpdateFieldsCommand;
        public DelegateCommand ShowUpdateFieldsCommand
        {
            get
            {
                return _showUpdateFieldsCommand ??= new DelegateCommand(
                    _ => ShowUpdateFields = !ShowUpdateFields,
                    _ => true
                );
            }
        }

        #endregion

        #region Methods

        private void LoadResult(TaskActionModel task, bool isCompleted)
        {
            _result = _resultService.CreateResult(task, isCompleted);
            if (_result != null)
            {
                ResultText = _result.ResultText;
                TaskName = _result.TaskName;
                DurationText = $"⏱️ EXECUTION TIME: {_result.Duration:hh\\:mm\\:ss}";
                EstimatedTimeTextBox = _result.EstimatedTime;
                PriorityTextBox = _result.Priority;
            }
        }

        private void ExecuteAction(ResultAction action)
        {
            if (_result == null) return;

            _result.SelectedAction = action;
            
            // カラム更新の場合は現在の値を設定
            if (action == ResultAction.UpdateColumn)
            {
                _result.EstimatedTime = EstimatedTimeTextBox;
                _result.Priority = PriorityTextBox;
            }

            _resultService.ProcessResultAction(_result);
            
            // 結果処理完了イベント発火
            ResultProcessed?.Invoke(this, new ResultProcessedEventArgs(_result, action));
        }

        #endregion

        #region Events

        public event EventHandler<ResultProcessedEventArgs> ResultProcessed;

        public class ResultProcessedEventArgs : EventArgs
        {
            public ResultModel Result { get; }
            public ResultAction Action { get; }

            public ResultProcessedEventArgs(ResultModel result, ResultAction action)
            {
                Result = result;
                Action = action;
            }
        }

        #endregion
    }
}