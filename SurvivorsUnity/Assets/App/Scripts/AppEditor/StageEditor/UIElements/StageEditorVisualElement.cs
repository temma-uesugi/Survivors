#if UNITY_EDITOR
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// ステージエディター用のVisualElement(StageEditorを初期化時にもらう)
    /// </summary>
    public abstract class StageEditorVisualElement : VisualElement
    {
        protected StageEditorWindow EditorWindow { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected StageEditorVisualElement(StageEditorWindow editorWindow)
        {
            EditorWindow = editorWindow;
            //クラス名の最初を小文字にしてnameに (小文字にしなくてもいいかも)
            var className = this.GetType().Name;
            string firstChar = className.Substring(0, 1);
            string restChars = className.Substring(1);
            name = firstChar.ToLower() + restChars;

            UniTask.Void(async () =>
            {
                await UniTask.WaitWhile(() => float.IsNaN(this.contentRect.width));
                Init();
            });

            RegisterCallback<DetachFromPanelEvent>(_ => OnRemoved());
        }

        /// <summary>
        /// 初期化
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// 除去時
        /// </summary>
        protected virtual void OnRemoved()
        {
        }
    }
}
#endif
