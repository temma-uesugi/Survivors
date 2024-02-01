using App.AppCommon;
using Constants;
using App.Battle2.Interfaces;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;

namespace App.Battle2.Core
{
    /// <summary>
    /// バトルイベント
    /// </summary>
    public static class BattleEvents2
    {
        /// <summary>
        /// TestEvent
        /// </summary>
        public readonly struct TestEvent : IBattleEvent
        {
            public int Id { get; init; }
            public string Name { get; init; }
        }
        
        /// <summary>
        /// TestAsyncEvent
        /// </summary>
        public readonly struct TestAsyncEvent : IAsyncBattleEvent
        {
            public int Id { get; init; }
            public string Name { get; init; }
        }
     
        /// <summary>
        /// バトルイベント
        /// </summary>
        public readonly struct OnBattleStartAsync : IAsyncBattleEvent
        {
        }
        
        /// <summary>
        /// ターン開始
        /// </summary>
        public readonly struct OnTurnStartAsync : IAsyncBattleEvent
        {
        }
      
        /// <summary>
        /// ターン終了
        /// </summary>
        public readonly struct OnTurnEndAsync : IAsyncBattleEvent
        {
        }
       
        /// <summary>
        /// フェイズ開始
        /// </summary>
        public readonly struct OnPhaseStartAsync : IAsyncBattleEvent
        {
            public PhaseType Phase { get; init; }
        }
      
        /// <summary>
        /// フェイズ終了
        /// </summary>
        public readonly struct OnPhaseEndAsync : IAsyncBattleEvent
        {
            public PhaseType Phase { get; init; }
        }
        
        /// <summary>
        /// 攻撃引数
        /// </summary>
        public readonly struct AttackArgs
        {
            public AttackType Type { get; init; }
            public IUnitModel2 AttackerUnit { get; init; }
            public IAttackTargetModel TargetUnit { get; init; }
            public int Damage { get; init; }
        }
        
        /// <summary>
        /// 船攻撃イベント
        /// </summary>
        public readonly struct OnShipAttacked : IBattleEvent
        {
            public AttackArgs Args { get; init; }
        }
      
        /// <summary>
        /// 敵攻撃イベント
        /// </summary>
        public readonly struct OnEnemyAttacked : IBattleEvent
        {
            public AttackArgs Args { get; init; }
        }
        
        /// <summary>
        /// 船移動イベント
        /// </summary>
        public readonly struct OnShipMovedEvent : IBattleEvent
        {
            public ShipUnitModel2 Ship { get; init; }
        }
       
        /// <summary>
        /// 敵移動イベント
        /// </summary>
        public readonly struct OnEnemyMovedEvent : IBattleEvent
        {
            public EnemyUnitModel2 Enemy { get; init; }
        }
        
        /// <summary>
        /// 敵行動終了イベント
        /// </summary>
        public readonly struct OnEnemyActionEndEvent : IBattleEvent
        {
            public uint UnitId { get; init; }
        }
       
         /// <summary>
         /// メッセージログ
         /// </summary>
         public readonly struct OnMessageLog : IBattleEvent
         {
             public string Text { get; init; }
         }
        
        // /// <summary>
        // /// Phase終了イベント
        // /// </summary>
        // public readonly struct PhaseEndEvent : IBattleEvent
        // {
        //     public PhaseType EndPhase { get; init; }
        //     public uint ActionUnitId { get; init; }
        // }
    }
}