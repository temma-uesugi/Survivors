using App.Battle.Turn;

namespace App.Battle.ValueObjects
{
    /// <summary>
    /// ターンのAction情報Value
    /// </summary>
    public struct TurnActionInfoValue : ITurnValue
    {
        public int[] ActionShipIndexes { get; }
        public int EnemyActionAmount { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TurnActionInfoValue(int enemyActionAmount, params int[] actionShipIndexes)
        {
            EnemyActionAmount = enemyActionAmount;
            ActionShipIndexes = actionShipIndexes;
        }
    }
}