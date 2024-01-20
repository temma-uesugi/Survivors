#if UNITY_EDITOR
using System;
using App.AppEditor.Common;
using App.AppEditor.StageEditor.Records;
using UniRx;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// ItemDataのリストの各Item
    /// </summary>
    public class ItemDataListItem : VisualElement
    {
        private readonly Button selectBtn;
        private readonly Button removeBtn;
        private ItemData itemData;
        public SettingRecord Record => itemData.Record;

        private readonly Subject<ItemData> onSelected = new();
        public IObservable<ItemData> OnSelected => onSelected;
        private readonly Subject<int> onRemoved = new();
        public IObservable<int> OnRemoved => onRemoved;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ItemDataListItem()
        {
            this.AddToClassList("listItem");
            this.Append<Button>(id: null, className: "selectBtn", out selectBtn);
            this.Append<Button>(id: null, className: "removeBtn", out removeBtn);
            removeBtn.text = "×";

            selectBtn.clickable.clicked += () =>
            {
                if (itemData != null)
                {
                    onSelected.OnNext(itemData);
                }
            };
            removeBtn.clickable.clicked += () =>
            {
                if (itemData != null)
                {
                    onRemoved.OnNext(itemData.Id);
                }
            };
        }

        /// <summary>
        /// セット
        /// </summary>
        public void SetLabel(ItemData data)
        {
            RemoveFromClassList("hidden");
            itemData = data;
            selectBtn.text = itemData.Record.ItemLabel;
            if (itemData.Record.IsPreset)
            {
                selectBtn.AddToClassList("preset");
            }
            else
            {
                selectBtn.RemoveFromClassList("preset");
            }
        }

        /// <summary>
        /// Gridのセット
        /// </summary>
        public void SetGrid(ItemData data)
        {
            RemoveFromClassList("hidden");
            itemData = data;
            selectBtn.text = $"x:{data.X}, y:{data.Y}";
        }

        /// <summary>
        /// 非表示
        /// </summary>
        public void Hide()
        {
            itemData = null;
            AddToClassList("hidden");
        }
    }

}
#endif
