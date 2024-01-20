using App.Battle2.Interfaces;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle2.UI.HexButtons
{
    /// <summary>
    /// 点滅攻撃対象Hex用ボタン
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class AttackTargetButton : UnitHexButton
    {
        [SerializeField] private Image selectedImage;

        private CanvasGroup _canvasGroup;
        private Tweener _blinkTween;
        private IAttackTargetModel _targetUnitModel;

        public IAttackTargetModel Target => _targetUnitModel;
        public uint TargetId => _targetUnitModel.Id;
        
        /// <summary>
        /// Awake
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
            SetActive(false);
        }

        /// <summary>
        /// Activeのセット
        /// </summary>
        public override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            SetSelected(false);
            _blinkTween?.Kill();
            _canvasGroup.alpha = 1;
            if (isActive)
            {
                _blinkTween = _canvasGroup.DOFade(0, 1f)
                    .SetEase(Ease.InOutCirc)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        /// <summary>
        /// 対象をセット
        /// </summary>
        public void SetTarget(IAttackTargetModel targetUnitModel)
        {
            _targetUnitModel = targetUnitModel;
            UpdatePosition();
        }

        /// <summary>
        /// 位置更新
        /// </summary>
        public void UpdatePosition()
        {
            if (_targetUnitModel == null)
            {
                return;
            }

            SetCell(_targetUnitModel.Cell.Value);
        }

        /// <summary>
        /// 対象をActiveに
        /// </summary>
        public void SetSelected(bool isSelected)
        {
            selectedImage.gameObject.SetActive(isSelected);
        }
    }
}