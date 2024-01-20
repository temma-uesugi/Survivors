// using System;
// using System.Collections.Generic;
// using System.Linq;
// using App.Core;
// using App.Game.Libs;
// using UniRx;
// using UniRx.Triggers;
// using UnityEngine;
//
// namespace App.Game.UI.BattleLog
// {
//     /// <summary>
//     /// バトルログView
//     /// </summary>
//     [ContainerRegisterMonoBehaviour(typeof(BattleLogView))]
//     public class BattleLogView : MonoBehaviour
//     {
//         [SerializeField] private BattleLogMessage messagePrefab;
//         [SerializeField] private RectTransform[] anchors;
//         [SerializeField] private Transform messageTrans;
//         // [SerializeField] private ObservableEventTrigger allLogBtnEventTrigger;
//
//         private GameObjectPool<BattleLogMessage> _messagePool;
//
//         private readonly List<BattleLogMessage> _logList = new();
//
//         private const float UpDuration = 0.2f;
//         private const float ShowDuration = 0.5f;
//
//         // public IObservable<Unit> OnAllLogClicked { get; private set; }
//
//         
//         /// <summary>
//         /// Setup
//         /// </summary>
//         public void Setup()
//         {
//             _messagePool = new GameObjectPool<BattleLogMessage>(messagePrefab, messageTrans);
//             messagePrefab.gameObject.SetActive(false);
//             // OnAllLogClicked = allLogBtnEventTrigger.OnPointerClickAsObservable().Select(_=> Unit.Default).AsObservable();
//         }
//
//         /// <summary>
//         /// ログの追加
//         /// </summary>
//         public void AddLog(string log)
//         {
//             if (!anchors.Any())
//             {
//                 return;
//             }
//             if (_logList.Count == anchors.Length)
//             {
//                 //最大
//                 SetAndRemove(log);
//             }
//             else
//             {
//                 //まだない
//                 Set(log);
//             }
//         }
//
//         /// <summary>
//         /// セットして一つリムーブ
//         /// </summary>
//         private void SetAndRemove(string logText)
//         {
//             var firstLog = _logList.First();
//             _logList.RemoveAt(0);
//             _messagePool.Return(firstLog);
//
//             var n = anchors.Length - _logList.Count - 1;
//             foreach (var log in _logList)
//             {
//                 var upPos = anchors[n].position;
//                 log.Move(upPos, UpDuration);
//                 n++;
//             }
//             Set(logText);
//         }
//
//         /// <summary>
//         /// セットのみ
//         /// </summary>
//         private void Set(string logText)
//         {
//             var idx = _logList.Count;
//             if (anchors.Length <= idx)
//             {
//                 return;
//             }
//             var lastAnchorPos = anchors[idx].position;
//             var log = _messagePool.Rent();
//             log.Show(logText, lastAnchorPos, ShowDuration);
//             _logList.Add(log);
//         }
//     }
// }