using System.Collections.Generic;
using System.Linq;
using App.AppCommon.Core;
using UnityEngine;
using Random = System.Random;
using App.AppCommon.Extensions;

namespace App.AppCommon.Utils
{
    /// <summary>
    /// ランダムUtil
    /// </summary>
    public static class RandomUtil
    {
        private static Random random => RandomProvider.GetThreadRandom();

        /// <summary>
        /// 0-1のランダムな値をとる
        /// </summary>
        public static float Value() => (float)random.NextDouble();

        /// <summary>
        /// 確率判定
        /// </summary>
        /// <returns>成功したか</returns>
        public static bool Judge(float probability) => probability > 0 && probability >= random.NextDouble();

        /// <summary>
        /// 指定間のランダムな値をとる
        /// </summary>
        public static float Range(float from, float to) => (float)(from + (to - from) * random.NextDouble());

        /// <summary>
        /// 指定間のランダムな値をとる
        /// </summary>
        public static int Range(int from, int to, bool isLessThanTo = true) => from + random.Next(to + (isLessThanTo ? 1 : 0) - from);
        
        /// <summary>
        /// 重みが格納されたリストからその重みで抽選してデータを返す。
        /// </summary>
        public static T DrawByWeight<T>(T[] list) where T : IDrawing
        {
            var totalWeight = list.Sum(x => x.Weight);
            if (totalWeight == 0)
            {
                //ランダムで
                return list.First();
            }

            var rnd = Range(0, totalWeight);
            foreach (var d in list)
            {
                if (rnd < d.Weight)
                {
                    return d;
                }
                rnd -= d.Weight;
            }
            return list.Last();
        }

        /// <summary>
        /// 番号をランダムに並び替える
        /// </summary>
        public static IEnumerable<int> RandomNumberSort(int num)
        {
            var list = new List<int>();
            for (int i = 0; i < num; i++)
            {
                list.Add(i);
            }
            return list.RandomSort();
        }

        /// <summary>
        /// 大きさが1のランダムな向きのベクトルを返す
        /// </summary>
        public static Vector2 Direction()
        {
            float randomAngle = Range(0f, Mathf.PI * 2);
            return new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        }

        /// <summary>
        /// 円内でランダムな位置を取る
        /// </summary>
        public static Vector2 PositionInCircle(Vector2 center, float radius)
        {
            if (radius <= 0)
            {
                return center;
            }
            var dir = Direction();
            var distance = Range(-radius, radius);
            return center + distance * dir;
        }

    }
}