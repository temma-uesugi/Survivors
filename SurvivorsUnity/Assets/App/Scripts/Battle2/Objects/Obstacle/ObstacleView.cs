using App.Battle2.Map.Cells;
using DG.Tweening;
using UnityEngine;

namespace App.Battle2.Objects.Obstacle
{
    /// <summary>
    /// 障害物のView
    /// </summary>
    public class ObstacleView : MonoBehaviour, IObjectView
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        public uint ObjectId { get; private set; }
        public HexCell Cell { get; private set; }
       
        public bool IsAlive { get; private set; } = true;
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(uint objectId, HexCell cell)
        {
            ObjectId = objectId;
            SetToCell(cell);
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
        /// 破壊
        /// </summary>
        public void OnDestroyed()
        {
            //TODO 死亡演出
            IsAlive = true;
            canvasGroup.DOFade(0, 1f)
                .SetEase(Ease.OutCubic)
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}