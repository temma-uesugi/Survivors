using App.AppCommon;
using Master.Constants;
using UniRx;
using UnityEngine;

namespace App.Battle2.Units.Ship
{
    /// <summary>
    /// ShipUnitのViewModel
    /// </summary>
    [RequireComponent(typeof(ShipUnitView2))]
    public class ShipUnitViewModel2 : MonoBehaviour, IUnitViewModel2<ShipUnitView2, ShipUnitModel2>
    {
        
        public ShipUnitView2 UnitView { get; private set; }
        public ShipUnitModel2 UnitModel { get; private set; }

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            UnitView = GetComponent<ShipUnitView2>();
        }
        
        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(ShipUnitModel2 unitModel2)
        {
            UnitModel = unitModel2;
            UnitView.Init(unitModel2.UnitId, unitModel2.Cell.Value, null);
            unitModel2.Cell.Subscribe(UnitView.SetToCell).AddTo(this);
            unitModel2.Direction.DistinctUntilChanged().Subscribe(UnitView.SetDirection).AddTo(this);
            unitModel2.IsActionEnd.DistinctUntilChanged().Subscribe(isEnd =>
            {
                UnitView.SetActive(!isEnd);
            }).AddTo(this);
            
            UnitView.BombRange.Setup(leftStatus: unitModel2.LeftBombStatus.Value, rightStatus: unitModel2.RightBombStatus.Value);
            unitModel2.IsSelected.Subscribe(UnitView.BombRange.SetActive).AddTo(this);
            unitModel2.IsBombardRangeActive.Subscribe(UnitView.BombRange.SetActive).AddTo(this);
            unitModel2.LeftBombStatus.Subscribe(x => UnitView.BombRange.UpdateStatus(BombSide.Left, x)).AddTo(this);
            unitModel2.RightBombStatus.Subscribe(x => UnitView.BombRange.UpdateStatus(BombSide.Right, x)).AddTo(this);
            UnitModel.OnDefeated.Subscribe(_ => UnitView.OnDefeated()).AddTo(this);
        }
    }
}