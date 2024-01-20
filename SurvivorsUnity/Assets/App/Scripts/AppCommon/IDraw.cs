using System.Collections.Generic;
using System.Linq;
using App.AppCommon.Utils;

namespace App.AppCommon
{
    /// <summary>
    /// 抽出可能
    /// </summary>
    public interface IDrawing
    {
        //重み
        int Weight { get; }
    }

    /// <summary>
    /// 抽選の対象となるクラスに持たせるインターフェースの拡張
    /// </summary>
    public static class IDrawingExtensions
    {
        /// <summary>
        /// 重みで抽選
        /// </summary>
        public static T DrawByWeight<T>(this T[] list) where T : IDrawing
        {
            return RandomUtil.DrawByWeight(list);
        }

        /// <summary>
        /// 重みで抽選
        /// </summary>
        public static T DrawByWeight<T>(this IEnumerable<T> list) where T : IDrawing
        {
            return RandomUtil.DrawByWeight(list.ToArray());
        }
    }
}