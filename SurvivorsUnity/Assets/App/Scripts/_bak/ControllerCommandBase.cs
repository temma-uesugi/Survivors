// using System;
// using App.AppCommon;
// using App.Game.Core;
// using App.Game.Units;
// using UniRx;
// using MessagePipe;
//
// namespace App.Game.UI
// {
//
//     /// <summary>
//     /// ControllerCommandの基本クラス
//     /// </summary>
//     [PhaseEndOrder(9)]
//     public abstract class ControllerCommandBase : GameProgressBehaviour, IDisposable
//     {
//         private readonly IDisposable _disposable;
//
//         protected readonly GameState gameState;
//         protected readonly EventMessageSystem eventMessageSystem;
//         private readonly UnitManger _unitManger;
//
//         protected ShipUnitModel currentShipModel;
//
//         //船選択
//         protected abstract void OnSelectedShipUpdate();
//         //船選択解除
//         protected abstract void OnUnselectedShip();
//         //味方フェイズ終了
//         protected abstract void OnAllyPhaseEnd();
//
//         //ポジション移動
//         private readonly Subject<Unit> _onPositionChanged = new();
//         public IObservable<Unit> OnPositionChanged => _onPositionChanged;
//         
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         protected ControllerCommandBase(
//             GameState gameState,
//             BattleCamera battleCamera,
//             EventMessageSystem eventMessageSystem,
//             UnitManger unitManger
//         )
//         {
//             this.gameState = gameState;
//             this.eventMessageSystem = eventMessageSystem;
//             _unitManger = unitManger;
//             
//             var bag = DisposableBag.CreateBuilder();
//
//             gameState.SelectedShipUnit.Subscribe(UpdateSelectedShip).AddTo(bag);
//             // var bag = DisposableBag.CreateBuilder();
//             // eventSub.Subscribe(x =>
//             // {
//             //     switch (x)
//             //     {
//             //         case EventMessages.OnUnitSelectedEvent evt:
//             //         {
//             //             if (IsActionableShip(evt))
//             //             {
//             //                 OnSelectedShipUpdate();
//             //             }
//             //             else
//             //             {
//             //                 OnUnselectedShip();
//             //             }
//             //             break;
//             //         }
//             //     }
//             // }).AddTo(bag);
//
//             //カメラ移動
//             battleCamera.Position.Where(_ => currentShipModel != null).Subscribe(_ =>
//             {
//                 _onPositionChanged.OnNext(Unit.Default);
//             }).AddTo(bag);
//             _disposable = bag.Build();
//         }
//
//         /// <summary>
//         /// 選択Shipの更新
//         /// </summary>
//         private void UpdateSelectedShip(ShipUnitModel shipModel)
//         {
//             if (shipModel == null)
//             {
//                 OnUnselectedShip(); 
//             }
//             else
//             {
//                 OnSelectedShipUpdate();
//             }
//         }
//         
//         /// <summary>
//         /// 位置調整
//         /// </summary>
//         protected void UpdatePosition()
//         {
//             _onPositionChanged.OnNext(Unit.Default);
//         }
//
//         // /// <summary>
//         // /// ViewModelの更新
//         // /// </summary>
//         // private bool IsActionableShip(EventMessages.OnUnitSelectedEvent evt)
//         // {
//         //     if (evt.UnitModel is not ShipUnitModel shipModel)
//         //     {
//         //         return false;
//         //     }
//         //     if (gameState.ActionUnitId != GameConst.InvalidUnitId && gameState.ActionUnitId != evt.UnitId)
//         //     {
//         //         return false;
//         //     }
//         //
//         //     currentShipModel = shipModel;
//         //     return !evt.UnitModel.IsActionEnd.Value;
//         // }
//
//         /// <summary>
//         /// ターン終了
//         /// </summary>
//         public override void OnPhaseEnd(PhaseType endPhase, uint actionUnitId)
//         {
//             if (endPhase != PhaseType.PlayerPhase)
//             {
//                 return;
//             }
//             currentShipModel = null;
//             OnAllyPhaseEnd();
//         }
//
//         /// <summary>
//         /// 破棄
//         /// </summary>
//         public virtual void Dispose()
//         {
//             _disposable.Dispose();
//         }
//     }
// }