using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle.Map;
using App.Battle.Map.Cells;
using App.Battle.Utils;
using UniRx;

namespace App.Battle.Units.Enemy
{
    /// <summary>
    /// 敵の移動範囲
    /// </summary>
    public class EnemyMoveRange
    {
        private readonly ReactiveProperty<IEnumerable<HexCell>> _moveRangeCell = new();
        public IReadOnlyReactiveProperty<IEnumerable<HexCell>> MoveRangeCell => _moveRangeCell;

        private readonly HexMapManager _mapManager;
        private readonly EnemyUnitModel _unitModel;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyMoveRange(HexMapManager mapManager, EnemyUnitModel unitModel)
        {
            _mapManager = mapManager;
            _unitModel = unitModel;
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
                .Where(x => HitUtil.IsCircleAndPoint(cell.Position, _unitModel.MovePower * 1.1f, x.Position));
        }

        /// <summary>
        /// セルの更新
        /// </summary>
        private void UpdateCellByDir(DirectionType dir, ref List<HexCell> hexCellList)
        {
            
        }
    }
}