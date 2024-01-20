using System;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;

namespace App.Battle2.Core
{
   
    [ContainerRegisterAttribute2(typeof(BattleEventHub2))]
    public class BattleEventHub2 : IDisposable
    {
        private readonly MessageBroker _messageBroker;
        private readonly AsyncMessageBroker2 asyncMessageBroker2;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public BattleEventHub2()
        {
            _messageBroker = new MessageBroker();
            asyncMessageBroker2 = new AsyncMessageBroker2();
        }

        /// <summary>
        /// メッセージ発信
        /// </summary>
        public void Publish<T>(T evt) where T : IBattleEvent
        {
            _messageBroker.Publish(evt);
        }

        /// <summary>
        /// メッセージ購読
        /// </summary>
        public IDisposable Subscribe<T>(Action<T> receiver) where T : IBattleEvent
        {
            return _messageBroker.Receive<T>().Subscribe(receiver);
        }

        /// <summary>
        /// メッセージ発信 (非同期)
        /// </summary>
        public UniTask PublishAsync<T>(T evt) where T : IAsyncBattleEvent
        {
            return asyncMessageBroker2.PublishAsync(evt);
        }

        /// <summary>
        /// 非同期のメッセージ購読
        /// </summary>
        public IDisposable Subscribe<T>(Func<T, UniTask> asyncMessageReceiver) where T : IAsyncBattleEvent
        {
            return asyncMessageBroker2.Subscribe(asyncMessageReceiver);
        }
        
        /// <inheritdoc/>
        public void Dispose()
        {
            _messageBroker.Dispose();
            asyncMessageBroker2.Dispose();
        }
    }
  
    /// <summary>
    /// Battle内で送信するEventのinterface
    /// </summary>
    public interface IBattleEvent { }

    /// <summary>
    /// Battle内で非同期送信するEventのinterface
    /// </summary>
    public interface IAsyncBattleEvent { }
}