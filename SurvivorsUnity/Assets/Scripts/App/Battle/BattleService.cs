﻿using System;
using App.AppCommon.Core;
using App.Battle2.Debug;
using Master.Battle.Map;
using Master.Battle.Units;
using Master.Constants;
using VContainer;
using VContainer.Unity;

namespace Master.Battle
{
    /// <summary>
    /// バトルService
    /// </summary>
    public class BattleService : IStartable, IDisposable
    {
        private MapManager _mapManager;
        private UnitManager _unitManager;
        private BattleCamera _battleCamera;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            MapManager mapManager,
            UnitManager unitManager,
            BattleCamera battleCamera
        )
        {
            _mapManager = mapManager;
            _unitManager = unitManager;
            _battleCamera = battleCamera;
        }
        
        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            _battleCamera.Setup();
            _mapManager.Setup(BattleConst.MapWidth, BattleConst.MapHeight);
         
            var randomObjectCreator = new RandomObjectCreator(BattleConst.MapWidth, BattleConst.MapHeight);
           
            //船を作成
            var shipParams = randomObjectCreator.GetRandomShipParam(5, true);
            foreach (var shipParam in shipParams)
            {
                _unitManager.CreateHero(shipParam.grid);
            }

            var a = MasterData.Facade.EnemyBaseTable.FindByEnemyId(1);
            Log.Debug(a);
            
            //敵をランダム作成
            foreach (var enemy in MasterData.Facade.EnemyLevelStatusTable.All)
            {
                var grid = randomObjectCreator.GetRandomGrid();
                _unitManager.CreateEnemy(grid, enemy.EnemyId, enemy.Level);
            }
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}