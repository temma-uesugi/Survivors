// using App.AppCommon;
// using App.Game.Map;
// using App.Game.Objects;
// using App.Game.Units;
// using App.Game.ValueObjects;
// using JetBrains.Annotations;
//
// namespace App.Game.Core
// {
//     /// <summary>
//     /// 進行イベントメッセージ
//     /// </summary>
//     public static class ProgressEventMessages
//     {
//         /// <summary>
//         /// Round開始イベント
//         /// </summary>
//         public readonly struct RoundStartEvent : IProgressGameEvent
//         {
//         }
//
//         /// <summary>
//         /// Round終了イベント
//         /// </summary>
//         public readonly struct RoundEndEvent : IProgressGameEvent
//         {
//         }
//
//         /// <summary>
//         /// Turn開始イベント
//         /// </summary>
//         public readonly struct TurnStartEvent : IProgressGameEvent
//         {
//             public WeatherValue Weather { get; init; }
//             public WindValue Wind { get; init; }
//             public IEnemyTurnValue Enemy { get; init; }
//         }
//
//         /// <summary>
//         /// Turn終了イベント
//         /// </summary>
//         public readonly struct TurnEndEvent : IProgressGameEvent
//         {
//         }
//
//         /// <summary>
//         /// Phase開始イベント
//         /// </summary>
//         public readonly struct PhaseStartEvent : IProgressGameEvent
//         {
//             public PhaseType StartPhase { get; init; }
//         }
//
//         /// <summary>
//         /// Phase終了イベント
//         /// </summary>
//         public readonly struct PhaseEndEvent : IProgressGameEvent
//         {
//             public PhaseType EndPhase { get; init; }
//             public uint ActionUnitId { get; init; }
//         }
//     }
// }