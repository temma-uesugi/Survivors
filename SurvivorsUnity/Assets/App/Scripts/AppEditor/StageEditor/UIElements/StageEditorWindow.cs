#if UNITY_EDITOR
using System;
using App.AppEditor.Common;
using App.AppEditor.StageEditor.Records;
using Cysharp.Threading.Tasks;
using FastEnumUtility;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.AppEditor.StageEditor.UIElements
{
    /// <summary>
    /// ステージEditor
    /// </summary>
    public class StageEditorWindow : EditorWindow
    {
        public static readonly string SourceDir = "Assets/App/Scripts/AppEditor/StageEditor/UIElements";

        private uint gridSize;
        public VisualElement Body { get; private set; }
        private Label modeLabel;
        public ScrollView GridMapScroller { get; private set; }
        public Box GridMap { get; private set; }
        public GridInfo GridInfo { get; private set; }
        public Box GridLayer { get; private set; }
        public GridIconLayer IconLayer { get; private set; }
        public ItemSelector Selector { get; private set; }
        public SqlExportDisplay SqlDisplay { get; private set; }
        public GridPositionIcon GridPosition { get; private set; }
        public MapItemList ItemList { get; private set; }

        private Vector2 gridContainerPos;

        private readonly Subject<Unit> onRenderCompleted = new();
        public IObservable<Unit> OnRenderCompleted => onRenderCompleted;
        private readonly Subject<Unit> onMouseOut = new();
        public IObservable<Unit> OnMouseOut => onMouseOut;
        private readonly Subject<Unit> onMouseEnter = new();
        public IObservable<Unit> OnMouseEnter => onMouseEnter;
        private readonly Subject<Vector2> onMouseMove = new();
        public IObservable<Vector2> OnMouseMove => onMouseMove;
        private readonly Subject<Vector2> onScroll = new();
        public IObservable<Vector2> OnScroll => onScroll;

        private readonly Subject<(int x, int y)> onGridClick = new();
        public IObservable<(int x, int y)> OnGridClick => onGridClick;

        private readonly Subject<KeyCode> onKeyDown = new();
        public IObservable<KeyCode> OnKeyDown => onKeyDown;

        /// <summary>
        /// OnGUI
        /// </summary>
        private void OnGUI()
        {
            var e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                onKeyDown.OnNext(e.keyCode);
            }
        }

        /// <summary>
        /// Viewを描画
        /// </summary>
        public void RenderView(uint width, uint height, uint gridSize, uint questId, uint stageNo, uint questStageId)
        {
            this.gridSize = gridSize;
            Body ??= rootVisualElement;
            Body.Clear();
            var uss = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{SourceDir}/StageEditorWindow.uss");
            Body.styleSheets.Add(uss);

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{SourceDir}/StageEditorWindow.uxml");
            visualTree.CloneTree(Body);

            modeLabel = Body.Q<Label>("currentMode");
            GridMapScroller = Body.Q<ScrollView>("gridMapScroller");
            GridMap = CreateGridMap(width, height);
            GridMapScroller.Add(GridMap);
            //GridInfo
            GridInfo = new GridInfo(this);
            Body.Add(GridInfo);
            //ItemSelector
            Selector = new ItemSelector(this);
            Body.Q<Box>("itemSelectorContainer").Add(Selector);
            //Sql表示
            SqlDisplay = new SqlExportDisplay(this);
            Body.Add(SqlDisplay);
            //アイテムリスト
            ItemList = new MapItemList(this);
            Body.Q<Box>("mapItemListContainer").Add(ItemList);

            Body.Q<Label>("stageTitle").text = $"{questStageId}({questId} - {stageNo})";

            UniTask.Void(async () =>
            {
                //renderが完了するまで待つ
                await UniTask.WaitWhile(() => float.IsNaN(GridMap.worldBound.width));

                gridContainerPos = GridMap.worldBound.position;

                //Note: 余裕があれば逆にする
                GridPosition = new GridPositionIcon(this);
                GridMap.Add(GridPosition);
                IconLayer = new GridIconLayer(this);
                GridMap.Add(IconLayer);

                onRenderCompleted.OnNext(Unit.Default);
            });
        }

        /// <summary>
        /// GridMapを作成
        /// </summary>
        private Box CreateGridMap(uint width, uint height)
        {
            var gridMap = new Box();
            gridMap.name = "gridMap";
            gridMap.style.width = new StyleLength(width * gridSize + 5);
            gridMap.style.height = new StyleLength(height * gridSize + 5);
            gridMap.Append<Box>("gridLayer", className: null, out var gridLayer);
            for (int x = 0; x <= width; x++)
            {
                gridLayer.Append<Box>(id: null, "gridBoard gridBoarderCol", out var boarder);
                boarder.style.left = new StyleLength(x * gridSize - 1);
            }
            for (int y = 0; y <= height; y++)
            {
                gridLayer.Append<Box>(id: null, "gridBoard gridBoarderRow", out var boarder);
                boarder.style.top = new StyleLength(y * gridSize - 1);
            }
            GridLayer = gridLayer;

            gridMap.RegisterCallback<MouseEnterEvent>(MouseEnter);
            gridMap.RegisterCallback<MouseOutEvent>(MouseOut);
            gridMap.RegisterCallback<MouseMoveEvent>(MouseMove);
            GridMapScroller.horizontalScroller.valueChanged += ScrollEvent;
            GridMapScroller.verticalScroller.valueChanged += ScrollEvent;
            gridMap.RegisterCallback<ClickEvent>(OnClick);

            return gridMap;
        }

        /// <summary>
        /// マウスイン
        /// </summary>
        private void MouseEnter(MouseEnterEvent e)
        {
            onMouseEnter.OnNext(Unit.Default);
        }

        /// <summary>
        /// マウスアウト
        /// </summary>
        private void MouseOut(MouseOutEvent e)
        {
            if (e.target is VisualElement vm && vm.ClassListContains("gridBoard"))
            {
                return;
            }
            onMouseOut.OnNext(Unit.Default);
        }

        /// <summary>
        /// マウス移動
        /// </summary>
        private void MouseMove(MouseMoveEvent e)
        {
            onMouseMove.OnNext(e.mousePosition);
        }

        /// <summary>
        /// スクロールイベント
        /// </summary>
        private void ScrollEvent(float _)
        {
            onScroll.OnNext(GridMapScroller.scrollOffset);
        }

        /// <summary>
        /// クリックイベント
        /// </summary>
        private void OnClick(ClickEvent e)
        {
            var grid = CalcGripPosition(e.position);
            onGridClick.OnNext(grid);
        }

        /// <summary>
        /// グリッドマップの位置計算
        /// </summary>
        public (float x, float y) CalcGripLayerPosition(Vector2 pos)
        {
            var x = pos.x - gridContainerPos.x + GridMapScroller.scrollOffset.x;
            var y = pos.y - gridContainerPos.y + GridMapScroller.scrollOffset.y;
            return (x, y);
        }

        /// <summary>
        /// グリッドマップの位置取得
        /// </summary>
        public (int x, int y) CalcGripPosition(Vector2 pos)
        {
            var (posX, posY) = CalcGripLayerPosition(pos);
            int x = (int)(posX / gridSize) + 1;
            int y = (int)(posY / gridSize) + 1;
            return (x, y);
        }

        /// <summary>
        /// グリッドをLocal位置に
        /// </summary>
        public (float x, float y) GridToLocalPosition(int x, int y)
        {
            return ((x - 1) * gridSize, (y - 1) * gridSize);
        }

        /// <summary>
        /// グリッドをWorld位置に
        /// </summary>
        public (float x, float y) GridToWorldPosition(int x, int y)
        {
            var localPos = GridToLocalPosition(x, y);
            return (localPos.x + gridContainerPos.x - GridMapScroller.scrollOffset.x, localPos.y + gridContainerPos.y - GridMapScroller.scrollOffset.y);
        }

        /// <summary>
        /// モード文字列のセット
        /// </summary>
        public void SetModeLabel(Define.ModeType mode, SettingRecord record)
        {
            var str = mode.GetLabel();
            if (mode == Define.ModeType.Add)
            {
                str += " : " + record.ItemLabel + (record.IsPreset ? "(プリセット)" : "");
            }
            modeLabel.text = str;
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            GridMap.UnregisterCallback<ClickEvent>(OnClick);
            GridMap.UnregisterCallback<MouseEnterEvent>(MouseEnter);
            GridMap.UnregisterCallback<MouseOutEvent>(MouseOut);
            GridMap.UnregisterCallback<MouseMoveEvent>(MouseMove);
            GridMapScroller.horizontalScroller.valueChanged -= ScrollEvent;
            GridMapScroller.verticalScroller.valueChanged -= ScrollEvent;
        }

        /// <summary>
        /// 閉じた
        /// </summary>
        private void OnDestroy()
        {
            Clear();
        }
    }
}
#endif
