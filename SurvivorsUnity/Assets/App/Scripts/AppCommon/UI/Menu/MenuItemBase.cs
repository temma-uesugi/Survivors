using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace App.AppCommon.UI
{
    /// <summary>
    /// Menuアイテム
    /// </summary>
    public class MenuItemBase<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        private MenuItemRecord<T> _record;
        
        public bool IsActive { get; private set; }
        public T ItemType => _record.ItemType;
        
        /// <summary>
        /// Setup
        /// </summary>
        public virtual void Setup(MenuItemRecord<T> record)
        {
            _record = record;
        }


        /// <summary>
        /// Activeの設定
        /// </summary>
        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        /// <summary>
        /// Activeの設定
        /// </summary>
        public virtual void SetEnabled(bool isEnabled)
        {
            canvasGroup.alpha = isEnabled ? 1 : 0.5f;
            IsActive = isEnabled;
        }
        
        /// <summary>
        /// フォーカス
        /// </summary>
        public virtual async UniTask SetFocusAsync(bool isFocus)
        {
        }
    }
}