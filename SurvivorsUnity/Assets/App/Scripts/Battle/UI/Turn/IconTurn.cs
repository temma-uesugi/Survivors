using System;
using App.Battle.Turn;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle.UI.Turn
{
    /// <summary>
    /// ターンアイコンの基本
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class IconTurn<T> : MonoBehaviour where T : ITurnValue 
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI labelText;
        
        private const float MoveDuration = 1f;
        private const float DisappearDuration = 0.5f;

        private CanvasGroup _canvasGroup;
        // private ObservableEventTrigger _eventTrigger;
        private RectTransform _rectTransform;

        public T Param { get; private set; }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(Vector3 position, Vector2 size, Sprite imageSprite, T param, string label = "")
        {
            Param = param;
            _rectTransform.position = position;
            _rectTransform.sizeDelta = size;
            _canvasGroup.alpha = 1;
            Setup(imageSprite, label);
        }

        /// <summary>
        /// Setup
        /// </summary>
        public virtual void Setup(Sprite imageSprite, string label = "")
        {
            if (imageSprite != null)
            {
                image.enabled = true;
                image.sprite = imageSprite;
            }
            else
            {
                image.enabled = false;
            }
            if (!string.IsNullOrEmpty(label))
            {
                labelText.enabled = true;
                labelText.SetText(label);
            }
            else
            {
                labelText.enabled = false;
            }
        }
        
        /// <summary>
        /// 空配置
        /// </summary>
        public void SetupEmpty(Vector3 position, Vector2 size)
        {
            _rectTransform.position = position;
            _rectTransform.sizeDelta = size;
            //TODO
            _canvasGroup.alpha = 0;
        }

        /// <summary>
        /// 空配置
        /// </summary>
        public void ChangeEmpty()
        {
            labelText.SetText(string.Empty);
            image.sprite = null;
            _canvasGroup.alpha = 0;
        }

        /// <summary>
        /// 移動
        /// </summary>
        public void MoveTo(Vector3 toPosition)
        {
            _rectTransform.DOMove(toPosition, MoveDuration)
                .SetEase(Ease.OutCubic);
        }

        /// <summary>
        /// 消える
        /// </summary>
        public async UniTask Disappear()
        {
            await _canvasGroup.DOFade(0, DisappearDuration)
                .SetEase(Ease.OutCubic)
                .ToUniTask();
        }
    }
}