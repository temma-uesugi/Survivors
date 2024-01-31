using App.Battle2.UI.HexButtons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Battle2.UI.Controller.Unit
{
    /// <summary>
    /// 移動ボタン
    /// </summary>
    public class MoveButton : UnitHexButton
    {
        [SerializeField] private TextMeshProUGUI needPower;
        [SerializeField] private Image image;

        private Color _validColor;
        private Color _invalidColor;

        /// <summary>
        /// Awake
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _validColor = image.color;
            _invalidColor = Color.gray;
            _invalidColor.a = 0.8f;
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        public void UpdateStatus(UnitController.MovableStatus status)
        {
            SetActive(status.IsActive);
            needPower.text = status.NeedPower.ToString("F1");
            image.color = status.IsMovable ? _validColor : _invalidColor;
        }
    }
}