using System;
using App.Battle2.Debug;
using App.Battle2.Facades;
using App.Battle2.Map;
using App.Battle2.Objects;
using App.Battle2.Turn;
using App.Battle2.UI.BattleLog;
using App.Battle2.UI.Turn;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using Cysharp.Threading.Tasks;
using Master;
using Master.Constants;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace App.Battle2
{
    
    /// <summary>
    /// バトルサービス
    /// </summary>
    public class BattleService2 : IStartable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private HexMapManager _hexMapManager;
        private BattleCamera2 battleCamera2;
        private UnitManger2 unitManger2;
        private MapObjectManger _objectManager;
        private WindManager _windManager;
        private WeatherManager _weatherManager;
        private EnemyActiveManager _enemyActiveManager;
        private TurnViewManager _turnViewManager;
        private BattleLogManager _logManager; 
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            HexMapManager hexMapManager,
            BattleCamera2 battleCamera2,
            UnitManger2 unitManger2,
            MapObjectManger objectManager,
            WindManager windManager,
            WeatherManager weatherManager,
            EnemyActiveManager enemyActiveManager,
            TurnViewManager turnViewManager,
            BattleLogManager logManager
        )
        {
            _hexMapManager = hexMapManager;
            this.battleCamera2 = battleCamera2;
            this.unitManger2 = unitManger2;
            _objectManager = objectManager;
            _windManager = windManager;
            _weatherManager = weatherManager;
            _enemyActiveManager = enemyActiveManager; 
            _turnViewManager = turnViewManager;
            _logManager = logManager;
        }

        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            _hexMapManager.Setup(GameConst.MapWidth, GameConst.MapHeight, _objectManager);
            // _battleCamera.SetPosition(_hexMapManager.Center);
            var randomObjectCreator = new RandomObjectCreator(GameConst.MapWidth, GameConst.MapHeight);

            //船をランダム作成
            var shipParams = randomObjectCreator.GetRandomShipParam(5, true);
            foreach (var shipParam in shipParams)
            {
                unitManger2.CreateShip(shipParam.grid, DirectionType.Right, shipParam.status);
            }

            //敵をランダム作成
            foreach (var enemy in MasterData.Facade.EnemyLevelStatusTable.All)
            {
                var grid = randomObjectCreator.GetRandomGrid();
                unitManger2.CreateEnemy(grid, enemy.EnemyId, enemy.Level);
            }
            
            //障害物をランダムに
            var obstacleGrids = randomObjectCreator.GetRandomObstacleParam(10);
            foreach (var grid in obstacleGrids)
            {
                _objectManager.CreateObstacle(grid);
            }

            //ランダムに浅瀬
            var randomGrids = randomObjectCreator.GetRandomGrid(10);
            foreach (var grid in randomGrids)
            {
                _hexMapManager.SwitchSeaToLand(grid); 
            }
            
            _windManager.Setup();
            _weatherManager.Setup();
          
            BattleProgress.Facade.StartBattleAsync().Forget();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}