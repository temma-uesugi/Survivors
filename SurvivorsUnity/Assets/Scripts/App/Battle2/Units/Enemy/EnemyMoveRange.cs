using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle2.Map;
using App.Battle2.Map.Cells;
using App.Battle2.Utils;
using UniRx;
using Constants;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵の移動範囲
    /// </summary>
    public class EnemyMoveRange
    {
        private readonly ReactiveProperty<IEnumerable<HexCell>> _moveRangeCell = new();
        public IReadOnlyReactiveProperty<IEnumerable<HexCell>> MoveRangeCell => _moveRangeCell;

        private readonly HexMapManager _mapManager;
        private readonly EnemyUnitModel2 unitModel2;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyMoveRange(HexMapManager mapManager, EnemyUnitModel2 unitModel2)
        {
            _mapManager = mapManager;
            this.unitModel2 = unitModel2;
        }
        
        /// <summary>
        /// 移動可能セル取得
        /// </summary>
        public void UpdateCell(HexCell cell)
        {
            // var list = new List<HexCell>();
            // for (int i = 0; i < _unitModel.MovePower; i++)
            // {
            //     
            // }
            
            //TODO 一旦単純な移動力移動力で行ける範囲
            _moveRangeCell.Value = _mapManager.AllSeaCells
                .Where(x => x.Grid != cell.Grid)
                .Where(x => HitUtil.IsCircleAndPoint(cell.Position, unitModel2.MovePower * 1.1f, x.Position));
        }

        /// <summary>
        /// セルの更新
        /// </summary>
        private void UpdateCellByDir(DirectionType dir, ref List<HexCell> hexCellList)
        {
            
        }
    }
}