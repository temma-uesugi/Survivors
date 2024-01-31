// using App.AppCommon;
// using App.Game.Map;
// using App.Game.Units;
// using UnityEngine;
//
// namespace App.Game.Core
// {
//     /// <summary>
//     /// ゲームMessage
//     /// </summary>
//     public static class EventMessages
//     {
//       
//       
//         // /// <summary>
//         // /// 船選択イベント
//         // /// </summary>
//         // public readonly struct OnUnitSelectedEvent : IShipGameEvent
//         // {
//         //     public uint ShipUnitId { get; init; }
//         // }
//         
//         /// <summary>
//         /// 船アクション終了イベント
//         /// </summary>
//         public readonly struct OnShipActionEndEvent : IShipGameEvent
//         {
//             public uint ShipUnitId { get; init; }
//         }
//
//         /// <summary>
//         /// 行動開始
//         /// </summary>
//         public readonly struct OnActionStartEvent : IShipGameEvent
//         {
//             public uint ShipUnitId { get; init; }
//         }
//       
//         /// <summary>
//         /// 敵行動終了イベント
//         /// </summary>
//         public readonly struct OnEnemyActionEndEvent : IEnemyGameEvent
//         {
//             public uint UnitId { get; init; }
//         }
//
//         /// <summary>
//         /// メッセージログ
//         /// </summary>
//         public readonly struct OnMessageLogEvent : IGameEvent
//         {
//             public string Text { get; init; }
//         }
//
//         /// <summary>
//         /// 船移動イベント
//         /// </summary>
//         public readonly struct OnShipMovedEvent : IGameEvent
//         {
//             public ShipUnitModel Ship { get; init; }
//         }
//         
//         /// <summary>
//         /// 敵移動イベント
//         /// </summary>
//         public readonly struct OnEnemyMovedEvent : IGameEvent
//         {
//             public EnemyUnitModel Enemy { get; init; }
//         }
//     
//         /// <summary>
//         /// 攻撃引数
//         /// </summary>
//         public readonly struct AttackArgs
//         {
//             public AttackType Type { get; init; }
//             public IUnitModel AttackerUnit { get; init; }
//             public IUnitModel TargetUnit { get; init; }
//             public int Damage { get; init; }
//         }
//         
//         /// <summary>
//         /// 船攻撃イベント
//         /// </summary>
//         public readonly struct OnShipAttackedEvent : IGameEvent
//         {
//             public AttackArgs Args { get; init; }
//         }
//       
//         /// <summary>
//         /// 敵攻撃イベント
//         /// </summary>
//         public readonly struct OnEnemyAttackedEvent : IGameEvent
//         {
//             public AttackArgs Args { get; init; }
//         }
//       
//         /// <summary>
//         /// 船死亡
//         /// </summary>
//         public readonly struct OnShipDefeatedEvent : IGameEvent
//         {
//             public uint ShipUnitId { get; init; }
//         }
//
//         /// <summary>
//         /// 敵死亡
//         /// </summary>
//         public readonly struct OnEnemyDefeatedEvent : IGameEvent
//         {
//             public uint EnemyUnitId { get; init; }
//         }
//     }
// }