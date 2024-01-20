#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using App.AppEditor.Common;
using UniRx;
using UnityEditor;
using UnityEngine.UIElements;
using VisualElement = UnityEngine.UIElements.VisualElement;

namespace App.AppEditor.BotNodeEditor
{
    /// <summary>
    /// NodeEditorのView
    /// </summary>
    public class NodeEditorView : EditorWindow
    {
        private static readonly string SourceDir = BotNodeEditorHelper.EditorDir + "/UIElements";

        private CompositeDisposable _disposable;

        private readonly Subject<int> _onClick = new Subject<int>();
        public IObservable<int> OnClick => _onClick;

        private readonly Subject<Unit> _onClosed = new Subject<Unit>();
        public IObservable<Unit> OnClosed => _onClosed;

        private readonly Subject<int> _onClear = new Subject<int>();
        public IObservable<int> OnClear => _onClear;

        private VisualElement _body;
        private ScrollView _scrollView;

        private VisualElement[][] _tableCellMap;

        /// <summary>
        /// OnEnable
        /// </summary>
        private void OnEnable()
        {
            _disposable = new CompositeDisposable();
            BotNodeEditorSystem.OnNodeSelectionChanged.Subscribe(OnNodeSelectedByHierarchy).AddTo(_disposable);
            BotNodeEditorSystem.OnHiddenInHierarchy.Subscribe(OnHiddenInHierarchy).AddTo(_disposable);
            BotNodeEditorSystem.OnShownInHierarchy.Subscribe(OnShownInHierarchy).AddTo(_disposable);
            BotNodeEditorSystem.OnDescriptionChanged.Subscribe(x => OnDescriptionChanged(x.id, x.description)).AddTo(_disposable);
            BotNodeEditorSystem.OnIdChanged.Subscribe(OnIdChanged).AddTo(_disposable);
            BotNodeEditorSystem.OnRootDescriptionChanged.Subscribe(OnRootDescriptionChanged).AddTo(_disposable);
        }

        /// <summary>
        /// NodeTreeを描画
        /// </summary>
        public void RenderNodeTrees(RootNodeComponent nodeRoot)
        {
            _body = _body ??= rootVisualElement;
            _body.Clear();
            var uss = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{SourceDir}/NodeEditorView.uss");
            _body.styleSheets.Add(uss);

            _body.Append<ScrollView>("", out _scrollView);
            var nodeTree = CreateNodeTree(nodeRoot);
            _scrollView.Add(nodeTree);

            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{SourceDir}/NodeEditorView.uxml");
            visualTree.CloneTree(_body);
            OnGUI();
        }


        /// <summary>
        /// NodeTreeを描画
        /// </summary>
        private VisualElement CreateNodeTree(RootNodeComponent root)
        {
            var block = new Box();
            block.AddToClassesList("block");
            block.Append<Box>("header", out var header);
            header
                .AppendWithText<Box>(root.Id, "title")
                .AppendWithText<Box>(root.Description, "description")
                .AppendWithText<Box>("", "separate");

            //NodeTreeのTableレイアウトを組んでいく

            var nodes = root.GetComponentsInChildren<NodeComponentBase>();
            int rowNum = 0;
            int colNum = 0;
            foreach (var node in nodes)
            {
                rowNum = Math.Max(rowNum, node.GetRowIndex());
                colNum = Math.Max(colNum, node.GetDepth());
            }

            //Tableを作って、セルのマップを受け取る
            _tableCellMap = block.AppendTable(rowNum + 1, colNum + 1);
            //コネクタ入れるセルを挿入
            var connectorMap = new VisualElement[_tableCellMap.Length][];
            for (int r = 0; r < _tableCellMap.Length; r++)
            {
                connectorMap[r] = new VisualElement[_tableCellMap[r].Length - 1];
                for (int c = 0; c < _tableCellMap[r].Length - 1; c++)
                {
                    var col = _tableCellMap[r][c];
                    col.After<Box>("col connector", out var connector);
                    connectorMap[r][c] = connector;
                }
            }

            //該当するセルに要素を入れる
            int nodeNo = 1;
            for (int r = 0; r < nodes.Length; r++)
            {
                var id = nodes[r].GetInstanceID();
                var row = r;
                var col = 2;
            }
            foreach (var node in nodes)
            {
                var id = node.GetInstanceID();
                var row = node.GetRowIndex();
                var col = node.GetDepth();
                var className = node switch
                {
                    var t when t is RootNodeComponent => "Root",
                    var t when t is ActionNodeComponent => "Action",
                    _ => ""
                };
                if (node is RootNodeComponent nodeRoot)
                {
                    _tableCellMap[row][col]?.AppendWithText<Button>("Root", className, ("id", id.ToString()));
                    node.name = nodeRoot.Id;
                }
                else
                {
                    var desc = node.Description;
                    _tableCellMap[row][col]?.AppendWithText<Button>(desc, className, ("id", id.ToString()));
                    node.name = $"[{nodeNo}] {className}";
                }
                nodeNo++;
                _tableCellMap[row][col].AddToClassList("node");

                //コネクターを作る
                DrawConnector(node);
            }

            block.Query<Button>().ForEach(x =>
            {
                x.clickable.clicked += () =>
                {
                    if (int.TryParse(x.Attr("id"), out var id))
                    {
                        _onClick.OnNext(id);
                    }
                };
            });

            return block;

            //コネクトを書く
            void DrawConnector(NodeComponentBase node)
            {
                //子要素を取得
                var children = node.GetNodeChildren().ToArray();
                if (children.Length == 0)
                {
                    return;
                }

                int row = node.GetRowIndex();
                int col = node.GetDepth();
                //子要素が1つ
                if (children.Length == 1)
                {
                    connectorMap[row]?[col]?.Add(new Label("─"));
                    return;
                }

                //子要素が1以上

                //最初の子要素への接続
                if (children[0].IsFirstChild())
                {
                    connectorMap[row]?[col]?.Add(new Label("┬"));
                }
                else
                {
                    connectorMap[row]?[col]?.Add(new Label("┌"));
                }

                //間の子要素への接続
                int rPoint = row + 1;
                //最初と最後を抜いて取得
                foreach (var child in children.Skip(1).Take(children.Length - 2))
                {
                    var nextRow = child.GetRowIndex();
                    connectorMap[nextRow]?[col]?.Add(new Label("├"));
                    while (rPoint < nextRow)
                    {
                        connectorMap[rPoint]?[col]?.Add(new Label("│"));
                        rPoint++;
                    }
                    rPoint++;
                }

                var lastRow = children.Last().GetRowIndex();
                connectorMap[lastRow]?[col]?.Add(new Label("└"));
                while (rPoint < lastRow)
                {
                    connectorMap[rPoint]?[col]?.Add(new Label("│"));
                    rPoint++;
                }
            }
            
        }

