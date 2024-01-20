using System;
using System.Linq;
using App.Battle.Turn;
using App.Battle.Units.Enemy;

namespace App.Battle.ValueObjects
{
    public interface IEnemyTurnValue : ITurnValue
    {
        string Label { get; }
        EnemyUnitModel[] Enemies { get; }
        
    }

    /// <summary>
    /// 敵のTurnValue
    /// </summary>
    public readonly struct EnemyTurnValue : IEnemyTurnValue
    {
        public string Label => Enemies.FirstOrDefault()?.Label;
        public EnemyUnitModel[] Enemies { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyTurnValue(EnemyUnitModel[] enemies)
        {
            Enemies = enemies;
        }
    }

    /// <summary>
    /// 空の敵のTurnValue
    /// </summary>
    public readonly struct EmptyEnemyTurnValue : IEnemyTurnValue, IEmptyTurnValue
    {
        public string Label => String.Empty;
        public EnemyUnitModel[] Enemies => Array.Empty<EnemyUnitModel>();
    }
}