using Constants;
using App.AppCommon.Core;
using App.Battle2.Core;
using App.Battle2.Turn;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using Cysharp.Threading.Tasks;
using VContainer;

namespace App.Battle2.Facades
{
    /// <summary>
    /// バトル進行
    /// </summary>
    [ContainerRegisterAttribute2(typeof(BattleProgress))]
    public class BattleProgress
    {
        public static BattleProgress Facade { get; private set; }
        
        private readonly BattleEventHub2 eventHub2;
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
            BattleEventHub2 eventHub2,
            WindManager windManager,
            WeatherManager weatherManager,
            // BotLogicManager botLogicManager,
            EnemyActionManager enemyActionManager
        )
        {
            this.eventHub2 = eventHub2;
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
            await eventHub2.PublishAsync(new BattleEvents2.OnBattleStartAsync());
            await NextTurnAsync();
        }

        /// <summary>
        /// アクション終了
        /// </summary>
        public async UniTask EndActionAsync(IUnitModel2 actionUnit)
        {
            if (actionUnit is ShipUnitModel2)
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
            await eventHub2.PublishAsync(new BattleEvents2.OnPhaseEndAsync
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
            BattleCamera2.Instance.ResetFromEnemyPhaseCamera();
            await eventHub2.PublishAsync(new BattleEvents2.OnPhaseEndAsync
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
            await eventHub2.PublishAsync(new BattleEvents2.OnTurnEndAsync
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
            await eventHub2.PublishAsync(new BattleEvents2.OnPhaseStartAsync
            {
                Phase = phaseType,
            });
            _battleState.UpdatePhaseStatus(phaseType);

            if (phaseType == PhaseType.EnemyPhase)
            {
                BattleCamera2.Instance.SetToEnemyPhaseCamera();
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
            
            await eventHub2.PublishAsync(new BattleEvents2.OnTurnStartAsync
            {
            });
            
            await NextPhaseAsync(PhaseType.PlayerPhase);
        }
        
    }
}