using App.AppCommon.Utils;
using App.Battle2.EnemyBots.NodeObjects;
using App.Battle2.Map;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;

namespace App.Battle2.EnemyBots
{
    /// <summary>
    /// 条件チェック
    /// </summary>
    public class ConditionChecker
    {
        private readonly HexMapManager _mapManager;
        private readonly UnitManger2 unitManger2;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ConditionChecker(
            HexMapManager mapManager,
            UnitManger2 unitManger2
        )
        {
            _mapManager = mapManager;
            this.unitManger2 = unitManger2;
        }

        /// <summary>
        /// Check
        /// </summary>
        public bool Check(BotNodeObject.BotCondition[] conditions, EnemyUnitModel2 enemyModel2)
        {
            foreach (var condition in conditions)
            {
                if (!CheckCondition(condition, enemyModel2))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 条件のチェック
        /// </summary>
        private bool CheckCondition(BotNodeObject.BotCondition condition, EnemyUnitModel2 enemyModel2)
        {
            return condition.Type switch
            {
                ConditionType.None => true,
                ConditionType.Random => CheckRandom(condition.Value),
                _ => true,
            };
        }

        /// <summary>
        /// ランダムチェック
        /// </summary>
        private bool CheckRandom(float rate) => RandomUtil.Judge(rate);
    }
}