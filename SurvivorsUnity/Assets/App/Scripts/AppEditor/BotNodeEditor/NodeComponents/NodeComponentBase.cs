#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace App.AppEditor.BotNodeEditor
{
    /// <summary>
    /// NodeComponentBase
    /// </summary>
    public abstract class NodeComponentBase :MonoBehaviour
    {
        [SerializeField] private string description = "no description";
        public string Description => description;

        private string _prevDescription = "";
        
        /// <summary>
        /// Descriptionに変更があるか
        /// </summary>
        public bool IsDescriptionChanged(out string desc)
        {
            desc = description;
            if (_prevDescription == description)
            {
               //変更なし
                return false;
            }
            //変更あり
            _prevDescription = string.Copy(description);
            return true;
        }
           
        /// <summary>
        /// 深度を取得
        /// </summary>
        public virtual int GetDepth()
        {
            return GetComponentsInParent<NodeComponentBase>().Length - 1;
        }

        /// <summary>
        /// 列のIndexを取得
        /// </summary>
        public virtual int GetRowIndex()
        {
            var parent = transform.parent.GetComponent<NodeComponentBase>();
            if (parent == null)
            {
                throw new Exception("invalid tree");
            }
            var rowIndex = parent.GetRowIndex();
            foreach (var brotherNode in parent.GetNodeChildren())
            {
                if (brotherNode.gameObject == gameObject)
                {
                    break;
                }
                rowIndex += 1 + brotherNode.GetChildrenSpreadSize();
            }

            return rowIndex;
        }
        
        /// <summary>
        /// 子要素を取得
        /// </summary>
        public IEnumerable<NodeComponentBase> GetNodeChildren()
        {
            return transform
                .OfType<Transform>()
                .Select(x => x.GetComponent<NodeComponentBase>())
                .Where(x => x != null);
        }
        
        /// <summary>
        /// 子孫がどこまで広がりをもっているか
        /// </summary>
        /// <returns></returns>
        private int GetChildrenSpreadSize()
        {
            int res = 0;
            bool fistChild = true;
            foreach (var node in GetNodeChildren())
            {
                res += node.GetChildrenSpreadSize();
                if (fistChild)
                {
                    fistChild = false;
                }
                else
                {
                    //最初の子でなければ列分 + 1
                    res += 1;
                }
            }
            return res;
        }
        
        /// <summary>
        /// 最初の子要素か
        /// </summary>
        public virtual bool IsFirstChild()
        {
            var parentNode = transform.parent.GetComponent<NodeComponentBase>();
            if (parentNode == null)
            {
                return false;
            }
            return parentNode.GetRowIndex() == GetRowIndex();
        }
    }
    
    /// <summary>
    /// インスペクタ拡張
    /// </summary>
    [CustomEditor(typeof(NodeComponentBase))]
    public abstract class NodeComponentBaseEditor : Editor
    {
        protected abstract GameObject TargetGameObject { get; }

        /// <summary>
        /// 更新処理
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            TargetGameObject.transform.hideFlags = HideFlags.HideInInspector;
        }
    }
}
#endif
