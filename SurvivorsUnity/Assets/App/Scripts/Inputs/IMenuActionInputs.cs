using System;
using UniRx;

namespace App.Inputs
{
    /// <summary>
    /// MenuアクションのInterface
    /// </summary>
    public interface IMenuActionInputs
    {
        IObservable<int> MoveCursor { get; }
        IObservable<Unit> Decide { get; }
        IObservable<Unit> Cancel { get; }
    }
}