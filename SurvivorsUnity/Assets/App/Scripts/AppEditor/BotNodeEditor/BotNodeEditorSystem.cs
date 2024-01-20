#if UNITY_EDITOR
using UniRx;
using System;
using System.Collections.Generic;
using App.AppCommon.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Unit = UniRx.Unit;

namespace App.AppEditor.BotNodeEditor
{
    /// <summary>
    /// BotNodeEditorのSystem
    /// </summary>
    public class BotNodeEditorSystem
    {
        private static BotNodeEditorSystem _instance;
       
        private static CompositeDisposable _disposable;
        private static IDisposable _viewClickDispose;
        private static IDisposable _viewCloseDispose;
      
        //ussのカラーに準拠
        private static readonly Color RootBoarderColor = new Color(1, 1, 1, 0.2f);
        private static readonly Color RootColor = new Color(105 / 255f, 105 / 255f, 105 / 255f);
        private static readonly Color ActionColor = new Color(255 / 255f, 182 / 255f, 193 / 255f);

        //NodeTreeView
        private static NodeEditorView _view = null;
        
        private static RootNodeComponent _currentNodeRoot = null;
        //NodeRootからのNodeのInstanceIdの合計
        //NodeRootからのNodeに増減がないかを簡易的にチェック
        private static int _currentNodesIdSum = 0;
       
        //選択ノード変更
        private static readonly Subject<int> _onNodeSelectionChanged = new Subject<int>();
        public static IObservable<int> OnNodeSelectionChanged => _onNodeSelectionChanged;
        //非表示
        private static readonly Subject<int> _onHiddenInHierarchy = new Subject<int>();
        public static IObservable<int> OnHiddenInHierarchy = _onHiddenInHierarchy;
        //表示
        private static readonly Subject<int> _onShownInHierarchy = new Subject<int>();
        public static IObservable<int> OnShownInHierarchy = _onShownInHierarchy;
        //説明文変更
        private static readonly Subject<(int, string)> _onDescriptionChanged = new Subject<(int, string)>();
        public static IObservable<(int id, string description)> OnDescriptionChanged => _onDescriptionChanged;
        //ID変更
        private static readonly Subject<string> _onIdChanged = new Subject<string>();
        public static IObservable<string> OnIdChanged => _onIdChanged;
        //RootNodeの説明文変更
        private static readonly Subject<string> _onRootDescriptionChanged = new Subject<string>();
        public static IObservable<string> OnRootDescriptionChanged => _onRootDescriptionChanged;
       
        //Updateで処理する間隔
        private const float CHECK_INTERVAL = 1;
        private static float updateInterval = 0;
        
        /// <summary>
        /// Init
        /// </summary>
        public static void Init()
        {
            if (_instance != null)
            {
                return;
            }
            _instance = new BotNodeEditorSystem();
        }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        private BotNodeEditorSystem()
        {
            _disposable = new CompositeDisposable();
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
            Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.update += OnUpdate;

            EditorSceneManager.sceneClosed += (scene) =>
            {
                if (scene == EditorSceneManager.GetActiveScene())
                {
                    Dispose();
                }
            };
        } 
      
        /// <summary>
        /// Viewを取得
        /// </summary>
        private static NodeEditorView GetView()
        {
            if (_view == null)
            {
                _view = EditorWindow.GetWindow<NodeEditorView>();
                _viewClickDispose = _view.OnClick.Subscribe(OnClickByView).AddTo(_disposable);
                _viewCloseDispose = _view.OnClosed.Subscribe(OnViewClosed).AddTo(_disposable);
            }
            return _view;
        }
        
        /// <summary>
        /// Viewが閉じた
        /// </summary>
        private static void OnViewClosed(Unit unit)
        {
            _viewClickDispose.Dispose();
            _viewCloseDispose.Dispose();
            _currentNodeRoot = null;
        }

        /// <summary>
        /// 選択しているものが変更した
        /// </summary>
        private static void OnSelectionChanged()
        {
            if (Selection.activeGameObject == null)
            {
                _onNodeSelectionChanged.OnNext(0);
                if (_currentNodeRoot != null)
                {
                    //prevNodeRootがあれば再レンダー
                    RerenderView();
                }
                return;
            }

            if (!Selection.activeGameObject.TryGetComponent(out NodeComponentBase node))
            {
                return;
            }

            var nodeRoot = node.GetComponentInParent<RootNodeComponent>();
            if (nodeRoot == null)
            {
                return;
            }
            if (nodeRoot == node)
            {
                //NodeComponentRootなら再レンダリング
                RenderView(nodeRoot);
            }
            else if (nodeRoot != _currentNodeRoot)
            {
                //違うRoot
                RenderView(nodeRoot);
            }
            else
            {
                //同じRoot
                var curNodesIdSum = nodeRoot.GetNodesIdSum();
                if (curNodesIdSum != _currentNodesIdSum)
                {
                    //nodeに増減があったので、再レンダリング
                    RenderView(nodeRoot);
                }
            }
            _onNodeSelectionChanged.OnNext(node.GetInstanceID());
        }

