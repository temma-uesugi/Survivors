using System.Collections.Generic;

namespace App.AppCommon.Extensions
{
    /// <summary>
    /// Listの拡張
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// 値を入れ替える
        /// </summary>
        /// <param name="i">インデックス1</param>
        /// <param name="j">インデックス2</param>
        public static void Swap<T>(this IList<T> self, int i, int j) => (self[i], self[j]) = (self[j], self[i]);
    }
}