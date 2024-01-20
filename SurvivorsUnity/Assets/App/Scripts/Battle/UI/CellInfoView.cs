using App.AppCommon.UI;
using App.Battle.Facades;
using App.Battle.Map.Cells;
using TMPro;
using UniRx;
using UnityEngine;

namespace App.Battle.UI
{
    /// <summary>
    /// セル情報
    /// </summary>
    public class CellInfoView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI infoText;

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            BattleState.Facade.FocusedHexCell.Subscribe(UpdateByCell).AddTo(this);
        }
        
        /// <summary>
        /// セルの更新
        /// </summary>
        private void UpdateByCell(HexCell cell)
        {
            var text = cell switch
            {
                var t when t is SeaHexCell sea => GetSeaHexInfo(sea),
                _ => ""
            };
            infoText.SetText(text);  
        }

        /// <summary>
        /// 海Hexの情報取得
        /// </summary>
        private string GetSeaHexInfo(SeaHexCell seaHexCell)
        {
            return seaHexCell.WaveHeight switch
            {
                1 => Lang.GetMasterString(MasterStringType.WaveHeight1),
                2 => Lang.GetMasterString(MasterStringType.WaveHeight2),
                3 => Lang.GetMasterString(MasterStringType.WaveHeight3),
                _ => "",
            };
        }
    }
}