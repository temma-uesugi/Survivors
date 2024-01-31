// using System;
// using App.AppCommon;
// using App.Core;
// using App.Game.Common;
// using App.Game.Core;
// using App.Game.Facades;
// using App.Game.Libs;
// using App.Game.Map;
// using App.Game.Objects;
// using App.Game.Operations;
// using App.Game.Units;
// using MessagePipe;
// using UniRx;
// using VContainer;
//
// namespace App.Game.UI
// {
//     /// <summary>
//     /// 移動Commander
//     /// </summary>
//     [RoundStartOrder(9)]
//     [ContainerRegister(typeof(MoveCommander), SceneType.Battle)]
//     public class MoveCommander : ControllerCommandBase
//     {
//         // /// <summary>
//         // /// 移動ステータス
//         // /// </summary>
//         // public struct MovableStatus
//         // {
//         //     public bool Movable { get; init; }
//         //     public double NeedPower { get; init; }
//         //     public MovableStatus(bool movable, double needPower)
//         //     {
//         //         Movable = movable;
//         //         NeedPower = needPower;
//         //     }
//         // }
//
//         private readonly CompositeDisposable _compositeDisposable = new();
//         private readonly HexMapMoveChecker _moveChecker;
//
//         private readonly Subject<ShipUnitModel> _onShipSelected = new();
//         public IObservable<ShipUnitModel> OnShipSelected => _onShipSelected;
//
//         private readonly ReactiveProperty<MovableStatus> _movableRight = new();
//         private readonly ReactiveProperty<MovableStatus> _movableTopRight = new();
//         private readonly ReactiveProperty<MovableStatus> _movableTopLeft = new();
//         private readonly ReactiveProperty<MovableStatus> _movableLeft = new();
//         private readonly ReactiveProperty<MovableStatus> _movableBottomLeft = new();
//         private readonly ReactiveProperty<MovableStatus> _movableBottomRight = new();
//
//         private readonly ReactiveProperty<bool> _isMovable = new();
//         public IReadOnlyReactiveProperty<bool> IsMovable => _isMovable;
//
//         public DelegateCommandRx<MovableStatus> CommandRight { get; }
//         public DelegateCommandRx<MovableStatus>  CommandTopRight { get; }
//         public DelegateCommandRx<MovableStatus>  CommandTopLeft { get; }
//         public DelegateCommandRx<MovableStatus>  CommandLeft { get; }
//         public DelegateCommandRx<MovableStatus>  CommandBottomLeft { get; }
//         public DelegateCommandRx<MovableStatus>  CommandBottomRight { get; }
//
//         public DirectionType CurrentDir => currentShipModel.Direction.Value;
//
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public MoveCommander(
//             GameState gameState,
//             BattleCamera battleCamera,
//             EventMessageSystem eventMessageSystem,
//             UnitManger unitManger,
//             HexMapMoveChecker moveChecker,
//             MovePadView movePadView
//         ) : base(gameState, battleCamera, eventMessageSystem, unitManger)
//         {
//             _moveChecker = moveChecker;
//
//             //Command定義
//             CommandRight = new DelegateCommandRx<MovableStatus> (
//                 () => InputMove(DirectionType.Right),
//                 _movableRight,
//                 status => status.Movable
//             );
//             CommandTopRight = new DelegateCommandRx<MovableStatus> (
//                 () => InputMove(DirectionType.TopRight),
//                 _movableTopRight,
//                 status => status.Movable
//             );
//             CommandTopLeft = new DelegateCommandRx<MovableStatus> (
//                 () => InputMove(DirectionType.TopLeft),
//                 _movableTopLeft,
//                 status => status.Movable
//             );
//             CommandLeft = new DelegateCommandRx<MovableStatus> (
//                 () => InputMove(DirectionType.Left),
//                 _movableLeft,
//                 status => status.Movable
//             );
//             CommandBottomLeft = new DelegateCommandRx<MovableStatus> (
//                 () => InputMove(DirectionType.BottomLeft),
//                 _movableBottomLeft,
//                 status => status.Movable
//             );
//             CommandBottomRight = new DelegateCommandRx<MovableStatus> (
//                 () => InputMove(DirectionType.BottomRight),
//                 _movableBottomRight,
//                 status => status.Movable
//             );
//
//             movePadView.Setup(this);
//
//             // actionInputs.OnMoveDirSelect
//             //     .Subscribe(SelectMoveDir)
//             //     .AddTo(_compositeDisposable);
//         }
//
//         /// <summary>
//         /// 移動方向の選択
//         /// </summary>
//         private void SelectMoveDir(InputDirectionType inputDir)
//         {
//             var dir = HexUtil.InputDirToMoveDir(inputDir, currentShipModel.Direction.Value);
//             InputMove(dir);
//         }
//         
//         /// <summary>
//         /// 移動入力
//         /// </summary>
//         private void InputMove(DirectionType dir)
//         {
//             if (currentShipModel == null)
//             {
//                 return;
//             }
//             MoveFacade.InputMove(currentShipModel.UnitId, dir);
//             UpdateStatus(currentShipModel);
//         }
//
//         /// <summary>
//         /// 状況更新
//         /// </summary>
//         private void UpdateStatus(ShipUnitModel shipUnitModel)
//         {
//             //TODO OnSelectedShipUpdate, OnShipMoved 両方から呼ばれる
//             // if (shipUnitModel.UnitId != currentShipModel.UnitId)
//             // {
//             //     return;
//             // }
//             // ShipMovableDirUpdated(shipUnitModel.MovableDirection.Value);
//             // _isMovable.Value = shipUnitModel.Status.MovePower.Current >= 1 && !shipUnitModel.IsActionEnd.Value;
//             // UpdatePosition();
//         }
//
//         /// <summary>
//         /// 選択船更新
//         /// </summary>
//         protected override void OnSelectedShipUpdate()
//         {
//             if (currentShipModel == null)
//             {
//                 _isMovable.Value = false;
//             }
//             _onShipSelected.OnNext(currentShipModel);
//             UpdateStatus(currentShipModel);
//         }
//
//         /// <summary>
//         /// 選択解除
//         /// </summary>
//         protected override void OnUnselectedShip()
//         {
//             _isMovable.Value = false;
//         }
//
//         //TODO
//         /// <summary>
//         /// 船移動
//         /// </summary>
//         public void OnShipMoved(ShipUnitModel shipUnitModel)
//         {
//             UpdateStatus(shipUnitModel);
//         }
//
//         /// <summary>
//         /// 味方フェイズ終了
//         /// </summary>
//         protected override void OnAllyPhaseEnd()
//         {
//             _isMovable.Value = false;
//         }
//
//         /// <summary>
//         /// 船移動可能方向のUpdate
//         /// </summary>
//         private void ShipMovableDirUpdated(DirectionType dirType)
//         {
//             _movableRight.Value = new MovableStatus(
//                 dirType.HasFlag(DirectionType.Right),
//                 _moveChecker.CalcMovePower(currentShipModel.Cell.Value, currentShipModel.Direction.Value, DirectionType.Right)
//             );
//             _movableTopRight.Value = new MovableStatus(
//                 dirType.HasFlag(DirectionType.TopRight),
//                 _moveChecker.CalcMovePower(currentShipModel.Cell.Value, currentShipModel.Direction.Value, DirectionType.TopRight)
//             );
//             _movableTopLeft.Value = new MovableStatus(
//                 dirType.HasFlag(DirectionType.TopLeft),
//                 _moveChecker.CalcMovePower(currentShipModel.Cell.Value, currentShipModel.Direction.Value, DirectionType.TopLeft)
//             );
//             _movableLeft.Value = new MovableStatus(
//                 dirType.HasFlag(DirectionType.Left),
//                 _moveChecker.CalcMovePower(currentShipModel.Cell.Value, currentShipModel.Direction.Value, DirectionType.Left)
//             );
//             _movableBottomLeft.Value = new MovableStatus(
//                 dirType.HasFlag(DirectionType.BottomLeft),
//                 _moveChecker.CalcMovePower(currentShipModel.Cell.Value, currentShipModel.Direction.Value, DirectionType.BottomLeft)
//             );
//             _movableBottomRight.Value = new MovableStatus(
//                 dirType.HasFlag(DirectionType.BottomRight),
//                 _moveChecker.CalcMovePower(currentShipModel.Cell.Value, currentShipModel.Direction.Value, DirectionType.BottomRight)
//             );
//         }
//
//         /// <summary>
//         /// Dispose
//         /// </summary>
//         public override void Dispose()
//         {
//             base.Dispose();
//             _compositeDisposable.Dispose();
//         }
//     }
// }