using App.Battle.Units.Enemy;
using UniRx;
using UnityEngine;

namespace App.Battle.UI.Status
{
    /// <summary>
    /// 敵ステータスViewModel
    /// </summary>
    [RequireComponent(typeof(EnemyStatusView))]
    public class EnemyStatusViewModel : StatusViewModelBase<EnemyUnitModel, EnemyStatusView>
    {
        /// <summary>
        /// Setup
        /// </summary>
        public override void Setup(EnemyUnitModel enemyModel, BattleCamera battleCamera)
        {
            base.Setup(enemyModel, battleCamera); 
            View.Setup(enemyModel.Label, enemyModel.Hp.Current, enemyModel.Hp.Max);

            enemyModel.Hp.OnUpdate.Subscribe(View.UpdateHp).AddTo(this);
            enemyModel.OnDefeated.Subscribe(_ => View.OnDefeated()).AddTo(this);
            enemyModel.NextActionTurns.Subscribe(View.UpdateNextActionTurns).AddTo(this);
            enemyModel.IsActive.Subscribe(View.SetActiveNextActionTurns).AddTo(this);
        }
    }
}