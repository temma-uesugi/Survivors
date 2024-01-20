using App.AppCommon.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace App.Battle2.UI.Damage
{
    /// <summary>
    /// ダメージView
    /// </summary>
    public class DamageView : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTrans;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private CanvasGroup canvasGroup;

        private Sequence _sequenceTask;

        /// <summary>
        /// 表示
        /// </summary>
        public async UniTask ShowAsync(int damage, Vector2 position)
        {
            rectTrans.position = RandomUtil.PositionInCircle(position, 100);
            text.SetText("{0}", damage);
            await DOTween.Sequence()
                .Append(canvasGroup.DOFade(1, 0.25f).SetEase(Ease.OutCirc))
                .Join(rectTrans.DOMoveY(10, 0.5f).SetRelative().SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutCirc)) 
                .AppendInterval(0.5f)
                .Append(canvasGroup.DOFade(0, 0.25f).SetEase(Ease.OutCirc))
                .SetLink(gameObject)
                .ToUniTask();
        }
    }
}