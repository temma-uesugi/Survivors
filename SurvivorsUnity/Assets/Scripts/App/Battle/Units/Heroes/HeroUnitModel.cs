using App.Battle.Map;
using App.Battle2.ValueObjects;
using UniRx;
using UnityEngine;

namespace App.Battle.Units
{
    /// <summary>
    /// 味方UnitModel
    /// </summary>
    public class HeroUnitModel : IUnitModel
    {
        /// <summary>
        /// 基本情報
        /// </summary>
        public uint UnitId { get; } 
        public uint Id => UnitId;
        public string Label { get; }
        private readonly MapManager _mapManager;
        public string ImageId => "Hero1";
        public int FormationId { get; }
        
        //位置
        private readonly ReactiveProperty<HexCell> _cell;
        public IReadOnlyReactiveProperty<HexCell> Cell => _cell;
        public GridValue Grid => _cell.Value.Grid;
        public Vector3 Position => _cell.Value.Position;

        public bool IsAlive => true;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HeroUnitModel(
            HeroCreateParam createParam,
            MapManager mapManager
        )
        {
            UnitId = createParam.HeroUnitId;
            Label = createParam.Label;
            _cell = new (createParam.InitCell);
            FormationId = createParam.FormationIndex;
            _mapManager = mapManager;
            
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