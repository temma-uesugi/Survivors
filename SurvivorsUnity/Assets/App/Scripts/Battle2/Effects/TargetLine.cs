using App.Battle2.Map.Cells;
using App.Battle2.Units.Enemy;
using App.Battle2.Units.Ship;
using UniRx;
using UnityEngine;

namespace App.Battle2.Effects
{
    /// <summary>
    /// ターゲットライン
    /// </summary>
    public class TargetLine : MonoBehaviour
    {
        [SerializeField] private TargetLineEffect effect;
       
        private EnemyUnitModel _enemyUnitModel;
        private ShipUnitModel _shipUnitModel;

        private readonly CompositeDisposable _disposable = new();
        private readonly CompositeDisposable _targetDisposable = new();

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(EnemyUnitModel enemyUnitModel)
        {
            _enemyUnitModel = enemyUnitModel;
            enemyUnitModel.TargetUnit.Subscribe(SetTarget).AddTo(_disposable);
            enemyUnitModel.Cell.DistinctUntilChanged().Subscribe(UpdatePosition).AddTo(_disposable);
        }

        /// <summary>
        /// 位置更新
        /// </summary>
        private void UpdatePosition(HexCell cell)
        {
            transform.position = cell.Position;
        }
        
        /// <summary>
        /// ターゲット設定
        /// </summary>
        private void SetTarget(ShipUnitModel shipUnitModel)
        {
            _targetDisposable.Clear();
            if (shipUnitModel == null)
            {
                effect.gameObject.SetActive(false);
                return;
            }
            
            effect.gameObject.SetActive(true);
            effect.SetPosition(shipUnitModel.Cell.Value.Position);
            shipUnitModel.Cell.Subscribe(x =>
            {
                effect.Clear();
                effect.SetPosition(x.Position);
            }).AddTo(_targetDisposable);
        }

        /// <summary>
        /// Clear
        /// </summary>
        public void Clear()
        {
            _disposable.Clear();    
            _targetDisposable.Clear();
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _disposable.Dispose();    
            _targetDisposable.Dispose();
        }
    }
}