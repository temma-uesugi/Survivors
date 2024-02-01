using Constants;
using MasterMemory;
using MessagePack;

namespace Master.Tables.Enemy
{

    /// <summary>
    /// 敵スキル効果
    /// </summary>
    [MemoryTable("EnemySkillEffect"), MessagePackObject(true), MasterData("Enemy")]
    public class EnemySkillEffect
    {
        /// <summary> EffectID </summary>
        [PrimaryKey]
        public uint EffectId { get; set; }

        /// <summary> 効果Type </summary>
        public Constants.SkillEffectType Type { get; set; }

        /// <summary> 効果値 </summary>
        public float Value { get; set; }

        /// <summary> 効果範囲タイプ </summary>
        public Constants.SkillEffectRangeType RangeType { get; set; }

        /// <summary> 効果範囲値 </summary>
        public int RangeValue { get; set; }

        /// <summary> イメージID </summary>
        public string ImageId { get; set; }

        public EnemySkillEffect(uint EffectId, SkillEffectType Type, float Value, SkillEffectRangeType RangeType, int RangeValue, string ImageId)
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