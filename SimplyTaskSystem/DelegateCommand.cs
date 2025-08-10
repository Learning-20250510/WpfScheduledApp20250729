using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTaskSystem
{
    using System.Windows.Input;

    internal class DelegateCommand : ICommand
    {



        /// <summary>
        /// この２つはデリゲートつまり、methodが登録しておける入れ物、so,要所要所でNullとかでるのは
        /// 登録されてなかったらこのでりげーと実行する必要がないからそこで条件式かいてるのかな？エラー出るのかな？
        /// </summary>
        private Action<object> _execute;

        private Func<object, bool> _canExecute;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="execute"></param>
        public DelegateCommand(Action<object> execute)
            : this(execute, null)
        {

        }



        /// <summary>
        /// 
        /// デリゲート初動で記述したのだから、何か登録しないと！そこでコンストラクタでインスタンス呼ばれた時に登録されるようにしている
        /// あと、CanExecute内部で、自身の登録先のデリゲートを評価することもできるのか
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {

            return (this._canExecute != null) ? this._canExecute(parameter) : true;
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var h = this.CanExecuteChanged;

            if (h != null) h(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            if (this._execute != null)
                this._execute(parameter);
        }

    }
}
