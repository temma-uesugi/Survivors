using System.Collections.Generic;
using App.AppCommon;
using App.Battle2.Core;
using App.Battle2.Effects;
using App.Battle2.Libs;
using UniRx;
using UnityEngine;
using VContainer;
using Constants;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵のターゲットライン管理
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(EnemyTargetLineManager))]
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
            UnitManger2 unitManger2,
            BattleEventHub2 eventHub2
        )
        {
            _targetLinePool = new GameObjectPool<TargetLine>(targetLinePrefab, layer);
            foreach (var enemy in unitManger2.AllAliveEnemies)
            {
                AddEnemy(enemy);
            }
            unitManger2.EnemyModelMap.ObserveAdd().Subscribe(x => AddEnemy(x.Value)).AddTo(this);
            unitManger2.EnemyModelMap.ObserveRemove().Subscribe(x => RemoveEnemy(x.Key)).AddTo(this);
            
            eventHub2.Subscribe<BattleEvents2.OnPhaseStartAsync>(async x =>
            {
                layer.gameObject.SetActive(x.Phase == PhaseType.PlayerPhase);
            }).AddTo(this);

            _targetLinePool.OnReturn.Subscribe(x => x.Clear()).AddTo(this);
        }


        /// <summary>
        /// 敵追加
        /// </summary>
        private void AddEnemy(EnemyUnitModel2 enemyUnitModel2)
        {
            var targetLine = _targetLinePool.Rent();
            targetLine.Setup(enemyUnitModel2);
            _targetLineMap.Add(enemyUnitModel2.UnitId, targetLine);
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