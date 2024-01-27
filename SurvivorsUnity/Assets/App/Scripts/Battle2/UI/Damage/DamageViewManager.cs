using App.Battle2.Core;
using App.Battle2.Libs;
using App.Battle2.Units;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer;

namespace App.Battle2.UI.Damage
{
    /// <summary>
    /// ダメージViewManager
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(DamageViewManager))]
    public class DamageViewManager : MonoBehaviour
    {
        [SerializeField] private DamageView damageViewPrefab;
        [SerializeField] private Transform viewLayer;

        private readonly CompositeDisposable _disposable = new();
        private UnitManger2 unitManger2;
        private BattleCamera2 battleCamera2;
        private GameObjectPool<DamageView> _viewPool;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Inject]
        public void Construct(
            UnitManger2 unitManger2,
            BattleCamera2 battleCamera2
        )
        {
            this.unitManger2 = unitManger2;
            this.battleCamera2 = battleCamera2;
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
        private async UniTask ShowAsync(BattleEvents2.AttackArgs args)
        {
            var view = _viewPool.Rent();
            var screenPos = battleCamera2.CellToScreenPoint(args.TargetUnit.Cell.Value);
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