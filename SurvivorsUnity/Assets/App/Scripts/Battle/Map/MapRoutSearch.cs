using System;
using System.Collections.Generic;
using App.AppCommon;
using App.AppCommon.Extensions;
using App.Battle.Core;
using App.Battle.Map.Cells;
using App.Battle.Units;
using App.Battle.ValueObjects;
using VContainer;

namespace App.Battle.Map
{
    /// <summary>
    /// 経路探索(A*探索)
    /// </summary>
    [ContainerRegister(typeof(MapRoutSearch))]
    public class MapRoutSearch
    {
        private static MapRoutSearch _instance;

        //Y偶数時のオフセット
        private static readonly (int x, int y)[] EventYOffsets =
        {
            (0, 1), //右上
            (1, 0), //右
            (0, -1), //右下
            (-1, -1), //左下
            (-1, 0), //左
            (-1, 1), //左上
        };

        //Y奇数時のオフセット
        private static readonly (int x, int y)[] OddYOffsets =
        {
            (1, 1), //右上
            (1, 0), //右
            (1, -1), //右下
            (0, 1), //左下
            (-1, 0), //左
            (0, -1), //左上
        };
        
        private HexMapManager _mapManager;
        private UnitManger _unitManger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(HexMapManager mapManager, UnitManger unitManger)
        {
            _mapManager = mapManager;
            _unitManger = unitManger;
            _instance = this;
        }

        /// <summary>
        /// 距離計算
        /// </summary>
        public static int HeuristicDistance(HexCell startCell, HexCell goalCell) =>
            HeuristicDistance((startCell.GridX, startCell.GridY), (goalCell.GridX, goalCell.GridY));
        
        /// <summary>
        /// 距離計算
        /// </summary>
        private static int HeuristicDistance((int x, int y) start, (int x, int y) goal)
        {
            int startX = start.x;
            int startY = start.y;
            int goalX = goal.x;
            int goalY = goal.y;
            int dy = Math.Abs(goalY - startY);
            int dx = Math.Abs(goalX - startX);
            int xAdjust = (startY % 2 == 0, startX - goalX) switch
            {
                (true, > 0) => (int)Math.Ceiling(dy / 2.0),
                (false, < 0) => (int)Math.Ceiling(dy / 2.0),
                _ => (int)(dy / 2.0),
            };
            dx = Math.Max(dx - xAdjust, 0);
            return dx + dy;
        }

        /// <summary>
        /// 経路探索
        /// </summary>
        public static List<GridValue> FindPath(HexCell startCell, HexCell goalCell) =>
            _instance._FindPath(startCell.Grid, goalCell.Grid);

        /// <summary>
        /// 経路探索
        /// </summary>
        private List<GridValue> _FindPath(GridValue startGrid, GridValue goalGrid)
        {
            var openList = new SortedSet<(double f, GridValue grid)>(Comparer<(double f, GridValue grid)>.Create((a, b) => a.f.CompareTo(b.f)));
            var closedSet = new HashSet<GridValue>();
            var cameFrom = new Dictionary<GridValue, GridValue>();
            var checkedList = new HashSet<GridValue>();

            openList.Add((0, startGrid));
            checkedList.Add(startGrid);

            while (openList.Count > 0)
            {
                var current = openList.Min;
                openList.Remove(current);

                if (current.grid == goalGrid)
                {
                    var path = new List<GridValue>();
                    while (cameFrom.ContainsKey(current.grid))
                    {
                        path.Add(current.grid);
                        current = (0, cameFrom[current.grid]);
                    }
                    path.Add(startGrid);
                    path.Reverse();
                    return path;
                }

                closedSet.Add(current.grid);

                var offsets = current.grid.Y % 2 == 0 ? EventYOffsets : OddYOffsets;
                foreach (var offset in offsets.RandomSort()) //経路をランダムにしたいなら
                // foreach (var offset in offsets.RandomSort())
                {
                    int x = current.grid.X + offset.x;
                    int y = current.grid.Y + offset.y;
                    var checkGrid = new GridValue(x, y);

                    if (_mapManager.IsBlocked(x, y) || closedSet.Contains(checkGrid))
                    {
                        continue;
                    }

                    if (!checkedList.Contains(checkGrid))
                    {
                        cameFrom[checkGrid] = current.grid;
                        checkedList.Add(checkGrid);
                        var heuristic = HeuristicDistance((x, y), (goalGrid.X, goalGrid.Y));
                        openList.Add((heuristic, checkGrid));
                    }
                }
            }

            return null;
        }
    }
}
