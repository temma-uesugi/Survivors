using App.Battle2.Units.Ship;
using UniRx;
using UnityEngine;

namespace App.Battle2.UI.Status.MapUnitStatus.Ship
{
    /// <summary>
    /// 船ステータスViewModel
    /// </summary>
    [RequireComponent(typeof(ShipStatusView))]
    public class ShipStatusViewModel :  StatusViewModelBase<ShipUnitModel2, ShipStatusView>
    {
       
        /// <summary>
        /// Setup
        /// </summary>
        public override void Setup(ShipUnitModel2 shipModel2, BattleCamera2 battleCamera2)
        {
            base.Setup(shipModel2, battleCamera2);
            View.Setup(shipModel2.Label, shipModel2.Status.ArmorPoint, shipModel2.Status.CrewPoint);
            
            shipModel2.ArmorPoint.Subscribe(View.UpdateArmor).AddTo(this);
            shipModel2.CrewPoint.Subscribe(View.UpdateCrew).AddTo(this);
            shipModel2.OnDefeated.Subscribe(_ => View.OnDefeated()).AddTo(this);
            shipModel2.NextActionTurns.Subscribe(x => View.UpdateNextActionTurns(x)).AddTo(this);
        }
    }
}