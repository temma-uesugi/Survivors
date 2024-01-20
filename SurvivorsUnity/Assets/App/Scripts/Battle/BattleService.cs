using System;
using App.AppCommon.Core;
using App.Battle.Map;
using VContainer;
using VContainer.Unity;

namespace App.Battle
{
    /// <summary>
    /// バトルService
    /// </summary>
    public class BattleService : IStartable, IDisposable
    {
        private MapManager _mapManager;
        private BattleCamera _battleCamera;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            MapManager mapManager,
            BattleCamera battleCamera
        )
        {
            _mapManager = mapManager;
            _battleCamera = battleCamera;
        }
        
        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            _mapManager.Setup(BattleConst.MapWidth, BattleConst.MapHeight);
        }
        
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
        }
    }
}