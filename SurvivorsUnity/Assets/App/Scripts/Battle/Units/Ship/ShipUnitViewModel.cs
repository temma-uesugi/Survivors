using App.AppCommon;
using UniRx;
using UnityEngine;

namespace App.Battle.Units.Ship
{
    /// <summary>
    /// ShipUnitのViewModel
    /// </summary>
    [RequireComponent(typeof(ShipUnitView))]
    public class ShipUnitViewModel : MonoBehaviour, IUnitViewModel<ShipUnitView, ShipUnitModel>
    {
        
        public ShipUnitView UnitView { get; private set; }
        public ShipUnitModel UnitModel { get; private set; }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            UnitView = GetComponent<ShipUnitView>();
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(ShipUnitModel unitModel)
        {
            UnitModel = unitModel;
            UnitView.Init(unitModel.UnitId, unitModel.Cell.Value, null);
            unitModel.Cell.Subscribe(UnitView.SetToCell).AddTo(this);
            unitModel.Direction.DistinctUntilChanged().Subscribe(UnitView.SetDirection).AddTo(this);
            unitModel.IsActionEnd.DistinctUntilChanged().Subscribe(isEnd =>
            {
                UnitView.SetActive(!isEnd);
            }).AddTo(this);
            
            UnitView.BombRange.Setup(leftStatus: unitModel.LeftBombStatus.Value, rightStatus: unitModel.RightBombStatus.Value);
            unitModel.IsSelected.Subscribe(UnitView.BombRange.SetActive).AddTo(this);
            unitModel.IsBombardRangeActive.Subscribe(UnitView.BombRange.SetActive).AddTo(this);
            unitModel.LeftBombStatus.Subscribe(x => UnitView.BombRange.UpdateStatus(BombSide.Left, x)).AddTo(this);
            unitModel.RightBombStatus.Subscribe(x => UnitView.BombRange.UpdateStatus(BombSide.Right, x)).AddTo(this);
            UnitModel.OnDefeated.Subscribe(_ => UnitView.OnDefeated()).AddTo(this);
        }
    }
}