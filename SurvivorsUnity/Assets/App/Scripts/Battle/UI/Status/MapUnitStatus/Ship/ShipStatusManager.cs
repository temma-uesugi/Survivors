using System.Collections.Generic;
using App.Battle.Core;
using App.Battle.Units;
using App.Battle.Units.Ship;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.UI.Status
{
    /// <summary>
    /// 船ステータスマネージャ
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(ShipStatusManager))]
    public class ShipStatusManager : MonoBehaviour
    {
        [SerializeField] private ShipStatusViewModel shipStatusPrefab;

        private readonly Dictionary<uint, ShipStatusViewModel> _shipStatusMap = new();

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

            unitManger.ShipModelMap
                .ObserveAdd()
                .Subscribe(x => AddShip(x.Value))
                .AddTo(this);
            unitManger.ShipModelMap
                .ObserveRemove()
                .Subscribe(x => RemoveShip(x.Key))
                .AddTo(this);
        }

        /// <summary>
        /// 船追加
        /// </summary>
        private void AddShip(ShipUnitModel shipModel)
        {
            var status = Instantiate(shipStatusPrefab, transform);
            status.Setup(shipModel, _battleCamera);
            _shipStatusMap.Add(shipModel.UnitId, status);
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