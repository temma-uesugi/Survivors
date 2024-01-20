using App.AppCommon;

namespace App.Battle.Turn
{
    /// <summary>
    /// 天気の抽選データ
    /// </summary>
    public class WeatherDrawingData : IDrawing
    {
        public WeatherType Type { get; init; }
        public int Weight { get; init; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WeatherDrawingData(WeatherType type, int weight)
        {
            Type = type;
            Weight = weight;
        }
    }
}