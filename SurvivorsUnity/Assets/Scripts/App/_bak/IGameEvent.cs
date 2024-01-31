// namespace App.Game.Core
// {
//     /// <summary>
//     /// ゲーム内イベント
//     /// </summary>
//     public interface IGameEvent
//     {
//     }
//
//     /// <summary>
//     /// ゲーム内進行イベント
//     /// </summary>
//     public interface IProgressGameEvent : IGameEvent
//     {
//     }
//
//     /// <summary>
//     /// Shipイベント
//     /// </summary>
//     public interface IShipGameEvent : IGameEvent
//     {
//         uint ShipUnitId { get; init; }
//     }
//
//     /// <summary>
//     /// 敵イベント
//     /// </summary>
//     public interface IEnemyGameEvent : IGameEvent
//     {
//         uint UnitId { get; init; }
//     }
// }