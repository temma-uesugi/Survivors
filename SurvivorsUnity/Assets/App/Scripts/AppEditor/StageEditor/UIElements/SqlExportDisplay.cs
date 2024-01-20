#if UNITY_EDITOR
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// SQL文表示
    /// </summary>
    public class SqlExportDisplay : StageEditorVisualElement
    {
        private readonly TextField textField;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SqlExportDisplay(StageEditorWindow editorWindow) : base(editorWindow)
        {
            var visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{StageEditorWindow.SourceDir}/SqlExportDisplay.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{StageEditorWindow.SourceDir}/SqlExportDisplay.uss");
            styleSheets.Add(styleSheet);
            visualTreeAsset.CloneTree(this);

            textField = this.Q<TextField>();
            AddToClassList("hidden");

            var copyBtn = this.Q<Button>("copyBtn");
            var closeBtn = this.Q<Button>("sqlCloseBtn");
            copyBtn.clickable.clicked += Copy;
            closeBtn.clickable.clicked += Hide;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected override void Init()
        {
            var width = EditorWindow.Body.contentRect.width * 0.8f;
            var height = EditorWindow.Body.contentRect.height * 0.8f;
            style.width = new StyleLength(width);
            style.height = new StyleLength(height);
            style.marginLeft = new StyleLength(-width / 2);
            style.marginTop = new StyleLength(-height / 2);
        }

        /// <summary>
        /// Hide
        /// </summary>
        public void Hide()
        {
            AddToClassList("hidden");
            textField.value = "";
        }

        /// <summary>
        /// Copy
        /// </summary>
        public void Copy()
        {
            GUIUtility.systemCopyBuffer = textField.value;
            EditorUtility.DisplayDialog("", "コピーしました", "OK");
        }

        /// <summary>
        /// テキストのセット
        /// </summary>
        public void SetText(params StringBuilder[] builders)
        {
            foreach (var builder in builders)
            {
                textField.value += builder.ToString();
                textField.value += "\n";
            }
            RemoveFromClassList("hidden");
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
