using MasterMemory;
using MessagePack;

namespace Master.Tables.Hero
{
    /// <summary>
    /// 味方陣形
    /// </summary>
    [MemoryTable("HeroFormation"), MessagePackObject(true)]
    public class HeroFormation
    {
        /// <summary> 陣形ID </summary>
        [PrimaryKey]
        public uint FormationId { get; set; }

        /// <summary> 名称 </summary>
        public string Name { get; set; }

        /// <summary> 説明 </summary>
        public string Description { get; set; }

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

        public HeroFormation(uint FormationId, string Name, string Description, float FrontDamageCutCoef, float SideDamageCutCoef, float BackDamageCutCoef, float BonusHpCoef, float BonusDamageCutCoef)
        {
            this.FormationId = FormationId;
            this.Name = Name;
            this.Description = Description;
            this.FrontDamageCutCoef = FrontDamageCutCoef;
            this.SideDamageCutCoef = SideDamageCutCoef;
            this.BackDamageCutCoef = BackDamageCutCoef;
            this.BonusHpCoef = BonusHpCoef;
            this.BonusDamageCutCoef = BonusDamageCutCoef;
        }
    }
}