using System.Collections.Generic;
using App.Battle2.Core;
using App.Battle2.Units;
using App.Battle2.Units.Ship;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.UI.Status.MapUnitStatus.Ship
{
    /// <summary>
    /// 船ステータスマネージャ
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(ShipStatusManager))]
    public class ShipStatusManager : MonoBehaviour
    {
        [SerializeField] private ShipStatusViewModel shipStatusPrefab;

        private readonly Dictionary<uint, ShipStatusViewModel> _shipStatusMap = new();

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

            unitManger2.ShipModelMap
                .ObserveAdd()
                .Subscribe(x => AddShip(x.Value))
                .AddTo(this);
            unitManger2.ShipModelMap
                .ObserveRemove()
                .Subscribe(x => RemoveShip(x.Key))
                .AddTo(this);
        }

        /// <summary>
        /// 船追加
        /// </summary>
        private void AddShip(ShipUnitModel2 shipModel2)
        {
            var status = Instantiate(shipStatusPrefab, transform);
            status.Setup(shipModel2, battleCamera2);
            _shipStatusMap.Add(shipModel2.UnitId, status);
        }

        /// <summary>
        /// 船削除
        /// </summary>
        private void RemoveShip(uint id)
        {
            if (_shipStatusMap.TryGetValue(id, out var status))
            {
                Destroy(status);
                _shipStatusMap.Remove(id);
            }
        }
    }
}