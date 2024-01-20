using System.Collections.Generic;
using App.AppCommon;
using App.AppCommon.Core;
using App.AppCommon.UI;
using App.Battle.Core;
using App.Battle.Facades;
using App.Battle.Map;
using App.Battle.Map.Cells;
using App.Battle.UI.Menu;
using App.Battle.Units;
using App.Battle.Units.Ship;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.UI.Controller
{
    /// <summary>
    /// 移動コントローラ
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(UnitController))]
    public class UnitController : MonoBehaviour
    {
        /// <summary>
        /// 移動ステータス
        /// </summary>
        public struct MovableStatus
        {
            public bool IsActive { get; }
            public double NeedPower { get; }
            public bool IsMovable { get; }
            public MovableStatus(bool isActive, double needPower, double movePower)
            {
                IsActive = isActive;
                NeedPower = needPower;
                IsMovable = isActive && movePower >= needPower;
            }
            public static MovableStatus Invalid => new MovableStatus(false, 0, 0);
        }
        
        [SerializeField] private MoveButton btnRight;
        [SerializeField] private MoveButton btnTopRight;
        [SerializeField] private MoveButton btnTopLeft;
        [SerializeField] private MoveButton btnLeft;
        [SerializeField] private MoveButton btnBottomLeft;
        [SerializeField] private MoveButton btnBottomRight;
        
        private HexMapMoveChecker _moveChecker;
        private HexMapManager _mapManager;
        private UnitManger _unitManger;
        private AttackController _attackController;

        private readonly CompositeDisposable _disposable = new();
        private DirectionType _movableDir = DirectionType.None;
        private ShipUnitModel _shipUnitModel = null;

        private readonly Dictionary<DirectionType, MovableStatus> _movableMap = new()
        {
            { DirectionType.Right, MovableStatus.Invalid },
            { DirectionType.TopRight, MovableStatus.Invalid },
            { DirectionType.TopLeft, MovableStatus.Invalid },
            { DirectionType.Left, MovableStatus.Invalid },
            { DirectionType.BottomLeft, MovableStatus.Invalid },
            { DirectionType.BottomRight, MovableStatus.Invalid },
        };

        private bool IsMovable(DirectionType dir) => _movableMap.TryGetValue(dir, out var status) && status.IsMovable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            HexMapMoveChecker moveChecker,
            HexMapManager mapManager,
            UnitManger unitManger,
            BattleCamera battleCamera,
            AttackController attackController,
            BattleEventHub eventHub
        )
        {
            _moveChecker = moveChecker;
            _mapManager = mapManager;
            _unitManger = unitManger;
            _attackController = attackController;
            BattleState.Facade.SelectedShipUnit.Subscribe(SelectShipUnit).AddTo(this);
           
            BattleOperation.Facade.Unit.Move
                .Where(_ => _shipUnitModel != null)
                .Subscribe(Move).AddTo(this);
            BattleOperation.Facade.Unit.Decide
                .Where(_ => _shipUnitModel != null)
                .Subscribe(_ => Decide()).AddTo(this);
            BattleOperation.Facade.Unit.Cancel
                .Where(_ => _shipUnitModel != null)
                .Subscribe(_ => Cancel()).AddTo(this);
            BattleOperation.Facade.Unit.Skill
                .Where(_ => _shipUnitModel != null)
                .Subscribe(_ => UnitSkillAsync().Forget()).AddTo(this);
            
            battleCamera.Position.Subscribe(_ => CameraPositionUpdated()).AddTo(this);
            eventHub.Subscribe<BattleEvents.OnPhaseStartAsync>(async _ => Clear()).AddTo(this);
        }

        /// <summary>
        /// 決定
        /// </summary>
        private void Decide()
        {
            if (_attackController.HasAttackTarget) return;
            // if (_shipUnitModel.IsCancellable)
            // {
            //     _shipUnitModel.DecideAction();
            // }
            // else
            // {
            //     OpenUnitMenuAsync().Forget();
            // }
            OpenUnitMenuAsync().Forget();
        }
        
        /// <summary>
        /// 選択解除
        /// </summary>
        private void Cancel()
        {
            if (_shipUnitModel.IsUnselectable)
            {
                // _shipUnitModel.Cancel();
                _shipUnitModel = null;
                BattleState.Facade.UpdateSelectedShipUnit(null);
                return;
            }
            // if (_shipUnitModel.IsCancellable)
            // {
            //     _shipUnitModel.Cancel();
            // }
        }
        
        /// <summary>
        /// 移動
        /// </summary>
        private void Move(Vector2 dir)
        {
            var inputDir = HexUtil.InputVectorToMoveDir(dir);
            var moveDir = HexUtil.InputDirToMoveDir(inputDir, _shipUnitModel.Direction.Value);
            if (!IsMovable(moveDir))
            {
                return; 
            }
            BattleMove.Facade.InputMove(_shipUnitModel.UnitId, moveDir);
        }
        
        /// <summary>
        /// 選択
        /// </summary>
        private void SelectShipUnit(ShipUnitModel shipUnitModel)
        {
            _disposable.Clear();
            _movableDir = DirectionType.None;

            if (shipUnitModel == null)
            {
                Clear();
                return;
            }

            _shipUnitModel = shipUnitModel;
            UpdateButtonPosition(_shipUnitModel.Cell.Value);
            SelectedShipMovableDirUpdated(_shipUnitModel, _shipUnitModel.MovableDirection.Value);
            _shipUnitModel.MovableDirection.SubscribeWithState(_shipUnitModel, (d, ship) =>
            {
                SelectedShipMovableDirUpdated(ship, d);
            }).AddTo(_disposable);
            _shipUnitModel.MovePower.Where(x => x == 0).Subscribe(_ =>
            {
                Clear();
            });
        }

        /// <summary>
        /// 移動可能方向が変更
        /// </summary>
        private void SelectedShipMovableDirUpdated(ShipUnitModel ship, DirectionType movableDir)
        {
            if (!ship.IsMovable)
            {
                Clear();
                return; 
            }
            var cell = ship.Cell.Value;
            var currentDir = ship.Direction.Value;
            _movableDir = movableDir;
            
            _movableMap[DirectionType.Right] = new MovableStatus(
                movableDir.HasFlag(DirectionType.Right),
                _moveChecker.CalcMovePower(cell, currentDir, DirectionType.Right),
                ship.Status.MovePower.Current
            ); 
            _movableMap[DirectionType.TopRight] = new MovableStatus(
                movableDir.HasFlag(DirectionType.TopRight),
                _moveChecker.CalcMovePower(cell, currentDir, DirectionType.TopRight),
                ship.Status.MovePower.Current
            ); 
            _movableMap[DirectionType.TopLeft] = 
            new MovableStatus(
                movableDir.HasFlag(DirectionType.TopLeft),
                _moveChecker.CalcMovePower(cell, currentDir, DirectionType.TopLeft),
                ship.Status.MovePower.Current
            ); 
            _movableMap[DirectionType.Left] = 
            new MovableStatus(
                movableDir.HasFlag(DirectionType.Left),
                _moveChecker.CalcMovePower(cell, currentDir, DirectionType.Left),
                ship.Status.MovePower.Current
            ); 
            _movableMap[DirectionType.BottomLeft] = 
            new MovableStatus(
                movableDir.HasFlag(DirectionType.BottomLeft),
                _moveChecker.CalcMovePower(cell, currentDir, DirectionType.BottomLeft),
                ship.Status.MovePower.Current
            ); 
            _movableMap[DirectionType.BottomRight] = 
            new MovableStatus(
                movableDir.HasFlag(DirectionType.BottomRight),
                _moveChecker.CalcMovePower(cell, currentDir, DirectionType.BottomRight),
                ship.Status.MovePower.Current
            ); 
            UpdateButton();
        }

        /// <summary>
        /// カメラ移動
        /// </summary>
        private void CameraPositionUpdated()
        {
            if (_shipUnitModel == null)
            {
                return;
            }
            UpdateButtonPosition(_shipUnitModel.Cell.Value);
        }
        
        /// <summary>
        /// 選択した船の位置更新
        /// </summary>
        private void UpdateButtonPosition(HexCell cell)
        {
            var rightNextCell =
                _mapManager.GetCellByGrid(HexUtil.GetNextGridByDirection(cell.Grid, DirectionType.Right));
            var topRightNextCell =
                _mapManager.GetCellByGrid(HexUtil.GetNextGridByDirection(cell.Grid, DirectionType.TopRight));
            var topLeftNextCell =
                _mapManager.GetCellByGrid(HexUtil.GetNextGridByDirection(cell.Grid, DirectionType.TopLeft));
            var leftNextCell =
                _mapManager.GetCellByGrid(HexUtil.GetNextGridByDirection(cell.Grid, DirectionType.Left));
            var bottomLeftNextCell =
                _mapManager.GetCellByGrid(HexUtil.GetNextGridByDirection(cell.Grid, DirectionType.BottomLeft));
            var bottomRightNextCell =
                _mapManager.GetCellByGrid(HexUtil.GetNextGridByDirection(cell.Grid, DirectionType.BottomRight));
            btnRight.SetCell(rightNextCell);
            btnTopRight.SetCell(topRightNextCell);
            btnTopLeft.SetCell(topLeftNextCell);
            btnLeft.SetCell(leftNextCell);
            btnBottomLeft.SetCell(bottomLeftNextCell);
            btnBottomRight.SetCell(bottomRightNextCell);
        }
        
        /// <summary>
        /// Clear
        /// </summary>
        private void Clear()
        {
            _movableMap[DirectionType.Right] = MovableStatus.Invalid;
            _movableMap[DirectionType.TopRight] = MovableStatus.Invalid;
            _movableMap[DirectionType.TopLeft] = MovableStatus.Invalid;
            _movableMap[DirectionType.Left] = MovableStatus.Invalid;
            _movableMap[DirectionType.BottomLeft] = MovableStatus.Invalid;
            _movableMap[DirectionType.BottomRight] = MovableStatus.Invalid;
            UpdateButton();
        }

        /// <summary>
        /// Buttonの更新
        /// </summary>
        private void UpdateButton()
        {
            btnRight.UpdateStatus(_movableMap[DirectionType.Right]);
            btnTopRight.UpdateStatus(_movableMap[DirectionType.TopRight]);
            btnTopLeft.UpdateStatus(_movableMap[DirectionType.TopLeft]);
            btnLeft.UpdateStatus(_movableMap[DirectionType.Left]);
            btnBottomLeft.UpdateStatus(_movableMap[DirectionType.BottomLeft]);
            btnBottomRight.UpdateStatus(_movableMap[DirectionType.BottomRight]);
        }
      
        /// <summary>
        /// Unityメニューを開く
        /// </summary>
        private async UniTask OpenUnitMenuAsync()
        {
            var res = await BattleMenus.Facade.OpenMainMenuAsync(BattleMenuItemType.MainMenu.UnitItems);
            if (res == BattleMenuItemType.MainMenu.EndTurn)
            {
                _shipUnitModel.EndAction();
                //終了  
                BattleProgress.Facade.EndActionAsync(_shipUnitModel).Forget();
            }
            else if (res == BattleMenuItemType.MainMenu.UnitSkill)
            {
                UnitSkillAsync().Forget();
            }
        }

        /// <summary>
        /// ユニットスキル
        /// </summary>
        private async UniTask UnitSkillAsync()
        {
            Log.Debug("SkillAsync");
        }
        
        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}