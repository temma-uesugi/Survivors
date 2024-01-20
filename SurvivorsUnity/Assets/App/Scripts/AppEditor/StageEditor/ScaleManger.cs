#if UNITY_EDITOR
using System;
using App.AppEditor.StageEditor.UIElements;
using UniRx;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// 拡縮管理
    /// </summary>
    public class ScaleManger
    {
        private readonly Label scaldLabel;
        private readonly Button downBtn;
        private readonly Button upBtn;

        private readonly Subject<float> onScaleChanged = new();
        public IObservable<float> OnScaleChanged => onScaleChanged;
        private decimal currentScale = 1.0m;
        private const decimal ScaleChangeValue = 0.2m;
        private const decimal MiniScale = 0.2m;
        private const decimal MaxScale = 1.0m;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ScaleManger(StageEditorWindow editorWindow)
        {
            scaldLabel = editorWindow.Body.Q<Label>("currentScale");
            downBtn = editorWindow.Body.Q<Button>("scaleDownBtn");
            upBtn = editorWindow.Body.Q<Button>("scaleUpBtn");

            scaldLabel.text = $"×{currentScale:F1}";
            upBtn.SetEnabled(false);
            upBtn.clickable.clicked += () => ChangeScale(ScaleChangeValue);
            downBtn.clickable.clicked += () => ChangeScale(-ScaleChangeValue);
        }

        /// <summary>
        /// スケール変更
        /// </summary>
        private void ChangeScale(decimal value)
        {
            var prevScale = currentScale;
            currentScale = Math.Clamp(currentScale + value, MiniScale, MaxScale);
            if (prevScale == currentScale)
            {
                return;
            }
            scaldLabel.text = $"×{currentScale:F1}";

            downBtn.SetEnabled(true);
            upBtn.SetEnabled(true);
            if (currentScale == MiniScale)
            {
                downBtn.SetEnabled(false);
            }
            if (currentScale == MaxScale)
            {
                upBtn.SetEnabled(false);
            }
            onScaleChanged.OnNext((float)currentScale);
        }
    }
}
#endif
