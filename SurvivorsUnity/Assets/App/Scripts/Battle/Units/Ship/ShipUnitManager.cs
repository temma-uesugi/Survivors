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
//     /// 船管理
//     /// </summary>
//     [ContainerRegisterMonoBehaviour(typeof(ShipUnitManager))]
//     public class ShipUnitManager : MonoBehaviour
//     {
//         [SerializeField] private Transform shipLayer;
//         [SerializeField] private ShipUnitViewModel shipPrefab;
//
//         //船
//         private readonly Dictionary<uint, ShipUnitView> _shipViewMap = new();
//         private readonly ReactiveDictionary<uint, ShipUnitModel> _shipModelMap = new();
//         public IReadOnlyReactiveDictionary<uint, ShipUnitModel> ShipModelMap => _shipModelMap;
//         public IEnumerable<ShipUnitModel> AllAliveShips => _shipModelMap.Values.Where(x => x.IsAlive);
//         private ShipUnitModel _currentShip;
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
//
//             _gameState.SelectedShipUnit.Subscribe(SelectShip).AddTo(this);
//         }
//
//         /// <summary>
//         /// 船選択
//         /// </summary>
//         private void SelectShip(ShipUnitModel shipModel)
//         {
//             if (_currentShip != null)
//             {
//                 _currentShip.UnSelect();
//             }
//             _currentShip = shipModel;
//             if (_currentShip != null)
//             {
//                 _currentShip.Select();
//             }
//         }
//         
//          /// <summary>
//          /// 船作成
//          /// </summary>
//          public void CreateShip(
//              ShipCreateParam createParam
//          )
//          {
//              var unitId = UnitUtil.GetUnitId();
//              var label = UnitUtil.GetShipLabel();
//              var shipModel = _factory.CreateShip(unitId, label, createParam);
//              var ship = Instantiate(shipPrefab, shipLayer);
//              ship.Setup(shipModel);
//              _shipModelMap.Add(unitId, shipModel);
//              _shipViewMap.Add(unitId, ship.UnitView);
//          }
//
//         /// <summary>
//         /// IDから船Model取得
//         /// </summary>
//         public ShipUnitModel GetShipModelById(uint id, bool includeNotAlive = false)
//         {
//             if (!_shipModelMap.TryGetValue(id, out var ship))
//             {
//                 return null;
//             }
//             if (!includeNotAlive && !ship.IsAlive)
//             {
//                 return null;
//             }
//             return ship;
//         }
//
//         /// <summary>
//         /// IDから船Model取得を試みる
//         /// </summary>
//         public bool TryGetShipModelById(uint id, out ShipUnitModel ship, bool includeNotAlive = false)
//         {
//             ship = GetShipModelById(id, includeNotAlive);
//             if (ship == null)
//             {
//                 return false;
//             }
//             return true;
//         }
//
//         /// <summary>
//         /// IDから船View取得
//         /// </summary>
//         public ShipUnitView GetShipViewById(uint id, bool includeNotAlive = false)
//         {
//             if (!_shipViewMap.TryGetValue(id, out var ship))
//             {
//                 return null;
//             }
//             if (!includeNotAlive && !ship.IsAlive)
//             {
//                 return null;
//             }
//             return ship;
//         }
//
//         /// <summary>
//         /// IDから船View取得を試みる
//         /// </summary>
//         public bool TryGetShipViewById(uint id, out ShipUnitView ship, bool includeNotAlive = false)
//         {
//             ship = GetShipViewById(id, includeNotAlive);
//             if (ship == null)
//             {
//                 return false;
//             }
//             return true;
//         }
//
//         /// <summary>
//         /// 次のIndexのUnityを取得
//         /// </summary>
//         public ShipUnitModel GetNextIdUnit(uint currentId, bool isAdd)
//         {
//             var sortedUnit = AllAliveShips.OrderBy(x => x.UnitId)
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
//             if (nextUnitIndex >= sortedUnit.Length)
//             {
//                 return sortedUnit.FirstOrDefault();
//             }
//             return sortedUnit.Skip(nextUnitIndex).FirstOrDefault();
//         }
//
//         /// <summary>
//         /// HexCellからそこにいるUnitを取得
//         /// </summary>
//         public ShipUnitModel GetUnitByHex(HexCell hexCell)
//             => AllAliveShips
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