using System.Collections.Generic;
using App.AppCommon.Utils;
using App.Battle2.Turn;
using App.Battle2.ValueObjects;
using FastEnumUtility;
using Master.Constants;
using UnityEngine;

namespace Master.Battle.Map
{
    /// <summary>
    /// HexUtil
    /// </summary>
    public static class HexUtil
    {
        // private const float CellSize = 1;
        private const float CellSize = 0.9f;
        private const float DegPerSide = 60;

        /// <summary>
        /// Hexグリッド座標からローカル座標へ変換
        /// </summary>
        public static Vector3 HexPosToLocalPos(int x, int y)
        {
            var hexHeight = CellSize * Mathf.Sin(DegPerSide * Mathf.Deg2Rad);
            var hexAdjust = CellSize * Mathf.Cos(DegPerSide * Mathf.Deg2Rad);

            float posX = CellSize * x + hexAdjust * Mathf.Abs(y % 2);
            float posY = hexHeight * y;
            return new Vector3(posX, posY, 0);
        }

        /// <summary>
        /// 方向の次にあるGridを取得
        /// </summary>
        public static GridValue GetNextGridByDirection(GridValue grid, DirectionType dir)
        {
            int x = (grid.Y % 2 == 0, dir) switch
            {
                (_, DirectionType.Right) => 1,
                (true, DirectionType.TopRight) => 0,
                (false, DirectionType.TopRight) => 1,
                (true, DirectionType.TopLeft) => -1,
                (false, DirectionType.TopLeft) => 0,
                (_, DirectionType.Left) => -1,
                (true, DirectionType.BottomLeft) => -1,
                (false, DirectionType.BottomLeft) => 0,
                (true, DirectionType.BottomRight) => 0,
                (false, DirectionType.BottomRight) => 1,
                _ => 0,
            };
            int y = dir switch
            {
                DirectionType.Right => 0,
                DirectionType.TopRight => 1,
                DirectionType.TopLeft => 1,
                DirectionType.Left => 0,
                DirectionType.BottomLeft => -1,
                DirectionType.BottomRight => -1,
                _ => 0,
            };
            return grid + new GridValue(x, y);
        }

        /// <summary>
        /// 方向の次にあるGridを取得
        /// </summary>
        public static GridValue GetNextGridByDirection(GridValue grid, DirectionType dir, int range)
        {
            //range先を再起的に求めて取得する
            if (range == 0)
            {
                return grid;
            }
            if (range == 1)
            {
                return GetNextGridByDirection(grid, dir);
            }
            return GetNextGridByDirection(GetNextGridByDirection(grid, dir), dir, range - 1);
        }

        /// <summary>
        /// 方向から角度を計算
        /// </summary>
        public static int GetDegByDir(DirectionType dir)
        {
            return dir switch
            {
                DirectionType.Right => 0,
                DirectionType.TopRight => 60,
                DirectionType.TopLeft => 120,
                DirectionType.Left => 180,
                DirectionType.BottomLeft => 240,
                DirectionType.BottomRight => 300,
                _ => -1,
            };
        }

        /// <summary>
        /// 船の向きから砲台の角度を取得
        /// </summary>
        public static int GetBombDegByShipDir(DirectionType shipDir, BombSide bombSide)
        {
            var shipDeg = GetDegByDir(shipDir);
            var bombDeg = shipDeg + (bombSide == BombSide.Right ? -90 : 90);
            bombDeg = (bombDeg + 360) % 360;
            return bombDeg;
        }
        
        /// <summary>
        /// 現在のHexCellから隣り合うHexCellを取得
        /// </summary>
        public static GridValue[] GetSideHexCell(GridValue grid)
        {
            return new GridValue[]
            {
                GetNextGridByDirection(grid, DirectionType.Right),
                GetNextGridByDirection(grid, DirectionType.TopRight),
                GetNextGridByDirection(grid, DirectionType.TopLeft),
                GetNextGridByDirection(grid, DirectionType.Left),
                GetNextGridByDirection(grid, DirectionType.BottomLeft),
                GetNextGridByDirection(grid, DirectionType.BottomRight),
            };
        }

