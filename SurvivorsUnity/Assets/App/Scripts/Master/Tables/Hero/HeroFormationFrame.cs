using MasterMemory;
using MessagePack;

namespace App.Master.Tables
{
    /// <summary>
    /// 味方陣形枠
    /// </summary>
    [MemoryTable("HeroFormationFrame"), MessagePackObject(true)]
    public class HeroFormationFrame
    {
        /// <summary> 陣形枠ID </summary>
        [PrimaryKey]
        public uint FormationFrameId { get; set; }
       
        /// <summary> XのOffset </summary>
        public int OffsetX { get; set; }
        
        /// <summary> YのOffset </summary>
        public int OffsetY { get; set; }
        
        /// <summary> ダメージ割合 </summary>
        public float DamageRatio { get; set; }
        
        /// <summary> HP係数 </summary>
        public float HpCoef { get; set; }
        
        /// <summary> 攻撃係数 </summary>
        public float AttackCoef { get; set; }
    }
}