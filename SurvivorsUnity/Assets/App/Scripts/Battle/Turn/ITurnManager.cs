using System;

namespace App.Battle.Turn
{
    /// <summary>
    /// ターン管理
    /// </summary>
    public interface ITurnManager<T> where T : ITurnValue
    {
        public IObservable<(int turnLineIndex, T data)> OnAddTurnLine { get; }
        public IObservable<T> OnTurnProceed { get; }
        public T CurrentValue { get; }
    }
}