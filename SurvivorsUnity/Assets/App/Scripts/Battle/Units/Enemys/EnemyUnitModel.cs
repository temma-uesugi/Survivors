using App.Battle.Map;
using App.Battle2.ValueObjects;
using UniRx;
using UnityEngine;

namespace App.Battle.Units
{
    /// <summary>
    /// 敵UnitModel
    /// </summary>
    public class EnemyUnitModel : IUnitModel
    {
        //位置
        private readonly ReactiveProperty<HexCell> _cell;
        public IReadOnlyReactiveProperty<HexCell> Cell => _cell;
        public GridValue Grid => _cell.Value.Grid;
        public Vector3 Position => _cell.Value.Position;
      
        public uint UnitId { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyUnitModel(
            EnemyCreateParam createParam,
            MapManager mapManager
        )
        {
            // UnitId = createParam2.EnemyUnitId;
            // Label = createParam2.Label;
            // _cell = new (createParam2.InitCell);
            // _mapManager = mapManager;
            //
            // var enemyBase = MasterData.Facade.EnemyBaseTable.FindByEnemyId(createParam2.EnemyId);
            // var enemyStatus = MasterData.Facade.EnemyLevelStatusTable.FindByEnemyIdAndLevel((createParam2.EnemyId, createParam2.Level));
            // _skills = MasterData.Facade.EnemySkillSetTable.FindBySkillSetId(enemyBase.SkillSetId)
            //     .Select(x => MasterData.Facade.EnemySkillTable.FindBySkillId(x.SkillId))
            //     .ToArray();
            // EnemyBase = enemyBase;
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