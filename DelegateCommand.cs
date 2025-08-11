using System;
using System.Windows.Input;

namespace WpfScheduledApp20250729
{
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// この２つはデリゲートつまり、methodが登録しておける入れ物、so,要所要所でNullとかでるのは
        /// 登録されてなかったらこのでりげーと実行する必要がないからそこで条件式かいてるのかな？エラー出るのかな？
        /// </summary>
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action<object?> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// デリゲート初動で記述したのだから、何か登録しないと！そこでコンストラクタでインスタンス呼ばれた時に登録されるようにしている
        /// あと、CanExecute内部で、自身の登録先のデリゲートを評価することもできるのか
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute != null ? _canExecute(parameter) : true;
        }

        // イベントを空のデリゲートで初期化（null警告を回避）
        public event EventHandler? CanExecuteChanged = delegate { };

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
