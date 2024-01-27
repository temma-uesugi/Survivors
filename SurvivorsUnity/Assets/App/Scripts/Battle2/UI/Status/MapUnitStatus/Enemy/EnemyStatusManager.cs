using System.Collections.Generic;
using App.Battle2.Core;
using App.Battle2.Units;
using App.Battle2.Units.Enemy;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.UI.Status.MapUnitStatus.Enemy
{
    /// <summary>
    /// 敵ステータス管理
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(EnemyStatusManager))]
    public class EnemyStatusManager : MonoBehaviour
    {
        [SerializeField] private EnemyStatusViewModel enemyStatusPrefab;

        private readonly Dictionary<uint, EnemyStatusViewModel> _enemyStatusMap = new();

        private BattleCamera2 battleCamera2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger2 unitManger2,
            BattleCamera2 battleCamera2
        )
        {
            this.battleCamera2 = battleCamera2;
            unitManger2.EnemyModelMap
                .ObserveAdd()
                .Subscribe(x => AddEnemy(x.Value))
                .AddTo(this);
            unitManger2.EnemyModelMap
                .ObserveRemove()
                .Subscribe(x => RemoveEnemy(x.Key))
                .AddTo(this);
        }

        /// <summary>
        /// 敵追加
        /// </summary>
        private void AddEnemy(EnemyUnitModel2 enemyModel2)
        {
            var status = Instantiate(enemyStatusPrefab, transform);
            status.Setup(enemyModel2, battleCamera2);
            _enemyStatusMap.Add(enemyModel2.UnitId, status);
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