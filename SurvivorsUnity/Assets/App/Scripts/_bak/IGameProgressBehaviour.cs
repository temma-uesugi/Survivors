// using App.AppCommon;
// using App.Game.Units;
// using App.Game.ValueObjects;
//
// namespace App.Game.Core
// {
//     public interface IGameProgressBehaviour
//     {
//         /// <summary>
//         /// ラウンドセット開始
//         /// </summary>
//         void OnRoundStart();
//
//         /// <summary>
//         /// ラウンドセット終了
//         /// </summary>
//         void OnRoundEnd();
//
//         /// <summary>
//         /// ターン開始
//         /// </summary>
//         void OnTurnStart(WindValue wind, WeatherValue weather, IEnemyTurnValue enemyTurnValue);
//
//         /// <summary>
//         /// ターン終了
//         /// </summary>
//         void OnTurnEnd();
//
//         /// <summary>
//         /// フェイズ開始
//         /// </summary>
//         void OnPhaseStart(PhaseType startPhase);
//
//         /// <summary>
//         /// フェイズ終了
//         /// </summary>
//         void OnPhaseEnd(PhaseType endPhase, uint actionUnitId);
//     }
// }