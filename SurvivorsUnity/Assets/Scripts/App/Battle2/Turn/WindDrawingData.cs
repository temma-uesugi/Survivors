using App.AppCommon;
using Master.Constants;

namespace App.Battle2.Turn
{
    /// <summary>
    /// 風向き抽出データ
    /// </summary>
    public class WindDirDrawingData : IDrawing
    {
        public DirectionType Type { get; init; }
        public int Weight { get; set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindDirDrawingData(DirectionType type, int weight)
        {
            Type = type;
            Weight = weight;
        }
    }

    /// <summary>
    /// 風速抽出データ
    /// </summary>
    public struct WindStrDrawingData : IDrawing
    {
        public int Strength { get; init; }
        public int Weight { get; init; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindStrDrawingData(int strength, int weight)
        {
            Strength = strength;
            Weight = weight;
        }
    }
}