using MasterMemory;
using MessagePack;
using Master.Constants;

namespace Master.Tables.Enemy
{
    /// <summary>
    /// 敵基本
    /// </summary>
    [MemoryTable("EnemyBase"), MessagePackObject(true)]
    public class EnemyBase
    {
        /// <summary> 敵ID </summary>
        [PrimaryKey]
        public uint EnemyId { get; }

        /// <summary> 敵名前 </summary>
        public string EnemyName { get; }

        /// <summary> 行動間隔 </summary>
        public int ActionInterval { get; }

        /// <summary> 移動力 </summary>
        public int MovePower { get; }

        /// <summary> スキルSetId </summary>
        public uint SkillSetId { get; }

        /// <summary> アクティブ化の条件タイプ </summary>
        public EnemyActiveConditionType ActiveConditionType { get; }

        /// <summary> アクティブ化の条件値 </summary>
        public int ActiveConditionValue { get; }

        /// <summary> 非アクティブ化の条件タイプ </summary>
        public EnemyInactiveConditionType InactiveConditionType { get; }

        /// <summary> 非アクティブ化の条件値 </summary>
        public int InactiveConditionValue { get; }

        /// <summary> 飛行しているか </summary>
        public bool IsFlight { get; }

        /// <summary> イメージID </summary>
        public string ImageId { get; }

        public EnemyBase(uint EnemyId, string EnemyName, int ActionInterval, int MovePower, uint SkillSetId, EnemyActiveConditionType ActiveConditionType, int ActiveConditionValue, EnemyInactiveConditionType InactiveConditionType, int InactiveConditionValue, bool IsFlight, string ImageId)
        {
            this.EnemyId = EnemyId;
            this.EnemyName = EnemyName;
            this.ActionInterval = ActionInterval;
            this.MovePower = MovePower;
            this.SkillSetId = SkillSetId;
            this.ActiveConditionType = ActiveConditionType;
            this.ActiveConditionValue = ActiveConditionValue;
            this.InactiveConditionType = InactiveConditionType;
            this.InactiveConditionValue = InactiveConditionValue;
            this.IsFlight = IsFlight;
            this.ImageId = ImageId;
        }
    }
}