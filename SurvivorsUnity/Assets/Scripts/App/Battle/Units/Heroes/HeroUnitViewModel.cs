using UniRx;
using UnityEngine;

namespace Master.Battle.Units.Heroes
{
    /// <summary>
    /// 美香UnitViewModel
    /// </summary>
    [RequireComponent(typeof(HeroUnitViewModel))]
    public class HeroUnitViewModel : MonoBehaviour, IUnitViewModel<HeroUnitView, HeroUnitModel>
    {
        public HeroUnitView UnitView { get; private set; }
        public HeroUnitModel UnitModel { get; private set; }
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            UnitView = GetComponent<HeroUnitView>();
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(HeroUnitModel model)
        {
            UnitModel = model;

            UnitView.Init(model.UnitId, model.Cell.Value, model.ImageId);
            model.Cell.Subscribe(UnitView.SetToCell).AddTo(this);
        }
    }
}