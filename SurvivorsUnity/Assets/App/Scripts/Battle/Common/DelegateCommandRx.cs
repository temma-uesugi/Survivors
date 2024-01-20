using System;
using UniRx;

namespace App.Battle.Common
{
    /// <summary>
    /// コマンド
    /// </summary>
    public abstract class DelegateCommandRxBase<T>
    {
        public abstract bool CanExecute();

        //条件パラメータ
        protected ReactiveProperty<T> CondValue;
        public IReadOnlyReactiveProperty<T> Value => CondValue;

        //実行可能か
        public IReadOnlyReactiveProperty<bool> Executable =>
            CondValue.Select(_ => CanExecute()).ToReadOnlyReactiveProperty();
    }

    /// <summary>
    /// コマンド
    /// </summary>
    public class DelegateCommandRx : DelegateCommandRxBase<bool>
    {
        private readonly Action _execute;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DelegateCommandRx(Action execute, ReactiveProperty<bool> condValue)
        {
            _execute = execute;
            CondValue = condValue;
        }

        /// <summary>
        /// CanExecute
        /// </summary>
        public override bool CanExecute() => CondValue.Value;

        /// <summary>
        /// 実行
        /// </summary>
        public void Execute()
        {
            _execute();
        }
    }

    /// <summary>
    /// コマンド
    /// </summary>
    public class DelegateCommandRx<T> : DelegateCommandRxBase<T>
    {
        private readonly Action _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DelegateCommandRx(Action execute, ReactiveProperty<T> condValue, Func<T, bool> canExecute)
        {
            _execute = execute;
            CondValue = condValue;
            _canExecute = canExecute;
        }

        /// <summary>
        /// CanExecute
        /// </summary>
        public override bool CanExecute() => _canExecute(CondValue.Value);

        /// <summary>
        /// 実行
        /// </summary>
        public void Execute()
        {
            _execute();
        }
    }

    /// <summary>
    /// コマンド(実行時引数)
    /// </summary>
    public class DelegateCommandRx<T, TA> : DelegateCommandRxBase<T>
    {
        private readonly Action<TA> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DelegateCommandRx(Action<TA> execute, ReactiveProperty<T> condValue)
        {
            if (typeof(T) != typeof(bool))
            {
                throw new Exception("DelegateCommandRx.Value is not bool");
            }
            _execute = execute;
            CondValue = condValue;
            _canExecute = val => Convert.ToBoolean(val);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DelegateCommandRx(Action<TA> execute, ReactiveProperty<T> condValue,  Func<T, bool> canExecute)
        {
            _execute = execute;
            CondValue = condValue;
            _canExecute = canExecute;
        }

        /// <summary>
        /// CanExecute
        /// </summary>
        public override bool CanExecute() => _canExecute(CondValue.Value);

        /// <summary>
        /// 実行
        /// </summary>
        public void Execute(TA actionParam)
        {
            _execute(actionParam);
        }
    }
}