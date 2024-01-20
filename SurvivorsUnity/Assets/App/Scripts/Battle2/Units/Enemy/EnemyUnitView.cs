using App.AppCommon;
using App.Battle2.Map.Cells;
using DG.Tweening;
using UnityEngine;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵ObjectのView
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
        public bool IsAlive { get; private set; } = true;

        private EnemyImage _enemyImage;

        //TODO デバッグ用
        public string Label;
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(uint unitId, HexCell cell, string imageId)
        {
            UnitId = unitId;
            SetToCell(cell);
            _transform = transform;
            var image = Resources.Load<EnemyImage>($"{imageId}");
            if (image != null)
            {                
                _enemyImage = Instantiate(image, transform);
                //TODO デバッグ用
                _enemyImage.Label = Label;
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
        /// 向きの設定
        /// </summary>
        public void SetDirection(DirectionType dir)
        {
            Direction = dir;
        }
       
        /// <summary>
        /// Activeかの設定
        /// </summary>
        public void SetActive(bool isActive)
        {
            _enemyImage.SetActive(isActive);
        }
        
        /// <summary>
        /// 死亡
        /// </summary>
        public void OnDefeated()
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