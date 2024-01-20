using System.Collections.Generic;
using App.AppCommon;
using App.AppCommon.Extensions;
using App.Battle.Units.Enemy;
using App.Battle.Units.Ship;
using App.Battle.ValueObjects;

namespace App.Battle.Debug
{
    //Note: 仮対応
    /// <summary>
    /// ランダムObject生成
    /// </summary>
    public class RandomObjectCreator
    {
        private static readonly IntRangeValue ApRange = new IntRangeValue(80, 120);
        private static readonly IntRangeValue CpRange = new IntRangeValue(80, 120);
        private static readonly IntRangeValue MovePowerRange = new IntRangeValue(5, 8);
        private static readonly IntRangeValue EnemyMovePowerRange = new IntRangeValue(3, 5);
        private static readonly IntRangeValue EnemyActionSpeedRange = new IntRangeValue(1, 3);
        private static readonly IntRangeValue ShipActionSpeedRange = new IntRangeValue(2, 4);
        private static readonly IntRangeValue HpRange = new IntRangeValue(20, 30);

        private readonly HashSet<GridValue> _grids = new();

        /// <summary>
        /// ランダム作成
        /// </summary>
        public RandomObjectCreator(int xCell, int yCell)
        {
            for (int x = 0; x < xCell; x++)
            {
                for (int y = 0; y < yCell; y++)
                {
                    _grids.Add(new GridValue(x, y));
                }
            }
        }

        /// <summary>
        /// ランダムに船生成Paramを取得
        /// </summary>
        public IEnumerable<(ShipStatus status, GridValue grid)> GetRandomShipParam(int amount, bool isRandom)
        {
            int rangeTypeInt = 1;
            var list = new List<(ShipStatus status, GridValue grid)>();
            for (int i = 0; i < amount; i++)
            {
                
                var rangeType = rangeTypeInt switch
                {
                    1 => BombRangeType.Narrow,
                    2 => BombRangeType.Middle,
                    3 => BombRangeType.Wide,
                    4 => BombRangeType.Narrow,
                    5 => BombRangeType.Middle,
                    6 => BombRangeType.Wide,
                };
                rangeTypeInt++;

                var grid = new GridValue(i * 2 + 1, 0);
                if (isRandom)
                {
                    grid = _grids.RandomFirst();
                    _grids.Remove(grid);
                }
                var movePower = new StatusValue<double>(MovePowerRange.Pick());
                var hp = new StatusValue<int>(ApRange.Pick());
                var cp = new StatusValue<int>(CpRange.Pick());
                var actionSpeed = ShipActionSpeedRange.Pick();
                var randStatus = new ShipStatus(movePower, hp, cp, actionSpeed, new BombStatus(rangeType, 4), new BombStatus(rangeType, 6));
                list.Add((randStatus, grid));
            }
            return list;
        }

        /// <summary>
        /// ランダムにGridを取得
        /// </summary>
        /// <returns></returns>
        public GridValue GetRandomGrid()
        {
            var grid = _grids.RandomFirst();
            _grids.Remove(grid);
            return grid;
        }
        
        /// <summary>
        /// ランダムに敵生成Paramを取得
        /// </summary>
        public IEnumerable<(EnemyStatus status, GridValue grid)> GetRandomEnemyParam(int amount)
        {
            var list = new List<(EnemyStatus status, GridValue grid)>();
            for (int i = 0; i < amount; i++)
            {
                var grid = _grids.RandomFirst();
                _grids.Remove(grid);
                var hp = new StatusValue<int>(HpRange.Pick());
                var randStatus = new EnemyStatus(hp, EnemyMovePowerRange.Pick(), EnemyActionSpeedRange.Pick(), EnemyAttackStatus.DummyStatus);
                // var param = new EnemyCreateParam(randStatus, grid);
                list.Add((randStatus, grid));
            }
            return list;
        }

        /// <summary>
        /// ランダムに障害物を作成
        /// </summary>
        public IEnumerable<GridValue> GetRandomObstacleParam(int amount)
        {
            var list = new List<GridValue>();
            for (int i = 0; i < amount; i++)
            {
                var grid = _grids.RandomFirst();
                _grids.Remove(grid);
                list.Add(grid);
            }
            return list;
        }

        /// <summary>
        /// ランダムにGridを取得
        /// </summary>
        public IEnumerable<GridValue> GetRandomGrid(int amount)
        {
            var list = new List<GridValue>();
            for (int i = 0; i < amount; i++)
            {
                var grid = _grids.RandomFirst();
                _grids.Remove(grid);
                list.Add(grid);
            }
            return list;
        }

    }
}