        /// <summary>
        /// ViewをRender
        /// </summary>
        private static void RenderView(RootNodeComponent nodeRoot)
        {
            var nodeId = nodeRoot.Id;
            // if (!_hiddenObjectsMap.TryGetValue(nodeId, out _currentHiddenObjects))
            // {
            //     var hiddenObjects = new HashSet<int>();
            //     _hiddenObjectsMap.Add(nodeId, hiddenObjects);
            //     _currentHiddenObjects = hiddenObjects;
            // }
            _currentNodeRoot = nodeRoot;
            _currentNodesIdSum = nodeRoot.GetNodesIdSum();
            GetView().RenderNodeTrees(nodeRoot);
            // _lastObjDisplayUpdateCnt.Clear();
            // _updateCnt = 0;
        }

        /// <summary>
        /// 再レンダー
        /// </summary>
        public static void RerenderView()
        {
            RenderView(_currentNodeRoot);
        }

        /// <summary>
        /// HierarchyのOnGUI
        /// </summary>
        private static void OnHierarchyGUI(int instanceId, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (obj == null)
            {
                return;
            }

            // var rect = selectionRect;
            // rect.x = rect.xMax - ICON_WIDTH;
            // rect.width = ICON_WIDTH;
            // rect.y += (rect.height - ICON_HEIGHT) / 2f;
            // rect.height = ICON_HEIGHT;
            // if (obj.GetComponent<NodeComponentRoot>())
            // {
            //     EditorGUI.DrawRect(rect, RootColor);
            //     var borderRect = selectionRect;
            //     borderRect.height = 1;
            //     borderRect.y = borderRect.yMax - 1;
            //     EditorGUI.DrawRect(borderRect, RootBoarderColor);
            // }
            // else if (obj.GetComponent<NodeComponentSelector>())
            // {
            //     EditorGUI.DrawRect(rect, SelectorColor);
            // }
            // else if (obj.GetComponent<NodeComponentAction>())
            // {
            //     EditorGUI.DrawRect(rect, ActionColor);
            // }
            //
            // _lastObjDisplayUpdateCnt[instanceId] = _updateCnt;
        }

        /// <summary>
        /// Focusさせる
        /// </summary>
        public static void Focus(int instanceId)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (obj != null)
            {
                Selection.activeObject = obj;
            }
        }

        /// <summary>
        /// OnUpdate
        /// </summary>
        private static void OnUpdate()
        {
            updateInterval -= Time.deltaTime;
            if (updateInterval > 0)
            {
                return;
            }
            updateInterval = CHECK_INTERVAL;

            if (_currentNodeRoot != null)
            {
                // foreach (var node in _currentNodeRoot.GetAllNodes())
                // {
                //    var objId = node.gameObject.GetInstanceID();
                //    var id = node.GetInstanceID();
                //    if (!_lastObjDisplayUpdateCnt.TryGetValue(objId, out var lastUpdateCnt) || lastUpdateCnt < _updateCnt)
                //    {
                //        if (!_currentHiddenObjects.Contains(id))
                //        {
                //            _onHiddenInHierarchy.OnNext(id);
                //            _currentHiddenObjects.Add(id);
                //            //TODO 非表示
                //            // SharedLog.Debug($"{node.name}は非表示");
                //        }
                //    }
                //    else
                //    {
                //        var removedCnt = _currentHiddenObjects.RemoveWhere(x => x == id);
                //        if (removedCnt > 0)
                //        {
                //            _onShownInHierarchy.OnNext(id);
                //            // SharedLog.Debug($"{node.name}は表示");
                //        }
                //    }
                // }

                //子ノードに説明に変更ないかチェック
                foreach (var edited in _currentNodeRoot.GetNodeDescriptionChanged())
                {
                    _onDescriptionChanged.OnNext(edited);
                }
                //IDに変更ないか
                if (_currentNodeRoot.IsIdChanged(out var newId))
                {
                    _onIdChanged.OnNext(newId);
                }
                //RootNodeの説明に変更ないか
                if (_currentNodeRoot.IsDescriptionChanged(out var newDesc))
                {
                    _onRootDescriptionChanged.OnNext(newDesc);
                }
            }
            // _updateCnt++;
            EditorApplication.DirtyHierarchyWindowSorting();
        }

        /// <summary>
        /// View側でClick
        /// </summary>
        private static void OnClickByView(int instanceId)
        {
            var node = EditorUtility.InstanceIDToObject(instanceId) as NodeComponentBase;
            if (node != null)
            {
                Selection.activeObject = node;
            }
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        private void Dispose()
        {
            _instance = null;
            // _disposable?.Dispose();
            // _disposable = null;
            // _view = null;
            // EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;
            // Selection.selectionChanged -= OnSelectionChanged;
            // EditorApplication.update -= OnUpdate;
        }
    }
}
#endif