// using UnityEngine;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using App.Core;
// using App.Game.Common;
// using App.Game.Core;
// using App.Game.Map;
// using App.Game.Utils;
// using MessagePipe;
// using UniRx;
// using VContainer;
//
// namespace App.Game.Units
// {
//     /// <summary>
//     /// 敵管理
//     /// </summary>
//     [ContainerRegisterMonoBehaviour(typeof(ShipUnitManager))]
//     public class EnemyUnitManager : MonoBehaviour
//     {
//         [SerializeField] private Transform enemyLayer;
//         [SerializeField] private EnemyUnitViewModel enemyPrefab;
//
//         //敵
//         private readonly Dictionary<uint, EnemyUnitView> _enemyViewMap = new();
//         private readonly ReactiveDictionary<uint, EnemyUnitModel> _enemyModelMap = new();
//         public IReadOnlyReactiveDictionary<uint, EnemyUnitModel> EnemyModelMap => _enemyModelMap;
//         public IEnumerable<EnemyUnitModel> AllAliveEnemies => _enemyModelMap.Values.Where(x => x.IsAlive);
//
//         private GameState _gameState;
//         private HexMapManager _hexMapManager;
//         private EventMessageSystem _messageSystem;
//         private ModelFactory _factory;
//
//         private IDisposable _disposable;
//
//         /// <summary>
//         /// コンストラクタ
//         /// </summary>
//         [Inject]
//         public void Construct(
//             GameState gameState,
//             HexMapManager hexMapManager,
//             ISubscriber<IGameEvent> eventSub,
//             EventMessageSystem eventMessageSystem,
//             ModelFactory factory
//         )
//         {
//             _gameState = gameState;
//             _hexMapManager = hexMapManager;
//             _messageSystem = eventMessageSystem;
//             _factory = factory;
//         }
//
//         /// <summary>
//         /// 敵作成
//         /// </summary>
//         public void CreateEnemy(
//             EnemyCreateParam createParam
//         )
//         {
//             var unitId = UnitUtil.GetUnitId();
//             var label = UnitUtil.GetEnemyLabel();
//             var enemyModel = _factory.CreateEnemy(unitId, label, createParam);
//             var enemy = Instantiate(enemyPrefab, enemyLayer);
//             enemy.Setup(enemyModel);
//             // _objectMap.Add(unitId, enemy.UnitView);
//             _enemyModelMap.Add(unitId, enemyModel);
//             _enemyViewMap.Add(unitId, enemy.UnitView);
//         }
//
//         /// <summary>
//         /// IDから敵Model取得
//         /// </summary>
//         public EnemyUnitModel GetEnemyModelById(uint id, bool includeNotAlive = false)
//         {
//             if (!_enemyModelMap.TryGetValue(id, out var enemy))
//             {
//                 return null;
//             }
//
//             if (!includeNotAlive && !enemy.IsAlive)
//             {
//                 return null;
//             }
//
//             return enemy;
//         }
//
//         /// <summary>
//         /// IDから敵Model取得を試みる
//         /// </summary>
//         public bool TryGetEnemyModelById(uint id, out EnemyUnitModel enemy, bool includeNotAlive = false)
//         {
//             enemy = GetEnemyModelById(id, includeNotAlive);
//             if (enemy == null)
//             {
//                 return false;
//             }
//
//             return true;
//         }
//
//         /// <summary>
//         /// IDから敵View取得
//         /// </summary>
//         public EnemyUnitView GetEnemyViewById(uint id, bool includeNotAlive = false)
//         {
//             if (!_enemyViewMap.TryGetValue(id, out var enemy))
//             {
//                 return null;
//             }
//
//             if (!includeNotAlive && !enemy.IsAlive)
//             {
//                 return null;
//             }
//
//             return enemy;
//         }
//
//         /// <summary>
//         /// IDから敵View取得を試みる
//         /// </summary>
//         public bool TryGetEnemyViewById(uint id, out EnemyUnitView enemy, bool includeNotAlive = false)
//         {
//             enemy = GetEnemyViewById(id, includeNotAlive);
//             if (enemy == null)
//             {
//                 return false;
//             }
//
//             return true;
//         }
//
//         /// <summary>
//         /// 次のIndexのUnityを取得
//         /// </summary>
//         public EnemyUnitModel GetNextIdUnit(uint currentId, bool isAdd)
//         {
//             var sortedUnit = AllAliveEnemies.OrderBy(x => x.UnitId)
//                 .ToArray();
//             var curUnit = sortedUnit
//                 .Select((x, i) => (Id: x.UnitId, Index: i))
//                 .FirstOrDefault(x => x.Id == currentId);
//             if (curUnit == default)
//             {
//                 return sortedUnit.FirstOrDefault();
//             }
//
//             var nextUnitIndex = curUnit.Index + (isAdd ? 1 : -1);
//             if (nextUnitIndex < 0)
//             {
//                 return sortedUnit.LastOrDefault();
//             }
//
//             if (nextUnitIndex >= sortedUnit.Length)
//             {
//                 return sortedUnit.FirstOrDefault();
//             }
//
//             return sortedUnit.Skip(nextUnitIndex).FirstOrDefault();
//         }
//
//         /// <summary>
//         /// HexCellからそこにいるUnitを取得
//         /// </summary>
//         public EnemyUnitModel GetUnitByHex(HexCell hexCell)
//             => AllAliveEnemies
//                 .FirstOrDefault(x => x.Cell.Value == hexCell);
//
//         /// <summary>
//         /// OnDestroy
//         /// </summary>
//         private void OnDestroy()
//         {
//             _disposable?.Dispose();
//         }
//     }
// }