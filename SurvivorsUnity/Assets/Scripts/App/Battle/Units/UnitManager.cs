using System.Collections.Generic;
using System.Linq;
using App.Battle2.Utils;
using App.Battle2.ValueObjects;
using Master.Battle.Core;
using Master.Battle.Map;
using Master.Battle.Units.Enemys;
using Master.Battle.Units.Heroes;
using UniRx;
using UnityEngine;
using VContainer;

namespace Master.Battle.Units
{
    /// <summary>
    /// 敵管理
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(UnitManager))]
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private Transform heroLayer;
        [SerializeField] private HeroUnitViewModel heroPrefab;
        [SerializeField] private Transform enemyLayer;
        [SerializeField] private EnemyUnitViewModel enemyPrefab;

        //味方
        private readonly Dictionary<uint, HeroUnitView> _heroViewMap = new();
        private readonly ReactiveDictionary<uint, HeroUnitModel> _heroModelMap = new();
        public IReadOnlyReactiveDictionary<uint, HeroUnitModel> HeroModelMap => _heroModelMap;
        public IEnumerable<HeroUnitModel> AllAliveHeroes => _heroModelMap.Values.Where(x => x.IsAlive);
        private HeroUnitModel _currentHero;
        
        //敵
        private readonly Dictionary<uint, EnemyUnitView> _enemyViewMap = new();
        private readonly ReactiveDictionary<uint, EnemyUnitModel> _enemyModelMap = new();
        public IReadOnlyReactiveDictionary<uint, EnemyUnitModel> EnemyModelMap => _enemyModelMap;
        public IEnumerable<EnemyUnitModel> AllEnemies => _enemyModelMap.Values;

        private MapManager _mapManager;
        private UnitModelFactory _factory;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            MapManager mapManager,
            UnitModelFactory factory
        )
        {
            _mapManager = mapManager;
            _factory = factory;
        }
      
        /// <summary>
        /// 味方作成
        /// </summary>
        public void CreateHero(
            GridValue initGrid
        )
        {
            var unitId = UnitUtil.GetUnitId();
            var label = UnitUtil.GetShipLabel();
            var index = UnitUtil.GetShipIndex();
            var initCell = _mapManager.GetCellByGrid(initGrid);
            var createParam = new HeroCreateParam(unitId, index, initCell, label);
            var heroModel = _factory.CreateHero(createParam);
            var hero = Instantiate(heroPrefab, heroLayer);
            hero.Setup(heroModel);
            _heroModelMap.Add(unitId, heroModel);
            _heroViewMap.Add(unitId, hero.UnitView);
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
            var enemy = Instantiate(enemyPrefab, enemyLayer);
            enemy.Setup(enemyModel);
            _enemyModelMap.Add(unitId, enemyModel);
            _enemyViewMap.Add(unitId, enemy.UnitView);
        }
    }
}