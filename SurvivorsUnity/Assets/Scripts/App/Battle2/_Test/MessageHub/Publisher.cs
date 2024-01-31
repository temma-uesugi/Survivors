using System;
using App.AppCommon.Core;
using App.Battle2.Core;
using Cysharp.Threading.Tasks;
using VContainer;

namespace App.Battle2._Test.MessageHub
{
    /// <summary>
    /// Subscriber
    /// </summary>
    [ContainerRegisterAttribute2(typeof(Publisher))]
    public class Publisher
    {
        private readonly BattleEventHub2 eventHub2;

        public AwaitableSubject<int> Subject { get; private set; } = new AwaitableSubject<int>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public Publisher(BattleEventHub2 eventHub2)
        {
            this.eventHub2 = eventHub2;
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public async UniTask SetupAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            eventHub2.Publish(new BattleEvents2.TestEvent
            {
                Id = 1,
                Name = "aa"
            });
            Log.Debug("publish after");
            await eventHub2.PublishAsync(new BattleEvents2.TestAsyncEvent
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