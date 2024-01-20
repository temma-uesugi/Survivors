using System;
using YamlDotNet.Serialization;

namespace App.Battle.EnemyBots.NodeObjects
{
    /// <summary>
    /// BotNodeのオブジェクト
    /// </summary>
    public static class BotNodeObject
    {
        /// <summary>
        /// Root
        /// </summary>
        public record Root
        {
            public Node[] Nodes { get; init; }
        }
        
        /// <summary>
        /// Node
        /// </summary>
        public record Node
        {
            [YamlMember(Alias = "cond")]
            public ConditionType[] Conditions { get; init; }
            public ActionType Action { get; init; }
            [YamlMember(Alias = "target")]
            public ActionTargetType[] TargetPriority { get; init; }
        }

        /// <summary>
        /// 条件
        /// </summary>
        [Serializable]
        public record BotCondition
        {
            public ConditionType Type;
            public float Value;
        }

        /// <summary>
        /// アクション
        /// </summary>
        [Serializable]
        public record BotAction
        {
            public ActionType ActionType;
            public ActionTargetType[] TargetTypes;
        }
        
        /// <summary>
        /// BotNode
        /// </summary>
        [Serializable]
        public record BotNode
        {
            public BotCondition[] Conditions;
            public BotAction Action;
        }

        [Serializable]
        public record BotNodeRoot
        {
            public BotNode[] Nodes;
        }
    }
}