using DG.Tweening;
using Master.Battle.Map.Cells;
using Master.Constants;
using UnityEngine;

namespace Master.Battle.Units
{
    /// <summary>
    /// UnitViewの基本
    /// </summary>
    public abstract class UnitViewBase : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        //ViewModel
        public HexCell Cell { get; private set; }
        public DirectionType Direction { get; private set; }

        private Transform _transform;
        public uint UnitId { get; private set; }
        public Vector3 Position => _transform.position;

        /// <summary>
        /// ImageのGameObject
        /// </summary>
        protected abstract GameObject ImageObject { get; }
        
        /// <summary>
        /// Imageをセット
        /// </summary>
        protected abstract void SetImage(string imageId);
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(uint unitId, HexCell cell, string imageId)
        {
            UnitId = unitId;
            SetToCell(cell);
            _transform = transform;
            SetImage(imageId); 
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
            ImageObject.gameObject.SetActive(isActive);
        }
        
        /// <summary>
        /// 死亡
        /// </summary>
        public virtual void OnDefeated()
        {
            //TODO 死亡演出
            canvasGroup.DOFade(0, 1f)
                .SetEase(Ease.OutCubic)
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
    
    /// <summary>
    /// UnitViewの基本
    /// </summary>
    public abstract class UnitViewBase<T> : UnitViewBase where T : MonoBehaviour
    {
        protected override GameObject ImageObject => _image.gameObject;
        private T _image;
        
        /// <inheritdoc />
        protected override void SetImage(string imageId)
        {
            var image = Resources.Load<T>($"{imageId}");
            if (image != null)
            {                
                _image = Instantiate(image, transform);
            }
        }
    }
}