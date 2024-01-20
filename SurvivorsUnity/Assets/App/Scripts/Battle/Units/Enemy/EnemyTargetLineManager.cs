using System;
using System.Collections.Generic;
using App.AppCommon;
using App.Battle.Core;
using App.Battle.Effects;
using App.Battle.Libs;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.Units.Enemy
{
    /// <summary>
    /// 敵のターゲットライン管理
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(EnemyTargetLineManager))]
    public class EnemyTargetLineManager : MonoBehaviour
    {
        [SerializeField] private TargetLine targetLinePrefab;
        [SerializeField] private Transform layer;

        private readonly Dictionary<uint, TargetLine> _targetLineMap = new();
        private GameObjectPool<TargetLine> _targetLinePool;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger unitManger,
            BattleEventHub eventHub
        )
        {
            _targetLinePool = new GameObjectPool<TargetLine>(targetLinePrefab, layer);
            foreach (var enemy in unitManger.AllAliveEnemies)
            {
                AddEnemy(enemy);
            }
            unitManger.EnemyModelMap.ObserveAdd().Subscribe(x => AddEnemy(x.Value)).AddTo(this);
            unitManger.EnemyModelMap.ObserveRemove().Subscribe(x => RemoveEnemy(x.Key)).AddTo(this);
            
            eventHub.Subscribe<BattleEvents.OnPhaseStartAsync>(async x =>
            {
                layer.gameObject.SetActive(x.Phase == PhaseType.PlayerPhase);
            }).AddTo(this);

            _targetLinePool.OnReturn.Subscribe(x => x.Clear()).AddTo(this);
        }


        /// <summary>
        /// 敵追加
        /// </summary>
        private void AddEnemy(EnemyUnitModel enemyUnitModel)
        {
            var targetLine = _targetLinePool.Rent();
            targetLine.Setup(enemyUnitModel);
            _targetLineMap.Add(enemyUnitModel.UnitId, targetLine);
        }

        /// <summary>
        /// 敵除去
        /// </summary>
        private void RemoveEnemy(uint unitId)
        {
            if (!_targetLineMap.TryGetValue(unitId, out var targetLine))
            {
                return;
            }
            _targetLinePool.Return(targetLine);
            _targetLineMap.Remove(unitId);
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            foreach (var targetLine in _targetLineMap.Values)
            {
                targetLine.Clear();
            }
        }
    }
}