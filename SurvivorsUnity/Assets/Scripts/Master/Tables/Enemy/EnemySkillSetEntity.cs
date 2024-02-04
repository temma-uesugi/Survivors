using MasterMemory;
using MessagePack;

namespace Master.Tables.Enemy
{
    /// <summary>
    /// 敵スキルSet
    /// </summary>
    [MemoryTable("EnemySkillSetEntity"), MessagePackObject(true)]
    public class EnemySkillSetEntity
    {
        /// <summary> SetID </summary>
        [PrimaryKey(0)]
        [SecondaryKey(0), NonUnique]
        public uint SkillSetId { get; set; }

        [PrimaryKey(1)]
        public uint SkillId { get; set; }

        public EnemySkillSetEntity(uint SkillSetId, uint SkillId)
        {
            this.SkillSetId = SkillSetId;
            this.SkillId = SkillId;
        }
    }
}