using System;
using Cysharp.Threading.Tasks;

namespace App.AppCommon.Core
{
    /// <summary>
    /// OnNext時に購読者の処理の終了をawaitできる
    /// Note: Donuts.AM3から
    /// </summary> 
    public class AwaitableSubject<T> : IDisposable
    {
        private bool _isDisposed = false;
        private UniRx.InternalUtil.ImmutableList<Func<T, UniTask>> _notifier;

        /// <summary>
        /// OnNext
        /// </summary>
        public UniTask OnNextAsync(T message)
        {
            if (_notifier == null)
            {
                return UniTask.CompletedTask;
            }
            var data = _notifier.Data;
            var awaiter = new UniTask[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                awaiter[i] = data[i].Invoke(message);
            }
            return UniTask.WhenAll(awaiter);
        }

        /// <summary>
        /// 購読
        /// </summary>
        public IDisposable Subscribe(Func<T, UniTask> asyncMessageReceiver)
        {
            if (_isDisposed) throw new ObjectDisposedException("AwaitableSubject");

            _notifier ??= UniRx.InternalUtil.ImmutableList<Func<T, UniTask>>.Empty;
            _notifier = _notifier.Add(asyncMessageReceiver);

            return new Subscription(this, asyncMessageReceiver);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _notifier = null;
            }
        }

        /// <summary>
        /// 購読
        /// </summary>
        class Subscription : IDisposable
        {
            readonly AwaitableSubject<T> _parent;
            readonly Func<T, UniTask> _asyncMessageReceiver;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public Subscription(AwaitableSubject<T> parent, Func<T, UniTask> asyncMessageReceiver)
            {
                _parent = parent;
                _asyncMessageReceiver = asyncMessageReceiver;
            }

            /// <summary>
            /// Dispose
            /// </summary>
            public void Dispose()
            {
                if (_parent._notifier == null) return;
                _parent._notifier = _parent._notifier.Remove(_asyncMessageReceiver);
            }
        }
    }
}