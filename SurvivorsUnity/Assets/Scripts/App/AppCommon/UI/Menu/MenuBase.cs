using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.AppCommon.Core;
using App.Inputs;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace App.AppCommon.UI
{
    /// <summary>
    /// Menu
    /// </summary>
    public abstract class MenuBase<T, T2> : MonoBehaviour
        where T : Enum
        where T2 : IMenuActionInputs
    {
        private readonly List<MenuItemBase<T>> _itemList = new();
        private readonly List<MenuItemBase<T>> _activeItemList = new();
        private readonly List<MenuItemBase<T>> _enabledItemList = new();

        private bool _isItemTypeFlags = false;
        private MenuItemBase<T> _focusItem;
        private int _focusIndex = 0;

        private readonly Subject<T> _onSelected = new();

        protected T2 ActionInputs { get; private set; }
        
        /// <summary>
        /// Item作成
        /// </summary>
        protected abstract IEnumerable<MenuItemBase<T>> CreateItems(IEnumerable<T> items);

        /// <summary>
        /// LayoutGroupを取得
        /// </summary>
        protected abstract LayoutGroup LayoutGroup { get; }
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public virtual void Setup(T2 actionInputs)
        {
            ActionInputs = actionInputs;
            var flagsAttr = typeof(T).GetCustomAttribute<System.FlagsAttribute>();
            _isItemTypeFlags = flagsAttr != null;

            var itemList = new List<(T type, int order)>();
            foreach (T t in Enum.GetValues(typeof(T)))
            {
                var fieldInfo = typeof(T).GetField(t.ToString());
                var ignoreAttr = fieldInfo.GetCustomAttribute<IgnoreMenuItemAttribute>();
                if (ignoreAttr != null) continue;
                var orderAttr = fieldInfo.GetCustomAttribute<OrderAttribute>();
                var order = orderAttr?.Order ?? 0;
                itemList.Add((t, order));
            }
            var orderItemList = itemList.OrderBy(x => x.order).Select(x => x.type);
            _itemList.AddRange(CreateItems(orderItemList));
           
            actionInputs.MoveCursor
                .Where(_ => gameObject.activeSelf)
                .Subscribe(MoveCursor).AddTo(this);
            actionInputs.Decide
                .Where(_ => gameObject.activeSelf)
                .Subscribe(_ =>
                {
                    _onSelected.OnNext(_focusItem.ItemType);    
                }).AddTo(this);
            actionInputs.Cancel
                .Where(_ => gameObject.activeSelf)
                .Subscribe(_ => _onSelected.OnNext(default)).AddTo(this);
        }

        /// <summary>
        /// 開く
        /// </summary>
        public async UniTask<T> OpenAsync(T itemType, params T[] disabledItems)
        {
            ResetActiveItems(itemType);
            ResetEnabledItems(disabledItems);
            // if (_enabledItemList.Count == 1)
            // {
            //     return _enabledItemList[0].ItemType;
            // }
            return await WaitResultAsync();
        }

        /// <summary>
        /// 開く
        /// </summary>
        public async UniTask<T> OpenAsync(params T[] disabledItems)
        {
            ResetActiveItems();
            ResetEnabledItems(disabledItems); 
            if (_enabledItemList.Count == 1)
            {
                return _enabledItemList[0].ItemType;
            }
            return await WaitResultAsync();
        }

        /// <summary>
        /// リザルト待ち
        /// </summary>
        private async UniTask<T> WaitResultAsync()
        {
            _focusIndex = 0;
            _focusItem = _enabledItemList[0];
            _focusItem.SetFocusAsync(true).Forget();
           
            gameObject.SetActive(true);
            OpenEffectAsync().Forget();
            
            var type = await _onSelected.ToUniTask(useFirstValue: true);
            CloseAsync().Forget();
            return type;
        }
        
        /// <summary>
        /// ActiveItemsのセット
        /// </summary>
        private void ResetActiveItems(params T[] itemTypes)
        {
            _activeItemList.Clear();
            foreach (var item in _itemList)
            {
                var isActive = _isItemTypeFlags ? CheckTypeFlags(item.ItemType) : CheckType(item.ItemType);
                item.SetActive(isActive);
                if (isActive)
                {
                    _activeItemList.Add(item);
                }
            }

            bool CheckType(T type)
            {
                return itemTypes.Any(x => x.Equals(type));
            }

            bool CheckTypeFlags(T type)
            {
                if (type.Equals((T)default))
                {
                    return false;
                }
                return itemTypes.Any(x => x.HasFlag(type));
            }
        }
        
        /// <summary>
        /// ActiveItemsのセット
        /// </summary>
        private void ResetActiveItems()
        {
            _activeItemList.Clear();
            foreach (var item in _itemList)
            {
                item.SetActive(true);
                _activeItemList.Add(item);
            }
        }
        
        /// <summary>
        /// Itemのセット
        /// </summary>
        private void ResetEnabledItems(params T[] disabledItems)
        {
            _enabledItemList.Clear();
            foreach (var item in _activeItemList)
            {
                var isEnable = !disabledItems.Any(x => x.Equals(item.ItemType));
                item.SetEnabled(isEnable);
                if (isEnable)
                {
                    _enabledItemList.Add(item);
                }
                item.SetFocusAsync(false).Forget();
            }
            LayoutGroup.CalculateLayoutInputHorizontal(); 
            LayoutGroup.CalculateLayoutInputVertical(); 
            LayoutGroup.SetLayoutHorizontal();
            LayoutGroup.SetLayoutVertical();
        }
        
        /// <summary>
        /// 開くエフェクト
        /// </summary>
        protected virtual async UniTask OpenEffectAsync()
        {
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        protected virtual async UniTask CloseAsync()
        {
            gameObject.SetActive(false); 
        }
        
        /// <summary>
        /// カーソル移動
        /// </summary>
        private void MoveCursor(int moveDir)
        {
            _focusItem.SetFocusAsync(false).Forget(); 
            if (moveDir < 0 && _focusIndex <= 0)
            {
                _focusIndex = _enabledItemList.Count;
            }
            if (moveDir > 0 && _focusIndex >= _enabledItemList.Count - 1)
            {
                _focusIndex = 0;
            }
            else
            {
                _focusIndex += moveDir;
            }

            _focusItem = _enabledItemList[_focusIndex];
            _focusItem.SetFocusAsync(true).Forget();
        }

        /// <summary>
        /// ActiveなItemを非アクティブに
        /// </summary>
        private void ClearActiveItems()
        {
            foreach (var item in _activeItemList)
            {
                item.SetActive(false);
            }
            _activeItemList.Clear();
        }
    }
}