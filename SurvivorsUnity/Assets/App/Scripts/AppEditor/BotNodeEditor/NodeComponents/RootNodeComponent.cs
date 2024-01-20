#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using App.AppEditor.Common;
using App.Battle.EnemyBots.NodeObjects;
using UnityEditor;
using UnityEngine;

namespace App.AppEditor.BotNodeEditor
{
    /// <summary>
    /// RootNode
    /// </summary>
    public class RootNodeComponent : NodeComponentBase
    {
        [SerializeField] private string id;
        public string Id => id;
    
        private string _prevId = "";
       
        public override int GetDepth() => 0;
        public override int GetRowIndex() => 0;
        public override bool IsFirstChild() => true;
        
        /// <summary>
        /// NodeRootからのNodeのInstanceIdの合計
        /// </summary>
        public int GetNodesIdSum()
        {
            return GetComponentsInChildren<NodeComponentBase>()
                .Select(x => x.GetInstanceID())
                .Sum();
        }
        
        /// <summary>
        /// 全ノードを取得
        /// </summary>
        public IEnumerable<NodeComponentBase> GetAllNodes() => GetComponentsInChildren<NodeComponentBase>().Where(x => x != this);
        
        /// <summary>
        /// Idに変更があるか
        /// </summary>
        public bool IsIdChanged(out string newId)
        {
            newId = id;
            if (_prevId == id)
            {
               //変更なし
                return false;
            }
            //変更あり
            _prevId = string.Copy(id);
            return true;
        }
        
        /// <summary>
        /// Nodeの説明に変更があったかチェック
        /// </summary>
        public IEnumerable<(int, string)> GetNodeDescriptionChanged()
        {
            foreach (var node in GetAllNodes())
            {
                if (node.IsDescriptionChanged(out var desc))
                {
                    yield return (node.GetInstanceID(), desc);
                }
            }
        }

        /// <summary>
        /// BotNodeRootに変換
        /// </summary>
        public BotNodeObject.BotNodeRoot ConvertBotNodeRoot()
        {
            var botNodes = GetComponentsInChildren<ActionNodeComponent>();
            return new BotNodeObject.BotNodeRoot
            {
                Nodes = botNodes.Select(x => x.ConvertBotNodeObject()).ToArray()
            };
        }
    }
    
    /// <summary>
    /// Editor拡張
    /// </summary>
    [CustomEditor(typeof(RootNodeComponent))]
    public class RootNodeComponentEditor : NodeComponentBaseEditor
    {
        private RootNodeComponent Self => (RootNodeComponent) target;
        
        /// <inheritdoc/>
        protected override GameObject TargetGameObject => Self.gameObject;
       
        /// <summary>
        /// OnEnable
        /// </summary>
        private void OnEnable()
        {
            BotNodeEditorSystem.Init(); 

            //labelが空ならGUID入れておく
            serializedObject.Update();
            var id = serializedObject.FindProperty("id");
            if (string.IsNullOrEmpty(id.stringValue) || id.stringValue == "no id")
            {
                id.stringValue = Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }
        
        /// <summary>
        /// 更新処理
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TargetGameObject.transform.hideFlags = HideFlags.HideInInspector;
           
            EditorHelper.Separate();
           
            if (GUILayout.Button("＋ Action"))
            {
                BotNodeEditorHelper.AddAction(TargetGameObject);
            }
            
            EditorHelper.Separate();
            
            // ボタン押下時
            if (GUILayout.Button("Export"))
            {
                if (!new Regex(@"^[a-zA-Z][\w\-]*?").IsMatch(Self.Id))
                {
                    EditorUtility.DisplayDialog("IDエラー", "英数字とハイフン、アンダーバーのみ入力可能", "確認");
                    return;
                }
                BotNodeCreator.Create(Self, Self.Id);
            }
        }
        
    }
}
#endif
