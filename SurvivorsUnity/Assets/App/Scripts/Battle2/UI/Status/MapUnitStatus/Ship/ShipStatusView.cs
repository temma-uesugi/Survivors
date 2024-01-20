using App.Battle2.ValueObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle2.UI.Status.MapUnitStatus.Ship
{
    /// <summary>
    /// 船ステータス
    /// </summary>
    public class ShipStatusView : StatusViewBase
    {
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Image armorValue;
        [SerializeField] private Image crewValue;

        private int _maxAp;
        private int _maxCp;


        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(string label, StatusValue<int> statusAp, StatusValue<int> statusCp)
        {
            labelText.SetText(label);
            _maxAp = statusAp.Max;
            _maxCp = statusCp.Max;
            UpdateArmor(statusAp.Current);
            UpdateCrew(statusCp.Current);
        }

        /// <summary>
        /// 装甲更新
        /// </summary>
        public void UpdateArmor(int ap)
        {
            armorValue.fillAmount = (float)ap / _maxAp;
        }

        /// <summary>
        /// 船員数更新
        /// </summary>
        public void UpdateCrew(int cp)
        {
            crewValue.fillAmount = (float)cp / _maxCp;
        }
    }
}