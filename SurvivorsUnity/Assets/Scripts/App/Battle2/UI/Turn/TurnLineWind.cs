using App.AppCommon;
using App.AppCommon.ResourceMaps;
using App.Battle2.ValueObjects;
using UnityEngine;

namespace App.Battle2.UI.Turn
{
    /// <summary>
    /// 風向き表示
    /// </summary>
    public class TurnLineWind : TurnLineBase<WindValue, IconTurn<WindValue>>
    {
        [SerializeField] private IconTurnWind iconTurnWind;
        protected override IconTurn<WindValue> IconPrefab => iconTurnWind;
        
        /// <summary>
        /// 画像取得
        /// </summary>
        protected override Sprite GetSprite(WindValue data)
        {
            return ResourceMaps.Instance.WindIcon.GetObject(data.Direction, data.Strength);
        }

        /// <summary>
        /// ラベル取得
        /// </summary>
        protected override string GetLabel(WindValue data) => "";
    }
}