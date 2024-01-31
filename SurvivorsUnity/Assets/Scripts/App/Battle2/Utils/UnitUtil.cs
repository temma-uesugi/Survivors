using App.AppCommon.Utils;

namespace App.Battle2.Utils
{
    /// <summary>
    /// ユニットUtil
    /// </summary>
    public static class UnitUtil
    {
        //Note: 複数スレッドからのアクセスの想定なし
        private static uint _seqId = 0;
        private static int _shipCnt = 1;
        private static int _enemyCnt = 0;
        private static int _shipIndex = 0;

        /// <summary>
        /// 船ラベル取得
        /// </summary>
        public static string GetShipLabel()
        {
            return _shipCnt++ switch
            {
                // 1 => "Ⅰ",
                // 2 => "Ⅱ",
                // 3 => "Ⅲ",
                // 4 => "Ⅳ",
                // 5 => "Ⅴ",
                // 6 => "Ⅵ",
                // 7 => "Ⅶ",
                // 8 => "Ⅷ",
                // 9 => "Ⅸ",
                // 10 => "Ⅹ",
                1 => "ⅰ",
                2 => "ⅱ",
                3 => "ⅲ",
                4 => "ⅳ",
                5 => "ⅴ",
                6 => "ⅵ",
                7 => "ⅶ",
                8 => "ⅷ",
                9 => "ⅸ",
                10 => "ⅹ",
                _ => "",
            };
        }

        /// <summary>
        /// 敵ラベル取得
        /// </summary>
        public static string GetEnemyLabel()
        {
            return StringUtil.IntToAlphabet(_enemyCnt++);
        }

        /// <summary>
        /// リセット
        /// </summary>
        public static void ResetShip()
        {
            _enemyCnt = 1;
        }
        
        /// <summary>
        /// リセット
        /// </summary>
        public static void ResetEnemy()
        {
            _enemyCnt = 0;
        }

        /// <summary>
        /// GetUnitId
        /// </summary>
        public static uint GetUnitId()
        {
            return ++_seqId;
        }

        /// <summary>
        /// Indexを取得
        /// </summary>
        public static int GetShipIndex()
        {
            return _shipIndex++;
            ;
        }
    }
}