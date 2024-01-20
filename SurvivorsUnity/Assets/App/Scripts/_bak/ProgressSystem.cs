// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Linq;
// using App.AppCommon;
// using App.Core;
// using App.Game.ValueObjects;
// using VContainer;
//
// namespace App.Game.Core
// {
//     /// <summary>
//     /// 進行の処理順序
//     /// </summary>
//     [AttributeUsage(AttributeTargets.Class)]
//     public abstract class ProgressOrderAttribute : Attribute
//     {
//         public short Order { get; }
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         public ProgressOrderAttribute(short order)
//         {
//             Order = order;
//         }
//     }
//
//     /// <summary>
//     /// Turn開始の処理順序
//     /// </summary>
//     public class TurnStartOrderAttribute : ProgressOrderAttribute
//     {
//         public TurnStartOrderAttribute(short order) : base(order) { }
//     }
//
//     /// <summary>
//     /// Turn終了の処理順序
//     /// </summary>
//     public class TurnEndOrderAttribute : ProgressOrderAttribute
//     {
//         public TurnEndOrderAttribute(short order) : base(order) { }
//     }
//
//     /// <summary>
//     /// Phase開始の処理順序
//     /// </summary>
//     public class PhaseStartOrderAttribute : ProgressOrderAttribute
//     {
//         public PhaseStartOrderAttribute(short order) : base(order) { }
//     }
//
//     /// <summary>
//     /// Phase終了の処理順序
//     /// </summary>
//     public class PhaseEndOrderAttribute : ProgressOrderAttribute
//     {
//         public PhaseEndOrderAttribute(short order) : base(order) { }
//     }
//     
//     //Note: 処理の順番を管理するために、pub/subやUniRxでやっていない
//     /// <summary>
//     /// 進行管理システム
//     /// </summary>
//     [ContainerRegister(typeof(ProgressSystem), SceneType.Battle)]
//     public class ProgressSystem
//     {
//         private List<IGameProgressBehaviour> _roundStartOrderList = new();
//         private List<IGameProgressBehaviour> _roundEndOrderList = new();
//         private List<IGameProgressBehaviour> _turnStartOrderList = new();
//         private List<IGameProgressBehaviour> _turnEndOrderList = new();
//         private List<IGameProgressBehaviour> _phaseStartOrderList = new();
//         private List<IGameProgressBehaviour> _phaseEndOrderList = new();
//
//         private readonly IObjectResolver _resolver;
//
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public ProgressSystem(
//             IObjectResolver resolver
//         )
//         {
//             _resolver = resolver;
//         }
//
//         /// <summary>
//         /// Setup
//         /// </summary>
//         public void Setup()
//         {
//             Assembly assembly = Assembly.GetExecutingAssembly();
//
//             var turnStartOrderTemp = new List<(int order, IGameProgressBehaviour instance)>();
//             var turnEndOrderTemp = new List<(int order, IGameProgressBehaviour instance)>();
//             var phaseStartOrderTemp = new List<(int order, IGameProgressBehaviour instance)>();
//             var phaseEndOrderTemp = new List<(int order, IGameProgressBehaviour instance)>();
//             foreach (var t in assembly.GetTypes().Where(x => x.GetInterfaces().Any(t => t == typeof(IGameProgressBehaviour))))
//             {
//                 try
//                 {
//                     var instance = _resolver.Resolve(t);
//                     {
//                         TurnStartOrderAttribute attr = t.GetCustomAttribute<TurnStartOrderAttribute>(true);
//                         var order = attr?.Order ?? 0;
//                         turnStartOrderTemp.Add((order, instance as IGameProgressBehaviour));
//                     }
//                     {
//                         TurnEndOrderAttribute attr = t.GetCustomAttribute<TurnEndOrderAttribute>(true);
//                         var order = attr?.Order ?? 0;
//                         turnEndOrderTemp.Add((order, instance as IGameProgressBehaviour));
//                     }
//                     {
//                         PhaseStartOrderAttribute attr = t.GetCustomAttribute<PhaseStartOrderAttribute>(true);
//                         var order = attr?.Order ?? 0;
//                         phaseStartOrderTemp.Add((order, instance as IGameProgressBehaviour));
//                     }
//                     {
//                         PhaseEndOrderAttribute attr = t.GetCustomAttribute<PhaseEndOrderAttribute>(true);
//                         var order = attr?.Order ?? 0;
//                         phaseEndOrderTemp.Add((order, instance as IGameProgressBehaviour));
//                     }
//                 }
//                 catch
//                 {
//                     continue;
//                 }
//             }
//             _turnStartOrderList = turnStartOrderTemp.OrderBy(x => x.order)
//                 .Select(x => x.instance)
//                 .ToList();
//             _turnEndOrderList = turnEndOrderTemp.OrderBy(x => x.order)
//                 .Select(x => x.instance)
//                 .ToList();
//             _phaseStartOrderList = phaseStartOrderTemp.OrderBy(x => x.order)
//                 .Select(x => x.instance)
//                 .ToList();
//             _phaseEndOrderList = phaseEndOrderTemp.OrderBy(x => x.order)
//                 .Select(x => x.instance)
//                 .ToList();
//         }
//
//         /// <summary>
//         /// TurnStart
//         /// </summary>
//         public void TurnStart(WindValue wind, WeatherValue weather, IEnemyTurnValue enemy)
//         {
//             foreach (var ins in _turnStartOrderList)
//             {
//                 ins.OnTurnStart(wind, weather, enemy);
//             }
//         }
//
//         /// <summary>
//         /// TurnEnd
//         /// </summary>
//         public void TurnEnd()
//         {
//             foreach (var ins in _turnEndOrderList)
//             {
//                 ins.OnTurnEnd();
//             }
//         }
//
//         /// <summary>
//         /// PhaseStart
//         /// </summary>
//         public void PhaseStart(PhaseType startPhase)
//         {
//             foreach (var ins in _phaseStartOrderList)
//             {
//                 ins.OnPhaseStart(startPhase);
//             }
//         }
//
//         /// <summary>
//         /// PhaseEnd
//         /// </summary>
//         public void PhaseEnd(PhaseType endPhase, uint actionUnitId)
//         {
//             foreach (var ins in _phaseEndOrderList)
//             {
//                 ins.OnPhaseEnd(endPhase, actionUnitId);
//             }
//         }
//     }
// }