using App.Battle2.Units.Enemy;
using UniRx;
using UnityEngine;

namespace App.Battle2.UI.Status.MapUnitStatus.Enemy
{
    /// <summary>
    /// 敵ステータスViewModel
    /// </summary>
    [RequireComponent(typeof(EnemyStatusView))]
    public class EnemyStatusViewModel : StatusViewModelBase<EnemyUnitModel2, EnemyStatusView>
    {
        /// <summary>
        /// Setup
        /// </summary>
        public override void Setup(EnemyUnitModel2 enemyModel2, BattleCamera2 battleCamera2)
        {
            base.Setup(enemyModel2, battleCamera2); 
            View.Setup(enemyModel2.Label, enemyModel2.Hp.Current, enemyModel2.Hp.Max);

            enemyModel2.Hp.OnUpdate.Subscribe(View.UpdateHp).AddTo(this);
            enemyModel2.OnDefeated.Subscribe(_ => View.OnDefeated()).AddTo(this);
            enemyModel2.NextActionTurns.Subscribe(View.UpdateNextActionTurns).AddTo(this);
            enemyModel2.IsActive.Subscribe(View.SetActiveNextActionTurns).AddTo(this);
        }
    }
}