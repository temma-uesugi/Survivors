using UnityEngine;
using App.AppCommon;
using App.Battle.Map;
using App.Battle2.Units.Enemy;
using DG.Tweening;

namespace App.Battle.Units
{
    /// <summary>
    /// 敵UnitView
    /// </summary>
    public class EnemyUnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        //ViewModel
        public HexCell Cell { get; private set; }
        public DirectionType Direction { get; private set; }

        private Transform _transform;
        public uint UnitId { get; private set; }
        public Vector3 Position => _transform.position;

        private EnemyImage2 enemyImage2;
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(uint unitId, HexCell cell, string imageId)
        {
            UnitId = unitId;
            SetToCell(cell);
            _transform = transform;
            var image = Resources.Load<EnemyImage2>($"{imageId}");
            if (image != null)
            {                
                enemyImage2 = Instantiate(image, transform);
            }
        }
        
        /// <summary>
        /// Cellに配置
        /// </summary>
        public void SetToCell(HexCell cell)
        {
            Cell = cell;
            transform.position = cell.Position;
        }

        /// <summary>
        /// Activeかの設定
        /// </summary>
        public void SetActive(bool isActive)
        {
            enemyImage2.SetActive(isActive);
        }
        
        /// <summary>
        /// 死亡
        /// </summary>
        public void OnDefeated()
        {
            //TODO 死亡演出
            canvasGroup.DOFade(0, 1f)
                .SetEase(Ease.OutCubic)
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}