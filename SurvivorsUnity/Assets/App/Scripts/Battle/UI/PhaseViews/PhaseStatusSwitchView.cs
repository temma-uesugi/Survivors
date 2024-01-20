using System.Collections.Generic;
using System.Linq;
using App.AppCommon;
using App.Battle.Core;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.UI.PhaseViews
{
    /// <summary>
    /// フェイズによるSwitch
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(PhaseStatusSwitchView))]
    public class PhaseStatusSwitchView : MonoBehaviour
    {
        [SerializeField] private GameObject[] playerPhaseObjects;
        [SerializeField] private GameObject[] enemyPhaseObjects;

        private readonly List<IPlayerPhaseView> _playerPhaseViews = new();
        private readonly List<IEnemyPhaseView> _enemyPhaseViews = new();

        /// <summary>
        /// Construct
        /// </summary>
        [Inject]
        public void Construct(BattleEventHub eventHub)
        {
            _playerPhaseViews.AddRange(GetComponentsInChildren<IPlayerPhaseView>());
            _enemyPhaseViews.AddRange(GetComponentsInChildren<IEnemyPhaseView>()); 
            
            OnPhaseEnd(PhaseType.PlayerPhase);
            OnPhaseEnd(PhaseType.EnemyPhase);
            
            eventHub.Subscribe<BattleEvents.OnPhaseStartAsync>(async x =>
            {
                OnPhaseStart(x.Phase);
            }).AddTo(this);
            eventHub.Subscribe<BattleEvents.OnPhaseEndAsync>(async x =>
            {
                OnPhaseEnd(x.Phase);
            }).AddTo(this);
        }

        /// <summary>
        /// フェイズ開始
        /// </summary>
        private void OnPhaseStart(PhaseType phaseType)
        {
            (IEnumerable<GameObject> gameObjects, IEnumerable<IPhaseView> phaseViews) = phaseType switch
            {
                PhaseType.PlayerPhase => (playerPhaseObjects, _playerPhaseViews),
                PhaseType.EnemyPhase => (enemyPhaseObjects, _enemyPhaseViews),
                _ => (Enumerable.Empty<GameObject>(), Enumerable.Empty<IPhaseView>()),
            };

            foreach (var go in gameObjects)
            {
                go.SetActive(true); 
            }
            foreach (var view in phaseViews)
            {
                view.PhaseStart(); 
            }
        }

        /// <summary>
        /// フェイズ終了
        /// </summary>
        private void OnPhaseEnd(PhaseType phaseType)
        {
            (IEnumerable<GameObject> gameObjects, IEnumerable<IPhaseView> phaseViews) = phaseType switch
            {
                PhaseType.PlayerPhase => (playerPhaseObjects, _playerPhaseViews),
                PhaseType.EnemyPhase => (enemyPhaseObjects, _enemyPhaseViews),
                _ => (Enumerable.Empty<GameObject>(), Enumerable.Empty<IPhaseView>()),
            };

            foreach (var go in gameObjects)
            {
                go.SetActive(false); 
            }
            foreach (var view in phaseViews)
            {
                view.PhaseEnd(); 
            }
        }
        
    }
}