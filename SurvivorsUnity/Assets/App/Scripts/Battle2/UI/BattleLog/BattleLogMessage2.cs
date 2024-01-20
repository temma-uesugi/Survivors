using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle2.UI.BattleLog
{
    /// <summary>
    /// バトルログメッセージ
    /// </summary>
    public class BattleLogMessage2 : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Text text;

        private Tweener _tween;
        private Sequence _sequence;
        private static readonly Ease EaseType = Ease.OutQuint;

        /// <summary>
        /// Show
        /// </summary>
        public void Show(string textVal ,Vector3 pos, float duration)
        {
            _tween?.Kill();
            text.text = textVal;
            var go = gameObject;
            go.SetActive(true);
            canvasGroup.alpha = 0;
            rectTransform.position = pos;
            _tween = canvasGroup.DOFade(1, duration)
                .SetEase(EaseType)
                .SetLink(go);
        }


        /// <summary>
        /// 移動
        /// </summary>
        public void Move(Vector3 toPos, float duration)
        {
            _tween?.Kill();
            _tween = rectTransform.DOMove(toPos, duration)
                .SetEase(EaseType)
                .SetLink(gameObject);
            // _sequence?.Kill();
            // _sequence = DOTween.Sequence()
            //     .Join(
            //         rectTransform.DOMove(toPos, duration)
            //             .SetEase(EaseType)
            //     )
            //     .SetLink(gameObject);
        }

        /// <summary>
        /// Fade
        /// </summary>
        public void Fade(float fromAlpha, float toAlpha, float duration)
        {
            _tween?.Kill();
            canvasGroup.alpha = fromAlpha;
            _tween = canvasGroup.DOFade(toAlpha, duration)
                .SetEase(EaseType)
                .SetLink(gameObject);
        }

        /// <summary>
        /// 移動とFade
        /// </summary>
        public void MoveAndFace(Vector3 toPos, float fromAlpha, float toAlpha, float duration)
        {
            MoveAndFace(rectTransform.position, toPos, fromAlpha, toAlpha, duration);
        }

        /// <summary>
        /// 移動とFade
        /// </summary>
        public void MoveAndFace(Vector3 fromPos, Vector3 toPos, float fromAlpha, float toAlpha, float duration)
        {
            _sequence?.Kill();
            rectTransform.position = fromPos;
            canvasGroup.alpha = fromAlpha;
            _sequence = DOTween.Sequence()
                .Join(
                    canvasGroup.DOFade(toAlpha, duration)
                        .SetEase(EaseType)
                )
                .SetLink(gameObject);
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _sequence?.Kill();
            _tween?.Kill();
        }
    }
}