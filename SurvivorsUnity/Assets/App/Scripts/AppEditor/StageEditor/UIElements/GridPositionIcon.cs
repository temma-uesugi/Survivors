#if UNITY_EDITOR
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// グリッド位置管理
    /// </summary>
    public class GridPositionIcon : StageEditorVisualElement
    {
        private readonly CompositeDisposable disposable = new();
        private readonly TextElement text;

        private Vector2 mousePos;
        public int CurrentX { get; private set; }
        public int CurrentY { get; private set; }
        private bool isLocked = false;
        private readonly Label posValueLabel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GridPositionIcon(StageEditorWindow editorWindow) : base(editorWindow)
        {
            posValueLabel = editorWindow.Body.Q<Label>("gridPositionValue");
            this.style.width = new StyleLength(StageEditorSystem.Instance.GridSize);
            this.style.height = new StyleLength(StageEditorSystem.Instance.GridSize);
            editorWindow.OnMouseMove.Subscribe(OnMove).AddTo(disposable);
            editorWindow.OnScroll.Subscribe(_ =>
            {
                OnMove(mousePos);
            }).AddTo(disposable);

            editorWindow.OnKeyDown //
                .Where(x => x is KeyCode.UpArrow or KeyCode.DownArrow or KeyCode.LeftArrow or KeyCode.RightArrow) //
                .Subscribe(MoveAxis);
        }

        /// <summary>
        /// 移動
        /// </summary>
        private void OnMove(Vector2 pos)
        {
            if (isLocked)
            {
                return;
            }
            mousePos = pos;
            var grid = EditorWindow.CalcGripPosition(pos);
            MoveToGrid(grid.x, grid.y);
        }

        /// <summary>
        /// 矢印移動
        /// </summary>
        private void MoveAxis(KeyCode code)
        {
            if (isLocked)
            {
                return;
            }
            var x = CurrentX;
            var y = CurrentY;
            if (code == KeyCode.RightArrow)
            {
                x = Math.Clamp(x + 1, 1, (int)StageEditorSystem.Instance.StageSetting.Width);
            }
            else if (code == KeyCode.LeftArrow)
            {
                x = Math.Clamp(x - 1, 1, (int)StageEditorSystem.Instance.StageSetting.Width);
            }
            else if (code == KeyCode.DownArrow)
            {
                y = Math.Clamp(y + 1, 1, (int)StageEditorSystem.Instance.StageSetting.Height);
            }
            else if (code == KeyCode.UpArrow)
            {
                y = Math.Clamp(y - 1, 1, (int)StageEditorSystem.Instance.StageSetting.Height);
            }
            MoveToGrid(x, y);
        }

        /// <summary>
        /// グリッド移動
        /// </summary>
        public void MoveToGrid(int x, int y)
        {
            var gridPos = EditorWindow.GridToLocalPosition(x, y);
            this.style.left = new StyleLength(gridPos.x);
            this.style.top = new StyleLength(gridPos.y);
            CurrentX = x;
            CurrentY = y;
            posValueLabel.text = $"x:{x} y:{y}";
        }

        /// <summary>
        /// ロック
        /// </summary>
        public void Lock()
        {
            isLocked = true;
        }

        /// <summary>
        /// ロック解除
        /// </summary>
        public void Unlock()
        {
            isLocked = false;
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
