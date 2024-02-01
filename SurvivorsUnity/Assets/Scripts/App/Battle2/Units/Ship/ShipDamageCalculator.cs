using App.AppCommon;
using App.AppCommon.Core;
using Constants;

namespace App.Battle2.Units.Ship
{
    /// <summary>
    /// 船のダメージ計算
    /// </summary>
    public class ShipDamageCalculator : DamageCalculator<ShipUnitModel2>
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShipDamageCalculator(ShipUnitModel2 unitModel2) : base(unitModel2)
        {
        }
        
        /// <summary>
        /// 与ダメ計算
        /// </summary>
        public override float CalcDealDamage(AttackType attackType)
        {
            return attackType switch
            {
                AttackType.Bomb => CalcDealDamageOfBomb(),
                AttackType.Slash => CalcDealDamageOfSlash(),
                AttackType.Assault => CalcDealDamageOfAssault(),
                _ => 0,
            };
        }

        /// <summary>
        /// 砲撃ダメージ計算
        /// </summary>
        private float CalcDealDamageOfBomb()
        {
            return 5;
        }
        
        /// <summary>
        /// 近接ダメージ計算
        /// </summary>
        private float CalcDealDamageOfSlash()
        {
            return 10;
        }

        /// <summary>
        /// 突撃ダメージ計算
        /// </summary>
        private float CalcDealDamageOfAssault()
        {
            Log.Debug("CalcDealDamageOfAssault", UnitModel2.Status.MovePower.Current);
            return 2 * (float)UnitModel2.Status.MovePower.Current;
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