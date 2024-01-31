// using App.AppCommon;
// using App.Game.Units;
// using App.Game.ValueObjects;
// using UnityEngine;
//
// namespace App.Game.Core
// {
//     /// <summary>
//     /// 進行管理があるMonoBehaviour(VContainerに登録が必要)
//     /// </summary>
//     public class GameProgressMonoBehaviour : MonoBehaviour, IGameProgressBehaviour
//     {
//         /// <summary>
//         /// ラウンドセット開始
//         /// </summary>
//         public virtual void OnRoundStart() { }
//
//         /// <summary>
//         /// ラウンドセット終了
//         /// </summary>
//         public virtual void OnRoundEnd() { }
//
//         /// <summary>
//         /// ラウンド開始
//         /// </summary>
//         public virtual void OnTurnStart(WindValue wind, WeatherValue weather, IEnemyTurnValue enemy) { }
//
//         /// <summary>
//         /// ラウンド開始
//         /// </summary>
//         public virtual void OnTurnEnd() { }
//
//         /// <summary>
//         /// ターン開始
//         /// </summary>
//         public virtual void OnPhaseStart(PhaseType startPhase) { }
//
//         /// <summary>
//         /// ターン終了
//         /// </summary>
//         public virtual void OnPhaseEnd(PhaseType endPhase, uint actionUnitId) { }
//     }
// }