using App.AppCommon;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵のダメージ計算
    /// </summary>
    public class EnemyDamageCalculator : DamageCalculator<EnemyUnitModel2>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyDamageCalculator(EnemyUnitModel2 unitModel2) : base(unitModel2)
        {
        }
        
        /// <summary>
        /// 与ダメ計算
        /// </summary>
        public override float CalcDealDamage(AttackType attackType)
        {
            return 5;
        }

        /// <summary>
        /// 被ダメ計算
        /// </summary>
        public override int CalcDamaged(float damage, AttackType attackType)
        {
            return (int)damage;
        }
    }
}