        /// <summary>
        /// 方向を取得
        /// </summary>
        public static DirectionType GetDirection(GridValue baseGrid, GridValue targetGrid)
        {
            var diffX = targetGrid.X - baseGrid.X;
            var diffY = targetGrid.Y - baseGrid.Y;
            if (diffY == 0)
            {
                //同じ段横方向
                if (baseGrid.X < targetGrid.X)
                {
                    return DirectionType.Right;
                }
                if (baseGrid.X > targetGrid.X)
                {
                    return DirectionType.Left;
                }
                return DirectionType.None;
            }

            if (baseGrid.Y % 2 == 0)
            {
                //偶数の段
                if (diffY > 0)
                {
                    //上方向
                    if (diffY - diffX == 1)
                    {
                        //右上
                        return DirectionType.TopRight;
                    }
                    if (-diffY == diffX)
                    {
                        //左上
                        return DirectionType.TopLeft;
                    }
                }
                else
                {
                    //下方向
                    if (diffY - diffX == -1)
                    {
                        //右下
                        return DirectionType.BottomRight;
                    }
                    if (diffY == diffX)
                    {
                        //左下
                        return DirectionType.BottomLeft;
                    }
                }
            }
            else
            {
                //奇数の段
                if (diffY > 0)
                {
                    if (diffY == diffX)
                    {
                        //右上
                        return DirectionType.TopRight;
                    }
                    if (diffY - diffX == 1)
                    {
                        //左上
                        return DirectionType.TopLeft;
                    }
                }
                else
                {
                    if (-diffY == diffX)
                    {
                        //右下
                        return DirectionType.BottomRight;
                    }
                    if (diffY - diffX == -1)
                    {
                        //左下
                        return DirectionType.BottomLeft;
                    }
                }
            }
            return DirectionType.None;
        }

        /// <summary>
        /// 方向から抽出の重みを計算
        /// </summary>
        public static void ReflectDrawTable(DirectionType dir, ref Dictionary<int, WindDirDrawingData> table)
        {
            switch (dir)
            {
                case DirectionType.Right:
                    table[DirectionType.Right.ToInt32()].Weight += 2;
                    table[DirectionType.TopRight.ToInt32()].Weight += 1;
                    table[DirectionType.BottomRight.ToInt32()].Weight += 1;
                    break;
                case DirectionType.TopRight:
                    table[DirectionType.TopRight.ToInt32()].Weight += 2;
                    table[DirectionType.Right.ToInt32()].Weight += 1;
                    table[DirectionType.TopLeft.ToInt32()].Weight += 1;
                    break;
                case DirectionType.TopLeft:
                    table[DirectionType.TopLeft.ToInt32()].Weight += 2;
                    table[DirectionType.Left.ToInt32()].Weight += 1;
                    table[DirectionType.TopRight.ToInt32()].Weight += 1;
                    break;
                case DirectionType.Left:
                    table[DirectionType.Left.ToInt32()].Weight += 2;
                    table[DirectionType.TopLeft.ToInt32()].Weight += 1;
                    table[DirectionType.BottomLeft.ToInt32()].Weight += 1;
                    break;
                case DirectionType.BottomLeft:
                    table[DirectionType.BottomLeft.ToInt32()].Weight += 2;
                    table[DirectionType.Left.ToInt32()].Weight += 1;
                    table[DirectionType.BottomRight.ToInt32()].Weight += 1;
                    break;
                case DirectionType.BottomRight:
                    table[DirectionType.BottomRight.ToInt32()].Weight += 2;
                    table[DirectionType.BottomLeft.ToInt32()].Weight += 1;
                    table[DirectionType.Right.ToInt32()].Weight += 1;
                    break;
                default:
                    table[DirectionType.Right.ToInt32()].Weight = GameConst.WindDirDrawDefaultWeight;
                    table[DirectionType.TopRight.ToInt32()].Weight = GameConst.WindDirDrawDefaultWeight;
                    table[DirectionType.TopLeft.ToInt32()].Weight = GameConst.WindDirDrawDefaultWeight;
                    table[DirectionType.Left.ToInt32()].Weight = GameConst.WindDirDrawDefaultWeight;
                    table[DirectionType.BottomLeft.ToInt32()].Weight = GameConst.WindDirDrawDefaultWeight;
                    table[DirectionType.BottomRight.ToInt32()].Weight = GameConst.WindDirDrawDefaultWeight;
                    break;
            }
        }
        
