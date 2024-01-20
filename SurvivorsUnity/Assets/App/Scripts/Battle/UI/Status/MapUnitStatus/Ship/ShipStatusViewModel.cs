using App.Battle.Units.Ship;
using UniRx;
using UnityEngine;

namespace App.Battle.UI.Status
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
        public override void Setup(ShipUnitModel shipModel, BattleCamera battleCamera)
        {
            base.Setup(shipModel, battleCamera);
            View.Setup(shipModel.Label, shipModel.Status.ArmorPoint, shipModel.Status.CrewPoint);
            
            shipModel.ArmorPoint.Subscribe(View.UpdateArmor).AddTo(this);
            shipModel.CrewPoint.Subscribe(View.UpdateCrew).AddTo(this);
            shipModel.OnDefeated.Subscribe(_ => View.OnDefeated()).AddTo(this);
            shipModel.NextActionTurns.Subscribe(x => View.UpdateNextActionTurns(x)).AddTo(this);
        }
    }
}