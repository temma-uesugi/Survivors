using App.Battle.ValueObjects;
using UnityEngine;
using App.AppCommon;

namespace App.Battle.UI.Turn
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
            return ResourceMaps.Instance.WindIcon.GetSprite(data.Direction, data.Strength);
        }

        /// <summary>
        /// ラベル取得
        /// </summary>
        protected override string GetLabel(WindValue data) => "";
    }
}