using App.AppCommon.Core;
using App.Battle2.EnemyBots.NodeObjects;
using App.Battle2.Facades;
using App.Battle2.Map;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using Cysharp.Threading.Tasks;

namespace App.Battle2.EnemyBots
{
    /// <summary>
    /// アクションの実行
    /// </summary>
    public class ActionExecutor
    {
        private readonly HexMapManager _mapManager;
        private readonly UnitManger2 unitManger2;
        private readonly TargetChecker _targetChecker;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ActionExecutor(
            HexMapManager mapManager,
            UnitManger2 unitManger2
        )
        {
            _mapManager = mapManager;
            this.unitManger2 = unitManger2;
            _targetChecker = new TargetChecker(mapManager, unitManger2);
        }

        /// <summary>
        /// 行動
        /// </summary>
        public async UniTask<bool> ActionAsync(BotNodeObject.BotAction action, EnemyUnitModel2 enemyModel2)
        {
            var target = enemyModel2.TargetUnit.Value;
            if (target == null)
            {
                return true;
            }
            //TODO 一旦攻撃 or 移動
            return action.ActionType switch
            {
                ActionType.None => await MoveAsync(enemyModel2, target),
                ActionType.Move => await MoveAsync(enemyModel2, target),
                ActionType.Attack => await AttackAsync(enemyModel2, target),
                ActionType.SkillAttack => await AttackAsync(enemyModel2, target),
                _ => true,
            };
        }

        /// <summary>
        /// 移動
        /// </summary>
        private async UniTask<bool> MoveAsync(EnemyUnitModel2 enemyModel2, ShipUnitModel2 target)
        {
            await BattleMove.Facade.EnemyMoveAsync(new BattleMove.MoveArgs(enemyModel2.UnitId, target,
                enemyModel2.MovePower));
            return true;
        }
        
        /// <summary>
        /// 攻撃
        /// </summary>
        private async UniTask<bool> AttackAsync(EnemyUnitModel2 enemyModel2, ShipUnitModel2 target)
        {
            await BattleAttack.Facade.EnemyAttackAsync(enemyModel2, target);
            return true;
        }
        
        /// <summary>
        /// スキル攻撃
        /// </summary>
        private async UniTask<bool> SkillAttackAsync(EnemyUnitModel2 enemyModel2, ShipUnitModel2 target)
        {
            Log.Debug("SkillAttack");
            return true;
        }
    }
}