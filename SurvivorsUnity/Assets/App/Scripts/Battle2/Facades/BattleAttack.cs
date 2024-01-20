using System;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Interfaces;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using Cysharp.Threading.Tasks;
using VContainer;

namespace App.Battle2.Facades
{
    /// <summary>
    /// 攻撃のFacade
    /// </summary>
    [ContainerRegisterAttribute2(typeof(BattleAttack))]
    public class BattleAttack
    {
        public static BattleAttack Facade { get; private set; }
        
        private readonly UnitManger _unitManger;
        private readonly BattleEventHub2 eventHub2;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public BattleAttack(
            UnitManger unitManger,
            BattleEventHub2 eventHub2
        )
        {
            _unitManger = unitManger;
            this.eventHub2 = eventHub2;

            Facade = this;
        }

        /// <summary>
        /// 船攻撃
        /// </summary>
        public void ShipAttack(ShipUnitModel ship, IAttackTargetModel target, AttackType attackType)
        {
            if (attackType == AttackType.Invalid)
            {
                return;
            } 
                
            // var ship = _unitManger.GetShipModelById(shipUnitId);
            // var target = _unitManger.GetEnemyModelById(targetUnitId);
            
            var damage = attackType switch
            {
                AttackType.Bomb => ShipBombAttack(ship, target),
                AttackType.Slash => ShipSlashAttack(ship, target),
                AttackType.Assault => ShipAssaultAttack(ship, target),
                _=> throw new Exception("AttackType is Invalid"),
            };
           
            target.DealDamage(damage, attackType);
            ship.DecideAction();
            eventHub2.Publish(new BattleEvents2.OnShipAttacked
            {
                Args = new BattleEvents2.AttackArgs
                {
                    Type = attackType,
                    AttackerUnit = ship,
                    TargetUnit = target,
                    Damage = damage,
                }
            });
        }
        
        /// <summary>
        /// 砲撃攻撃
        /// </summary>
        private int ShipBombAttack(ShipUnitModel ship, IAttackTargetModel target)
        {
            var originDamage = ship.DamageCalculator.CalcDealDamage(AttackType.Bomb);
            var damage = target.CalcDamaged(originDamage, AttackType.Bomb);
            return damage;
        }

        /// <summary>
        /// 近接攻撃
        /// </summary>
        private int ShipSlashAttack(ShipUnitModel ship, IAttackTargetModel target)
        {
            var originDamage = ship.DamageCalculator.CalcDealDamage(AttackType.Slash);
            var damage = target.CalcDamaged(originDamage, AttackType.Slash);
            ship.SetMovePower(0);
            return damage;
        }

        /// <summary>
        /// 突撃攻撃
        /// </summary>
        private int ShipAssaultAttack(ShipUnitModel ship, IAttackTargetModel target)
        {
            var originDamage = ship.DamageCalculator.CalcDealDamage(AttackType.Assault);
            var damage = target.CalcDamaged(originDamage, AttackType.Assault);
            ship.SetMovePower(0);
            return damage;
        }

        /// <summary>
        /// 敵の攻撃
        /// </summary>
        public async UniTask EnemyAttackAsync(EnemyUnitModel enemy, IAttackTargetModel target)
        {
            var originDamage = enemy.DamageCalculator.CalcDealDamage(AttackType.EnemyAttack);
            var damage = target.CalcDamaged(originDamage, AttackType.EnemyAttack);

            // var distance = MapRoutSearch.HeuristicDistance(enemy.Cell.Value, target.Cell.Value);
            // if (distance > 1)
            // {
            //     //先に移動
            //     await BattleMove.Facade.EnemyMoveAsync(new BattleMove.MoveArgs(enemy.UnitId, target, 0, enemy.AttackRange));
            // }
            
            target.DealDamage(damage, AttackType.EnemyAttack);
            eventHub2.Publish(new BattleEvents2.OnEnemyAttacked
            {
                Args = new BattleEvents2.AttackArgs
                {
                    Type = AttackType.EnemyAttack,
                    AttackerUnit = enemy,
                    TargetUnit = target,
                    Damage = damage,
                }
            });
        }
    }
}