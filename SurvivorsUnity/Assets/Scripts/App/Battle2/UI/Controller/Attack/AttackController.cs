using System;
using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.AppCommon.Core;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle2.Interfaces;
using App.Battle2.Libs;
using App.Battle2.Map;
using App.Battle2.Map.Cells;
using App.Battle2.UI.HexButtons;
using App.Battle2.Units;
using App.Battle2.Units.Ship;
using Cysharp.Threading.Tasks;
using Master.Constants;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.UI.Controller.Attack
{
    /// <summary>
    /// 攻撃コントローラ
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(AttackController))]
    public class AttackController : MonoBehaviour
    {
        [SerializeField] private BombButton bombButtonPrefab;
        [SerializeField] private SlashButton slashButtonPrefab;
        [SerializeField] private AssaultButton assaultButton;

        private GameObjectPool<BombButton> _bombBtnPool;
        private readonly List<BombButton> _bombBtnList = new();

        private GameObjectPool<SlashButton> _attackBtnPool;
        private readonly List<SlashButton> _attackBtnList = new();

        private HexMapAttackChecker _attackChecker;
        private UnitManger2 unitManger2;
        private BattleCamera2 battleCamera2;

        private readonly List<IAttackTargetModel> _slashTargetList = new();
        private readonly List<IAttackTargetModel> _bombTargetList = new();
        private IAttackTargetModel _assaultTarget = null;

        private readonly CompositeDisposable _disposable = new();
        private ShipUnitModel2 shipUnitModel2 = null;

        private int _curSelectedTargetIndex = 0;
        private AttackTargetButton _curSelectedTargetButton;
        private IAttackTargetModel[] _sortedTargets;
        private readonly Dictionary<uint, AttackTargetButton> _buttonMap = new();

        public bool HasAttackTarget => _curSelectedTargetButton != null;

        private IAttackTargetModel _prevTarget = null;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger2 unitManger2,
            HexMapAttackChecker attackChecker,
            BattleCamera2 battleCamera2,
            BattleEventHub2 eventHub2
        )
        {
            _attackChecker = attackChecker;
            this.unitManger2 = unitManger2;
            var trans = gameObject.transform;
            _bombBtnPool = new GameObjectPool<BombButton>(bombButtonPrefab, trans);
            _attackBtnPool = new GameObjectPool<SlashButton>(slashButtonPrefab, trans);

            BattleState.Facade.SelectedShipUnit.Subscribe(SelectShipUnit).AddTo(this);

            BattleOperation.Facade.Unit
                .SwitchTargetL.Select(_ => -1)
                .Merge(BattleOperation.Facade.Unit.SwitchTargetR.Select(_ => 1))
                .Where(_ => shipUnitModel2 != null)
                .Subscribe(SwitchTarget)
                .AddTo(this);
            BattleOperation.Facade.Unit.Decide
                .Where(_ => shipUnitModel2 != null)
                .Subscribe(_ => DecideAttackAsync().Forget())
                .AddTo(this);
            BattleOperation.Facade.Unit.Skill
                .Where(_ => shipUnitModel2 != null)
                .Subscribe(_ => SubMenuAsync().Forget())
                .AddTo(this);
            BattleOperation.Facade.Unit.Cancel
                .Where(_ => shipUnitModel2 != null)
                .Subscribe(_ => SwitchTarget(0))
                .AddTo(this);
            
            battleCamera2.Position.Subscribe(_ => CameraPositionUpdated()).AddTo(this);
            eventHub2.Subscribe<BattleEvents2.OnPhaseStartAsync>(async _ => Clear()).AddTo(this);
        }

        /// <summary>
        /// 船移動
        /// </summary>
        private void OnShipMoved(ShipUnitModel2 shipUnitModel2)
        {
            // UpdateTargets(); 
        }
        
        /// <summary>
        /// 選択
        /// </summary>
        private void SelectShipUnit(ShipUnitModel2 shipUnitModel2)
        {
            Clear();
            _disposable.Clear();
            _prevTarget = null;

            if (shipUnitModel2 == null)
            {
                return;
            }
            
            this.shipUnitModel2 = shipUnitModel2;
            UpdateTargets(this.shipUnitModel2.Cell.Value, this.shipUnitModel2.Direction.Value, this.shipUnitModel2);
            this.shipUnitModel2.Cell.CombineLatest(
                this.shipUnitModel2.Direction,
                (cell, dir) => (cell, dir)
            ).SubscribeWithState(this.shipUnitModel2, (x, ship) => UpdateTargets(x.cell, x.dir, ship))
            .AddTo(_disposable);
        }

        /// <summary>
        /// ターゲット更新
        /// </summary>
        private void UpdateTargets(HexCell cell, DirectionType dir, ShipUnitModel2 shipUnitModel2)
        {
            Clear();
            UpdateAssaultAttackTargets(cell, dir);
            UpdateSlashAttackTargets(cell, dir);
            UpdateBombAttackTargets(cell, dir, shipUnitModel2);
            _sortedTargets = _slashTargetList
                .Concat(_bombTargetList)
                .Prepend(_assaultTarget)
                .Where(x => x != null)
                .OrderBy(x => x.Id)
                .ToArray();
           
            //前回の攻撃対象がいればカーソルを合わせる
            if (_prevTarget != null && _sortedTargets.Any() && _buttonMap.TryGetValue(_prevTarget.Id, out var btn))
            {
                var idx = Array.IndexOf(_sortedTargets, _prevTarget);
                _curSelectedTargetIndex = idx;
                _curSelectedTargetButton = btn;
                _curSelectedTargetButton.SetSelected(true);
            }
        }

        /// <summary>
        /// 突撃対象を更新
        /// </summary>
        private void UpdateAssaultAttackTargets(HexCell cell, DirectionType dir)
        {
            _assaultTarget = _attackChecker.GetAssaultTargetUnit(cell, dir);

            if (_assaultTarget == null)
            {
                assaultButton.SetActive(false);
                return;
            }

            assaultButton.SetActive(true);
            assaultButton.SetTarget(_assaultTarget);
            _buttonMap.TryAdd(_assaultTarget.Id, assaultButton);
        }
        
        /// <summary>
        /// 斬り込み対象の更新
        /// </summary>
        private void UpdateSlashAttackTargets(HexCell cell, DirectionType dir)
        {
            var attackTargetUnits = _attackChecker.GetAttackTargetUnits(cell, dir);
            _slashTargetList.AddRange(attackTargetUnits.Select(x => x));

            if (!_slashTargetList.Any())
            {
                return;
            }

            foreach (var target in _slashTargetList)
            {
                var btn = _attackBtnPool.Rent();
                btn.SetTarget(target);
                btn.SetActive(true);
                _attackBtnList.Add(btn);
                _buttonMap.TryAdd(target.Id, btn);
            }
        }

        /// <summary>
        /// 砲撃対象を更新
        /// </summary>
        private void UpdateBombAttackTargets(HexCell cell, DirectionType dir, ShipUnitModel2 ship)
        {
            var rightBombTargets = _attackChecker.GetBombTargetUnits(BombSide.Right, ship.UnitId,
                cell, dir, ship.RightBombStatus.Value);
            var leftBombTargets = _attackChecker.GetBombTargetUnits(BombSide.Left, ship.UnitId,
                cell, dir, ship.LeftBombStatus.Value);
            _bombTargetList.AddRange(rightBombTargets);
            _bombTargetList.AddRange(leftBombTargets);

            if (!_bombTargetList.Any())
            {
                return;
            }

            foreach (var target in _bombTargetList)
            {
                var btn = _bombBtnPool.Rent();
                btn.SetTarget(target);
                btn.SetActive(true);
                _bombBtnList.Add(btn);
                _buttonMap.TryAdd(target.Id, btn);
            }
        }

        /// <summary>
        /// カメラ移動
        /// </summary>
        private void CameraPositionUpdated()
        {
            if (shipUnitModel2 == null)
            {
                return;
            }

            foreach (var btn in _bombBtnList)
            {
                btn.UpdatePosition(); 
            }
            foreach (var btn in _attackBtnList)
            {
                btn.UpdatePosition(); 
            }
            assaultButton.UpdatePosition();
        }

        /// <summary>
        /// Clear
        /// </summary>
        private void Clear()
        {
            foreach (var btn in _bombBtnList)
            {
                _bombBtnPool.Return(btn);
            }
            foreach (var btn in _attackBtnList)
            {
                _attackBtnPool.Return(btn);
            }
            assaultButton.SetActive(false);
            
            _buttonMap.Clear();
            _bombBtnList.Clear();
            _bombTargetList.Clear();
            _attackBtnList.Clear();
            _slashTargetList.Clear(); 
            _assaultTarget = null;
            assaultButton.SetActive(false);
            
            if (_curSelectedTargetButton != null)
            {
                _curSelectedTargetButton.SetSelected(false);
            }
            _curSelectedTargetIndex = 0;
            _curSelectedTargetButton = null;
        }

        /// <summary>
        /// 攻撃決定
        /// </summary>
        private async UniTask DecideAttackAsync()
        {
            if (!HasAttackTarget)
            {
                return;
            }

            switch (_curSelectedTargetButton)
            {
                case BombButton:
                    BattleAttack.Facade.ShipAttack(shipUnitModel2, _curSelectedTargetButton.Target, AttackType.Bomb);
                    break;
                case SlashButton:
                    BattleAttack.Facade.ShipAttack(shipUnitModel2, _curSelectedTargetButton.Target, AttackType.Slash);
                    break;
                case AssaultButton:
                    BattleAttack.Facade.ShipAttack(shipUnitModel2, _curSelectedTargetButton.Target, AttackType.Assault);
                    break;
                default:
                    return; 
            }
            _prevTarget = _curSelectedTargetButton.Target;
            shipUnitModel2.DecideAction();
            await UniTask.Yield();
            Clear();
        }

        /// <summary>
        /// サブメニュー
        /// </summary>
        private async UniTask SubMenuAsync()
        {
            if (!HasAttackTarget)
            {
                return;
            }
            switch (_curSelectedTargetButton)
            {
                case BombButton:
                    await BombSkillMenuAsync();
                    break;
                case SlashButton:
                    await SlashSkillMenuAsync();
                    break;
                case AssaultButton:
                    await AssaultSkillMenuAsync();
                    break;
                default:
                    return; 
            }
            // var isAttacked = _curSelectedTargetButton switch
            // {
            //     BombButton => await BombAttackAsync(),
            //     SlashButton => await SlashAttackAsync(),
            //     AssaultButton => await AssaultAttackAsync(),
            //     _ => false
            // };
            //
            // if (!isAttacked)
            // {
            //     return;     
            // }
            // _prevTarget = _curSelectedTargetButton.Target;
            // _shipUnitModel.DecideAction();
            // Clear();
        }

        /// <summary>
        /// 砲撃サブメニュー
        /// </summary>
        private async UniTask BombSkillMenuAsync()
        {
            Log.Debug("BombSkillMenuAsync");
            // await BattleMenus.Facade.OpenAttackMenuAsync(BattleMenuItemType.AttackMenu.BombItems);
        }

        /// <summary>
        /// 切り込みサブメニュー
        /// </summary>
        private async UniTask SlashSkillMenuAsync()
        {
            Log.Debug("SlashSkillMenuAsync");
            // await BattleMenus.Facade.OpenAttackMenuAsync(BattleMenuItemType.AttackMenu.SlashItems);
        }

        /// <summary>
        /// 突撃サブメニュー
        /// </summary>
        private async UniTask AssaultSkillMenuAsync()
        {
            Log.Debug("AssaultSkillMenuAsync");
            // await BattleMenus.Facade.OpenAttackMenuAsync(BattleMenuItemType.AttackMenu.AssaultItems);
        }
        
        /// <summary>
        /// 対象選択
        /// </summary>
        private void SwitchTarget(int indexDir)
        {
            if (indexDir == 0)
            {
                _curSelectedTargetIndex = 0;
                if (_curSelectedTargetButton != null)
                {
                    _curSelectedTargetButton.SetSelected(false);
                }
                _curSelectedTargetButton = null;
                return;
            }
            
            if (!_sortedTargets.Any())
            {
                return;
            }

            if (_curSelectedTargetButton != null)
            {
                _curSelectedTargetButton.SetSelected(false);
            }
            
            var idx = _curSelectedTargetIndex + indexDir;
            if (idx < 0)
            {
                idx = _sortedTargets.Length - 1;
            }
            else if (idx >= _sortedTargets.Length)
            {
                idx = 0;
            }

            var selectedUnit = _sortedTargets[idx];
            _curSelectedTargetIndex = idx;
            if (!_buttonMap.TryGetValue(selectedUnit.Id, out var btn))
            {
                _curSelectedTargetButton = null;
                return; 
            }
            _curSelectedTargetButton = btn;
            _curSelectedTargetButton.SetSelected(true);
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