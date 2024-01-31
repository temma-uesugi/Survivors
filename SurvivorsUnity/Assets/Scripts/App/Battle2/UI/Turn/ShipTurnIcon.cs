using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Battle2.UI.Turn
{
    /// <summary>
    /// 船ターンアイコン
    /// </summary>
    public class ShipTurnIcon : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI labelText;
        [SerializeField] private CanvasGroup canvasGroup;

        private Tweener _animTween;
        private Vector3 _prevPos;
       
        public uint UnitId { get; private set; }
        public int Turn { get; private set; } = -1;
        public int PrevTurn { get; private set; } = -1;
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Setup(uint unitId, string label, int turn)
        {
            UnitId = unitId;
            labelText.SetText(label);
            SetTurn(turn);
        }

        /// <summary>
        /// ターンセット
        /// </summary>
        public void SetTurn(int turn)
        {
            PrevTurn = Turn;
            Turn = turn;
        }

        /// <summary>
        /// 位置指定
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            rectTransform.position = position;
            _prevPos = position;
        }
        
        /// <summary>
        /// スケジュール設定
        /// </summary>
        public void SetSchedule(Vector3 position)
        {
            _prevPos = rectTransform.position;
            rectTransform.position = position;
            _animTween?.Kill();
            canvasGroup.alpha = 1;
            _animTween =  canvasGroup.DOFade(0, 0.6f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic)
                .SetLink(gameObject);
        }

        /// <summary>
        /// スケジュールClear
        /// </summary>
        public void ClearSchedule()
        {
            _animTween?.Kill(); 
            canvasGroup.alpha = 1;
            rectTransform.position = _prevPos;
        }
    }
}