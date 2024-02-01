using Master.Constants;

namespace App.Battle2.Units
{
    /// <summary>
    /// ダメージ計算
    /// </summary>
    public abstract class DamageCalculator
    {
        /// <summary>
        /// 与ダメ計算
        /// </summary>
        public abstract float CalcDealDamage(AttackType attackType);

        /// <summary>
        /// 被ダメ計算
        /// </summary>
        public abstract int CalcDamaged(float damage, AttackType attackType);
    }
    
    /// <summary>
    /// ダメージ計算
    /// </summary>
    public abstract class DamageCalculator<T> : DamageCalculator where T : IUnitModel2
    {
        protected T UnitModel2 { get; private set; }
        
        /// <summary>
        /// Setup
        /// </summary>
        public DamageCalculator(T unitModel2)
        {
            UnitModel2 = unitModel2;
        }
    }
}