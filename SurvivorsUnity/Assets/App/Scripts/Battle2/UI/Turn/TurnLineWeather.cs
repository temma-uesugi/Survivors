using App.AppCommon;
using App.Battle2.ValueObjects;
using UnityEngine;

namespace App.Battle2.UI.Turn
{
    /// <summary>
    /// 天候表示
    /// </summary>
    public class TurnLineWeather : TurnLineBase<WeatherValue, IconTurn<WeatherValue>>
    {
        [SerializeField] private IconTurnWeather iconTurnWeather;
        protected override IconTurn<WeatherValue> IconPrefab => iconTurnWeather;
        
        /// <summary>
        /// 画像取得
        /// </summary>
        protected override Sprite GetSprite(WeatherValue data)
        {
            return ResourceMaps.Instance.WeatherIcon.GetObject(data.Weather);
        }

        /// <summary>
        /// ラベル取得
        /// </summary>
        protected override string GetLabel(WeatherValue data) => "";
    }
}