        /// <summary>
        /// Hierarchy上で選択
        /// </summary>
        private void OnNodeSelectedByHierarchy(int instanceId)
        {
            _body?.Q<Button>(className: "focused")?.RemoveFromClassList("focused");
            var buttons = _body?.Query<Button>();
            if (!buttons.HasValue)
            {
                return;
            }
            foreach (var button in buttons.Value.ToList())
            {
                if (button.Attr("id") == instanceId.ToString())
                {
                    button.AddToClassList("focused");
                    //Note: スクロールの各値がどうなっているのかいまいちわからず
                    // scrollView.verticalScroller.value = button.worldBound.yMax;
                    break;
                }
            }
        }

        /// <summary>
        /// ヒエラルキ上で非表示
        /// </summary>
        private void OnHiddenInHierarchy(int instanceId)
        {
            var buttons = _body?.Query<Button>();
            if (!buttons.HasValue)
            {
                return;
            }
            foreach (var button in buttons.Value.ToList())
            {
                if (button.Attr("id") == instanceId.ToString())
                {
                    button.parent.AddToClassList("hidden");
                    break;
                }
            }
        }

        /// <summary>
        /// ヒエラルキ上で表示
        /// </summary>
        private void OnShownInHierarchy(int instanceId)
        {
            var buttons = _body?.Query<Button>();
            if (!buttons.HasValue)
            {
                return;
            }
            foreach (var button in buttons.Value.ToList())
            {
                if (button.Attr("id") == instanceId.ToString())
                {
                    button.parent.RemoveFromClassList("hidden");
                    break;
                }
            }
        }

        /// <summary>
        /// 説明変更
        /// </summary>
        private void OnDescriptionChanged(int id, string description)
        {
            var buttons = _body?.Query<Button>();
            if (!buttons.HasValue)
            {
                return;
            }
            foreach (var button in buttons.Value.ToList())
            {
                if (button.Attr("id") == id.ToString())
                {
                    var textElem = button.Q<TextElement>();
                    if (textElem != null)
                    {
                        textElem.text = description;
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// IDの変更
        /// </summary>
        private void OnIdChanged(string id)
        {
            var titleText = _body?.Q<Box>(className: "title")?.Q<TextElement>();
            if (titleText == null)
            {
                return;
            }
            titleText.text = id;
        }

        /// <summary>
        /// RootDescriptionの変更
        /// </summary>
        private void OnRootDescriptionChanged(string description)
        {
            var descText = _body?.Q<Box>(className: "description")?.Q<TextElement>();
            if (descText == null)
            {
                return;
            }
            descText.text = description;
        }

        /// <summary>
        /// OnGUI
        /// </summary>
        private void OnGUI()
        {
            var rows = _body?.Query<Box>(className: "row");
            if (!rows.HasValue)
            {
                return;
            }
            foreach (var row in rows.Value.ToList())
            {
                if (row.Query<Box>(classes: "node").ToList().All(x => x.ClassListContains("hidden")))
                {
                    row.AddToClassList("hidden");
                }
                else
                {
                    row.RemoveFromClassList("hidden");
                }
            }
        }

        /// <summary>
        /// 閉じた
        /// </summary>
        private void OnDestroy()
        {
            _disposable.Dispose();
            _disposable = null;
            _onClosed.OnNext(Unit.Default);
        }
    }
}
#endif