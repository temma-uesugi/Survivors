using App.AppCommon;
using App.Battle2.Turn;

namespace App.Battle2.ValueObjects
{
    /// <summary>
    /// 天気のValueObject
    /// </summary>
    public readonly struct WeatherValue : ITurnValue
    {
        public WeatherType Weather { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WeatherValue(WeatherType weather)
        {
            Weather = weather;
        }
        
        //無風
        public static WeatherValue None => new WeatherValue(WeatherType.Cloud);
    }
}