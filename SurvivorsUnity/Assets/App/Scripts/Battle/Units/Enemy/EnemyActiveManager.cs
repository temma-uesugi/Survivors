﻿using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.AppCommon.Extensions;
using App.Battle.Core;
using App.Battle.Facades;
using App.Battle.Map;
using App.Battle.Units.Ship;
using App.Master;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;

namespace App.Battle.Units.Enemy
{
    /// <summary>
    /// 敵のアクティブ管理
    /// </summary>
    [ContainerRegister(typeof(EnemyActiveManager))]
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
            public ShipUnitModel TargetShipModel { get; private set; }
            public bool IsActive => TargetShipModel != null;

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
            public void SetTarget(ShipUnitModel targetShipModel)
            {
                TargetShipModel = targetShipModel;
            }
        }
        
        private readonly CompositeDisposable _disposable = new();
        private readonly UnitManger _unitManger;

        private readonly Dictionary<uint, (EnemyUnitModel model, ActiveCondition status)> _dic = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public EnemyActiveManager(
            UnitManger unitManger,
            BattleEventHub eventHub
        )
        {
            _unitManger = unitManger;
            foreach (var enemy in unitManger.AllAliveEnemies)
            {
                AddEnemy(enemy);
            }
            unitManger.EnemyModelMap.ObserveAdd().Subscribe(x => AddEnemy(x.Value)).AddTo(_disposable);
            unitManger.EnemyModelMap.ObserveRemove().Subscribe(x => RemoveEnemy(x.Key)) .AddTo(_disposable);
            
            eventHub.Subscribe<BattleEvents.OnTurnStartAsync>(async _ => await OnTurnStartAsync()).AddTo(_disposable);
            eventHub.Subscribe<BattleEvents.OnTurnEndAsync>(async _ => await OnTurnEndAsync()).AddTo(_disposable);
            BattleState.Facade.OnShipMoved.Subscribe(OnShipMoved).AddTo(_disposable);
            BattleState.Facade.TurnWeather.Skip(1).Subscribe(x => OnWeatherChanged(x.Weather)).AddTo(_disposable);
        }

        /// <summary>
        /// 敵追加
        /// </summary>
        private void AddEnemy(EnemyUnitModel enemyUnitModel)
        {
            var enemyMaster = MasterData.Facade.EnemyBaseTable.FindByEnemyId(enemyUnitModel.EnemyId);
            var inactiveCondType = enemyMaster.InactiveConditionType;
            var inactiveCondValue = enemyMaster.InactiveConditionValue;
            if (inactiveCondType == EnemyInactiveConditionType.Invert)
            {
                //逆の場合
                inactiveCondType = enemyMaster.ActiveConditionType switch
                {
                    EnemyActiveConditionType.ProgressTurn => EnemyInactiveConditionType.ProgressTurn,
                    EnemyActiveConditionType.InRange => EnemyInactiveConditionType.OutRange,
                    EnemyActiveConditionType.InAttackRange => EnemyInactiveConditionType.OutAttackRange,
                    _ => EnemyInactiveConditionType.None,
                };
                inactiveCondValue = enemyMaster.ActiveConditionValue;
            }

            var activeCondition = new ActiveCondition
            {
                ActiveCondType = enemyMaster.ActiveConditionType,
                ActiveCondValue = enemyMaster.ActiveConditionValue,
                InactiveCondType = inactiveCondType,
                InactiveCondValue = inactiveCondValue,
            };
           
            _dic.Add(enemyUnitModel.UnitId, (enemyUnitModel, activeCondition));
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
                (bool isActive, ShipUnitModel target) = value.status.ActiveCondType switch
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
        private void OnShipMoved(ShipUnitModel shipUnitModel)
        {
            foreach (var value in _dic.Values)
            {
                if (value.status.IsActive)
                {
                    //アクティブ => 非アクティブチェック
                    //ターゲットでなければ何もしない
                    if (shipUnitModel != value.status.TargetShipModel) continue;
                    var isInactive = value.status.ActiveCondType switch
                    {
                        EnemyActiveConditionType.InRange => CheckOutRange(value.model, value.status.TargetShipModel, value.status.InactiveCondValue),
                        EnemyActiveConditionType.InAttackRange => CheckOutAttackRange(value.model, value.status.TargetShipModel),
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
                        EnemyActiveConditionType.InRange => CheckInRange(value.model, shipUnitModel, value.status.ActiveCondValue),
                        EnemyActiveConditionType.InAttackRange => CheckInAttackRange(value.model, shipUnitModel),
                        _ => false,
                    };
                    if (isActive)
                    {
                        value.status.SetTarget(shipUnitModel);
                        value.model.SetTarget(shipUnitModel);
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
        private ShipUnitModel GetClosestShip(EnemyUnitModel enemyUnitModel)
        {
            return _unitManger.AllAliveShips
                .RandomSort()
                .OrderBy(x => MapRoutSearch.HeuristicDistance(enemyUnitModel.Cell.Value, x.Cell.Value))
                .First();
        }

        /// <summary>
        /// 範囲内に敵
        /// </summary>
        private bool CheckInRange(EnemyUnitModel enemyUnitModel, ShipUnitModel ship, int range)
            => HexUtil.InRange(enemyUnitModel.Cell.Value, ship.Cell.Value, range);

        /// <summary>
        /// 攻撃範囲内に敵
        /// </summary>
        private bool CheckInAttackRange(EnemyUnitModel enemyUnitModel, ShipUnitModel ship) =>
            CheckInRange(enemyUnitModel, ship, enemyUnitModel.AttackRange);

        
        /// <summary>
        /// 範囲内に敵
        /// </summary>
        private (bool, ShipUnitModel) CheckInRange(EnemyUnitModel enemyUnitModel, int range)
        {
            var ship = GetClosestShip(enemyUnitModel);
            var inRange = CheckInRange(enemyUnitModel, ship, range);
            return (inRange, ship);
        }

        /// <summary>
        /// 攻撃範囲内に敵
        /// </summary>
        private (bool, ShipUnitModel) CheckInAttackRange(EnemyUnitModel enemyUnitModel) =>
            CheckInRange(enemyUnitModel, enemyUnitModel.AttackRange);

        /// <summary>
        /// 範囲外かのチェック
        /// </summary>
        private bool CheckOutRange(EnemyUnitModel enemyUnitModel, ShipUnitModel targetUnitModel, int range)
            => !CheckInRange(enemyUnitModel, targetUnitModel, range);

        /// <summary>
        /// 攻撃範囲外かのチェック
        /// </summary>
        private bool CheckOutAttackRange(EnemyUnitModel enemyUnitModel, ShipUnitModel targetUnitModel) =>
            CheckOutRange(enemyUnitModel, targetUnitModel, enemyUnitModel.AttackRange);
        
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