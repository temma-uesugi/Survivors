using System;
using App.AppCommon.Core;
using App.Battle2.Core;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2._Test.MessageHub
{
    [ContainerRegisterMonoBehaviourAttribute2(typeof(SubscriberObject))]
    public class SubscriberObject : MonoBehaviour
    {
        private BattleEventHub2 eventHub2;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(BattleEventHub2 eventHub2, Publisher publisher)
        {
            this.eventHub2 = eventHub2;

            // publisher.Subject.Subscribe(async x =>
            // {
            //     await UniTask.Delay(TimeSpan.FromSeconds(3));
            //     Log.Debug("object", x);
            // }).AddTo(this);
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup()
        {
            // Log.Debug(_eventHub);
            // _eventHub.Subscribe<BattleEvents.TestEvent>(x =>
            // {
            //     Log.Debug("object", x.Id, x.Name);
            // }).AddTo(this);
            eventHub2.Subscribe<BattleEvents2.TestAsyncEvent>(async x =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(5));
                Log.Debug("object", x.Id, x.Name);
            }).AddTo(this);
        }
    }
}