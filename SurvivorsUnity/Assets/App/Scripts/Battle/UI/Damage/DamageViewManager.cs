using App.Battle.Core;
using App.Battle.Libs;
using App.Battle.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle.UI.Damage
{
    /// <summary>
    /// ダメージViewManager
    /// </summary>
    [ContainerRegisterMonoBehaviour(typeof(DamageViewManager))]
    public class DamageViewManager : MonoBehaviour
    {
        [SerializeField] private DamageView damageViewPrefab;
        [SerializeField] private Transform viewLayer;

        private readonly CompositeDisposable _disposable = new();
        private UnitManger _unitManger;
        private BattleCamera _battleCamera;
        private GameObjectPool<DamageView> _viewPool;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger unitManger,
            BattleCamera battleCamera
        )
        {
            _unitManger = unitManger;
            _battleCamera = battleCamera;
            _viewPool = new GameObjectPool<DamageView>(damageViewPrefab, viewLayer);

            // eventSub.Subscribe(msg =>
            // {
            //     switch (msg)
            //     {
            //         case EventMessages.OnShipAttackedEvent evt:
            //             ShowAsync(evt.Args).Forget();
            //             break;
            //         case EventMessages.OnEnemyAttackedEvent evt:
            //             ShowAsync(evt.Args).Forget();
            //             break;
            //     }
            // }).AddTo(_disposable);
        }

        /// <summary>
        /// 攻撃された
        /// </summary>
        private async UniTask ShowAsync(BattleEvents.AttackArgs args)
        {
            var view = _viewPool.Rent();
            var screenPos = _battleCamera.CellToScreenPoint(args.TargetUnit.Cell.Value);
            await view.ShowAsync(args.Damage, screenPos);
            _viewPool.Return(view);
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _disposable.Dispose();
        }
    }
}