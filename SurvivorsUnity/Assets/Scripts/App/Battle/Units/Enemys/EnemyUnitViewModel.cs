using UniRx;
using UnityEngine;

namespace App.Battle.Units
{
    /// <summary>
    /// 敵UnitViewModel
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
        public void Setup(EnemyUnitModel model)
        {
            UnitModel = model;
            
            UnitView.Init(model.UnitId, model.Cell.Value, model.EnemyBase.ImageId);
            model.Cell.Subscribe(UnitView.SetToCell).AddTo(this);
            // UnitModel.Hp.OnUpdate.Subscribe(x =>
            // {
            //     
            // }).AddTo(this);
            // model.OnDefeated.Subscribe(_ => UnitView.OnDefeated()).AddTo(this);
            // model.IsActive.DistinctUntilChanged().Subscribe(isActive =>
            // {
            //     UnitView.SetActive(isActive);
            // }).AddTo(this);
        }
    }
}