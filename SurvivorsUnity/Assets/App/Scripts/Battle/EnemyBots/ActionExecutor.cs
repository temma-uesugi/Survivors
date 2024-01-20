using System.Linq;
using App.AppCommon.Core;
using App.Battle.EnemyBots.NodeObjects;
using App.Battle.Facades;
using App.Battle.Map;
using App.Battle.Units;
using App.Battle.Units.Enemy;
using App.Battle.Units.Ship;
using Cysharp.Threading.Tasks;

namespace App.Battle.EnemyBots
{
    /// <summary>
    /// アクションの実行
    /// </summary>
    public class ActionExecutor
    {
        private readonly HexMapManager _mapManager;
        private readonly UnitManger _unitManger;
        private readonly TargetChecker _targetChecker;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ActionExecutor(
            HexMapManager mapManager,
            UnitManger unitManger
        )
        {
            _mapManager = mapManager;
            _unitManger = unitManger;
            _targetChecker = new TargetChecker(mapManager, unitManger);
        }

        /// <summary>
        /// 行動
        /// </summary>
        public async UniTask<bool> ActionAsync(BotNodeObject.BotAction action, EnemyUnitModel enemyModel)
        {
            var target = enemyModel.TargetUnit.Value;
            if (target == null)
            {
                return true;
            }
            //TODO 一旦攻撃 or 移動
            return action.ActionType switch
            {
                ActionType.None => await MoveAsync(enemyModel, target),
                ActionType.Move => await MoveAsync(enemyModel, target),
                ActionType.Attack => await AttackAsync(enemyModel, target),
                ActionType.SkillAttack => await AttackAsync(enemyModel, target),
                _ => true,
            };
        }

        /// <summary>
        /// 移動
        /// </summary>
        private async UniTask<bool> MoveAsync(EnemyUnitModel enemyModel, ShipUnitModel target)
        {
            await BattleMove.Facade.EnemyMoveAsync(new BattleMove.MoveArgs(enemyModel.UnitId, target,
                enemyModel.MovePower));
            return true;
        }
        
        /// <summary>
        /// 攻撃
        /// </summary>
        private async UniTask<bool> AttackAsync(EnemyUnitModel enemyModel, ShipUnitModel target)
        {
            await BattleAttack.Facade.EnemyAttackAsync(enemyModel, target);
            return true;
        }
        
        /// <summary>
        /// スキル攻撃
        /// </summary>
        private async UniTask<bool> SkillAttackAsync(EnemyUnitModel enemyModel, ShipUnitModel target)
        {
            Log.Debug("SkillAttack");
            return true;
        }
    }
}