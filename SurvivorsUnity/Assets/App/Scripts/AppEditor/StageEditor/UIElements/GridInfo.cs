#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using App.AppEditor.Common;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// グリッドに設定されているItemのリスト
    /// </summary>
    public class GridInfo : StageEditorVisualElement
    {
        private readonly CompositeDisposable disposable = new();

        private readonly Box overlay;
        private readonly Box container;
        private readonly Button addBtn;
        private readonly Box list;
        private readonly Label label;

        private readonly List<ItemDataListItem> itemDataList = new();

        private readonly Subject<(int x, int y)> onAddItem = new();
        public IObservable<(int x, int y)> OnAddItem => onAddItem;
        private readonly Subject<(int x, int y)> onRemoveAll = new();
        public IObservable<(int x, int y)> OnRemoveAll => onRemoveAll;
        private readonly Subject<int> onRemoveItem = new();
        public IObservable<int> OnRemoveItem => onRemoveItem;
        private readonly Subject<ItemData> onSelectedItem = new();
        public IObservable<ItemData> OnSelectedItem => onSelectedItem;
        private readonly Subject<Unit> onOpened = new();
        public IObservable<Unit> OnOpened => onOpened;
        private readonly Subject<Unit> onClosed = new();
        public IObservable<Unit> OnClosed => onClosed;

        private Rect containerRect;
        private Rect rootRect;

        private int currentX;
        private int currentY;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GridInfo(StageEditorWindow editorWindow) : base(editorWindow)
        {
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{StageEditorWindow.SourceDir}/GridInfo.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{StageEditorWindow.SourceDir}/GridInfo.uss");
            styleSheets.Add(styleSheet);
            visualTreeAsset.CloneTree(this);

            overlay = this.Q<Box>("overlay");
            container = this.Q<Box>("gridInfoContainer");
            var closeBtn = this.Q<Button>(classes: "closeBtn");
            addBtn = this.Q<Button>("addItemBtn");
            var removeAllBtn = this.Q<Button>("removeAllItemBtn");
            list = this.Q<Box>("gridInfoList");
            label = this.Q<Label>(classes: "label");
            closeBtn.clickable.clicked += Hide;

            addBtn.clickable.clicked += () =>
            {
                onAddItem.OnNext((currentX, currentY));
            };
            removeAllBtn.clickable.clicked += () =>
            {
                onRemoveAll.OnNext((currentX, currentY));
            };
            overlay.RegisterCallback<ClickEvent>(e =>
            {
                Hide();
            });

            Hide();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init()
        {
            containerRect = container.contentRect;
            rootRect = EditorWindow.Body.contentRect;
            overlay.style.height = new StyleLength(rootRect.height);
        }

        /// <summary>
        /// Show
        /// </summary>
        public void Show(int x, int y, List<ItemData> itemDataList, bool isAddBtn)
        {
            currentX = x;
            currentY = y;
            label.text = $"x:{x}, y:{y}";
            addBtn.SetEnabled(isAddBtn);
            foreach (var item in this.itemDataList)
            {
                item.Hide();
            }
            var lenDiff = itemDataList.Count - this.itemDataList.Count;
            if (lenDiff > 0)
            {
                for (int i = 0; i < lenDiff; i++)
                {
                    AddItem();
                }
            }
            for (int i = 0; i < itemDataList.Count; i++)
            {
                this.itemDataList[i].SetLabel(itemDataList[i]);
            }

            //位置合わせ
            var pos = EditorWindow.GridToWorldPosition(x + 1, y);
            var posX = pos.x;
            var posY = pos.y;
            if (posX + containerRect.width + 20 >= rootRect.xMax)
            {
                posX -= containerRect.width;
            }
            if (posY + containerRect.height + 20 >= rootRect.yMax)
            {
                posY -= containerRect.height + 30;
            }
            container.style.left = new StyleLength(posX);
            container.style.top = new StyleLength(posY);

            this.RemoveFromClassList("hidden");
            onOpened.OnNext(Unit.Default);
        }

        /// <summary>
        /// Hide
        /// </summary>
        public void Hide()
        {
            this.AddToClassList("hidden");
            onClosed.OnNext(Unit.Default);
        }

        /// <summary>
        /// アイテム追加
        /// </summary>
        private void AddItem()
        {
            list.Append<ItemDataListItem>(out var item);
            itemDataList.Add(item);
            item.OnRemoved.Subscribe(id =>
            {
                item.Hide();
                onRemoveItem.OnNext(id);
            }).AddTo(disposable);
            item.OnSelected.Subscribe(x =>
            {
                onSelectedItem.OnNext(x);
            }).AddTo(disposable);
        }

        /// <summary>
        /// 除去時
        /// </summary>
        protected override void OnRemoved()
        {
            base.OnRemoved();
            disposable.Dispose();
        }
    }
}
#endif
