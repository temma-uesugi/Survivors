using MasterMemory;
using MessagePack;

namespace App.Master.Tables
{
    /// <summary>
    /// 味方陣形
    /// </summary>
    [MemoryTable("HeroFormation"), MessagePackObject(true), MasterData("Hero")]
    public class HeroFormation
    {
        /// <summary> 陣形ID </summary>
        [PrimaryKey]
        public uint FormationId { get; set; }
        
        /// <summary> 名称 </summary>
        public string Name { get; set; }
        
        /// <summary> 説明 </summary>
        public string Description { get; set; }
        
        /// <summary> 枠1 </summary>
        public uint FrameId1 { get; set; }
        
        /// <summary> 枠2 </summary>
        public uint FrameId2 { get; set; }
        
        /// <summary> 枠3 </summary>
        public uint FrameId3 { get; set; }
        
        /// <summary> 枠4 </summary>
        public uint FrameId4 { get; set; }
        
        /// <summary> 枠5 </summary>
        public uint FrameId5 { get; set; }
        
        /// <summary> 前面ダメージカット率 </summary>
        public float FrontDamageCutCoef { get; set; }
        
        /// <summary> 側面ダメージカット率 </summary>
        public float SideDamageCutCoef { get; set; }
        
        /// <summary> 後方ダメージカット率 </summary>
        public float BackDamageCutCoef { get; set; }
        
        /// <summary> ボーナスHP係数 </summary>
        public float BonusHpCoef { get; set; }
        
        /// <summary> ボーナスダメージカット係数 </summary>
        public float BonusDamageCutCoef { get; set; }
    }
}