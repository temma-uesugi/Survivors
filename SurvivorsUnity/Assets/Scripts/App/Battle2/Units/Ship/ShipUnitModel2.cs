using System;
using App.AppCommon;
using App.Battle2.Facades;
using App.Battle2.Interfaces;
using App.Battle2.Map;
using App.Battle2.Map.Cells;
using App.Battle2.ValueObjects;
using Master.Constants;
using UniRx;
using UnityEngine;

namespace App.Battle2.Units.Ship
{
    /// <summary>
    /// 船のModel
    /// </summary>
    public class ShipUnitModel2 : IUnitModel2, IAttackTargetModel, IBlockBombModel
    {
        public CompositeDisposable ModelDisposable { get; } = new CompositeDisposable();
        
        private readonly HexMapMoveChecker _mapMoveChecker;
        private readonly HexMapAttackChecker _attackChecker;

        public uint UnitId { get; }
        public uint Id => UnitId;
        public string Label { get; }
        public int Index { get; init; }
        
        public ShipStatus Status { get; private set; }
        public DamageCalculator DamageCalculator { get; private set; }
        public int CalcDamaged(float damage, AttackType attackType) => DamageCalculator.CalcDamaged(damage, attackType);
        
        public uint UserId => GameConst.PlayerUserId;
        public bool IsAlive => Status.ArmorPoint.Current >= 1 && Status.CrewPoint.Current >= 1;

        public GridValue Grid => _cell.Value.Grid;
        public Vector3 Position => _cell.Value.Position;

        //位置
        private readonly ReactiveProperty<HexCell> _cell;
        public IReadOnlyReactiveProperty<HexCell> Cell => _cell;

        //向き
        private readonly ReactiveProperty<DirectionType> _direction = new();
        public IReadOnlyReactiveProperty<DirectionType> Direction => _direction;

        //移動可能方向
        private readonly ReactiveProperty<DirectionType> _movableDir = new();
        public IReadOnlyReactiveProperty<DirectionType> MovableDirection => _movableDir;

        //移動力
        public IReadOnlyReactiveProperty<double> MovePower => Status.MovePower.OnUpdate.ToReadOnlyReactiveProperty();
        //Ap(耐久力)
        public IReadOnlyReactiveProperty<int> ArmorPoint => Status.ArmorPoint.OnUpdate.ToReadOnlyReactiveProperty();
        //船員数
        public IReadOnlyReactiveProperty<int> CrewPoint => Status.CrewPoint.OnUpdate.ToReadOnlyReactiveProperty();

        //選択中
        private readonly IReactiveProperty<bool> _isSelected = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<bool> IsSelected => _isSelected;
        //行動中
        private readonly IReactiveProperty<bool> _isOnAction = new ReactiveProperty<bool>(false);
        public IReadOnlyReactiveProperty<bool> IsOnAction => _isOnAction;
        //行動済み
        // private readonly IReactiveProperty<bool> _isActionEnd = new ReactiveProperty<bool>(false);
        // public IReadOnlyReactiveProperty<bool> IsActionEnd => _isActionEnd;
        public IReadOnlyReactiveProperty<bool> IsActionEnd => _nextActionTurns.Select(x => x > 0).ToReactiveProperty();
        //移動可能か
        public bool IsMovable => Status.MovePower.Current >= 1;
        //砲撃範囲
        private bool _isAllBombardRangeActive = false;
        private readonly IReactiveProperty<bool> _isBombardRangeActive = new ReactiveProperty<bool>(false);
        public IReadOnlyReactiveProperty<bool> IsBombardRangeActive => _isBombardRangeActive;

        //砲台のステータス
        private readonly ReactiveProperty<BombStatus> _leftBombStatus = new();
        public IReadOnlyReactiveProperty<BombStatus> LeftBombStatus => _leftBombStatus;
        private readonly ReactiveProperty<BombStatus> _rightBombStatus = new();
        public IReadOnlyReactiveProperty<BombStatus> RightBombStatus => _rightBombStatus;

        private readonly Subject<uint> _onDefeated = new();
        public IObservable<uint> OnDefeated => _onDefeated;
        private readonly Subject<uint> _onRevived = new();
        public IObservable<uint> OnRevived => _onRevived;

        //移動
        // private HexCell _cancellationCell;
        // private DirectionType _cancellationDir;
        // private double _cancellationMovePower;
        // public bool IsCancellable { get; private set; } = false;
        
        //選択解除可能か
        public bool IsUnselectable => !_isOnAction.Value;
      
        //行動速度
        public int ActionSpeed => Status.ActionSpeed;
        
        //次の行動までのTurn
        private readonly IReactiveProperty<int> _nextActionTurns = new ReactiveProperty<int>(0);
        public IReadOnlyReactiveProperty<int> NextActionTurns => _nextActionTurns;

