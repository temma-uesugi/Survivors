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
       
        private EnemyUnitModel2 enemyUnitModel2;
        private ShipUnitModel2 shipUnitModel2;

        private readonly CompositeDisposable _disposable = new();
        private readonly CompositeDisposable _targetDisposable = new();

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(EnemyUnitModel2 enemyUnitModel2)
        {
            this.enemyUnitModel2 = enemyUnitModel2;
            enemyUnitModel2.TargetUnit.Subscribe(SetTarget).AddTo(_disposable);
            enemyUnitModel2.Cell.DistinctUntilChanged().Subscribe(UpdatePosition).AddTo(_disposable);
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
        private void SetTarget(ShipUnitModel2 shipUnitModel2)
        {
            _targetDisposable.Clear();
            if (shipUnitModel2 == null)
            {
                effect.gameObject.SetActive(false);
                return;
            }
            
            effect.gameObject.SetActive(true);
            effect.SetPosition(shipUnitModel2.Cell.Value.Position);
            shipUnitModel2.Cell.Subscribe(x =>
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