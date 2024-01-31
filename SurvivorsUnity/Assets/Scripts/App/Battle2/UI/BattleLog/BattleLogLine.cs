using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle2.UI.BattleLog
{
    /// <summary>
    /// バトルログライン
    /// </summary>
    public class BattleLogLine : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTrans;
        [SerializeField] private Text logText;
        [SerializeField] private RectTransform textRectTrans;
        [SerializeField] private CanvasGroup canvasGroup;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(string log)
        {
            canvasGroup.alpha = 0;
            logText.text = log;
        }

        /// <summary>
        /// 表示アニメ
        /// </summary>
        public void ShowAnimation()
        {
            var width = rectTrans.rect.width;
            textRectTrans.anchoredPosition = new Vector2(-width, 0);
            DOTween.Sequence()
                .Append(textRectTrans.DOAnchorPosX(0, 0.3f).SetEase(Ease.OutCirc))
                .Join(canvasGroup.DOFade(1, 0.3f).SetEase(Ease.InCirc))
                .SetLink(gameObject)
                .Play();
        }
    }
}