using MasterMemory;
using MessagePack;

namespace App.Master.Tables
{
    
    /// <summary>
    /// 敵スキル効果
    /// </summary>
    [MemoryTable("EnemySkillEffect"), MessagePackObject(true)]
    public class EnemySkillEffect
    {
        /// <summary> EffectID </summary>
        [PrimaryKey]
        public uint EffectId { get; set; }
        
        /// <summary> 効果Type </summary>
        public App.AppCommon.SkillEffectType Type { get; set; }
        
        /// <summary> 効果値 </summary>
        public float Value { get; set; }
        
        /// <summary> 効果範囲タイプ </summary>
        public App.AppCommon.SkillEffectRangeType RangeType { get; set; }
        
        /// <summary> 効果範囲値 </summary>
        public int RangeValue { get; set; }
        
        /// <summary> イメージID </summary>
        public string ImageId { get; set; }
    }
}