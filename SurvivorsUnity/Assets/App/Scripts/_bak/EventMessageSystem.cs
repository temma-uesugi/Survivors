// using System;
// using System.Collections.Generic;
// using App.AppCommon;
// using App.Core;
// using MessagePipe;
// using VContainer;
//
// namespace App.Game.Core
// {
//     /// <summary>
//     /// イベント管理 (PubSub)
//     /// </summary>
//     [ContainerRegister(typeof(EventMessageSystem), SceneType.Battle)]
//     public class EventMessageSystem : IDisposable
//     {
//         private readonly Queue<IGameEvent> _eventQueue = new();
//         private bool _isRunningEvents = false;
//         private bool _disposed = false;
//
//         private readonly IPublisher<IGameEvent> _eventPub;
//         private readonly IPublisher<uint, IShipGameEvent> _shipEventPub;
//         private readonly IPublisher<uint, IEnemyGameEvent> _enemyEventPub;
//
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public EventMessageSystem(
//             IPublisher<IGameEvent> eventPub,
//             IPublisher<uint, IShipGameEvent> shipEventPub,
//             IPublisher<uint, IEnemyGameEvent> enemyEventPub
//         )
//         {
//             _eventPub = eventPub;
//             _shipEventPub = shipEventPub;
//             _enemyEventPub = enemyEventPub;
//         }
//
//         /// <summary>
//         /// イベント追加
//         /// </summary>
//         public void AddEvent(IGameEvent gameEvent)
//         {
//             _eventPub.Publish(gameEvent);
//
//             //ShipEvent,EnemyEventはID指定でも飛ばす
//             if (gameEvent is IShipGameEvent shipGameEvent && shipGameEvent.ShipUnitId != GameConst.InvalidUnitId)
//             {
//                 _shipEventPub.Publish(shipGameEvent.ShipUnitId, shipGameEvent);
//             }
//             if (gameEvent is IEnemyGameEvent enemyGameEvent && enemyGameEvent.UnitId != GameConst.InvalidUnitId)
//             {
//                 _enemyEventPub.Publish(enemyGameEvent.UnitId, enemyGameEvent);
//             }
//             
//             if (gameEvent is IProgressGameEvent progressEvt)
//             {
//                 switch (progressEvt)
//                 {
//                     // case ProgressEventMessages.TurnStartEvent e:
//                     // {
//                     //     _progressSystem.TurnStart(e.Wind, e.Weather, e.Enemy);
//                     //     break;
//                     // }
//                     // case ProgressEventMessages.TurnEndEvent e:
//                     // {
//                     //     _progressSystem.TurnEnd();
//                     //     break;
//                     // }
//                     // case ProgressEventMessages.PhaseStartEvent e:
//                     // {
//                     //     _progressSystem.PhaseStart(e.StartPhase);
//                     //     break;
//                     // }
//                     // case ProgressEventMessages.PhaseEndEvent e:
//                     // {
//                     //     _progressSystem.PhaseEnd(e.EndPhase, e.ActionUnitId);
//                     //     break;
//                     // }
//                 }
//             }
//         }
//
//         // /// <summary>
//         // /// イベント追加
//         // /// </summary>
//         // public void AddEvent(IGameEvent gameEvent)
//         // {
//         //     _eventQueue.Enqueue(gameEvent);
//         //     if (!_isRunningEvents)
//         //     {
//         //         Log.Debug("koko", gameEvent.ToString());
//         //         PublishEvents();
//         //     }
//         // }
//         
//         /// <summary>
//         /// イベント追加
//         /// </summary>
//         public void AddEvent(params IGameEvent[] gameEvents)
//         {
//             foreach (var evt in gameEvents)
//             {
//                 _eventQueue.Enqueue(evt);
//             }
//             if (!_isRunningEvents)
//             {
//                 PublishEvents();
//             }
//         }
//          
//         
//         /// <summary>
//         /// イベントのPublish
//         /// </summary>
//         private void PublishEvents()
//         {
//             _isRunningEvents = true;
//             while (_eventQueue.TryDequeue(out var evt))
//             {
//                 if (_disposed)
//                 {
//                     break;
//                 }
//
//                 //eventのPublish
//                 _eventPub.Publish(evt);
//
//                 //ShipEvent,EnemyEventはID指定でも飛ばす
//                 if (evt is IShipGameEvent shipGameEvent && shipGameEvent.ShipUnitId != GameConst.InvalidUnitId)
//                 {
//                     _shipEventPub.Publish(shipGameEvent.ShipUnitId, shipGameEvent);
//                 }
//                 if (evt is IEnemyGameEvent enemyGameEvent && enemyGameEvent.UnitId != GameConst.InvalidUnitId)
//                 {
//                     _enemyEventPub.Publish(enemyGameEvent.UnitId, enemyGameEvent);
//                 }
//                 
//                 if (evt is IProgressGameEvent progressEvt)
//                 {
//                     switch (progressEvt)
//                     {
//                         // case ProgressEventMessages.TurnStartEvent e:
//                         // {
//                         //     _progressSystem.TurnStart(e.Wind, e.Weather, e.Enemy);
//                         //     break;
//                         // }
//                         // case ProgressEventMessages.TurnEndEvent e:
//                         // {
//                         //     _progressSystem.TurnEnd();
//                         //     break;
//                         // }
//                         // case ProgressEventMessages.PhaseStartEvent e:
//                         // {
//                         //     _progressSystem.PhaseStart(e.StartPhase);
//                         //     break;
//                         // }
//                         // case ProgressEventMessages.PhaseEndEvent e:
//                         // {
//                         //     _progressSystem.PhaseEnd(e.EndPhase, e.ActionUnitId);
//                         //     break;
//                         // }
//                     }
//                 }
//             }
//             _isRunningEvents = false;
//         }
//
//         /// <inheritdoc/>
//         public void Dispose()
//         {
//             _disposed = true;
//         }
//     }
// }
