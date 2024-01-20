#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using App.AppEditor.Common;
using UniRx;
using UnityEditor;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// マップ内のアイテムリスト表示
    /// </summary>
    public class MapItemList : StageEditorVisualElement
    {

        private readonly CompositeDisposable disposable = new();
        private readonly ScrollView container;
        private readonly Box objectListBox;
        private readonly Box enemyListBox;

        private readonly Subject<int> onRemoveItem = new();
        public IObservable<int> OnRemoveItem => onRemoveItem;
        private readonly Subject<ItemData> onSelectedItem = new();
        public IObservable<ItemData> OnSelectedItem => onSelectedItem;

        private readonly Dictionary<int, ItemDataListItem> itemDataMap = new();
        private readonly Dictionary<int, Box> recordBoxList = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapItemList(StageEditorWindow editorWindow) : base(editorWindow)
        {
            this.Append<ScrollView>(id: "mapItemListScroller", className: null, out container);
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{StageEditorWindow.SourceDir}/MapItemList.uss");
            styleSheets.Add(styleSheet);

            objectListBox = this.Q<Box>("mapObjectItemList");
            enemyListBox = this.Q<Box>("mapEnemyItemList");
        }

        /// <summary>
        /// アイテムをリストに追加
        /// </summary>
        public void AddItem(ItemData itemData)
        {
            var hashCode = itemData.Record.GetHashCode();
            if (!recordBoxList.TryGetValue(hashCode, out Box box))
            {
                container.Append<Box>(id: null, className: "recordListBox", out box);
                box.Append<Label>(out var label);
                label.text = itemData.Record.ItemLabel;
                recordBoxList.Add(hashCode, box);
                if (itemData.Record.IsPreset)
                {
                    label.AddToClassList("preset");
                }
            }
            box.Append<ItemDataListItem>(out var item);
            item.SetGrid(itemData);
            itemDataMap.Add(itemData.Id, item);
            item.OnRemoved.Subscribe(id =>
            {
                onRemoveItem.OnNext(id);
            }).AddTo(disposable);
            item.OnSelected.Subscribe(x =>
            {
                onSelectedItem.OnNext(x);
            }).AddTo(disposable);
        }

        /// <summary>
        /// アイテム除去
        /// </summary>
        public void RemoveItem(int id)
        {
            if (!itemDataMap.TryGetValue(id, out var item))
            {
                return;
            }
            var hashCode = item.Record.GetHashCode();
            if (!recordBoxList.TryGetValue(hashCode, out Box box))
            {
                return;
            }
            box.Remove(item);
            itemDataMap.Remove(id);
            if (box.childCount == 1) //Labelは必ず入っているので1
            {
                recordBoxList.Remove(hashCode);
                container.Remove(box);
            }
        }
    }
}
#endif
