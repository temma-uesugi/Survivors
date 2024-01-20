using MasterMemory;
using MessagePack;

namespace App.Master.Tables
{
    /// <summary>
    /// 敵スキルSet
    /// </summary>
    [MemoryTable("EnemySkillSet"), MessagePackObject(true)]
    public class EnemySkillSet
    {
        /// <summary> SetID </summary>
        [PrimaryKey(0)]
        [SecondaryKey(0), NonUnique]
        public uint SkillSetId { get; set; }
        
        [PrimaryKey(1)]
        public uint SkillId { get; set; }
    }
}