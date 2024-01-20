using System;

namespace App.Battle2.Common
{
    /// <summary>
    /// コマンド
    /// </summary>
    public class DelegateCommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DelegateCommand(Action execute)
        {
            _execute = execute;
        }

        /// <summary>
        /// 登録
        /// </summary>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 実行可能か
        /// </summary>
        public bool CanExecute()
        {
            return _canExecute();
        }

        /// <summary>
        /// 実行
        /// </summary>
        public void Execute()
        {
            _execute();
        }
    }

    /// <summary>
    /// 引数を指定してのコマンド
    /// </summary>
    public class DelegateCommand<T>
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DelegateCommand(Action<T> execute)
        {
            _execute = execute;
        }

        /// <summary>
        /// 登録
        /// </summary>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 実行可能
        /// </summary>
        public bool CanExecute(T param)
        {
            return _canExecute(param);
        }

        /// <summary>
        /// 実行
        /// </summary>
        public void Execute(T param)
        {
            _execute(param);
        }
    }

}