using App.AppCommon.Utils;

namespace App.Battle2.ValueObjects
{
    /// <summary>
    /// intの範囲Value
    /// </summary>
    public readonly struct IntRangeValue
    {
        public int Min { get; }
        public int Max { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public IntRangeValue(int min, int max)
        {
            Min = min;
            Max = max;
        }

    }

    /// <summary>
    /// floatの範囲Value
    /// </summary>
    public readonly struct FloatRangeValue
    {
        public float Min { get; }
        public float Max { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public FloatRangeValue(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
    
    /// <summary>
    /// 拡張
    /// </summary>
    public static class RangeValueExtensions
    {
        /// <summary>
        /// 取得
        /// </summary>
        public static int Pick(this IntRangeValue value) => RandomUtil.Range(value.Min, value.Max);
        
        /// <summary>
        /// 取得
        /// </summary>
        public static float Pick(this FloatRangeValue value) => RandomUtil.Range(value.Min, value.Max);
    }

}