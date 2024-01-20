using App.AppCommon;
using App.AppCommon.Core;
using App.AppCommon.UI;
using App.Battle.Core;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.UI.PhaseViews
{
    /// <summary>
    /// PhaseEffectのView
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(PhaseEffectView))]
    public class PhaseEffectView : MonoBehaviour
    {
        [SerializeField] private TextAnim playerPhaseText;
        [SerializeField] private TextAnim enemyPhaseText;

        /// <summary>
        /// Construct
        /// </summary>
        [Inject]
        public void Construct(BattleEventHub eventHub)
        {
            enemyPhaseText.gameObject.SetActive(false);
            eventHub.Subscribe<BattleEvents.OnPhaseStartAsync>(async x =>
            {
                if (x.Phase == PhaseType.EnemyPhase)
                {
                    await EnemyPhaseStartAsync();
                }
            }).AddTo(this);
            eventHub.Subscribe<BattleEvents.OnPhaseStartAsync>(async x =>
            {
                if (x.Phase == PhaseType.PlayerPhase)
                {
                    await PlayerPhaseStartAsync();
                }
            }).AddTo(this);
        }

        /// <summary>
        /// プレイヤーPhase開始
        /// </summary>
        private async UniTask PlayerPhaseStartAsync()
        {
            playerPhaseText.gameObject.SetActive(true);
            await playerPhaseText.ShowAndHideAsync();
        }
        
        /// <summary>
        /// 敵Phase開始
        /// </summary>
        private async UniTask EnemyPhaseStartAsync()
        {
            enemyPhaseText.gameObject.SetActive(true);
            await enemyPhaseText.ShowAndHideAsync();
        }
    }
}