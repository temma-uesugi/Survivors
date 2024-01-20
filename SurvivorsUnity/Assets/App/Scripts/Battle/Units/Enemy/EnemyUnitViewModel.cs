using UniRx;
using UnityEngine;

namespace App.Battle.Units.Enemy
{
    /// <summary>
    /// 敵ObjectのViewModel
    /// </summary>
    [RequireComponent(typeof(EnemyUnitView))]
    public class EnemyUnitViewModel : MonoBehaviour, IUnitViewModel<EnemyUnitView, EnemyUnitModel>
    {
        public EnemyUnitView UnitView { get; private set; }
        public EnemyUnitModel UnitModel { get; private set; }
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            UnitView = GetComponent<EnemyUnitView>();
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(EnemyUnitModel unitModel)
        {
            UnitModel = unitModel;
            //TODO デバッグ用
            UnitView.Label = unitModel.Label;
            UnitView.Init(unitModel.UnitId, unitModel.Cell.Value, unitModel.EnemyBase.ImageId);
            unitModel.Cell.Subscribe(UnitView.SetToCell).AddTo(this);
            UnitModel.Hp.OnUpdate.Subscribe(x =>
            {
                
            }).AddTo(this);
            UnitModel.OnDefeated.Subscribe(_ => UnitView.OnDefeated()).AddTo(this);
            unitModel.IsActive.DistinctUntilChanged().Subscribe(isActive =>
            {
                UnitView.SetActive(isActive);
            }).AddTo(this);
        }
    }
}