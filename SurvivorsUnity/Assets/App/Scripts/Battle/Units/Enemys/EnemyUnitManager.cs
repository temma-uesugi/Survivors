using System;
using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle.Core;
using App.Battle2.Common;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle.Map;
using App.Battle2.Map.Cells;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using App.Battle2.Utils;
using App.Battle2.ValueObjects;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.Units
{
    /// <summary>
    /// 敵管理
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(EnemyUnitManager))]
    public class EnemyUnitManager : MonoBehaviour
    {
        [SerializeField] private Transform enemeyLayer;
        [SerializeField] private EnemyUnitViewModel enemyPrefab;
        
        //敵
        private readonly Dictionary<uint, EnemyUnitView> _enemyViewMap = new();
        private readonly ReactiveDictionary<uint, EnemyUnitModel> _enemyModelMap = new();
        public IReadOnlyReactiveDictionary<uint, EnemyUnitModel> EnemyModelMap => _enemyModelMap;
        public IEnumerable<EnemyUnitModel> AllEnemies => _enemyModelMap.Values;

        private MapManager _mapManager;
        private ModelFactory _factory;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            MapManager mapManager,
            ModelFactory factory
        )
        {
            _mapManager = mapManager;
            _factory = factory;
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
            var initCell = _mapManager.GetCellByGrid(initGrid);
            var createParam = new EnemyCreateParam(unitId, enemyId, level, initCell, label);
            var enemyModel = _factory.CreateEnemy(createParam);
            var enemy = Instantiate(enemyPrefab, enemeyLayer);
            enemy.Setup(enemyModel);
            _enemyModelMap.Add(unitId, enemyModel);
            _enemyViewMap.Add(unitId, enemy.UnitView);
        }
    }
}