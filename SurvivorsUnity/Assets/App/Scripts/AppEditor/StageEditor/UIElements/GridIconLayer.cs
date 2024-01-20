#if UNITY_EDITOR
using System.Collections.Generic;
using App.AppEditor.Common;
using App.AppEditor.StageEditor.Records;
using UnityEditor;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// グリッドアイコンLayer
    /// </summary>
    public class GridIconLayer : StageEditorVisualElement
    {
        private readonly Dictionary<int, (Box icon, SettingRecord record)> iconMap = new();
        private readonly Box initPosIcon;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GridIconLayer(StageEditorWindow editorWindow) : base(editorWindow)
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{StageEditorWindow.SourceDir}/GridIconLayer.uss");
            styleSheets.Add(styleSheet);
            style.height = EditorWindow.GridMap.style.height;
            this.Append<Box>(id: "initPosIcon", className: null, out initPosIcon);
            initPosIcon.style.height = new StyleLength(StageEditorSystem.Instance.GridSize);
            initPosIcon.style.width = new StyleLength(StageEditorSystem.Instance.GridSize);
            //円に
            initPosIcon.style.borderBottomRightRadius = new StyleLength(StageEditorSystem.Instance.GridSize / 2f);
            initPosIcon.style.borderBottomLeftRadius = new StyleLength(StageEditorSystem.Instance.GridSize / 2f);
            initPosIcon.style.borderTopLeftRadius = new StyleLength(StageEditorSystem.Instance.GridSize / 2f);
            initPosIcon.style.borderTopRightRadius = new StyleLength(StageEditorSystem.Instance.GridSize / 2f);
        }

        /// <summary>
        /// Add
        /// </summary>
        public void AddItem(int id, SettingRecord record, int x, int y)
        {
            this.Append<Box>(id: null, className: "icon", out var icon);
            icon.style.height = new StyleLength(StageEditorSystem.Instance.GridSize);
            icon.style.width = new StyleLength(StageEditorSystem.Instance.GridSize);
            icon.style.backgroundColor = new StyleColor(record.IconColor);
            var (left, top) = EditorWindow.GridToLocalPosition(x, y);
            icon.style.left = new StyleLength(left);
            icon.style.top = new StyleLength(top);
            iconMap.Add(id, (icon, record));
        }

        /// <summary>
        /// Remove
        /// </summary>
        public void RemoveItem(int id)
        {
            if (!iconMap.TryGetValue(id, out var val))
            {
                return;
            }
            iconMap.Remove(id);
            Remove(val.icon);
        }

        /// <summary>
        /// レコードのUpdate
        /// </summary>
        public void Update(SettingRecord record)
        {
            foreach (var (icon, rec) in iconMap.Values)
            {
                if (record == rec)
                {
                    icon.style.backgroundColor = new StyleColor(record.IconColor);
                }
            }
        }

        /// <summary>
        /// 初期配置を更新
        /// </summary>
        public void UpdateInitPosition(int x, int y)
        {
            var (left, top) = EditorWindow.GridToLocalPosition(x, y);
            initPosIcon.style.left = new StyleLength(left);
            initPosIcon.style.top = new StyleLength(top);
        }

        /// <summary>
        /// 設定Recordの変更
        /// </summary>
        public void ChangeRecord(int itemId, SettingRecord record)
        {
            if (!iconMap.TryGetValue(itemId, out var pair))
            {
                return;
            }
            pair.record = record;
            pair.icon.style.backgroundColor = new StyleColor(record.IconColor);
            iconMap[itemId] = pair;
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
