#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// ステージエディターのView
    /// </summary>
    public class StageEditorSetting : MonoBehaviour
    {
        [field: SerializeField, Tooltip("クエストID")]
        public uint QuestId { get; private set; } = 0;

        [field: SerializeField, Tooltip("ステージNo")]
        public uint StageNo { get; private set; } = 0;

        public uint QuestStageId { get; set; }

        [Tooltip("フィールドID")]
        public uint FieldId = 0;

        [Tooltip("制限時間秒")]
        public uint TimeLimitSec = 0;

        [Tooltip("必要ポイント")]
        public uint NeedPoint = 0;

        [Header("===== 編集領域設定 =====")]
        [Tooltip("横")]
        public uint Width;

        [Tooltip("高さ")]
        public uint Height;

        [field: SerializeField, Tooltip("グリッドサイズ")]
        public uint GridSize { get; private set; } = 10;
    }

    /// <summary>
    /// Editor拡張
    /// </summary>
    [CustomEditor(typeof(StageEditorSetting))]
    public class StageEditorSettingEditor : Editor
    {
        private StageEditorSetting self => (StageEditorSetting) target;

        /// <summary>
        /// 更新処理
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(4);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(4);

            if (GUILayout.Button("新規作成開始"))
            {
                if (self.Width == 0 || self.Height == 0 || self.FieldId == 0 || self.QuestId == 0 || self.StageNo == 0 || self.TimeLimitSec == 0 || self.GridSize == 0)
                {
                    EditorUtility.DisplayDialog("", "必要な数値が0です", "OK");
                    return;
                }
                self.QuestStageId = self.QuestId * 10 + self.StageNo;
                StageEditorSystem.Instance.Reset();
                StageEditorSystem.Instance.CreateNewStage(self, self.GridSize);
            }

            GUILayout.Space(4);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(4);

            if (GUILayout.Button("編集"))
            {
                self.QuestStageId = self.QuestId * 10 + self.StageNo;
                var data = DataFileManager.GetData(self.QuestId, self.StageNo);
                if (data == null)
                {
                    EditorUtility.DisplayDialog("", "データがありませんでした", "OK");
                    return;
                }
                self.FieldId = data.FieldId;
                self.TimeLimitSec = data.TimeLimitSec;
                self.NeedPoint = data.NeedPoint;
                self.Width = data.Width;
                self.Height = data.Height;
                if (self.Width == 0 || self.Height == 0 || self.FieldId == 0 || self.QuestId == 0 || self.StageNo == 0 || self.TimeLimitSec == 0 || self.GridSize == 0)
                {
                    EditorUtility.DisplayDialog("", "必要な数値が0です", "OK");
                    return;
                }
                StageEditorSystem.Instance.Reset();
                StageEditorSystem.Instance.EditStage(self, self.GridSize, data.MotherBasePositionX, data.MotherBasePositionY, data.Objects, data.Enemies);
            }

            GUILayout.Space(4);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(4);

            if (GUILayout.Button("保存 / SQL吐き出し"))
            {
                StageEditorSystem.Instance.SaveAndExportSql();
            }

            GUILayout.Space(4);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(4);

            if (GUILayout.Button("Presetデータでの更新SQL吐き出し"))
            {
                StageEditorSystem.Instance.ExportPresetSql();
            }
        }
    }
}
#endif
