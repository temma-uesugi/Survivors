// using System;
// using System.Collections.Generic;
// using System.Linq;
// using App.AppCommon;
// using App.Core;
// using App.Game.Common;
// using App.Game.Core;
// using App.Game.Facades;
// using App.Game.Libs;
// using App.Game.Map;
// using App.Game.Objects;
// using App.Game.Units;
// using UniRx;
// using MessagePipe;
// using VContainer;
//
// namespace App.Game.UI
// {
//     /// <summary>
//     /// AttackのCommander
//     /// </summary>
//     [ContainerRegister(typeof(AttackCommander), SceneType.Battle)]
//     public class AttackCommander : ControllerCommandBase
//     {
//         private readonly HexMapAttackChecker _attackChecker;
//
//         private readonly List<IUnitModel> _slashTargetList = new();
//         private readonly List<IUnitModel> _bombTargetList = new();
//         private readonly ReactiveProperty<List<IUnitModel>> _slashTargets;
//         private readonly ReactiveProperty<IUnitModel> _assaultTarget = new();
//         private readonly ReactiveProperty<List<IUnitModel>> _bombTargets;
//
//         public DelegateCommandRx<List<IUnitModel>, uint> CommandAttack { get; }
//         public DelegateCommandRx<IUnitModel> CommandAssault { get; }
//         public DelegateCommandRx<List<IUnitModel>, uint> CommandBomb { get; }
//
//         private readonly IDisposable _disposable;
//
//         private readonly Subject<bool> _onActivate = new();
//         public IObservable<bool> OnActivate => _onActivate;
//
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public AttackCommander(
//             GameState gameState,
//             BattleCamera battleCamera,
//             EventMessageSystem eventMessageSystem,
//             UnitManger unitManger,
//             HexMapAttackChecker attackChecker,
//             AttackPadView attackPadView
//         ) : base(gameState, battleCamera, eventMessageSystem, unitManger)
//         {
//             _attackChecker = attackChecker;
//             
//             _slashTargets = new(_slashTargetList);
//             _bombTargets = new(_bombTargetList);
//
//             //コマンド定義
//             CommandAttack = new DelegateCommandRx<List<IUnitModel>, uint>(
//                 SlashAttack,
//                 _slashTargets,
//                 list => list.Count > 0
//             );
//             CommandAssault = new DelegateCommandRx<IUnitModel>(
//                 AssaultAttack,
//                 _assaultTarget,
//                 obj => obj != null
//             );
//             CommandBomb = new DelegateCommandRx<List<IUnitModel>, uint>(
//                 BombAttack,
//                 _bombTargets,
//                 list => list.Count > 0
//             );
//
//             attackPadView.Setup(this);
//
//             var bag = DisposableBag.CreateBuilder();
//             // eventSub.Subscribe(msg =>
//             // {
//             //     switch (msg)
//             //     {
//             //         case EventMessages.OnShipMovedEvent evt:
//             //         {
//             //             OnShipMoved(evt.Ship);
//             //             break;
//             //         }
//             //         case EventMessages.OnShipAttackedEvent evt:
//             //         {
//             //             OnShipAttacked(evt.Args);
//             //             break;
//             //         }
//             //     }
//             // })
//             // .AddTo(bag);
//             _disposable = bag.Build();
//         }
//
//         /// <summary>
//         /// ターゲット更新
//         /// </summary>
//         private void UpdateTargets(HexCell cell, DirectionType dir)
//         {
//             UpdateAssaultAttackTargets(cell, dir);
//             UpdateSlashAttackTargets(cell, dir);
//             UpdateBombAttackTargets();
//         }
//
//         /// <summary>
//         /// 斬り込み対象の更新
//         /// </summary>
//         private void UpdateSlashAttackTargets(HexCell cell, DirectionType dir)
//         {
//             var attackTargetUnits = _attackChecker.GetAttackTargetUnits(cell, dir);
//             _slashTargetList.Clear();
//             _slashTargetList.AddRange(attackTargetUnits.Select(x => x));
//             _slashTargets.SetValueAndForceNotify(_slashTargetList);
//         }
//
//         /// <summary>
//         /// 突撃対象を更新
//         /// </summary>
//         private void UpdateAssaultAttackTargets(HexCell cell, DirectionType dir)
//         {
//             var assaultUnit = _attackChecker.GetAssaultTargetUnit(cell, dir);
//             _assaultTarget.Value = assaultUnit;
//         }
//
//         /// <summary>
//         /// 砲撃対象を更新
//         /// </summary>
//         private void UpdateBombAttackTargets()
//         {
//             _bombTargetList.Clear();
//             var rightBombTargets = _attackChecker.GetBombTargetUnits(BombSide.Right, currentShipModel.UnitId, currentShipModel.Cell.Value,
//                 currentShipModel.Direction.Value, currentShipModel.RightBombStatus.Value);
//             var leftBombTargets = _attackChecker.GetBombTargetUnits(BombSide.Left, currentShipModel.UnitId, currentShipModel.Cell.Value,
//                 currentShipModel.Direction.Value, currentShipModel.LeftBombStatus.Value);
//             _bombTargetList.AddRange(rightBombTargets);
//             _bombTargetList.AddRange(leftBombTargets);
//             _bombTargets.SetValueAndForceNotify(_bombTargetList);
//         }
//         
//         /// <summary>
//         /// 選択船更新
//         /// </summary>
//         protected override void OnSelectedShipUpdate()
//         {
//             if (gameState.ActionUnitId != GameConst.InvalidUnitId && currentShipModel.UnitId != gameState.ActionUnitId)
//             {
//                 return;
//             }
//             if (gameState.ActionUnitId == GameConst.InvalidUnitId)
//             {
//                 _onActivate.OnNext(true); 
//             }
//             // UpdateTargets(currentShipModel.Cell.Value, currentShipModel.Direction.Value);
//             // UpdatePosition();
//         }
//
//         /// <summary>
//         /// 選択解除
//         /// </summary>
//         protected override void OnUnselectedShip()
//         {
//             _onActivate.OnNext(false);
//         }
//         
//         /// <summary>
//         /// 切り込み
//         /// </summary>
//         private void SlashAttack(uint enemyUnitId)
//         {
//             if (_slashTargetList.All(x => x.UnitId != enemyUnitId))
//             {
//                 Log.Error("Invalid SlashAttack Target");
//                 return; 
//             }
//             // EventSystem.AddEvent(new EventMessages.ActionStartEvent
//             // {
//             //     ShipUnitId = CurrentShipModel.UnitId,
//             // });
//             AttackFacade.ShipAttack(currentShipModel.UnitId, enemyUnitId, AttackType.Slash);
//         }
//
//         /// <summary>
//         /// 突撃
//         /// </summary>
//         private void AssaultAttack()
//         {
//             if (!_assaultTarget.HasValue)
//             {
//                 Log.Error("Invalid AssaultAttack Target");
//                 return; 
//             }
//             eventMessageSystem.AddEvent(new EventMessages.OnActionStartEvent
//             {
//                 ShipUnitId = currentShipModel.UnitId,
//             });
//             AttackFacade.ShipAttack(currentShipModel.UnitId, _assaultTarget.Value.UnitId, AttackType.Assault);
//         }
//
//         /// <summary>
//         /// 砲撃
//         /// </summary>
//         private void BombAttack(uint enemyUnitId)
//         {
//             if (_bombTargetList.All(x => x.UnitId != enemyUnitId))
//             {
//                 Log.Error("Invalid BombAttack Target");
//                 return; 
//             }
//             eventMessageSystem.AddEvent(new EventMessages.OnActionStartEvent
//             {
//                 ShipUnitId = currentShipModel.UnitId,
//             });
//             AttackFacade.ShipAttack(currentShipModel.UnitId, enemyUnitId, AttackType.Bomb);
//         }
//
//         /// <summary>
//         /// 味方フェイズ終了
//         /// </summary>
//         protected override void OnAllyPhaseEnd()
//         {
//             //攻撃対象
//             _slashTargetList.Clear();
//             _slashTargets.SetValueAndForceNotify(_slashTargetList);
//
//             //砲撃対象
//             _bombTargetList.Clear();
//             _bombTargets.SetValueAndForceNotify(_bombTargetList);
//
//             //突撃対象
//             _assaultTarget.Value = null;
//         }
//
//         /// <summary>
//         /// 船移動
//         /// </summary>
//         public void OnShipMoved(ShipUnitModel shipUnitModel)
//         {
//             if (shipUnitModel.UnitId != currentShipModel.UnitId)
//             {
//                 return;
//             }
//             _onActivate.OnNext(true);
//             UpdateTargets(shipUnitModel.Cell.Value, shipUnitModel.Direction.Value);
//             UpdatePosition();
//         }
//
//         /// <summary>
//         /// 船攻撃
//         /// </summary>
//         private void OnShipAttacked(EventMessages.AttackArgs args)
//         {
//             _onActivate.OnNext(false);
//         }
//         
//         
//         /// <summary>
//         /// Dispose
//         /// </summary>
//         public override void Dispose()
//         {
//             base.Dispose(); 
//             _disposable.Dispose();
//         }
//     }
// }