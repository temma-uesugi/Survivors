using App.AppCommon;
using App.Battle2.Core;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.UI.MovePower
{
    /// <summary>
    /// 移動力View
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(MovePowerView))]
    public class MovePowerView : MonoBehaviour, IPlayerPhaseView
    {
        [SerializeField] private TextMeshProUGUI maxPower;
        [SerializeField] private TextMeshProUGUI currentPower;

        private Sequence _reducedSequence;

        /// <summary>
        /// Construct
        /// </summary>
        [Inject]
        public void Construct(MovePowerViewModel viewModel, BattleEventHub2 eventHub2)
        {
            maxPower.SetText("0");
            currentPower.SetText("0");
            viewModel.MovePower.Subscribe(x =>
            {
                currentPower.text = x.ToString("F1");
            }).AddTo(this);
            viewModel.OnChangeShip.Subscribe(x =>
            {
                currentPower.text = x.ToString("F1");
                maxPower.text = x.ToString("F1");
            }).AddTo(this);
            viewModel.OnShipActionEnd
                .Merge(viewModel.OnShipReleased).Subscribe(_ =>
                {
                    maxPower.text = "--";
                    currentPower.text = "-";
                }).AddTo(this);

            eventHub2.Subscribe<BattleEvents2.OnPhaseStartAsync>(async x =>
            {
                if (x.Phase == PhaseType.PlayerPhase)
                {
                    gameObject.SetActive(true);
                }
            }).AddTo(this);
            eventHub2.Subscribe<BattleEvents2.OnPhaseEndAsync>(async x =>
            {
                if (x.Phase == PhaseType.PlayerPhase)
                {
                    gameObject.SetActive(false);
                }
            }).AddTo(this);
        }

        /// <inheritdoc/>
        public void PhaseStart()
        {
            gameObject.SetActive(true);
        }

        /// <inheritdoc/>
        public void PhaseEnd()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _reducedSequence?.Kill();
        }
    }
}