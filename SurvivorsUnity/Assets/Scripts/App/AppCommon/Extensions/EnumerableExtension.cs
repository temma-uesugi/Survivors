using System.Collections.Generic;
using System.Linq;
using System;

namespace App.AppCommon.Extensions
{
    /// <summary>
    /// Enumerableの拡張
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// ランダムに並び替える
        /// </summary>
        public static IEnumerable<T> RandomSort<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.OrderBy(_ => Guid.NewGuid());
        }

        /// <summary>
        /// ランダムに一つ選ぶ。
        /// 見つからない場合はDefault
        /// </summary>
        public static T RandomFirst<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.RandomSort().FirstOrDefault();
        }
    }
}