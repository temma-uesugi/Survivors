using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.UI.Status
{
    /// <summary>
    /// 敵ステータス
    /// </summary>
    public class EnemyStatusView : StatusViewBase
    {
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private Image hpValue;
        [SerializeField] private GameObject nextTurn;

        private int _maxHp;
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(string label, int currentHp, int maxHp)
        {
            _maxHp = maxHp;
            UpdateHp(currentHp);
            labelText.SetText(label);
        }

        /// <summary>
        /// HP更新
        /// </summary>
        public void UpdateHp(int hp)
        {
            hpValue.fillAmount = (float)hp / _maxHp;
        }

        /// <summary>
        /// 次の行動ターンの表示・非表示
        /// </summary>
        public void SetActiveNextActionTurns(bool isActive)
        {
            nextTurn.SetActive(isActive); 
        }
    }
}