        //次の行動までのTurn予約
        private readonly Subject<(bool, int)> _nextTurnSchedule = new();
        public IObservable<(bool isOn, int trun)> NextTurnSchedule => _nextTurnSchedule;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShipUnitModel2(
            ShipCreateParam createParam,
            HexMapMoveChecker mapMoveChecker,
            HexMapAttackChecker attackChecker
        )
        {
            UnitId = createParam.ShipUnitId;
            Label = createParam.Label;
            Status = createParam.Status;
            _cell = new (createParam.InitCell);
            _direction.Value = createParam.InitDirection;
            _leftBombStatus.Value = createParam.Status.LeftBombStatus;
            _rightBombStatus.Value = createParam.Status.RightBombStatus;
            DamageCalculator = new ShipDamageCalculator(this);
            Index = createParam.Index;
            
            //移動距離
            _movableDir.SetValueAndForceNotify(mapMoveChecker.GetMovableDirection(Cell.Value, createParam.InitDirection));

            _mapMoveChecker = mapMoveChecker;
            _attackChecker = attackChecker;
        }
        
        /// <summary>
        /// 選択解除
        /// </summary>
        public void UnSelect()
        {
            _isSelected.Value = false;
            _isBombardRangeActive.Value = false;
            _nextTurnSchedule.OnNext((false, 0));
        }
        
        /// <summary>
        /// 自分の船が選択された
        /// </summary>
        public void Select()
        {
            _isSelected.Value = true;
            _isBombardRangeActive.Value = true;
            _movableDir.SetValueAndForceNotify(_mapMoveChecker.GetMovableDirection(_cell.Value, _direction.Value));
            _nextTurnSchedule.OnNext((true, ActionSpeed));
        }

        // /// <summary>
        // /// キャンセル値をセット
        // /// </summary>
        // private void SetCancellation()
        // {
        //     _cancellationCell = _cell.Value;
        //     _cancellationDir = _direction.Value;
        //     _cancellationMovePower = Status.MovePower.Current;
        // }
        
        /// <summary>
        /// どれかの船が移動
        /// </summary>
        private void AnyShipMoveStart()
        {
            _isOnAction.Value = false;
            //TODO 必要？
            _isBombardRangeActive.Value = _isAllBombardRangeActive || _isSelected.Value;
        }

        /// <summary>
        /// 移動開始
        /// </summary>
        private void ShipMoveStart()
        {
            if (IsActionEnd.Value)
            {
                return;
            }
            _isOnAction.Value = true;
            //TODO 必要？
            _isBombardRangeActive.Value = _isAllBombardRangeActive || _isSelected.Value;
        }

        /// <summary>
        /// ターン開始
        /// </summary>
        public void OnTurnStart()
        {
        }
        
        /// <summary>
        /// ターン終了
        /// </summary>
        public void OnTurnEnd()
        {
            Status.MovePower.Reset();
            _isSelected.Value = false;
            if (!_isOnAction.Value)
            {
                if (_nextActionTurns.Value > 0)
                {
                    _nextActionTurns.Value -= 1;
                }
            }
            _isOnAction.Value = false;
        }

        /// <summary>
        /// 移動
        /// </summary>
        public void Move(HexCell moveCell, DirectionType directionType, double reducedMovePower)
        {
            _cell.Value = moveCell;
            _direction.Value = directionType;
            Status.MovePower.Add(-reducedMovePower);
            _movableDir.SetValueAndForceNotify(_mapMoveChecker.GetMovableDirection(moveCell, directionType));
            BattleState.Facade.UpdateFocusedHexCell(moveCell);
            _isOnAction.Value = true;
            // IsCancellable = true;
        }

        /// <summary>
        /// 行動決定
        /// </summary>
        public void DecideAction()
        {
            // IsCancellable = false;
            // SetCancellation();
            // _isOnAction.Value = true;
        }
        
        // /// <summary>
        // /// キャンセル
        // /// </summary>
        // public void Cancel()
        // {
        //     Move(_cancellationCell, _cancellationDir, Status.MovePower.Current - _cancellationMovePower); 
        //     BattleCamera.Instance.SetPosition(Position);
        //     IsCancellable = false;
        // }

        /// <summary>
        /// 行動終了
        /// </summary>
        public void EndAction()
        {
            //TODO _isSelected.Valueの方必要？
            _isBombardRangeActive.Value = _isAllBombardRangeActive || _isSelected.Value;
            _isOnAction.Value = true;
            _nextTurnSchedule.OnNext((false, ActionSpeed));
            UpdateNextActionTurns(ActionSpeed);
        }
        
        /// <summary>
        /// 移動力をセット
        /// </summary>
        public void SetMovePower(int power)
        {
            Status.MovePower.Set(power);
        }

        /// <summary>
        /// 移動力を割合でセット
        /// </summary>
        public void SetMovePowerRate(float rate)
        {
            var power = Status.MovePower.Current * rate;
            var round = Math.Round(power, 1);
            Status.MovePower.Set(round);
        }
        
        /// <summary>
        /// 行動ステータスをリセット
        /// </summary>
        public void ResetActionStatus()
        {
            // _isActionEnd.Value = false;
            Status.MovePower.Reset();
        }

        /// <summary>
        /// ダメージを与える
        /// </summary>
        public void DealDamage(int damage, AttackType attackType)
        {
            Status.ArmorPoint.Add(-damage);
            Status.CrewPoint.Add(-damage);
            if (Status.ArmorPoint.Current <= 0 || Status.CrewPoint.Current <= 0)
            {
                _onDefeated.OnNext(UnitId);
            }
        }
        
        /// <summary>
        /// 次のターンの更新
        /// </summary>
        public void UpdateNextActionTurns(int turns)
        {
            _nextActionTurns.Value = turns;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            ModelDisposable.Dispose();
        }
    }
}