using System;
using System.Security.Cryptography;
using System.Threading;

namespace App.AppCommon.Core
{
    /// <summary>
    /// 安全なランダムのためのプロバイダー
    /// </summary>
    /// <remarks>http://neue.cc/2013/03/06_399.html</remarks>
    public class RandomProvider
    {
        /// <summary>
        /// ランダムのWrapper
        /// </summary>
        private static readonly ThreadLocal<Random> RandomWrapper = new ThreadLocal<Random>(() =>
        {
            using var rng = RandomNumberGenerator.Create();
            var buffer = new byte[sizeof(int)];
            rng.GetBytes(buffer);
            var seed = BitConverter.ToInt32(buffer, 0);

            return new Random(seed);
        });

        /// <summary>
        /// 安全なRandomを生成する
        /// </summary>
        public static Random GetThreadRandom() => RandomWrapper.Value;
    }
}