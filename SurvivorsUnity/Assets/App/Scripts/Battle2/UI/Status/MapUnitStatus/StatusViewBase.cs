using TMPro;
using UnityEngine;

namespace App.Battle2.UI.Status.MapUnitStatus
{

    /// <summary>
    /// ステータスView基底
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class StatusViewBase : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nextActionTurns;
        [SerializeField] private GameObject nextActionTurnsObject;
        
        private RectTransform _rectTransform;
        private BattleCamera2 battleCamera2;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
       
        /// <summary>
        /// スケールのUpdate
        /// </summary>
        public void UpdateScale(float ratio)
        {
            _rectTransform.localScale = Vector3.one * ratio;
        }
        
        /// <summary>
        /// 位置セット
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            _rectTransform.position = position;
        }

        /// <summary>
        /// 撃破された
        /// </summary>
        public void OnDefeated()
        {
            gameObject.SetActive(false); 
        }
        
        /// <summary>
        /// 次の行動ターン更新
        /// </summary>
        public void UpdateNextActionTurns(int turns)
        {
            nextActionTurns.SetText("{0}", turns);
            nextActionTurnsObject.SetActive(turns > 0);
        }
    }
}