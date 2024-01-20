using System.Linq;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Map.Cells;
using App.Battle2.Units;
using VContainer;

namespace App.Battle2.Map
{
    /// <summary>
    /// Hexマップの移動可能チェッカ
    /// </summary>
    [ContainerRegisterAttribute2(typeof(HexMapMoveChecker))]
    public class HexMapMoveChecker
    {
        private static HexMapManager _hexMapManager;
        private static UnitManger _unitManger;

        /// <summary>
        /// SetUp
        /// </summary>
        [Inject]
        public HexMapMoveChecker(
            HexMapManager hexMapManager,
            UnitManger unitManger
        )
        {
            _hexMapManager = hexMapManager;
            _unitManger = unitManger;
        }

        /// <summary>
        /// 移動可能な方向を取得
        /// </summary>
        public DirectionType GetMovableDirection(HexCell curCell, DirectionType curDir)
        {
            var movableDir = _hexMapManager.GetMovableDirection(curCell, curDir);
            var sideHexCells = HexUtil2.GetSideHexCell(curCell.Grid);
            //ユニットが存在するマップには侵入できない
            foreach (var unit in _unitManger.AllAliveUnitModels)
            {
                if (sideHexCells.Any(x => x == unit.Grid))
                {
                    var dir = HexUtil2.GetDirection(curCell.Grid, unit.Grid);
                    if (dir != DirectionType.None)
                    {
                        movableDir &= ~dir;
                    }
                }
            }
            //マップの侵入禁止セルのチェック
            foreach (var cell in _hexMapManager.NoEntryCells)
            {
                if (sideHexCells.Any(x => x == cell.Grid))
                {
                    var dir = HexUtil2.GetDirection(curCell.Grid, cell.Grid);
                    if (dir != DirectionType.None)
                    {
                        movableDir &= ~dir;
                    }
                }
            }

            return movableDir;
        }

        /// <summary>
        /// 必要な移動力を計算
        /// </summary>
        public double CalcMovePower(HexCell curCell, DirectionType curDir, DirectionType moveDir)
        {
            //TODO いろいろな計算
            if (curDir == moveDir)
            {
                return 1;
            }
            return 1.5;
        }
    }
}