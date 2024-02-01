using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Master.Battle.Core
{
    /// <summary>
    /// イベント管理 (非同期PubSub).
    /// publish時、すべてのSubscriberの非同期処理の終了待ちができる。
    /// Note : UniRx.AsyncMessageBrokerをUniTask版に改変している。
    /// Note: Donuts.AM3から
    /// </summary>
    public class AsyncMessageBroker : IDisposable
    {
        private bool _isDisposed = false;
        private readonly Dictionary<Type, object> _notifiers = new Dictionary<Type, object>();

        /// <summary>
        /// メッセージ発信
        /// </summary>
        public UniTask PublishAsync<T>(T message)
        {
            UniRx.InternalUtil.ImmutableList<Func<T, UniTask>> notifier;
            lock (_notifiers)
            {
                if (_isDisposed) throw new ObjectDisposedException("AsyncMessageBroker");

                if (_notifiers.TryGetValue(typeof(T), out object tmpOut))
                {
                    notifier = (UniRx.InternalUtil.ImmutableList<Func<T, UniTask>>)tmpOut;
                }
                else
                {
                    return UniTask.CompletedTask;
                }
            }

            var data = notifier.Data;
            var awaiter = new UniTask[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                awaiter[i] = data[i].Invoke(message);
            }
            return UniTask.WhenAll(awaiter);
        }

        /// <summary>
        /// メッセージ購読
        /// </summary>
        public IDisposable Subscribe<T>(Func<T, UniTask> asyncMessageReceiver) where T : IAsyncBattleEvent
        {
            lock (_notifiers)
            {
                if (_isDisposed) throw new ObjectDisposedException("AsyncMessageBroker");

                if (!_notifiers.TryGetValue(typeof(T), out object tmpOut))
                {
                    var notifier = UniRx.InternalUtil.ImmutableList<Func<T, UniTask>>.Empty;
                    notifier = notifier.Add(asyncMessageReceiver);
                    _notifiers.Add(typeof(T), notifier);
                }
                else
                {
                    var notifier = (UniRx.InternalUtil.ImmutableList<Func<T, UniTask>>)tmpOut;
                    notifier = notifier.Add(asyncMessageReceiver);
                    _notifiers[typeof(T)] = notifier;
                }
            }

            return new Subscription<T>(this, asyncMessageReceiver);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            lock (_notifiers)
            {
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    _notifiers.Clear();
                }
            }
        }

        /// <summary>
        /// 購読クラス
        /// </summary>
        public class Subscription<T> : IDisposable
        {
            readonly AsyncMessageBroker _parent;
            readonly Func<T, UniTask> _asyncMessageReceiver;

            public Subscription(AsyncMessageBroker parent, Func<T, UniTask> asyncMessageReceiver)
            {
                _parent = parent;
                _asyncMessageReceiver = asyncMessageReceiver;
            }

            /// <summary>
            /// Dispose
            /// </summary>
            public void Dispose()
            {
                lock (_parent._notifiers)
                {
                    if (_parent._notifiers.TryGetValue(typeof(T), out object tmpOut))
                    {
                        var notifier = (UniRx.InternalUtil.ImmutableList<Func<T, UniTask>>)tmpOut;
                        notifier = notifier.Remove(_asyncMessageReceiver);

                        _parent._notifiers[typeof(T)] = notifier;
                    }
                }
            }
        }
    }
}
