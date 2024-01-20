using System;
using App.AppCommon;
using App.AppCommon.Core;
using App.Battle.Core;
using App.Battle.EnemyBots;
using App.Battle.Turn;
using App.Battle.Units;
using App.Battle.Units.Enemy;
using App.Battle.Units.Ship;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;

namespace App.Battle.Facades
{
    /// <summary>
    /// バトル進行
    /// </summary>
    [ContainerRegister(typeof(BattleProgress))]
    public class BattleProgress
    {
        public static BattleProgress Facade { get; private set; }
        
        private readonly BattleEventHub _eventHub;
        private readonly WindManager _windManager;
        private readonly WeatherManager _weatherManager;
        private readonly BattleState _battleState;
        // private readonly BotLogicManager _botLogicManager;
        private readonly EnemyActionManager _enemyActionManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public BattleProgress(
            BattleEventHub eventHub,
            WindManager windManager,
            WeatherManager weatherManager,
            // BotLogicManager botLogicManager,
            EnemyActionManager enemyActionManager
        )
        {
            _eventHub = eventHub;
            _windManager = windManager;
            _weatherManager = weatherManager;
            _battleState = BattleState.Facade;
            // _botLogicManager = botLogicManager;
            _enemyActionManager = enemyActionManager;

            Facade = this;
        }

        /// <summary>
        /// バトル開始
        /// </summary>
        public async UniTask StartBattleAsync()
        {
            await _eventHub.PublishAsync(new BattleEvents.OnBattleStartAsync());
            await NextTurnAsync();
        }

        /// <summary>
        /// アクション終了
        /// </summary>
        public async UniTask EndActionAsync(IUnitModel actionUnit)
        {
            if (actionUnit is ShipUnitModel)
            {
                await EndPlayerPhaseAsync();
            }
            else //TODO
            {
                //TODO
                await EndEnemyPhaseAsync();
            }
        }

        /// <summary>
        /// フェイズパス
        /// </summary>
        public async UniTask PassPhase()
        {
            if (_battleState.Phase == PhaseType.PlayerPhase)
            {
                await EndPlayerPhaseAsync();
            }
            else if (_battleState.Phase == PhaseType.EnemyPhase)
            {
                await EndEnemyPhaseAsync();
            }
        }
        
        /// <summary>
        /// 味方フェイズ終了
        /// </summary>
        private async UniTask EndPlayerPhaseAsync()
        {
            await _eventHub.PublishAsync(new BattleEvents.OnPhaseEndAsync
            {
                Phase = PhaseType.PlayerPhase,
            });
            await NextPhaseAsync(PhaseType.EnemyPhase);
        }

        /// <summary>
        /// 敵フェイズ終了
        /// </summary>
        private async UniTask EndEnemyPhaseAsync()
        {
            BattleCamera.Instance.ResetFromEnemyPhaseCamera();
            await _eventHub.PublishAsync(new BattleEvents.OnPhaseEndAsync
            {
                Phase = PhaseType.EnemyPhase,
            });
            await EndTurnAsync();
        }

        /// <summary>
        /// ターン終了
        /// </summary>
        private async UniTask EndTurnAsync()
        {
            await _eventHub.PublishAsync(new BattleEvents.OnTurnEndAsync
            {
            });
            _battleState.EndTurn();
            await NextTurnAsync();
        }
        
        /// <summary>
        /// 次Phase
        /// </summary>
        private async UniTask NextPhaseAsync(PhaseType phaseType)
        {
            Log.Debug("NextPhaseAsync", phaseType.ToString());
            await _eventHub.PublishAsync(new BattleEvents.OnPhaseStartAsync
            {
                Phase = phaseType,
            });
            _battleState.UpdatePhaseStatus(phaseType);

            if (phaseType == PhaseType.EnemyPhase)
            {
                BattleCamera.Instance.SetToEnemyPhaseCamera();
            }

            if (phaseType == PhaseType.EnemyPhase)
            {
                Log.Debug("BotsActionAsync before");
                // await _botLogicManager.BotsActionAsync();
                await _enemyActionManager.StartEnemyActionAsync();
                Log.Debug("BotsActionAsync after");
                await EndActionAsync(default);
            }
        }
        
        /// <summary>
        /// 次のターン
        /// </summary>
        private async UniTask NextTurnAsync()
        {
            var wind = _windManager.Proceed();
            var weather = _weatherManager.Proceed();
           
            _battleState.UpdateTurnStatus(wind, weather);
            
            await _eventHub.PublishAsync(new BattleEvents.OnTurnStartAsync
            {
            });
            
            await NextPhaseAsync(PhaseType.PlayerPhase);
        }
        
    }
}