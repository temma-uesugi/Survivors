using MasterMemory;
using MessagePack;

namespace Master.Tables.Hero
{
    /// <summary>
    /// 味方陣形枠
    /// </summary>
    [MemoryTable("HeroFormationFrame"), MessagePackObject(true), MasterData("Hero")]
    public class HeroFormationFrame
    {
        /// <summary> 陣形ID </summary>
        [PrimaryKey(0)]
        [SecondaryKey(0), NonUnique]
        public uint FormationId { get; set; }

        /// <summary> 陣形の枠Index </summary>
        [PrimaryKey(1)]
        public int FrameIndex { get; set; }

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

        public HeroFormationFrame(uint FormationId, int FrameIndex, int OffsetX, int OffsetY, float DamageRatio, float HpCoef, float AttackCoef)
        {
            this.FormationId = FormationId;
            this.FrameIndex = FrameIndex;
            this.OffsetX = OffsetX;
            this.OffsetY = OffsetY;
            this.DamageRatio = DamageRatio;
            this.HpCoef = HpCoef;
            this.AttackCoef = AttackCoef;
        }
    }
}