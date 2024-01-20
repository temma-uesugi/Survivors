using System.Collections.Generic;

namespace App.Battle.Map
{
    /// <summary>
    /// セルのグリッド計算
    /// </summary>
    public static class CellGridCalculator
    {
        /// <summary>
        /// 浅瀬セル
        /// </summary>
        public static List<(int x, int y)> CalcFordCell(int xAmount, int yAmount)
        {
            var list = new List<(int x, int y)>();
            for (int i = -1; i < yAmount + 1; i++)
            {
                list.Add((-1, i));
            }
            //右端
            for (int i = -1; i < yAmount + 1; i++)
            {
                list.Add((xAmount, i));
            }
            //下
            for (int i = 0; i < xAmount; i++)
            {
                list.Add((i, -1));
            }
            //上
            for (int i = 0; i < xAmount; i++)
            {
                list.Add((i, yAmount));
            }
            return list;
        }


        /// <summary>
        /// 陸セル計算
        /// </summary>
        /// <returns></returns>
        public static List<(int x, int y)> CalcGroundCell(int xAmount, int yAmount, int offset, int amount)
        {
            var list = new List<(int x, int y)>();
            //縦
            for (int y = -offset - amount; y < yAmount + offset + amount; y++)
            {
                for (int i = 0; i < amount; i++)
                {
                    var left = -offset - i - 1;
                    list.Add((left, y));
                    var right = xAmount + offset + i;
                    list.Add((right, y));
                }
            }

            //横
            for (int x = -offset; x < xAmount + offset; x++)
            {
                for (int i = 0; i < amount; i++)
                {
                    var bottom = -offset - i - 1;
                    list.Add((x, bottom));
                    var top = yAmount + offset + i;
                    list.Add((x, top));
                }
            }

            return list;
        }
    }
}