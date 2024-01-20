using System;
using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.AppCommon.Utils;
using App.Battle2.Core;
using App.Battle2.Map.Cells;
using App.Battle2.Objects;
using App.Battle2.Objects.Obstacle;
using App.Battle2.ValueObjects;
using UniRx;
using UnityEngine;

namespace App.Battle2.Map
{
    /// <summary>
    /// Hexグリッド
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(HexMapManager))]
    public class HexMapManager : MonoBehaviour
    {
        [SerializeField] private SeaHexCell seaCellPrefab;
        [SerializeField] private FordHexCell fordHexCell;
        [SerializeField] private GroundHexCell groundHexCell;
        [SerializeField] private MapController mapController; 

        private int _xAmount;
        private int _yAmount;

        public int Width => _xAmount;
        public int Height => _yAmount;
        
        private readonly HashSet<HexCell> _cellList = new HashSet<HexCell>();
        private SeaHexCell[,] _seaHexCells;
        public SeaHexCell[] AllSeaCells { get; private set; }

        // 浅瀬Cell
        private readonly HashSet<FordHexCell> _fordHexCells = new();
        public IEnumerable<FordHexCell> FordHexCells => _fordHexCells;

        private readonly HashSet<GroundHexCell> _groundHexCells = new();
        
        //障害物Cell
        private readonly Dictionary<uint, HexCell> _obstacleCellMap = new();
        public IEnumerable<HexCell> ObstacleCells => _obstacleCellMap.Values;
        //陸Cell
        private readonly HashSet<GroundHexCell> _landCells = new();
        public IReadOnlyCollection<GroundHexCell> LandCells => _landCells;
        //侵入禁止Cell
        public IEnumerable<HexCell> NoEntryCells => ObstacleCells.Concat(_landCells);

        //中心
        public Vector3 Center { get; private set; }

        //端チェック
        private bool IsLeftEdge(HexCell cell) => cell.Grid.X == 0;
        private bool IsRightEdge(HexCell cell) => cell.Grid.X == _xAmount - 1;
        private bool IsBottomEdge(HexCell cell) => cell.GridY == 0;
        private bool IsTopEdge(HexCell cell) => cell.GridY == _yAmount - 1;
        //縦軸ずれ
        private bool IsVerticalHalf(HexCell cell) => cell.GridX % 2 == 1;
        //横軸ずれ
        private bool IsHorizontalHalf(HexCell cell) => cell.GridY % 2 == 1;

        /// <summary>
        /// 障害物あり(ルート探索用に用意)
        /// </summary>
        public bool IsBlocked(int x, int y)
        {
            return false;
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(int xAmount, int yAmount, MapObjectManger objectManager)
        {
            _xAmount = xAmount;
            _yAmount = yAmount;

            //海のセル作成
            _seaHexCells = new SeaHexCell[_yAmount, _xAmount];
            for (int y = 0; y < _yAmount; y++)
            {
                for (int x = 0; x < _xAmount; x++)
                {
                    //TODO
                    var waveHeight = RandomUtil.Range(1, 3);
                    var cell = CreateSeaCell(x, y, waveHeight);
                    _cellList.Add(cell);
                    _seaHexCells[y, x] = cell;
                }
            }
            AllSeaCells = _seaHexCells.Cast<SeaHexCell>().ToArray();

            //周りに浅瀬セル
            foreach (var (x, y) in CellGridCalculator2.CalcFordCell(xAmount, yAmount))
            {
                var cell = CreateCell<FordHexCell>(x, y);
                _cellList.Add(cell);
                _fordHexCells.Add(cell);
            }

            //周りに陸セル4つずつ
            foreach (var (x, y) in CellGridCalculator2.CalcGroundCell(xAmount, yAmount, 1, 4))
            {
                var cell = CreateCell<GroundHexCell>(x, y);
                _cellList.Add(cell);
                _groundHexCells.Add(cell);
            }

            var groundHexCellOrders = _groundHexCells.OrderBy(x => x.GridX + x.GridY).ToArray();
            var maxPos = groundHexCellOrders.Last();
            var minPos = groundHexCellOrders.First();
            var width = maxPos.GridX - minPos.GridX;
            var height = (maxPos.GridY - minPos.GridY) * 0.85f;
            var mapRect = new Rect(0, 0, width, height);
            var seaMaxPos = _seaHexCells[yAmount - 1, xAmount - 1].Position;
            var seaRect = new Rect(0, 0, seaMaxPos.x, seaMaxPos.y);

            Center = _seaHexCells[(int)yAmount / 2, (int)xAmount / 2].Position;
            
            mapController.Setup();
           
            //障害物の追加
            foreach (var obstacle in objectManager.AllAliveObstacles)
            {
                OnAddedObstacle(obstacle);
            }
            objectManager.ObstacleModelMap.ObserveAdd().Subscribe(x => OnAddedObstacle(x.Value)).AddTo(this);
            objectManager.ObstacleModelMap.ObserveRemove().Subscribe(x => _obstacleCellMap.Remove(x.Key)).AddTo(this);
        }

        /// <summary>
        /// 海セルの作成
        /// </summary>
        private SeaHexCell CreateSeaCell(int x, int y, int waveHeight)
        {
            var pos = HexUtil2.HexPosToLocalPos(x, y);
            var seaCell = Instantiate(seaCellPrefab, transform);
            seaCell.transform.localPosition = pos;
            seaCell.Setup(x, y, waveHeight);
            return seaCell;
        }
        
        /// <summary>
        /// セルの作成
        /// </summary>
        private T CreateCell<T>(int x, int y) where T : HexCell
        {
            var pos = HexUtil2.HexPosToLocalPos(x, y);
            HexCell cell = typeof(T) switch
            {
                var t when t == typeof(SeaHexCell) => Instantiate<SeaHexCell>(seaCellPrefab, transform),
                var t when t == typeof(FordHexCell) => Instantiate<FordHexCell>(fordHexCell, transform),
                var t when t == typeof(GroundHexCell) => Instantiate<GroundHexCell>(groundHexCell, transform),
                _ => throw new Exception("HexCell 相違")
            };
            cell.transform.localPosition = pos;
            cell.Setup(x, y);
            return cell as T;
        }

        /// <summary>
        /// グリッドでセルを取得
        /// </summary>
        public HexCell GetCellByGrid(GridValue grid, bool includeFord = true)
        {
            var x = grid.X;
            var y = grid.Y;
            if (x < 0 || x >= _xAmount || y < 0 || y >= _yAmount)
            {
                return includeFord ?  _fordHexCells.FirstOrDefault(c => c.GridX == x && c.GridY == y) : null;
            }
            return _seaHexCells[y, x];
        }

        /// <summary>
        /// 方向を指定して次のセルは海か
        /// </summary>
        public bool IsSeaNextCellByDir(HexCell curCell, DirectionType dir)
        {
            //縦方向の制限
            //y軸最小
            if (IsBottomEdge(curCell))
            {
                //縦の-方向は通さない
                if (DirectionType.DiagonalBottom.HasFlag(dir))
                {
                    return false;
                }
            }
            //y軸最大
            else if (IsTopEdge(curCell))
            {
                //縦の+方向は通さない
                if (DirectionType.DiagonalTop.HasFlag(dir))
                {
                    return false;
                }
            }

            //横方向の制限
            //x軸最小
            if (IsLeftEdge(curCell))
            {
                //単純な左移動は通さない
                if (dir == DirectionType.Left)
                {
                    return false;
                }
                //x軸ずれていない場合、左移動を含む縦移動は通さない
                if (!IsHorizontalHalf(curCell))
                {
                    if (DirectionType.DiagonalLeft.HasFlag(dir))
                    {
                        return false;
                    }
                }
            }
            //x軸最大
            else if (IsRightEdge(curCell))
            {
                //単純な右移動は通さない
                if (dir == DirectionType.Right)
                {
                    return false;
                }
                //x軸ずれている場合、右移動を含む縦移動は通さない
                if (IsHorizontalHalf(curCell))
                {
                    if (DirectionType.DiagonalRight.HasFlag(dir))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 方向を指定して次のセル
        /// </summary>
        public HexCell GetNextCellByDir(HexCell curCell, DirectionType dir)
        {
            //縦方向の制限
            //y軸最小
            if (IsBottomEdge(curCell))
            {
                //縦の-方向は通さない
                if (DirectionType.DiagonalBottom.HasFlag(dir))
                {
                    return curCell;
                }
            }
            //y軸最大
            else if (IsTopEdge(curCell))
            {
                //縦の+方向は通さない
                if (DirectionType.DiagonalTop.HasFlag(dir))
                {
                    return curCell;
                }
            }
            
            //横方向の制限
            //x軸最小
            if (IsLeftEdge(curCell))
            {
                //単純な左移動は通さない
                if (dir == DirectionType.Left)
                {
                    return curCell;
                }
                //x軸ずれていない場合、左移動を含む縦移動は通さない
                if (!IsHorizontalHalf(curCell))
                {
                    if (DirectionType.DiagonalLeft.HasFlag(dir))
                    {
                        return curCell;
                    }
                }
            }
            //x軸最大
            else if (IsRightEdge(curCell))
            {
                //単純な右移動は通さない
                if (dir == DirectionType.Right)
                {
                    return curCell;
                }
                //x軸ずれている場合、右移動を含む縦移動は通さない
                if (IsHorizontalHalf(curCell))
                {
                    if (DirectionType.DiagonalRight.HasFlag(dir))
                    {
                        return curCell;
                    }
                }
            }
            
            var nextGrid = HexUtil2.GetNextGridByDirection(curCell.Grid, dir);
            var nextCell = GetCellByGrid(nextGrid);
            return nextCell;
        }

        /// <summary>
        /// 移動可能な方向を取得
        /// </summary>
        public DirectionType GetMovableDirection(HexCell curCell, DirectionType curDir)
        {
            var dir = DirectionType.None;

            switch (curDir)
            {
                case DirectionType.Right:
                {
                    //現在右
                    dir = DirectionType.AllRight;
                    if (IsRightEdge(curCell))
                    {
                        //一番右端 -> 右を削除
                        dir &= ~DirectionType.Right;
                    }
                    break;
                }
                case DirectionType.TopRight:
                {
                    //現在右上
                    dir = DirectionType.Right | DirectionType.TopRight | DirectionType.TopLeft;
                    if (IsTopEdge(curCell))
                    {
                        //一番上 -> 右上削除
                        dir &= ~DirectionType.TopRight;
                    }
                    if (IsRightEdge(curCell) && IsHorizontalHalf(curCell))
                    {
                        //一番右端かつx軸ずれている -> 右上削除
                        dir &= ~DirectionType.TopRight;
                    }
                    break;
                }
                case DirectionType.TopLeft:
                {
                    //現在左上
                    dir = DirectionType.TopRight | DirectionType.TopLeft | DirectionType.Left;
                    if (IsTopEdge(curCell))
                    {
                        //一番上 -> 左上削除
                        dir &= ~DirectionType.TopLeft;
                    }
                    if (IsLeftEdge(curCell) && !IsHorizontalHalf(curCell))
                    {
                        //一番左端かつx軸ずれていない -> 左上削除
                        dir &= ~DirectionType.TopLeft;
                    }
                    break;
                }
                case DirectionType.Left:
                {
                    //現在左
                    dir = DirectionType.AllLeft;
                    if (IsLeftEdge(curCell))
                    {
                        //一番左端 -> 左を削除
                        dir &= ~DirectionType.Left;
                    }
                    break;
                }
                case DirectionType.BottomLeft:
                {
                    //現在左下
                    dir = DirectionType.Left | DirectionType.BottomLeft | DirectionType.BottomRight;
                    if (IsBottomEdge(curCell))
                    {
                        //一番下 -> 左下削除
                        dir &= ~DirectionType.BottomLeft;
                    }
                    if (IsLeftEdge(curCell) && !IsHorizontalHalf(curCell))
                    {
                        //一番左端かつx軸ずれていない -> 左上削除
                        dir &= ~DirectionType.BottomLeft;
                    }
                    break;
                }
                case DirectionType.BottomRight:
                {
                    //現在右下
                    dir = DirectionType.BottomLeft | DirectionType.BottomRight | DirectionType.Right;
                    if (IsBottomEdge(curCell))
                    {
                        //一番下 -> 右下削除
                        dir &= ~DirectionType.BottomRight;
                    }
                    if (IsRightEdge(curCell) && IsHorizontalHalf(curCell))
                    {
                        //一番左端かつx軸ずれている -> 右上削除
                        dir &= ~DirectionType.BottomRight;
                    }
                    break;
                }
            }
            return dir;
        }

        /// <summary>
        /// 海を陸地に変換
        /// </summary>
        public void SwitchSeaToLand(GridValue grid)
        {
            var seaCell = GetCellByGrid(grid);
            if (seaCell == null) return;
           
            seaCell.gameObject.SetActive(false);
            var groundCell = CreateCell<GroundHexCell>(grid.X, grid.Y);
            _landCells.Add(groundCell);
        }
        
        /// <summary>
        /// 障害物を追加
        /// </summary>
        private void OnAddedObstacle(ObstacleModel obstacleModel)
        {
            if (_obstacleCellMap.ContainsKey(obstacleModel.ObjectId)) return;
            var cell = GetCellByGrid(obstacleModel.Grid);
            _obstacleCellMap[obstacleModel.ObjectId] = cell;
        }
        
        /// <summary>
        /// 障害物が除去
        /// </summary>
        private void OnRemovedObstacle(ObstacleModel obstacleModel)
        {
            _obstacleCellMap.Remove(obstacleModel.ObjectId);
        }
    }
}