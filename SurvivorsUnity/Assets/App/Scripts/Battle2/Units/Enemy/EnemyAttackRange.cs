using System.Collections.Generic;
using System.Linq;
using App.Battle2.Map;
using App.Battle2.Map.Cells;
using App.Battle2.Utils;
using UniRx;
using Unity.VisualScripting;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵の攻撃範囲
    /// </summary>
    public class EnemyAttackRange
    {
        private readonly ReactiveProperty<IEnumerable<HexCell>> _attackRangeCell = new();
        public IReadOnlyReactiveProperty<IEnumerable<HexCell>> AttackRangeCell => _attackRangeCell;

        private readonly HexMapManager _mapManager;
        private readonly EnemyUnitModel2 unitModel2;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyAttackRange(HexMapManager mapManager, EnemyUnitModel2 unitModel2)
        {
            _mapManager = mapManager;
            this.unitModel2 = unitModel2;
        }

        /// <summary>
        /// 攻撃可能セル取得
        /// </summary>
        public void UpdateCell(HexCell cell)
        {
            var cellHashSet = new HashSet<HexCell>();
            foreach (var moveHex in unitModel2.MoveRangeCell.Value)
            {
                var cells = _mapManager.AllSeaCells
                    .Where(x => x.Grid != moveHex.Grid)
                    //TODO: なんで1.1fしてるんだっけ？
                    .Where(x => HitUtil.IsCircleAndPoint(moveHex.Position, unitModel2.AttackRange * 1.1f, x.Position));
                cellHashSet.AddRange(cells);
            }

            _attackRangeCell.Value = cellHashSet;
            // _attackRangeCell.Value = _mapManager.AllSeaCells
            //     .Where(x => x.Grid != cell.Grid)
            //     .Where(x => HitUtil.IsCircleAndPoint(cell.Position, _unitModel.AttackStatus.Value.AttackRangeDistance, x.Position));
        }
    }
}