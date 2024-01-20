using System;
using App.AppCommon.Core;
using App.Battle.Core;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle._Test.MessageHub
{
    [ContainerRegisterMonoBehaviour(typeof(SubscriberObject))]
    public class SubscriberObject : MonoBehaviour
    {
        private BattleEventHub _eventHub;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(BattleEventHub eventHub, Publisher publisher)
        {
            _eventHub = eventHub;

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
            _eventHub.Subscribe<BattleEvents.TestAsyncEvent>(async x =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(5));
                Log.Debug("object", x.Id, x.Name);
            }).AddTo(this);
        }
    }
}