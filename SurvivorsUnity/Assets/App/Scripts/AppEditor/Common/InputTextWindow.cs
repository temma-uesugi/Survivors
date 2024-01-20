#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace App.AppEditor.Common
{
    /// <summary>
    /// テキスト入力するPopupWindowContent
    /// </summary>
    public class InputTextWindow : PopupWindowContent
    {
        private const float WINDOW_PADDING = 8.0f;
        private static readonly string FieldName = "InputTextWindow.text";

        private readonly string label;
        private string placeHolder;
        private string inputText;
        //Utilityの中なのでActionで
        private readonly Action<string> onInput;
        private readonly GUIStyle messageLabelStyle;
        private readonly Vector2 windowSize;
        private bool isFocus = false;

        /// <summary>
        /// 表示
        /// </summary>
        public static void Show(Vector2 position, string label, float width, Action<string> onInput)
        {
            var rect = new Rect(position, Vector2.zero);
            var content = new InputTextWindow(label, width, onInput);
            PopupWindow.Show(rect, content);
        }

        /// <summary>
        /// テキストwindowを表示
        /// </summary>
        private InputTextWindow(string label, float width, Action<string> onInput)
        {
            this.label = label;
            this.onInput = onInput;
            inputText = "";

            messageLabelStyle = new GUIStyle(EditorStyles.boldLabel);
            messageLabelStyle.wordWrap = true;

            // ウィンドウサイズを計算する
            windowSize = Vector2.zero;
            windowSize.x = width;
            windowSize.y += WINDOW_PADDING; // Space
            windowSize.y += EditorGUIUtility.standardVerticalSpacing; // Space
            windowSize.y += EditorGUIUtility.singleLineHeight; // TextField
            windowSize.y += WINDOW_PADDING; // Space
        }

        /// <summary>
        /// OnGUI
        /// </summary>
        public override void OnGUI(Rect rect)
        {
            // Enterで閉じる
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                editorWindow.Close();
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.Space(WINDOW_PADDING);
                using (new EditorGUILayout.VerticalScope())
                {
                    // タイトルを描画
                    EditorGUILayout.LabelField(label, messageLabelStyle);
                    // TextFieldを描画
                    using (new EditorGUI.ChangeCheckScope())
                    {
                        GUI.SetNextControlName(FieldName);
                        inputText = EditorGUILayout.TextField(inputText);
                    }
                }
                GUILayout.Space(WINDOW_PADDING);
            }
            // 最初の一回だけ自動的にフォーカスする
            if (!isFocus)
            {
                GUI.FocusControl(FieldName);
                isFocus = true;
            }
        }

        /// <summary>
        /// Close
        /// </summary>
        public override void OnClose()
        {
            onInput?.Invoke(EditorGUILayout.TextField(inputText));
            base.OnClose();
        }
    }
}
#endif
