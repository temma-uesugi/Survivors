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
        public EnemyUnitView UnitView { get;  }
        public EnemyUnitModel UnitModel { get; }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(EnemyUnitModel model)
        {
            
        }
    }
}