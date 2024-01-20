using System;
using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle.Common;
using App.Battle.Core;
using App.Battle.Facades;
using App.Battle.Map;
using App.Battle.Map.Cells;
using App.Battle.Units.Enemy;
using App.Battle.Units.Ship;
using App.Battle.Utils;
using App.Battle.ValueObjects;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.Units
{
    /// <summary>
    /// ユニット管理
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(UnitManger))]
    public class UnitManger : MonoBehaviour
    {
        [SerializeField] private Transform shipLayer;
        [SerializeField] private ShipUnitViewModel shipPrefab;
        [SerializeField] private Transform enemyLayer;
        [SerializeField] private EnemyUnitViewModel enemyPrefab;

        //船
        private readonly Dictionary<uint, ShipUnitView> _shipViewMap = new();
        private readonly ReactiveDictionary<uint, ShipUnitModel> _shipModelMap = new();
        public IReadOnlyReactiveDictionary<uint, ShipUnitModel> ShipModelMap => _shipModelMap;
        public IEnumerable<ShipUnitModel> AllAliveShips => _shipModelMap.Values.Where(x => x.IsAlive);
        private ShipUnitModel _currentShip;

        //敵
        private readonly Dictionary<uint, EnemyUnitView> _enemyViewMap = new();
        private readonly ReactiveDictionary<uint, EnemyUnitModel> _enemyModelMap = new();
        public IReadOnlyReactiveDictionary<uint, EnemyUnitModel> EnemyModelMap => _enemyModelMap;
        public IEnumerable<EnemyUnitModel> AllAliveEnemies => _enemyModelMap.Values.Where(x => x.IsAlive);

        public IEnumerable<IUnitModel> AllAliveUnitModels =>
            _shipModelMap.Values.OfType<IUnitModel>().Concat(_enemyModelMap.Values).Where(x => x.IsAlive);

        private HexMapManager _hexMapManager;
        private ModelFactory _factory;

        private IDisposable _disposable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            HexMapManager hexMapManager,
            ModelFactory factory,
            BattleEventHub eventHub
        )
        {
            _hexMapManager = hexMapManager;
            _factory = factory;

            BattleState.Facade.SelectedShipUnit.Subscribe(SelectShip).AddTo(this);
            
            eventHub.Subscribe<BattleEvents.OnTurnStartAsync>(async _ => TurnStart()).AddTo(this);
            eventHub.Subscribe<BattleEvents.OnTurnEndAsync>(async _ => TurnEnd()).AddTo(this);
        }

        /// <summary>
        /// ターン開始
        /// </summary>
        private void TurnStart()
        {
            foreach (var unit in AllAliveUnitModels)
            {
                unit.OnTurnStart();
            }
        }
        
        /// <summary>
        /// ターン終了
        /// </summary>
        private void TurnEnd()
        {
            foreach (var unit in AllAliveUnitModels)
            {
                unit.OnTurnEnd();
            }
        }
        
        /// <summary>
        /// 船選択
        /// </summary>
        private void SelectShip(ShipUnitModel shipUnitModel)
        {
            if (_currentShip != null)
            {
                _currentShip.UnSelect();
            }
            if (shipUnitModel != null)
            {
                _currentShip = shipUnitModel;
                _currentShip.Select();
            }
        }

        /// <summary>
        /// 船作成
        /// </summary>
        public void CreateShip(
            GridValue initGrid,
            DirectionType initDir, 
            ShipStatus status
        )
        {
            var unitId = UnitUtil.GetUnitId();
            var label = UnitUtil.GetShipLabel();
            var index = UnitUtil.GetShipIndex();
            var initCell = _hexMapManager.GetCellByGrid(initGrid);
            var createParam = new ShipCreateParam(unitId, index, status, initCell, initDir, label);
            var shipModel = _factory.CreateShip(createParam);
            var ship = Instantiate(shipPrefab, shipLayer);
            ship.Setup(shipModel);
            _shipModelMap.Add(unitId, shipModel);
            _shipViewMap.Add(unitId, ship.UnitView);
        }

        /// <summary>
        /// 敵作成
        /// </summary>
        public void CreateEnemy(
            GridValue initGrid,
            uint enemyId,
            int level 
        )
        {
            var unitId = UnitUtil.GetUnitId();
            var label = UnitUtil.GetEnemyLabel();
            var initCell = _hexMapManager.GetCellByGrid(initGrid);
            var createParam = new EnemyCreateParam(unitId, enemyId, level, initCell, label);
            var enemyModel = _factory.CreateEnemy(createParam);
            var enemy = Instantiate(enemyPrefab, enemyLayer);
            enemy.Setup(enemyModel);
            // _objectMap.Add(unitId, enemy.UnitView);
            _enemyModelMap.Add(unitId, enemyModel);
            _enemyViewMap.Add(unitId, enemy.UnitView);
        }

        /// <summary>
        /// IDから船Model取得
        /// </summary>
        public ShipUnitModel GetShipModelById(uint id, bool includeNotAlive = false)
        {
            if (!_shipModelMap.TryGetValue(id, out var ship))
            {
                return null;
            }

            if (!includeNotAlive && !ship.IsAlive)
            {
                return null;
            }

            return ship;
        }

        /// <summary>
        /// IDから船Model取得を試みる
        /// </summary>
        public bool TryGetShipModelById(uint id, out ShipUnitModel ship, bool includeNotAlive = false)
        {
            ship = GetShipModelById(id, includeNotAlive);
            if (ship == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// IDから船View取得
        /// </summary>
        public ShipUnitView GetShipViewById(uint id, bool includeNotAlive = false)
        {
            if (!_shipViewMap.TryGetValue(id, out var ship))
            {
                return null;
            }

            if (!includeNotAlive && !ship.IsAlive)
            {
                return null;
            }

            return ship;
        }

        /// <summary>
        /// IDから船View取得を試みる
        /// </summary>
        public bool TryGetShipViewById(uint id, out ShipUnitView ship, bool includeNotAlive = false)
        {
            ship = GetShipViewById(id, includeNotAlive);
            if (ship == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// IDから敵Model取得
        /// </summary>
        public EnemyUnitModel GetEnemyModelById(uint id, bool includeNotAlive = false)
        {
            if (!_enemyModelMap.TryGetValue(id, out var enemy))
            {
                return null;
            }

            if (!includeNotAlive && !enemy.IsAlive)
            {
                return null;
            }

            return enemy;
        }

        /// <summary>
        /// IDから敵Model取得を試みる
        /// </summary>
        public bool TryGetEnemyModelById(uint id, out EnemyUnitModel enemy, bool includeNotAlive = false)
        {
            enemy = GetEnemyModelById(id, includeNotAlive);
            if (enemy == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// IDから敵View取得
        /// </summary>
        public EnemyUnitView GetEnemyViewById(uint id, bool includeNotAlive = false)
        {
            if (!_enemyViewMap.TryGetValue(id, out var enemy))
            {
                return null;
            }

            if (!includeNotAlive && !enemy.IsAlive)
            {
                return null;
            }

            return enemy;
        }

        /// <summary>
        /// IDから敵View取得を試みる
        /// </summary>
        public bool TryGetEnemyViewById(uint id, out EnemyUnitView enemy, bool includeNotAlive = false)
        {
            enemy = GetEnemyViewById(id, includeNotAlive);
            if (enemy == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// GetUnit
        /// </summary>
        public IUnitModel GetUnit(uint id, bool includeNotAlive = false)
        {
            if (_shipModelMap.TryGetValue(id, out var ship))
            {
                if (!includeNotAlive && !ship.IsAlive)
                {
                    return null;
                }

                return ship;
            }

            if (_enemyModelMap.TryGetValue(id, out var enemy))
            {
                if (!includeNotAlive && !enemy.IsAlive)
                {
                    return null;
                }

                return enemy;
            }

            return null;
        }

        /// <summary>
        /// IdからUnit取得を試みる
        /// </summary>
        public bool TryGetUnit(uint id, out IUnitModel unit, bool includeNotAlive = false)
        {
            unit = GetUnit(id, includeNotAlive);
            if (unit == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 次のIndexのUnityを取得
        /// </summary>
        public IUnitModel GetNextIdUnit(uint currentId, bool isAdd, bool isEnemy = false)
        {
            IUnitModel[] sortedUnit = isEnemy
                ? AllAliveEnemies.OrderBy(x => x.UnitId).OfType<IUnitModel>().ToArray()
                : AllAliveShips.Where(x => !x.IsActionEnd.Value).OrderBy(x => x.UnitId).OfType<IUnitModel>().ToArray();
            if (sortedUnit.Length == 1)
            {
                return sortedUnit.First();
            }
            
            var curUnit = sortedUnit
                .Select((x, i) => (Id: x.UnitId, Index: i))
                .FirstOrDefault(x => x.Id == currentId);
            if (curUnit == default)
            {
                return sortedUnit.FirstOrDefault();
            }

            var nextUnitIndex = curUnit.Index + (isAdd ? 1 : -1);
            if (nextUnitIndex < 0)
            {
                return sortedUnit.LastOrDefault();
            }

            if (nextUnitIndex >= sortedUnit.Length)
            {
                return sortedUnit.FirstOrDefault();
            }

            return sortedUnit.Skip(nextUnitIndex).FirstOrDefault();
        }

        /// <summary>
        /// HexCellからそこにいるUnitを取得
        /// </summary>
        public IUnitModel GetUnitByHex(HexCell hexCell)
            => AllAliveUnitModels
                .FirstOrDefault(x => x.Cell.Value == hexCell);

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}