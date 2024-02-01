using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.AppCommon.Extensions;
using App.Battle2.Core;
using App.Battle2.Facades;
using App.Battle2.Map;
using App.Battle2.Units.Ship;
using Cysharp.Threading.Tasks;
using Master;
using UniRx;
using VContainer;
using Constants;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵のアクティブ管理
    /// </summary>
    [ContainerRegisterAttribute2(typeof(EnemyActiveManager))]
    public class EnemyActiveManager
    {
        /// <summary>
        /// 活性化条件
        /// </summary>
        private class ActiveCondition
        {
            public EnemyActiveConditionType ActiveCondType { get; init; }
            public int ActiveCondValue { get; init; }
            public EnemyInactiveConditionType InactiveCondType { get; init; }
            public int InactiveCondValue { get; init; }
            public int CurrentCondValue { get; private set; } = 0;
            public ShipUnitModel2 TargetShipModel2 { get; private set; }
            public bool IsActive => TargetShipModel2 != null;

            /// <summary>
            /// カウント追加
            /// </summary>
            public bool AddCount(int add = 1)
            {
                CurrentCondValue += 1;
                return CurrentCondValue >= (IsActive ? InactiveCondValue : ActiveCondValue);
            }

            /// <summary>
            /// ターゲットのセット
            /// </summary>
            public void SetTarget(ShipUnitModel2 targetShipModel2)
            {
                TargetShipModel2 = targetShipModel2;
            }
        }
        
        private readonly CompositeDisposable _disposable = new();
        private readonly UnitManger2 unitManger2;

        private readonly Dictionary<uint, (EnemyUnitModel2 model, ActiveCondition status)> _dic = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public EnemyActiveManager(
            UnitManger2 unitManger2,
            BattleEventHub2 eventHub2
        )
        {
            this.unitManger2 = unitManger2;
            foreach (var enemy in unitManger2.AllAliveEnemies)
            {
                AddEnemy(enemy);
            }
            unitManger2.EnemyModelMap.ObserveAdd().Subscribe(x => AddEnemy(x.Value)).AddTo(_disposable);
            unitManger2.EnemyModelMap.ObserveRemove().Subscribe(x => RemoveEnemy(x.Key)) .AddTo(_disposable);
            
            eventHub2.Subscribe<BattleEvents2.OnTurnStartAsync>(async _ => await OnTurnStartAsync()).AddTo(_disposable);
            eventHub2.Subscribe<BattleEvents2.OnTurnEndAsync>(async _ => await OnTurnEndAsync()).AddTo(_disposable);
            BattleState.Facade.OnShipMoved.Subscribe(OnShipMoved).AddTo(_disposable);
            BattleState.Facade.TurnWeather.Skip(1).Subscribe(x => OnWeatherChanged(x.Weather)).AddTo(_disposable);
        }

        /// <summary>
        /// 敵追加
        /// </summary>
        private void AddEnemy(EnemyUnitModel2 enemyUnitModel2)
        {
            // var enemyMaster = MasterData.Facade.EnemyBaseTable.FindByEnemyId(enemyUnitModel2.EnemyId);
            // var enemyMaster = null;
            // var inactiveCondType = enemyMaster.InactiveConditionType;
            // var inactiveCondValue = enemyMaster.InactiveConditionValue;
            // if (inactiveCondType == EnemyInactiveConditionType.Invert)
            // {
            //     //逆の場合
            //     inactiveCondType = enemyMaster.ActiveConditionType switch
            //     {
            //         EnemyActiveConditionType.ProgressTurn => EnemyInactiveConditionType.ProgressTurn,
            //         EnemyActiveConditionType.InRange => EnemyInactiveConditionType.OutRange,
            //         EnemyActiveConditionType.InAttackRange => EnemyInactiveConditionType.OutAttackRange,
            //         _ => EnemyInactiveConditionType.None,
            //     };
            //     inactiveCondValue = enemyMaster.ActiveConditionValue;
            // }

            var activeCondition = new ActiveCondition
            {
                // ActiveCondType = enemyMaster.ActiveConditionType,
                // ActiveCondValue = enemyMaster.ActiveConditionValue,
                // InactiveCondType = inactiveCondType,
                // InactiveCondValue = inactiveCondValue,
            };
           
            _dic.Add(enemyUnitModel2.UnitId, (enemyUnitModel2, activeCondition));
        }

        /// <summary>
        /// 敵除去
        /// </summary>
        private void RemoveEnemy(uint unitId)
        {
            _dic.Remove(unitId);
        }
        
        /// <summary>
        /// ターン開始
        /// </summary>
        private async UniTask OnTurnStartAsync()
        {
            foreach (var value in _dic.Values)
            {
                if (value.status.IsActive) continue;
                (bool isActive, ShipUnitModel2 target) = value.status.ActiveCondType switch
                {
                    EnemyActiveConditionType.None => (true, GetClosestShip(value.model)),
                    EnemyActiveConditionType.ProgressTurn => (value.status.CurrentCondValue >= value.status.ActiveCondValue, GetClosestShip(value.model)), 
                    _ => (false, null),
                };
                if (isActive)
                {
                    value.status.SetTarget(target);
                    value.model.SetTarget(target);
                }
            }
        }
        
        /// <summary>
        /// ターン終了
        /// </summary>
        private async UniTask OnTurnEndAsync()
        {
            foreach (var value in _dic.Values)
            {
                if (!value.status.IsActive) continue;
                bool isInactive = value.status.InactiveCondType switch
                {
                    EnemyInactiveConditionType.ProgressTurn => value.status.AddCount(),
                    _ => false,
                };
                if (isInactive)
                {
                    value.status.SetTarget(null);
                    value.model.SetTarget(null);
                }
            }
        }

        /// <summary>
        /// 船の移動
        /// </summary>
        private void OnShipMoved(ShipUnitModel2 shipUnitModel2)
        {
            foreach (var value in _dic.Values)
            {
                if (value.status.IsActive)
                {
                    //アクティブ => 非アクティブチェック
                    //ターゲットでなければ何もしない
                    if (shipUnitModel2 != value.status.TargetShipModel2) continue;
                    var isInactive = value.status.ActiveCondType switch
                    {
                        EnemyActiveConditionType.InRange => CheckOutRange(value.model, value.status.TargetShipModel2, value.status.InactiveCondValue),
                        EnemyActiveConditionType.InAttackRange => CheckOutAttackRange(value.model, value.status.TargetShipModel2),
                        _ => false,
                    };
                    if (isInactive)
                    {
                        value.status.SetTarget(null);
                        value.model.SetTarget(null);
                    }
                }
                else
                {
                    //非アクティブ => アクティブチェック
                    bool isActive = value.status.ActiveCondType switch
                    {
                        EnemyActiveConditionType.InRange => CheckInRange(value.model, shipUnitModel2, value.status.ActiveCondValue),
                        EnemyActiveConditionType.InAttackRange => CheckInAttackRange(value.model, shipUnitModel2),
                        _ => false,
                    };
                    if (isActive)
                    {
                        value.status.SetTarget(shipUnitModel2);
                        value.model.SetTarget(shipUnitModel2);
                    }
                }
            }
        }

        /// <summary>
        /// 天気の変更
        /// </summary>
        private void OnWeatherChanged(WeatherType weatherType)
        {
            foreach (var value in _dic.Values)
            {
                if (value.status.IsActive)
                {
                    //アクティブ => 非アクティブチェック
                    if (value.status.InactiveCondType == EnemyInactiveConditionType.ByWeather &&
                        !CheckWeather(weatherType, value.status.InactiveCondValue))
                    {
                        value.status.SetTarget(null);
                        value.model.SetTarget(null);
                    }
                }
                else
                {
                    //非アクティブ => アクティブチェック
                    if (value.status.ActiveCondType == EnemyActiveConditionType.ByWeather &&
                        CheckWeather(weatherType, value.status.InactiveCondValue))
                    {
                        var target = GetClosestShip(value.model);
                        value.status.SetTarget(target);
                        value.model.SetTarget(target);
                    }
                }
            }
        }
        
        /// <summary>
        /// 一番近い船を取得
        /// </summary>
        private ShipUnitModel2 GetClosestShip(EnemyUnitModel2 enemyUnitModel2)
        {
            return unitManger2.AllAliveShips
                .RandomSort()
                .OrderBy(x => MapRoutSearch.HeuristicDistance(enemyUnitModel2.Cell.Value, x.Cell.Value))
                .First();
        }

        /// <summary>
        /// 範囲内に敵
        /// </summary>
        private bool CheckInRange(EnemyUnitModel2 enemyUnitModel2, ShipUnitModel2 ship, int range)
            => HexUtil2.InRange(enemyUnitModel2.Cell.Value, ship.Cell.Value, range);

        /// <summary>
        /// 攻撃範囲内に敵
        /// </summary>
        private bool CheckInAttackRange(EnemyUnitModel2 enemyUnitModel2, ShipUnitModel2 ship) =>
            CheckInRange(enemyUnitModel2, ship, enemyUnitModel2.AttackRange);

        
        /// <summary>
        /// 範囲内に敵
        /// </summary>
        private (bool, ShipUnitModel2) CheckInRange(EnemyUnitModel2 enemyUnitModel2, int range)
        {
            var ship = GetClosestShip(enemyUnitModel2);
            var inRange = CheckInRange(enemyUnitModel2, ship, range);
            return (inRange, ship);
        }

        /// <summary>
        /// 攻撃範囲内に敵
        /// </summary>
        private (bool, ShipUnitModel2) CheckInAttackRange(EnemyUnitModel2 enemyUnitModel2) =>
            CheckInRange(enemyUnitModel2, enemyUnitModel2.AttackRange);

        /// <summary>
        /// 範囲外かのチェック
        /// </summary>
        private bool CheckOutRange(EnemyUnitModel2 enemyUnitModel2, ShipUnitModel2 targetUnitModel2, int range)
            => !CheckInRange(enemyUnitModel2, targetUnitModel2, range);

        /// <summary>
        /// 攻撃範囲外かのチェック
        /// </summary>
        private bool CheckOutAttackRange(EnemyUnitModel2 enemyUnitModel2, ShipUnitModel2 targetUnitModel2) =>
            CheckOutRange(enemyUnitModel2, targetUnitModel2, enemyUnitModel2.AttackRange);
        
        /// <summary>
        /// 天気チェック
        /// </summary>
        private bool CheckWeather(WeatherType weatherType, int conditionValue)
        {
            return weatherType switch
            {
                WeatherType.Sun => (conditionValue & (int)WeatherType.Sun) != 0,
                WeatherType.Rain => (conditionValue & (int)WeatherType.Rain) != 0,
                WeatherType.Cloud => (conditionValue & (int)WeatherType.Cloud) != 0,
                WeatherType.Fog => (conditionValue & (int)WeatherType.Fog) != 0,
                WeatherType.Storm => (conditionValue & (int)WeatherType.Storm) != 0,
                WeatherType.Thunderstorm => (conditionValue & (int)WeatherType.Thunderstorm) != 0,
                _ => false,
            };
        }
    }
}