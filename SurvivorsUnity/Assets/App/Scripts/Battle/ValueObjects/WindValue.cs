using App.AppCommon;
using App.Battle.Turn;

namespace App.Battle.ValueObjects
{
    /// <summary>
    /// 風速ValueObject
    /// </summary>
    public readonly struct WindValue : ITurnValue
    {
        public DirectionType Direction { get; }
        public int Strength { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindValue(DirectionType direction, int strength)
        {
            Direction = direction;
            Strength = strength;
        }

        //無風
        public static WindValue None => new WindValue(DirectionType.None, 0);
    }
}