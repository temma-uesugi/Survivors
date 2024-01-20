using System;

namespace App.Battle2.ValueObjects
{
    /// <summary>
    /// GridのValueObject
    /// </summary>
    public record GridValue(int X, int Y)
    {
        /// <summary>
        /// operator +
        /// </summary>
        public static GridValue operator +(GridValue left, GridValue right)
        {
            return new GridValue(left.X + right.X, left.Y + right.Y);
        }

        /// <summary>
        /// operator -
        /// </summary>
        public static GridValue operator -(GridValue left, GridValue right)
        {
            return new GridValue(left.X - right.X, left.Y - right.Y);
        }
        
        /// <summary>
        /// 範囲内に収める
        /// </summary>
        public static GridValue InRange(GridValue grid, GridValue minGrid, GridValue maxGrid)
        {
            var x = Math.Min(Math.Max(grid.X, minGrid.X), maxGrid.X);
            var y = Math.Min(Math.Max(grid.Y, minGrid.Y), maxGrid.Y);
            return new GridValue(x, y);
        }

        /// <summary>
        /// グリッド0
        /// </summary>
        public static GridValue GridZero => new GridValue(0, 0);

    }
}