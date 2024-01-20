// using System;
// using App.AppCommon;
// using App.Core;
// using App.Game.EnemyBots;
// using App.Game.Turn;
// using App.Game.Units;
// using Cysharp.Threading.Tasks;
// using MessagePipe;
// using VContainer;
//
// namespace App.Game.Core
// {
//     /// <summary>
//     /// ゲームの進行管理
//     /// </summary>
//     [TurnEndOrder(999), PhaseEndOrder(999)]
//     [ContainerRegister(typeof(GameProgressManager), SceneType.Battle)]
//     public class GameProgressManager : GameProgressBehaviour, IDisposable
//     {
//         private readonly IDisposable _disposable;
//
//         private readonly GameState _gameState;
//         private readonly EnemyTurnManager _enemyTurnManager;
//         private readonly EventMessageSystem _eventMessageSystem;
//         private readonly WindManager _windManager;
//         private readonly WeatherManager _weatherManager;
//         private readonly BotLogicManager _botLogicManager;
//         
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public GameProgressManager(
//             GameState gameState,
//             EnemyTurnManager enemyTurnManager,
//             EventMessageSystem eventMessageSystem,
//             WindManager windManager,
//             WeatherManager weatherManager,
//             BotLogicManager botLogicManager,
//             ISubscriber<IGameEvent> eventSub
//         )
//         {
//             _gameState = gameState;
//             _enemyTurnManager = enemyTurnManager;
//             _eventMessageSystem = eventMessageSystem;
//             _windManager = windManager;
//             _weatherManager = weatherManager;
//             _botLogicManager = botLogicManager;
//
//             var bag = DisposableBag.CreateBuilder();
//             eventSub.Subscribe(msg =>
//             {
//                 switch (msg)
//                 {
//                     case EventMessages.OnShipMovedEvent evt:
//                     {
//                         OnShipMoved(evt.Ship);
//                         break;
//                     }
//                     case EventMessages.OnEnemyMovedEvent evt:
//                     {
//                         OnEnemyMoved(evt.Enemy);
//                         break;
//                     }
//                     case EventMessages.OnShipAttackedEvent evt:
//                     {
//                         OnShipAttacked(evt.Args);
//                         break;
//                     }
//                     case EventMessages.OnEnemyAttackedEvent evt:
//                     {
//                         OnEnemyAttacked(evt.Args);
//                         break;
//                     }
//                 }
//             }).AddTo(bag);
//             _disposable = bag.Build();
//         }
//
//         /// <summary>
//         /// ゲーム開始
//         /// </summary>
//         public void StartGame()
//         {
//             StartRound();
//             _eventMessageSystem.AddEvent(new ProgressEventMessages.PhaseStartEvent
//             {
//                 StartPhase = PhaseType.PlayerPhase,
//             });
//         }
//
//         /// <summary>
//         /// ラウンドをスタートさせる
//         /// </summary>
//         private void StartRound()
//         {
//             _eventMessageSystem.AddEvent(new ProgressEventMessages.RoundStartEvent
//             {
//             });
//             
//             // UniTask.Void(async () =>
//             // {
//             //     await UniTask.Delay(TimeSpan.FromSeconds(1));
//             // });
//             StartTurn();
//         }
//
//         /// <summary>
//         /// ターンをスタートさせる
//         /// </summary>
//         private void StartTurn()
//         {
//             _eventMessageSystem.AddEvent(new ProgressEventMessages.TurnStartEvent
//             {
//                 Wind = _windManager.NextValue,
//                 Weather = _weatherManager.NextValue,
//                 Enemy = _enemyTurnManager.NextValue,
//             });
//         }
//
//         /// <inheritdoc />
//         public override void OnTurnEnd()
//         {
//             if (_gameState.RoundTurnRemaining == 0)
//             {
//                 //残りターン数がない
//                 //ラウンド終了させる
//                 _eventMessageSystem.AddEvent(new ProgressEventMessages.RoundEndEvent
//                 {
//                 });
//             }
//             else
//             {
//                 StartTurn();
//             }
//         }
//
//         /// <inheritdoc />
//         public override void OnRoundEnd()
//         {
//             StartRound();
//         }
//
//         /// <inheritdoc />
//         public override void OnPhaseEnd(PhaseType endPhase, uint actionUnitId)
//         {
//             var nextTurn = PhaseType.None;
//             if (endPhase == PhaseType.PlayerPhase)
//             {
//                 //プレイヤーターン終了
//                 nextTurn = PhaseType.EnemyPhase;
//             }
//             else if (endPhase == PhaseType.EnemyPhase)
//             {
//                 //敵ターン終了
//                 nextTurn = PhaseType.PlayerPhase;
//                 _eventMessageSystem.AddEvent(new ProgressEventMessages.TurnEndEvent
//                 {
//                 });
//             }
//
//             _eventMessageSystem.AddEvent(new ProgressEventMessages.PhaseStartEvent()
//             {
//                 StartPhase = nextTurn,
//             });
//         }
//
//         /// <inheritdoc />
//         public override void OnPhaseStart(PhaseType phaseType)
//         {
//             if (phaseType == PhaseType.EnemyPhase)
//             {
//                 _botLogicManager.BotsActionAsync().Forget();
//             }
//         }
//
//         /// <summary>
//         /// 船移動
//         /// </summary>
//         private void OnShipMoved(ShipUnitModel shipUnitModel)
//         {
//             if (_gameState.ActionUnitId == GameConst.InvalidUnitId)
//             {
//                 _eventMessageSystem.AddEvent(new EventMessages.OnActionStartEvent
//                 {
//                     ShipUnitId = shipUnitModel.UnitId,
//                 });
//             }
//             // _eventMessageSystem.AddEvent(new EventMessages.OnShipSelectedEvent
//             // {
//             //     ShipUnitId = shipUnitModel.UnitId
//             // });
//         }
//
//         /// <summary>
//         /// 敵移動
//         /// </summary>
//         private void OnEnemyMoved(EnemyUnitModel enemyUnitModel)
//         {
//         }
//
//         /// <summary>
//         /// 船攻撃
//         /// </summary>
//         private void OnShipAttacked(EventMessages.AttackArgs args)
//         {
//             if (args.AttackerUnit is not ShipUnitModel shipUnitModel)
//             {
//                 return; 
//             }
//
//             if (_gameState.ActionUnitId == GameConst.InvalidUnitId)
//             {
//                 _eventMessageSystem.AddEvent(new EventMessages.OnActionStartEvent
//                 {
//                     ShipUnitId = shipUnitModel.UnitId,
//                 });
//             }
//             
//             if (!args.TargetUnit.IsAlive)
//             {
//                 _eventMessageSystem.AddEvent(new EventMessages.OnEnemyDefeatedEvent
//                 {
//                     EnemyUnitId = args.TargetUnit.UnitId,
//                 });
//             }
//             if (!shipUnitModel.IsMovable)
//             {
//                 //味方フェイズを終了
//                 _eventMessageSystem.AddEvent(new ProgressEventMessages.PhaseEndEvent
//                 {
//                     EndPhase = PhaseType.PlayerPhase,
//                     ActionUnitId = shipUnitModel.UnitId,
//                 });
//             }
//         }
//
//         /// <summary>
//         /// 敵攻撃
//         /// </summary>
//         private void OnEnemyAttacked(EventMessages.AttackArgs args)
//         {
//         }
//         
//         /// <summary>
//         /// Dispose
//         /// </summary>
//         public void Dispose()
//         {
//             _disposable.Dispose();
//         }
//     }
// }