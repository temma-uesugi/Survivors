#if UNITY_EDITOR
using App.Battle.EnemyBots.NodeObjects;
using UnityEditor;
using UnityEngine;

namespace App.AppEditor.BotNodeEditor
{
    /// <summary>
    /// ActionNode
    /// </summary>
    public class ActionNodeComponent : NodeComponentBase
    {
        [SerializeField] private BotNodeObject.BotCondition[] conditions;
        [SerializeField] private ActionType actionType;
        [SerializeField] private ActionTargetType[] targetTypes;

        /// <summary>
        /// BotNodeに変換
        /// </summary>
        public BotNodeObject.BotNode ConvertBotNodeObject() => new BotNodeObject.BotNode
        {
            Conditions = conditions,
            Action = new BotNodeObject.BotAction
            {
                ActionType = actionType,
                TargetTypes = targetTypes,
            },
        };
    }
    
    /// <summary>
    /// Editor拡張
    /// </summary>
    [CustomEditor(typeof(ActionNodeComponent))]
    public class ActionNodeComponentEditor : NodeComponentBaseEditor
    {
        private ActionNodeComponent Self => (ActionNodeComponent) target;
        
        /// <inheritdoc/>
        protected override GameObject TargetGameObject => Self.gameObject;
        
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
