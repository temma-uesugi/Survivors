// using System;
// using Cysharp.Threading.Tasks;
// using UniRx;
// using VContainer;
//
// namespace App.Battle.Core
// {
//     public class EventHub : IDisposable
//     {
//         private readonly MessageBroker _messageBroker;
//         private readonly AsyncMessageBroker _asyncMessageBroker;
//        
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public EventHub()
//         {
//             _messageBroker = new MessageBroker();
//             _asyncMessageBroker = new AsyncMessageBroker();
//         }
//        
//         /// <summary>
//         /// メッセージ発信
//         /// </summary>
//         public void Publish<T>(T evt) where T : IEventBase
//         {
//             _messageBroker.Publish(evt);
//         }
//
//         /// <summary>
//         /// メッセージ購読
//         /// </summary>
//         public IDisposable Subscribe<T>(Action<T> receiver) where T : IEventBase
//         {
//             return _messageBroker.Receive<T>().Subscribe(receiver);
//         }
//
//         /// <summary>
//         /// メッセージ発信 (非同期)
//         /// </summary>
//         public UniTask PublishAsync<T>(T evt) where T : IAsyncEventBase
//         {
//             return _asyncMessageBroker.PublishAsync(evt);
//         }
//
//         /// <summary>
//         /// 非同期のメッセージ購読
//         /// </summary>
//         public IDisposable Subscribe<T>(Func<T, UniTask> asyncMessageReceiver) where T : IAsyncEventBase
//         {
//             return _asyncMessageBroker.Subscribe(asyncMessageReceiver);
//         }
//         
//         /// <inheritdoc/>
//         public void Dispose()
//         {
//             _messageBroker.Dispose();
//             _asyncMessageBroker.Dispose();
//         }
//     }
//     
//     /// <summary>
//     /// 送信するEventのinterface
//     /// </summary>
//     public interface IEventBase { }
//
//     /// <summary>
//     /// 非同期送信するEventのinterface
//     /// </summary>
//     public interface IAsyncEventBase { }
// }