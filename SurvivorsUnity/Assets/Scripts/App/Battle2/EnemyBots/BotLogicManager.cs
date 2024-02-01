using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.EnemyBots.NodeObjects;
using App.Battle2.Facades;
using App.Battle2.Map;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Master.Constants;
using UniRx;
using VContainer;

namespace App.Battle2.EnemyBots
{
    /// <summary>
    /// BotLogic管理
    /// </summary>
    [ContainerRegisterAttribute2(typeof(BotLogicManager))]
    public class BotLogicManager
    {
        private readonly CompositeDisposable _disposable = new();
        
        private readonly HexMapManager _mapManager;
        private readonly UnitManger2 unitManger2;
        private readonly BattleEventHub2 eventHub2;
       
        private readonly ConditionChecker _conditionChecker;
        private readonly ActionExecutor _actionExecutor;

        private readonly Dictionary<uint, BotNodeObject.BotNodeRoot> _botNodeMap = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public BotLogicManager(
            HexMapManager mapManager,
            UnitManger2 unitManger2,
            BattleEventHub2 eventHub2
        )
        {
            _mapManager = mapManager;
            this.unitManger2 = unitManger2;
            this.eventHub2 = eventHub2;
            _conditionChecker = new ConditionChecker(mapManager, unitManger2);
            _actionExecutor = new ActionExecutor(mapManager, unitManger2);

            foreach (var enemy in this.unitManger2.AllAliveEnemies)
            {
                AddEnemy(enemy);
            }

            SetSubscribe();
        }

        /// <summary>
        /// 購読登録
        /// </summary>
        private void SetSubscribe()
        {
            unitManger2.EnemyModelMap.ObserveAdd().Subscribe(x => AddEnemy(x.Value)).AddTo(_disposable);
        }
        
        /// <summary>
        /// 敵追加
        /// </summary>
        private void AddEnemy(EnemyUnitModel2 enemyModel2)
        {
            if (_botNodeMap.ContainsKey(enemyModel2.UnitId))
            {
                return;
            }
            if (!BotNodeHelper.TryGetBotNodeRoot("test", out var root))
            {
                return;     
            }
            _botNodeMap.Add(enemyModel2.UnitId, root); 
        }
        
        /// <summary>
        /// 複数Botの行動
        /// </summary>
        public async UniTask BotsActionAsync()
        {
            await foreach (var enemy in unitManger2.AllAliveEnemies.Where(x => x.IsActionable).ToUniTaskAsyncEnumerable())
            {
                if (enemy == null)
                {
                    continue;
                }
                await BotActionAsync(enemy);
            }
            BattleProgress.Facade.EndActionAsync(default).Forget();
        }

        /// <summary>
        /// Botの行動
        /// </summary>
        private async UniTask BotActionAsync(EnemyUnitModel2 enemy)
        {
            var enemyId = enemy.UnitId;
            var actionId = GameConst.InvalidUnitId;
            if (unitManger2.TryGetEnemyModelById(enemyId, out var enemyModel) &&
                _botNodeMap.TryGetValue(enemyId, out var root))
            {
                actionId = enemyId;
                await StartNodeBehaveAsync(root, enemyModel);
            }
            
            eventHub2.Publish(new BattleEvents2.OnEnemyActionEndEvent
            {
                UnitId = actionId,
            });
            enemy.EndAction();
        }
        
        /// <summary>
        /// BotNodeの振るまい開始
        /// </summary>
        private async UniTask StartNodeBehaveAsync(BotNodeObject.BotNodeRoot root, EnemyUnitModel2 enemyModel2)
        {
            //カメラフォーカス
            // await BattleCamera.Instance.MoveToByAnimationAsync(enemyModel.Position);

            foreach (var node in root.Nodes)
            {
                if (!_conditionChecker.Check(node.Conditions, enemyModel2))
                {
                    continue; 
                }
                if (await _actionExecutor.ActionAsync(node.Action, enemyModel2))
                {
                    //アクション実行
                    break;
                }
            }
        }
    }
}