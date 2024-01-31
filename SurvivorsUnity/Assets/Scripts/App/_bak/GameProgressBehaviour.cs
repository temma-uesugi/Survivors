// using App.AppCommon;
// using App.Game.Units;
// using App.Game.ValueObjects;
//
// namespace App.Game.Core
// {
//     /// <summary>
//     /// 進行管理があるBehaviour(VContainerに登録される)
//     /// </summary>
//     public abstract class GameProgressBehaviour : IGameProgressBehaviour
//     {
//         /// <summary>
//         /// ラウンド開始
//         /// </summary>
//         public virtual void OnRoundStart() { }
//
//         /// <summary>
//         /// ラウンド終了
//         /// </summary>
//         public virtual void OnRoundEnd() { }
//
//         /// <summary>
//         /// ターン開始
//         /// </summary>
//         public virtual void OnTurnStart(WindValue wind, WeatherValue weather, IEnemyTurnValue enemyTurnValue) { }
//
//         /// <summary>
//         /// ターン開始
//         /// </summary>
//         public virtual void OnTurnEnd() { }
//
//         /// <summary>
//         /// フェイズ開始
//         /// </summary>
//         public virtual void OnPhaseStart(PhaseType startPhase) { }
//
//         /// <summary>
//         /// フェイズ終了
//         /// </summary>
//         public virtual void OnPhaseEnd(PhaseType endPhase, uint actionUnitId) { }
//     }
// }