        /// <summary>
        /// 移動方向の計算
        /// </summary>
        public static InputDirectionType InputVectorToMoveDir(Vector2 vector2)
        {
            var angle = MathUtil.Vector2ToAngle(vector2);
            return angle switch
            {
                var i when i <= 22.5 => InputDirectionType.Right,
                var i when i > 22.5 && i <= 67.5 => InputDirectionType.TopRight,
                var i when i > 67.5 && i <= 112.5 => InputDirectionType.Top,
                var i when i > 112.5 && i <= 157.5 => InputDirectionType.TopLeft,
                var i when i > 157.5 && i <= 202.5 => InputDirectionType.Left,
                var i when i > 202.5 && i <= 247.5 => InputDirectionType.BottomLeft,
                var i when i > 247.5 && i <= 292.5 => InputDirectionType.Bottom,
                var i when i > 292.5 && i <= 337.5 => InputDirectionType.BottomRight,
                var i when i > 337.5 && i <= 360 => InputDirectionType.Right,
                _ => InputDirectionType.None,
            };
        }

        /// <summary>
        /// 入力方向をHexの方向に
        /// </summary>
        public static DirectionType InputDirToMoveDir(InputDirectionType inputDir, DirectionType curDir)
        {
            
            var dir = DirectionType.None;
            if (curDir == DirectionType.Right)
            {
                dir = inputDir switch
                {
                    InputDirectionType.Right => DirectionType.Right,
                    InputDirectionType.TopRight => DirectionType.TopRight,
                    InputDirectionType.Top => DirectionType.TopRight,
                    InputDirectionType.Bottom => DirectionType.BottomRight,
                    InputDirectionType.BottomRight => DirectionType.BottomRight,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.TopRight)
            {
                dir = inputDir switch
                {
                    InputDirectionType.Right => DirectionType.Right,
                    InputDirectionType.BottomRight => DirectionType.Right,
                    InputDirectionType.Top => DirectionType.TopRight,
                    InputDirectionType.TopRight => DirectionType.TopRight,
                    InputDirectionType.TopLeft => DirectionType.TopLeft,
                    InputDirectionType.Left => DirectionType.TopLeft,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.TopLeft)
            {
                dir = inputDir switch
                {
                    InputDirectionType.Left => DirectionType.Left,
                    InputDirectionType.BottomLeft => DirectionType.Left,
                    InputDirectionType.Top => DirectionType.TopLeft,
                    InputDirectionType.TopLeft => DirectionType.TopLeft,
                    InputDirectionType.TopRight => DirectionType.TopRight,
                    InputDirectionType.Right => DirectionType.TopRight,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.Left)
            {
                dir = inputDir switch
                {
                    InputDirectionType.Left => DirectionType.Left,
                    InputDirectionType.TopLeft => DirectionType.TopLeft,
                    InputDirectionType.Top => DirectionType.TopLeft,
                    InputDirectionType.Bottom => DirectionType.BottomLeft,
                    InputDirectionType.BottomLeft => DirectionType.BottomLeft,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.BottomLeft)
            {
                dir = inputDir switch
                {
                    InputDirectionType.Left => DirectionType.Left,
                    InputDirectionType.TopLeft => DirectionType.Left,
                    InputDirectionType.Bottom => DirectionType.BottomLeft,
                    InputDirectionType.BottomLeft => DirectionType.BottomLeft,
                    InputDirectionType.BottomRight => DirectionType.BottomRight,
                    InputDirectionType.Right => DirectionType.BottomRight,
                    _=> DirectionType.None,
                };
            }
            else if (curDir == DirectionType.BottomRight)
            {
                dir = inputDir switch
                {
                    InputDirectionType.Right => DirectionType.Right,
                    InputDirectionType.TopRight => DirectionType.Right,
                    InputDirectionType.Bottom => DirectionType.BottomRight,
                    InputDirectionType.BottomRight => DirectionType.BottomRight,
                    InputDirectionType.BottomLeft => DirectionType.BottomLeft,
                    InputDirectionType.Left => DirectionType.BottomLeft,
                    _=> DirectionType.None,
                };
            }
            return dir;
        }

        // /// <summary>
        // /// 範囲内か
        // /// </summary>
        // public static bool InRange(HexCell cellFrom, HexCell cellTo, int range)
        //     => MapRoutSearch.HeuristicDistance(cellFrom, cellTo) <= range;
    }
}