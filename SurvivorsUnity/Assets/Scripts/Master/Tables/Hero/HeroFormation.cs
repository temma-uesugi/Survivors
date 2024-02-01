using MasterMemory;
using MessagePack;

namespace Master.Tables.Hero
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

        public HeroFormation(uint FormationId, string Name, string Description, uint FrameId1, uint FrameId2, uint FrameId3, uint FrameId4, uint FrameId5, float FrontDamageCutCoef, float SideDamageCutCoef, float BackDamageCutCoef, float BonusHpCoef, float BonusDamageCutCoef)
        {
            this.FormationId = FormationId;
            this.Name = Name;
            this.Description = Description;
            this.FrameId1 = FrameId1;
            this.FrameId2 = FrameId2;
            this.FrameId3 = FrameId3;
            this.FrameId4 = FrameId4;
            this.FrameId5 = FrameId5;
            this.FrontDamageCutCoef = FrontDamageCutCoef;
            this.SideDamageCutCoef = SideDamageCutCoef;
            this.BackDamageCutCoef = BackDamageCutCoef;
            this.BonusHpCoef = BonusHpCoef;
            this.BonusDamageCutCoef = BonusDamageCutCoef;
        }
    }
}