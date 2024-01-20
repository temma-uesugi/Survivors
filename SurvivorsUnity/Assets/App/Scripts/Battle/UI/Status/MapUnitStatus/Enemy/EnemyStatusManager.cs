using System.Collections.Generic;
using App.Battle.Core;
using App.Battle.Units;
using App.Battle.Units.Enemy;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.UI.Status
{
    /// <summary>
    /// 敵ステータス管理
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(EnemyStatusManager))]
    public class EnemyStatusManager : MonoBehaviour
    {
        [SerializeField] private EnemyStatusViewModel enemyStatusPrefab;

        private readonly Dictionary<uint, EnemyStatusViewModel> _enemyStatusMap = new();

        private BattleCamera _battleCamera;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger unitManger,
            BattleCamera battleCamera
        )
        {
            _battleCamera = battleCamera;
            unitManger.EnemyModelMap
                .ObserveAdd()
                .Subscribe(x => AddEnemy(x.Value))
                .AddTo(this);
            unitManger.EnemyModelMap
                .ObserveRemove()
                .Subscribe(x => RemoveEnemy(x.Key))
                .AddTo(this);
        }

        /// <summary>
        /// 敵追加
        /// </summary>
        private void AddEnemy(EnemyUnitModel enemyModel)
        {
            var status = Instantiate(enemyStatusPrefab, transform);
            status.Setup(enemyModel, _battleCamera);
            _enemyStatusMap.Add(enemyModel.UnitId, status);
        }

        /// <summary>
        /// 敵削除
        /// </summary>
        private void RemoveEnemy(uint id)
        {
            if (_enemyStatusMap.TryGetValue(id, out var status))
            {
                Destroy(status);
                _enemyStatusMap.Remove(id);
            }
        }
    }
}