using System;
using System.Linq;
using App.Battle2.Turn;
using App.Battle2.Units.Enemy;

namespace App.Battle2.ValueObjects
{
    public interface IEnemyTurnValue : ITurnValue
    {
        string Label { get; }
        EnemyUnitModel2[] Enemies { get; }
        
    }

    /// <summary>
    /// 敵のTurnValue
    /// </summary>
    public readonly struct EnemyTurnValue : IEnemyTurnValue
    {
        public string Label => Enemies.FirstOrDefault()?.Label;
        public EnemyUnitModel2[] Enemies { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EnemyTurnValue(EnemyUnitModel2[] enemies)
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
        public EnemyUnitModel2[] Enemies => Array.Empty<EnemyUnitModel2>();
    }
}