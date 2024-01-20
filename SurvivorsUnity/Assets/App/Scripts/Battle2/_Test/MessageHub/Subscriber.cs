using System;
using App.AppCommon.Core;
using App.Battle2.Core;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;

namespace App.Battle2._Test.MessageHub
{
    [ContainerRegisterAttribute2(typeof(Subscriber))]
    public class Subscriber
    {
        private readonly BattleEventHub2 eventHub2;
        private readonly CompositeDisposable _disposable = new();
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public Subscriber(BattleEventHub2 eventHub2, Publisher publisher)
        {
            this.eventHub2 = eventHub2;
            
            // publisher.Subject.Subscribe(async x =>
            // {
            //     await UniTask.Delay(TimeSpan.FromSeconds(5));
            //     Log.Debug(x);
            // }).AddTo(_disposable);
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // Log.Debug(_eventHub);
            // _eventHub.Subscribe<BattleEvents.TestEvent>(x =>
            // {
            //     Log.Debug(x.Id, x.Name);
            // }).AddTo(_disposable);
            eventHub2.Subscribe<BattleEvents2.TestAsyncEvent>(async x =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(3));
                Log.Debug(x.Id, x.Name);
            }).AddTo(_disposable);
        }
    }
}