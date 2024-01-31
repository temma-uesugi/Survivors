using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.AppCommon.Core;
using App.Battle2.Core;
using App.Battle2.Interfaces;
using App.Battle2.Map.Cells;
using App.Battle2.Objects;
using App.Battle2.Units;
using App.Battle2.Units.Ship;
using App.Battle2.Utils;
using FastEnumUtility;
using UnityEngine;
using VContainer;

namespace App.Battle2.Map
{
    /// <summary>
    /// Hexマップの攻撃可能
    /// </summary>
    [ContainerRegisterAttribute2(typeof(HexMapAttackChecker))]
    public class HexMapAttackChecker
    {
        private readonly HexMapManager _hexMapManager;
        private readonly UnitManger2 unitManger2;
        private readonly MapObjectManger _objectManager;

        /// <summary>
        /// SetUp
        /// </summary>
        [Inject]
        public HexMapAttackChecker(
            HexMapManager hexMapManager,
            UnitManger2 unitManger2,
            MapObjectManger objectManager
        )
        {
            _hexMapManager = hexMapManager;
            this.unitManger2 = unitManger2;
            _objectManager = objectManager;
        }

        /// <summary>
        /// 切り込み対象となるユニットのID取得
        /// </summary>
        public IEnumerable<IAttackTargetModel> GetAttackTargetUnits(HexCell curCell, DirectionType curDir)
        {
            //チェックする方向
            List<DirectionType> checkDir;
            //向き毎の処理分け
            if (curDir == DirectionType.None)
            {
                //なし
                checkDir = new List<DirectionType>()
                {
                    DirectionType.Right,
                    DirectionType.TopRight,
                    DirectionType.TopLeft,
                    DirectionType.Left,
                    DirectionType.BottomLeft,
                    DirectionType.BottomRight,
                };
            }
            else if (DirectionType.Horizontal.HasFlag(curDir))
            {
                //水平
                checkDir = new List<DirectionType>()
                {
                    DirectionType.TopRight,
                    DirectionType.TopLeft,
                    DirectionType.BottomLeft,
                    DirectionType.BottomRight,
                };
            }
            else if ((DirectionType.TopRight | DirectionType.BottomLeft).HasFlag(curDir))
            {
                //斜め '／'の方向の斜め
                checkDir = new List<DirectionType>()
                {
                    DirectionType.Right,
                    DirectionType.TopLeft,
                    DirectionType.Left,
                    DirectionType.BottomRight,
                };
            }
            else if ((DirectionType.TopLeft | DirectionType.BottomRight).HasFlag(curDir))
            {
                //斜め '＼'の方向の斜め
                checkDir = new List<DirectionType>()
                {
                    DirectionType.Right,
                    DirectionType.TopRight,
                    DirectionType.Left,
                    DirectionType.BottomLeft,
                };
            }
            else
            {
                Log.Error("エラー");
                checkDir = new List<DirectionType>();
            }

            var attackCell = checkDir.Select(x => _hexMapManager.GetNextCellByDir(curCell, x));
            var list = unitManger2.AllAliveEnemies
                .Where(x => attackCell.Any(c => c  == x.Cell.Value));
            return list;
        }


        /// <summary>
        /// 突撃対象となるユニットのID取得
        /// </summary>
        public IAttackTargetModel GetAssaultTargetUnit(HexCell curCell, DirectionType curDir)
        {
            var assaultCell = _hexMapManager.GetNextCellByDir(curCell, curDir);
            return unitManger2.AllAliveEnemies
                .FirstOrDefault(x => x.Cell.Value == assaultCell);
        }

        /// <summary>
        /// 砲撃対象の取得
        /// </summary>
        public IEnumerable<IAttackTargetModel> GetBombardTargetUnits(ShipUnitModel2 attackerShip)
        {
            var list = new List<IAttackTargetModel>();
            list.AddRange(GetBombTargetUnits(BombSide.Right, attackerShip.UnitId, attackerShip.Cell.Value, 
                attackerShip.Direction.Value, attackerShip.RightBombStatus.Value));
            list.AddRange(GetBombTargetUnits(BombSide.Left, attackerShip.UnitId, attackerShip.Cell.Value, 
                attackerShip.Direction.Value, attackerShip.LeftBombStatus.Value));
            return list;
        }

        /// <summary>
        /// 砲撃対象の取得(サイド毎)
        /// </summary>
        public IEnumerable<IAttackTargetModel> GetBombTargetUnits(BombSide side, uint unitId, HexCell shipCell,
            DirectionType shipDir, BombStatus bombStatus, bool isPenetration = false, bool isHighParabola = false)
        {
            if (!bombStatus.IsAttackPossible)
            {
                return Enumerable.Empty<IAttackTargetModel>();
            }
            var bombDeg = HexUtil2.GetBombDegByShipDir(shipDir, side);
            var bombRad = bombDeg * Mathf.Deg2Rad;
            var bombDir = new Vector3(Mathf.Cos(bombRad), Mathf.Sin(bombRad), 0);
            var rangeTargets = unitManger2.AllAliveEnemies.OfType<IAttackTargetModel>()
                .Concat(_objectManager.AllAliveObstacles)
                .Where(target => HitUtil.IsArcAndPoint(
                        shipCell.Position,
                        bombStatus.RangeDistance,
                        bombDir,
                        bombStatus.RangeType.ToInt32(),
                        target.Position
                    )
                )
                .Where(target => (target.Position - shipCell.Position).sqrMagnitude >= 2)
                .ToList();

            IEnumerable<IAttackTargetModel> targets;
            if (!isPenetration && !isHighParabola)
            {
                //障害物の影響を受ける
                var allBlocks = unitManger2.AllAliveUnitModels.OfType<IBlockBombModel>()
                    .Concat(_objectManager.AllAliveObstacles)
                    .Where(x => x.Id != unitId).ToArray();
                targets = rangeTargets
                    .Where(target =>
                    {
                        //遮蔽物を排除
                        return !allBlocks
                            .Where(x => x.Id != target.Id)
                            .Any(unit =>
                                HitUtil.IsArcAndPoint(
                                    shipCell.Position,
                                    (target.Position - shipCell.Position).magnitude,
                                    (target.Position - shipCell.Position).normalized,
                                    15,
                                    unit.Cell.Value.Position
                                )
                        );
                    });
            }
            else
            {
                //障害物の影響を受けない
                targets = rangeTargets;
            }
            return targets;
        }
    }
}