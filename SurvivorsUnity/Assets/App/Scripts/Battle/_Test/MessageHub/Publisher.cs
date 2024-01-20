using System;
using App.AppCommon.Core;
using App.Battle.Core;
using Cysharp.Threading.Tasks;
using VContainer;

namespace App.Battle._Test.MessageHub
{
    /// <summary>
    /// Subscriber
    /// </summary>
    [ContainerRegister(typeof(Publisher))]
    public class Publisher
    {
        private readonly BattleEventHub _eventHub;

        public AwaitableSubject<int> Subject { get; private set; } = new AwaitableSubject<int>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public Publisher(BattleEventHub eventHub)
        {
            _eventHub = eventHub;
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public async UniTask SetupAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _eventHub.Publish(new BattleEvents.TestEvent
            {
                Id = 1,
                Name = "aa"
            });
            Log.Debug("publish after");
            await _eventHub.PublishAsync(new BattleEvents.TestAsyncEvent
            {
                Id = 2,
                Name = "bb"
            });
            Log.Debug("publishAsync after");

            Log.Debug("OnNextAsync before");
            await Subject.OnNextAsync(1);
            Log.Debug("OnNextAsync after");
        }
    }
}