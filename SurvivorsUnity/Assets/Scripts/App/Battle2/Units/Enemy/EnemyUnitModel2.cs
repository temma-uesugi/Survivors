using System;
using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle2.Interfaces;
using App.Battle2.Map;
using App.Battle2.Map.Cells;
using App.Battle2.Units.Ship;
using App.Battle2.ValueObjects;
using Cysharp.Threading.Tasks;
using Master;
using Master.Constants;
using Master.Tables.Enemy;
using UniRx;
using UnityEngine;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 船のModel
    /// </summary>
    public class EnemyUnitModel2 : IUnitModel2, IAttackTargetModel, IBlockBombModel
    {
        public CompositeDisposable ModelDisposable { get; } = new CompositeDisposable();
        
        public EnemyBase EnemyBase { get; }
        public uint EnemyId => EnemyBase.EnemyId;
        public uint UnitId { get; }
        public uint Id => UnitId;
        public string Label { get; }
        private readonly HexMapManager _mapManager;

        public DamageCalculator DamageCalculator { get; private set; }
        public int CalcDamaged(float damage, AttackType attackType) => DamageCalculator.CalcDamaged(damage, attackType);

        private readonly EnemySkill[] _skills;
        
        //位置
        private readonly ReactiveProperty<HexCell> _cell;
        public IReadOnlyReactiveProperty<HexCell> Cell => _cell;
        public GridValue Grid => _cell.Value.Grid;
        public Vector3 Position => _cell.Value.Position;

        //向き
        private readonly ReactiveProperty<DirectionType> _direction = new();
        public IReadOnlyReactiveProperty<DirectionType> Direction => _direction;
        
        //Hp
        public readonly StatusValue<int> Hp;
        public bool IsAlive => Hp.Current >= 1;

        public int AttackRange { get; }
        
        //攻撃範囲
        private readonly EnemyAttackRange _enemyAttackRange;
        public IReadOnlyReactiveProperty<IEnumerable<HexCell>> AttackRangeCell => _enemyAttackRange.AttackRangeCell;
        //移動範囲
        private readonly EnemyMoveRange _enemyMoveRange;
        public IReadOnlyReactiveProperty<IEnumerable<HexCell>> MoveRangeCell => _enemyMoveRange.MoveRangeCell;
       
        //移動力
        public int MovePower { get; private set; }
        public int MovePowerRest { get; private set; }
        
        //攻撃回数
        public int AttackCountRest { get; private set; }
        
        private readonly Subject<uint> _onDefeated = new();
        public IObservable<uint> OnDefeated => _onDefeated;
        private readonly Subject<uint> _onRevived = new();
        public IObservable<uint> OnRevived => _onRevived;
       
        //行動済み
        // private readonly IReactiveProperty<bool> _isActionEnd = new ReactiveProperty<bool>(false);
        public IReadOnlyReactiveProperty<bool> IsActionEnd => _nextActionTurns.Select(x => x > 0).ToReactiveProperty();
       
        //選択中
        private readonly IReactiveProperty<bool> _isSelected = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<bool> IsSelected => _isSelected;

        //行動間隔
        public int ActionInterval { get; private set; }
        
        //次の行動までのTurn
        private readonly IReactiveProperty<int> _nextActionTurns = new ReactiveProperty<int>(0);
        public IReadOnlyReactiveProperty<int> NextActionTurns => _nextActionTurns;

        //攻撃の対象としている船
        private readonly ReactiveProperty<ShipUnitModel2> _targetUnit = new();
        public IReadOnlyReactiveProperty<ShipUnitModel2> TargetUnit => _targetUnit;
        //Activeか
        public IReadOnlyReactiveProperty<bool> IsActive =>
            _targetUnit.Select(x => x != null).ToReadOnlyReactiveProperty();

        //行動可能か
        public bool IsActionable => IsActive.Value && _nextActionTurns.Value == 0;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyUnitModel2(
            EnemyCreateParam2 createParam2,
            HexMapManager mapManager
        )
        {
            UnitId = createParam2.EnemyUnitId;
            Label = createParam2.Label;
            _cell = new (createParam2.InitCell);
            _mapManager = mapManager;

            // var enemyBase = MasterData.Facade.EnemyBaseTable.FindByEnemyId(createParam2.EnemyId);
            // var enemyStatus = MasterData.Facade.EnemyLevelStatusTable.FindByEnemyIdAndLevel((createParam2.EnemyId, createParam2.Level));
            // _skills = MasterData.Facade.EnemySkillSetTable.FindBySkillSetId(enemyBase.SkillSetId)
            //     .Select(x => MasterData.Facade.EnemySkillTable.FindBySkillId(x.SkillId))
            //     .ToArray();
            // EnemyBase = enemyBase;

            // ActionInterval = enemyBase.ActionInterval;
            // MovePower = enemyBase.MovePower;
            // Hp = new StatusValue<int>(enemyStatus.Hp);
            DamageCalculator = new EnemyDamageCalculator(this);
            AttackRange = _skills.Max(x => x.MaxRange);
            _nextActionTurns.Value = ActionInterval - 1;

            _enemyMoveRange = new EnemyMoveRange(mapManager, this);
            _enemyMoveRange.UpdateCell(_cell.Value);
            _enemyAttackRange = new EnemyAttackRange(mapManager, this);
            _enemyAttackRange.UpdateCell(_cell.Value);
        }

        /// <summary>
        /// アクション回数をリセット
        /// </summary>
        public void ResetActionCount()
        {
            AttackCountRest = 1;
            MovePowerRest = MovePower;
        }
        
        /// <summary>
        /// 移動
        /// </summary>
        public void Move(HexCell moveCell)
        {
            _cell.Value = moveCell;
            MovePowerRest--;
        }
        
        /// <summary>
        /// 攻撃済み
        /// </summary>
        public void Attacked()
        {
            AttackCountRest--; 
        }
        
        /// <summary>
        /// 移動
        /// </summary>
        public async UniTask MoveCellsAsync(IEnumerable<GridValue> grids)
        {
            var cells = grids
                .Select(grid => _mapManager.GetCellByGrid(grid));
            foreach (var cell in cells)
            {
                Move(cell);
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f));
            }
        }
        
        /// <summary>
        /// ダメージを与える
        /// </summary>
        public void DealDamage(int damage, AttackType attackType)
        {
            Hp.Add(-damage);
            if (Hp.Current <= 0)
            {
                _onDefeated.OnNext(UnitId);
            }
        }

        /// <summary>
        /// ターンのスタート
        /// </summary>
        public void OnTurnStart()
        {
            // _isActionEnd.Value = false;
            if (!IsActive.Value) return;
            var val = Math.Max(_nextActionTurns.Value - 1, 0);
            _nextActionTurns.Value = val;
        }

        /// <summary>
        /// ターンのスタート
        /// </summary>
        public void OnTurnEnd()
        {
            // _isActionEnd.Value = false;
        }
        
        /// <summary>
        /// 行動終了
        /// </summary>
        public void EndAction()
        {
            UpdateNextActionTurns(ActionInterval);
        }

        /// <summary>
        /// 次のターンの更新
        /// </summary>
        public void UpdateNextActionTurns(int turns)
        {
            _nextActionTurns.Value = turns;
        }

        /// <summary>
        /// ターゲットをセット
        /// </summary>
        public void SetTarget(ShipUnitModel2 shipUnitModel2)
        {
            _targetUnit.Value = shipUnitModel2;
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