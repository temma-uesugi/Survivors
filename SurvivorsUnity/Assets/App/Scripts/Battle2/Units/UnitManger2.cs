using System;
using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle2.Common;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle2.Map;
using App.Battle2.Map.Cells;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using App.Battle2.Utils;
using App.Battle2.ValueObjects;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.Units
{
    /// <summary>
    /// ユニット管理
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(UnitManger2))]
    public class UnitManger2 : MonoBehaviour
    {
        [SerializeField] private Transform shipLayer;
        [SerializeField] private ShipUnitViewModel2 shipPrefab;
        [SerializeField] private Transform enemyLayer;
        [SerializeField] private EnemyUnitViewModel2 enemyPrefab;

        //船
        private readonly Dictionary<uint, ShipUnitView2> _shipViewMap = new();
        private readonly ReactiveDictionary<uint, ShipUnitModel2> _shipModelMap = new();
        public IReadOnlyReactiveDictionary<uint, ShipUnitModel2> ShipModelMap => _shipModelMap;
        public IEnumerable<ShipUnitModel2> AllAliveShips => _shipModelMap.Values.Where(x => x.IsAlive);
        private ShipUnitModel2 _currentShip;

        //敵
        private readonly Dictionary<uint, EnemyUnitView2> _enemyViewMap = new();
        private readonly ReactiveDictionary<uint, EnemyUnitModel2> _enemyModelMap = new();
        public IReadOnlyReactiveDictionary<uint, EnemyUnitModel2> EnemyModelMap => _enemyModelMap;
        public IEnumerable<EnemyUnitModel2> AllAliveEnemies => _enemyModelMap.Values.Where(x => x.IsAlive);

        public IEnumerable<IUnitModel2> AllAliveUnitModels =>
            _shipModelMap.Values.OfType<IUnitModel2>().Concat(_enemyModelMap.Values).Where(x => x.IsAlive);

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
            BattleEventHub2 eventHub2
        )
        {
            _hexMapManager = hexMapManager;
            _factory = factory;

            BattleState.Facade.SelectedShipUnit.Subscribe(SelectShip).AddTo(this);

            eventHub2.Subscribe<BattleEvents2.OnTurnStartAsync>(async _ => TurnStart()).AddTo(this);
            eventHub2.Subscribe<BattleEvents2.OnTurnEndAsync>(async _ => TurnEnd()).AddTo(this);
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
        private void SelectShip(ShipUnitModel2 shipUnitModel2)
        {
            if (_currentShip != null)
            {
                _currentShip.UnSelect();
            }

            if (shipUnitModel2 != null)
            {
                _currentShip = shipUnitModel2;
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
            var createParam = new EnemyCreateParam2(unitId, enemyId, level, initCell, label);
            var enemyModel = _factory.CreateEnemy2(createParam);
            var enemy = Instantiate(enemyPrefab, enemyLayer);
            enemy.Setup(enemyModel);
            // _objectMap.Add(unitId, enemy.UnitView);
            _enemyModelMap.Add(unitId, enemyModel);
            _enemyViewMap.Add(unitId, enemy.UnitView);
        }

        /// <summary>
        /// IDから船Model取得
        /// </summary>
        public ShipUnitModel2 GetShipModelById(uint id, bool includeNotAlive = false)
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
        public bool TryGetShipModelById(uint id, out ShipUnitModel2 ship, bool includeNotAlive = false)
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
        public ShipUnitView2 GetShipViewById(uint id, bool includeNotAlive = false)
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
        public bool TryGetShipViewById(uint id, out ShipUnitView2 ship, bool includeNotAlive = false)
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
        public EnemyUnitModel2 GetEnemyModelById(uint id, bool includeNotAlive = false)
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
        public bool TryGetEnemyModelById(uint id, out EnemyUnitModel2 enemy, bool includeNotAlive = false)
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
        public EnemyUnitView2 GetEnemyViewById(uint id, bool includeNotAlive = false)
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
        public bool TryGetEnemyViewById(uint id, out EnemyUnitView2 enemy, bool includeNotAlive = false)
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
        public IUnitModel2 GetUnit(uint id, bool includeNotAlive = false)
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
        public bool TryGetUnit(uint id, out IUnitModel2 unit, bool includeNotAlive = false)
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
        public IUnitModel2 GetNextIdUnit(uint currentId, bool isAdd, bool isEnemy = false)
        {
            IUnitModel2[] sortedUnit = isEnemy
                ? AllAliveEnemies.OrderBy(x => x.UnitId).OfType<IUnitModel2>().ToArray()
                : AllAliveShips.Where(x => !x.IsActionEnd.Value).OrderBy(x => x.UnitId).OfType<IUnitModel2>().ToArray();
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
        public IUnitModel2 GetUnitByHex(HexCell hexCell)
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