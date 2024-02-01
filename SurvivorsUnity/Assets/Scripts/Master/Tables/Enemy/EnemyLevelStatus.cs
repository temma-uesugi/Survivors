using MasterMemory;
using MessagePack;

namespace Master.Tables.Enemy
{
    /// <summary>
    /// 敵ステータス
    /// </summary>
    [MemoryTable("EnemyStatus"), MessagePackObject(true), MasterData("Enemy")]
    public class EnemyLevelStatus
    {
        /// <summary> 敵ID </summary>
        [PrimaryKey(0)]
        public uint EnemyId { get; set; }

        /// <summary> レベル </summary>
        [PrimaryKey(1)]
        public int Level { get; set; }

        /// <summary> HP </summary>
        public int Hp { get; set; }

        /// <summary> 直接攻撃の防御力 </summary>
        public int DirectAttackDefense { get; set; }

        /// <summary> 遠隔攻撃の防御力 </summary>
        public int RangedAttackDefense { get; set; }

        /// <summary> 攻撃力 </summary>
        public int AttackPower { get; set; }

        public EnemyLevelStatus(uint EnemyId, int Level, int Hp, int DirectAttackDefense, int RangedAttackDefense, int AttackPower)
        {
            this.EnemyId = EnemyId;
            this.Level = Level;
            this.Hp = Hp;
            this.DirectAttackDefense = DirectAttackDefense;
            this.RangedAttackDefense = RangedAttackDefense;
            this.AttackPower = AttackPower;
        }
    }
}