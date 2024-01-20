using System;
using App.AppCommon.Core;
using App.Battle.Core;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;

namespace App.Battle._Test.MessageHub
{
    [ContainerRegister(typeof(Subscriber))]
    public class Subscriber
    {
        private readonly BattleEventHub _eventHub;
        private readonly CompositeDisposable _disposable = new();
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public Subscriber(BattleEventHub eventHub, Publisher publisher)
        {
            _eventHub = eventHub;
            
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
            _eventHub.Subscribe<BattleEvents.TestAsyncEvent>(async x =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(3));
                Log.Debug(x.Id, x.Name);
            }).AddTo(_disposable);
        }
    }
}