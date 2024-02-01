using App.AppCommon;
using App.Battle2.ValueObjects;
using Constants;

namespace App.Battle2.Units.Enemy
{
    //TODO ここマスタに移す
    /// <summary>
    /// 敵ステータス
    /// </summary>
    public record EnemyStatus(
        StatusValue<int> Hp,
        int MovePower,
        int ActionSpeed,
        EnemyAttackStatus AttackStatus
    )
    {
        public int ActionAmount { get; private set; } = 1;
    }

    /// <summary>
    /// 敵の攻撃ステータス
    /// </summary>
    public record EnemyAttackStatus(
        int AttackRangeDistance
    )
    {
        /// <summary>
        /// ダミーステータス
        /// </summary>
        public static EnemyAttackStatus DummyStatus => new EnemyAttackStatus(
            GameConst.DefaultEnemyAttackRangeDistance
        );
    }
}