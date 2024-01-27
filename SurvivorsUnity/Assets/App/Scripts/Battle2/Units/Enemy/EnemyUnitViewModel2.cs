using UniRx;
using UnityEngine;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵ObjectのViewModel
    /// </summary>
    [RequireComponent(typeof(EnemyUnitView2))]
    public class EnemyUnitViewModel2 : MonoBehaviour, IUnitViewModel2<EnemyUnitView2, EnemyUnitModel2>
    {
        public EnemyUnitView2 UnitView { get; private set; }
        public EnemyUnitModel2 UnitModel { get; private set; }
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            UnitView = GetComponent<EnemyUnitView2>();
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(EnemyUnitModel2 unitModel2)
        {
            UnitModel = unitModel2;
            //TODO デバッグ用
            UnitView.Label = unitModel2.Label;
            UnitView.Init(unitModel2.UnitId, unitModel2.Cell.Value, unitModel2.EnemyBase.ImageId);
            unitModel2.Cell.Subscribe(UnitView.SetToCell).AddTo(this);
            UnitModel.Hp.OnUpdate.Subscribe(x =>
            {
                
            }).AddTo(this);
            UnitModel.OnDefeated.Subscribe(_ => UnitView.OnDefeated()).AddTo(this);
            unitModel2.IsActive.DistinctUntilChanged().Subscribe(isActive =>
            {
                UnitView.SetActive(isActive);
            }).AddTo(this);
        }
    }
}