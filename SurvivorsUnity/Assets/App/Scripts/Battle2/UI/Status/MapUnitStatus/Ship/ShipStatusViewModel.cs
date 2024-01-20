using App.Battle2.Units.Ship;
using UniRx;
using UnityEngine;

namespace App.Battle2.UI.Status.MapUnitStatus.Ship
{
    /// <summary>
    /// 船ステータスViewModel
    /// </summary>
    [RequireComponent(typeof(ShipStatusView))]
    public class ShipStatusViewModel :  StatusViewModelBase<ShipUnitModel, ShipStatusView>
    {
       
        /// <summary>
        /// Setup
        /// </summary>
        public override void Setup(ShipUnitModel shipModel, BattleCamera2 battleCamera2)
        {
            base.Setup(shipModel, battleCamera2);
            View.Setup(shipModel.Label, shipModel.Status.ArmorPoint, shipModel.Status.CrewPoint);
            
            shipModel.ArmorPoint.Subscribe(View.UpdateArmor).AddTo(this);
            shipModel.CrewPoint.Subscribe(View.UpdateCrew).AddTo(this);
            shipModel.OnDefeated.Subscribe(_ => View.OnDefeated()).AddTo(this);
            shipModel.NextActionTurns.Subscribe(x => View.UpdateNextActionTurns(x)).AddTo(this);
        }
    }
}