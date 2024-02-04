using MasterMemory;
using MessagePack;
using Master.Constants;

namespace Master.Tables.Enemy
{
    /// <summary>
    /// 敵スキル効果
    /// </summary>
    [MemoryTable("EnemySkillEffectEntity"), MessagePackObject(true)]
    public class EnemySkillEffectEntity
    {
        /// <summary> EffectID </summary>
        [PrimaryKey]
        public uint EffectId { get; set; }

        /// <summary> 効果Type </summary>
        public SkillEffectType Type { get; set; }

        /// <summary> 効果値 </summary>
        public float Value { get; set; }

        /// <summary> 効果範囲タイプ </summary>
        public SkillEffectRangeType RangeType { get; set; }

        /// <summary> 効果範囲値 </summary>
        public int RangeValue { get; set; }

        /// <summary> イメージID </summary>
        public string ImageId { get; set; }

        public EnemySkillEffectEntity(uint EffectId, SkillEffectType Type, float Value, SkillEffectRangeType RangeType, int RangeValue, string ImageId)
        {
            this.EffectId = EffectId;
            this.Type = Type;
            this.Value = Value;
            this.RangeType = RangeType;
            this.RangeValue = RangeValue;
            this.ImageId = ImageId;
        }
    }
}