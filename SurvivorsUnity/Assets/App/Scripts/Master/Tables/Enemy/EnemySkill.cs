using MasterMemory;
using MessagePack;

namespace App.Master.Tables
{
    /// <summary>
    /// 敵スキル
    /// </summary>
    [MemoryTable("EnemySkill"), MessagePackObject(true)]
    public class EnemySkill
    {
        /// <summary> SkillID </summary>
        [PrimaryKey]
        public uint SkillId { get; set; }
        
        /// <summary> 最小攻撃範囲 </summary>
        public int MinRange { get; set; }
        
        /// <summary> 最大攻撃範囲 </summary>
        public int MaxRange { get; set; }
       
        /// <summary> 遮蔽物を無視するか </summary>
        public bool IsIgnoreObstacles { get; set; }
        
        /// <summary> 効果1 </summary>
        public int Effect1 { get; set; }
        
        /// <summary> 効果2 </summary>
        public int Effect2 { get; set; }
        
        /// <summary> 効果3 </summary>
        public int Effect3 { get; set; }
        
        /// <summary> 効果4 </summary>
        public int Effect4 { get; set; }
        
        /// <summary> 効果5 </summary>
        public int Effect5 { get; set; }
        
        /// <summary> イメージID </summary>
        public string ImageId { get; set; }
    }
}