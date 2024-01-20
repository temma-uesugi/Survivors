#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using App.AppEditor.StageEditor.Records;
using UniRx;
using UnityEditor;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// アイテム選択
    /// </summary>
    public class ItemSelector : StageEditorVisualElement
    {
        public Define.ModeType Mode { get; private set; } = Define.ModeType.Select;
        public SettingRecord CurrentRecord { get; private set; }

        private readonly Button selectModeBtn;
        private readonly Button removeModeBtn;
        private readonly Button initPosModeBtn;
        private readonly Box objectListBox;
        private readonly Box enemyListBox;
        private readonly HashSet<Button> listItems = new();
        private readonly Dictionary<int, Button> presetBtnMap = new();

        private readonly Subject<Define.ModeType> onModeChanged = new();
        public IObservable<Define.ModeType> OnModeChanged => onModeChanged;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ItemSelector(StageEditorWindow editorWindow) : base(editorWindow)
        {
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{StageEditorWindow.SourceDir}/ItemSelector.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{StageEditorWindow.SourceDir}/ItemSelector.uss");
            styleSheets.Add(styleSheet);
            visualTreeAsset.CloneTree(this);

            selectModeBtn = this.Q<Button>("selectMode");
            removeModeBtn = this.Q<Button>("removeMode");
            initPosModeBtn = this.Q<Button>("initPosMode");
            objectListBox = this.Q<Box>("objectItemList");
            enemyListBox = this.Q<Box>("enemyItemList");

            selectModeBtn.clickable.clicked += SetSelectMode;
            removeModeBtn.clickable.clicked += SetRemoveMode;
            initPosModeBtn.clickable.clicked += SetInitPosMode;
        }

        /// <summary>
        /// アイテムをリストに追加
        /// </summary>
        public void AddItem(SettingRecord record)
        {
            Button btn = new Button();
            if (record is ObjectRecord objectRecord)
            {
                objectListBox.Add(btn);
            }
            else if (record is EnemyRecord enemyRecord)
            {
                enemyListBox.Add(btn);
            }
            btn.AddToClassList("listItem");
            btn.text = record.ItemLabel;
            if (!string.IsNullOrEmpty(record.ItemDesc))
            {
                btn.tooltip = record.ItemDesc;
            }
            listItems.Add(btn);
            if (record.IsPreset)
            {
                presetBtnMap.Add(record.PresetId, btn);
                btn.AddToClassList("preset");
            }

            btn.clickable.clicked += () =>
            {
                OnListItemClick(record);
                btn.AddToClassList("selected");
            };
        }

        /// <summary>
        /// アイテム除去
        /// </summary>
        public void RemoveItem(SettingRecord record)
        {
            if (!record.IsPreset || !presetBtnMap.TryGetValue(record.PresetId, out var btn))
            {
                return;
            }

            if (record is ObjectRecord)
            {
                objectListBox.Remove(btn);
            }
            else if (record is EnemyRecord)
            {
                enemyListBox.Remove(btn);
            }
            listItems.Remove(btn);
            presetBtnMap.Remove(record.PresetId);
            CurrentRecord = null;
            Mode = Define.ModeType.Select;
        }

        /// <summary>
        /// 選択アイテムの更新
        /// </summary>
        public void UpdateItem(SettingRecord record)
        {
            if (!record.IsPreset || !presetBtnMap.TryGetValue(record.PresetId, out var btn))
            {
                return;
            }
            btn.text = record.ItemLabel;
            btn.tooltip = record.ItemDesc;
        }

        /// <summary>
        /// リストアイテムのクリック
        /// </summary>
        private void OnListItemClick(SettingRecord record)
        {
            Mode = Define.ModeType.Add;
            CurrentRecord = record;
            StageEditorSystem.Instance.Inspector.SetAddRecord(record);

            selectModeBtn.RemoveFromClassList("selected");
            removeModeBtn.RemoveFromClassList("selected");
            initPosModeBtn.RemoveFromClassList("selected");
            foreach (var item in listItems)
            {
                item.RemoveFromClassList("selected");
            }
            onModeChanged.OnNext(Define.ModeType.Add);

        }

        /// <summary>
        /// 選択モード
        /// </summary>
        public void SetSelectMode()
        {
            Mode = Define.ModeType.Select;
            foreach (var item in listItems)
            {
                item.RemoveFromClassList("selected");
            }
            selectModeBtn.AddToClassList("selected");
            removeModeBtn.RemoveFromClassList("selected");
            initPosModeBtn.RemoveFromClassList("selected");
            StageEditorSystem.Instance.Inspector.Unset();
            onModeChanged.OnNext(Define.ModeType.Select);
        }

        /// <summary>
        /// 削除モード
        /// </summary>
        private void SetRemoveMode()
        {
            Mode = Define.ModeType.Remove;
            foreach (var item in listItems)
            {
                item.RemoveFromClassList("selected");
            }
            selectModeBtn.RemoveFromClassList("selected");
            removeModeBtn.AddToClassList("selected");
            initPosModeBtn.RemoveFromClassList("selected");
            StageEditorSystem.Instance.Inspector.Unset();
            onModeChanged.OnNext(Define.ModeType.Remove);
        }

        /// <summary>
        /// 初期位置配置モード
        /// </summary>
        private void SetInitPosMode()
        {
            Mode = Define.ModeType.InitPos;
            foreach (var item in listItems)
            {
                item.RemoveFromClassList("selected");
            }
            selectModeBtn.RemoveFromClassList("selected");
            removeModeBtn.RemoveFromClassList("selected");
            initPosModeBtn.AddToClassList("selected");
            onModeChanged.OnNext(Define.ModeType.InitPos);
        }

        /// <summary>
        /// 選択状態に
        /// </summary>
        public void SetSelectItem(SettingRecord record)
        {
            if (!record.IsPreset || !presetBtnMap.TryGetValue(record.PresetId, out var btn))
            {
                return;
            }
            //TODO: スクロールしていたらスクロール位置合わせる必要あり
            OnListItemClick(record);
            btn.AddToClassList("selected");
        }

        /// <summary>
        /// 除去時
        /// </summary>
        protected override void OnRemoved()
        {
            base.OnRemoved();
        }
    }
}
#endif
