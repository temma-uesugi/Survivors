using System;
using App.AppCommon;
using App.AppCommon.Core;
using App.AppCommon.UI;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle2.Map.Cells;
using App.Battle2.UI.Menu;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;
using Constants;

namespace App.Battle2.Map
{
    /// <summary>
    /// マップ操作管理
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(MapController))]
    public class MapController : MonoBehaviour
    {
        private BattleState _battleState;
        private UnitManger2 unitManger2;
        private HexMapManager _mapManager;

        private IDisposable _moveUpdateDisposable;
        private InputDirectionType _currentDir = InputDirectionType.None;
        private HexCell _focusedHexCell;
        private IUnitModel2 _focusedUnit = default;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger2 unitManger2,
            HexMapManager hexMapManager,
            BattleEventHub2 eventHub2
        )
        {
            _battleState = BattleState.Facade;
            this.unitManger2 = unitManger2;
            _mapManager = hexMapManager;

            BattleOperation.Facade.Map.Move.Subscribe(OnMoveStart).AddTo(this);
            BattleOperation.Facade.Map.Decide.Subscribe(_ => Select()).AddTo(this);
            BattleOperation.Facade.Map.SwitchUnitL.Select(_ => -1)
                .Merge(BattleOperation.Facade.Map.SwitchUnitR.Select(_ => 1))
                .Subscribe(SwitchUnit)
                .AddTo(this);
            BattleOperation.Facade.Map.Skill.Subscribe(_ => AdmiralSkillAsync().Forget()).AddTo(this);

            _battleState.FocusedHexCell.Subscribe(x => _focusedHexCell = x).AddTo(this);

            eventHub2.Subscribe<BattleEvents2.OnTurnStartAsync>(async _ => await OnTurnStartAsync()).AddTo(this);
        }

        /// <summary>
        /// ターン開始
        /// </summary>
        private async UniTask OnTurnStartAsync()
        {
            var uniId = _focusedUnit is ShipUnitModel2 ? _focusedUnit.UnitId : GameConst.InvalidUnitId;
            _focusedUnit = unitManger2.GetNextIdUnit(uniId, isAdd: true, isEnemy: false);
            _focusedHexCell = _mapManager.GetCellByGrid(_focusedUnit.Grid);
            _battleState.UpdateFocusedHexCell(_focusedHexCell);
        }

        /// <summary>
        /// マップ初期化
        /// </summary>
        public void Setup()
        {
            // _focusedHexCell = _mapManager.GetCellByGrid(new GridValue(0, 0));
            // _battleState.UpdateFocusedHexCell(_focusedHexCell);
        }

        /// <summary>
        /// MoveStart
        /// </summary>
        private void OnMoveStart(Vector2 vec2)
        {
            var inputDir = HexUtil2.InputVectorToMoveDir(vec2);

            _currentDir = inputDir;
            Move(inputDir);
            _moveUpdateDisposable?.Dispose();
            _moveUpdateDisposable = Observable.Interval(TimeSpan.FromSeconds(0.2f))
                .TakeUntil(BattleOperation.Facade.Map.StopMove.Merge(
                    BattleOperation.Facade.ModeUpdated.AsUnitObservable()))
                .SubscribeWithState2(inputDir, this, (_, d, self) => { self.Move(d); })
                .AddTo(this);
        }


        /// <summary>
        /// Hex移動
        /// </summary>
        private void Move(InputDirectionType inputDir)
        {
            var dir = (inputDir, _focusedHexCell.GridY % 2 == 0) switch
            {
                (InputDirectionType.Right, _) => DirectionType.Right,
                (InputDirectionType.TopRight, _) => DirectionType.TopRight,
                (InputDirectionType.TopLeft, _) => DirectionType.TopLeft,
                (InputDirectionType.Left, _) => DirectionType.Left,
                (InputDirectionType.BottomLeft, _) => DirectionType.BottomLeft,
                (InputDirectionType.BottomRight, _) => DirectionType.BottomRight,
                (InputDirectionType.Top, true) => DirectionType.TopRight,
                (InputDirectionType.Top, false) => DirectionType.TopLeft,
                (InputDirectionType.Bottom, true) => DirectionType.BottomRight,
                (InputDirectionType.Bottom, false) => DirectionType.BottomLeft,
                _ => DirectionType.None
            };
            _focusedHexCell = _mapManager.GetNextCellByDir(_focusedHexCell, dir);
            _battleState.UpdateFocusedHexCell(_focusedHexCell);
            _focusedUnit = unitManger2.GetUnitByHex(_focusedHexCell);
            _battleState.UpdateFocusedUnit(_focusedUnit);
        }

        /// <summary>
        /// Hex選択
        /// </summary>
        private void Select()
        {
            if (_focusedUnit is not ShipUnitModel2 shipModel)
            {
                OpenMapMenuAsync().Forget();
                return;
            }

            if (!shipModel.IsActionEnd.Value)
            {
                _battleState.UpdateSelectedShipUnit(shipModel);
            }
        }

        /// <summary>
        /// Unit切り替え
        /// </summary>
        private void SwitchUnit(int indexDir)
        {
            bool isEnemy = _focusedUnit is EnemyUnitModel2;
            _focusedUnit = unitManger2.GetNextIdUnit(_focusedUnit?.UnitId ?? 0, isAdd: indexDir > 0, isEnemy);
            _focusedHexCell = _mapManager.GetCellByGrid(_focusedUnit.Grid);
            _battleState.UpdateFocusedHexCell(_focusedHexCell);
            _battleState.UpdateFocusedUnit(_focusedUnit);
        }

        /// <summary>
        /// Mapメニューを開く
        /// </summary>
        private async UniTask OpenMapMenuAsync()
        {
            var res = await BattleMenus.Facade.OpenMainMenuAsync(BattleMenuItemType.MainMenu.MapItems);
            if (res == BattleMenuItemType.MainMenu.EndTurn)
            {
                //終了  
                BattleProgress.Facade.PassPhase().Forget();
            }
            else if (res == BattleMenuItemType.MainMenu.AdmiralSkill)
            {
                AdmiralSkillAsync().Forget();
            }
        }

        /// <summary>
        /// 提督スキル
        /// </summary>
        private async UniTask AdmiralSkillAsync()
        {
            Log.Debug("AdmiralSkillAsync");
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _moveUpdateDisposable?.Dispose();
        }
    }
}