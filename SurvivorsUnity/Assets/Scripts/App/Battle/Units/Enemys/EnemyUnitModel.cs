using App.Battle.Map;
using App.Battle2.ValueObjects;
using Master;
using Master.Tables.Enemy;
using UniRx;
using UnityEngine;

namespace App.Battle.Units
{
    /// <summary>
    /// 敵UnitModel
    /// </summary>
    public class EnemyUnitModel : IUnitModel
    {
        //基本Data
        public EnemyBaseEntity EnemyBaseEntity { get; }
        public uint EnemyId => EnemyBaseEntity.EnemyId;
        public uint UnitId { get; } 
        public uint Id => UnitId;
        public string Label { get; }
        private readonly MapManager _mapManager;
        public string ImageId => EnemyBaseEntity.ImageId;
        
        //位置
        private readonly ReactiveProperty<HexCell> _cell;
        public IReadOnlyReactiveProperty<HexCell> Cell => _cell;
        public GridValue Grid => _cell.Value.Grid;
        public Vector3 Position => _cell.Value.Position;

        public bool IsAlive => true;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyUnitModel(
            EnemyCreateParam createParam,
            MapManager mapManager
        )
        {
            UnitId = createParam.EnemyUnitId;
            Label = createParam.Label;
            _cell = new (createParam.InitCell);
            _mapManager = mapManager;
            
            var enemyBase = MasterData.Facade.EnemyBaseEntityTable.FindByEnemyId(createParam.EnemyId);
            var enemyStatus = MasterData.Facade.EnemyLevelStatusEntityTable.FindByEnemyIdAndLevel((createParam.EnemyId, createParam.Level));
            EnemyBaseEntity = enemyBase;
            
            //
            // ActionInterval = enemyBase.ActionInterval;
            // MovePower = enemyBase.MovePower;
            // Hp = new StatusValue<int>(enemyStatus.Hp);
            // DamageCalculator = new EnemyDamageCalculator(this);
            // AttackRange = _skills.Max(x => x.MaxRange);
            // _nextActionTurns.Value = ActionInterval - 1;
            //
            // _enemyMoveRange = new EnemyMoveRange(mapManager, this);
            // _enemyMoveRange.UpdateCell(_cell.Value);
            // _enemyAttackRange = new EnemyAttackRange(mapManager, this);
            // _enemyAttackRange.UpdateCell(_cell.Value);
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            
        }
    }
}