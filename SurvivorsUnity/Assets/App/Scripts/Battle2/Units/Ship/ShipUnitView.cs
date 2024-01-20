using App.AppCommon;
using App.Battle2.Map.Cells;
using DG.Tweening;
using UnityEngine;

namespace App.Battle2.Units.Ship
{
    /// <summary>
    /// ShipObjectのView
    /// </summary>
    public class ShipUnitView : MonoBehaviour, IUnitView
    {
        [SerializeField] private ShipAnimation anim;
        [SerializeField] private ShipBombardRange bombardRange;
        [SerializeField] private SpriteRenderer shipRenderer;
        [SerializeField] private CanvasGroup canvasGroup;
        
        //ViewModel
        public HexCell Cell { get; private set; }
        public DirectionType Direction { get; private set; }

        public uint UnitId { get; private set; }
        public Vector3 Position => transform.position;
        public bool IsAlive { get; private set; } = true;

        public ShipBombardRange BombRange => bombardRange;
      
        /// <summary>
        /// 初期化
        /// </summary>
        public void Init(uint unitId, HexCell cell, string imageId)
        {
            UnitId = unitId;
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
        /// 向きの設定
        /// </summary>
        public void SetDirection(DirectionType dir)
        {
            anim.SetDirectionAnim(dir);
            bombardRange.SetDirection(dir);
            Direction = dir;
            // var dig = HexUtil.GetDigByDir(dir);
            // transform.rotation = Quaternion.Euler(new Vector3(0, 0, dig));
        }

        /// <summary>
        /// Activeかのセット
        /// </summary>
        public void SetActive(bool isActive)
        {
            shipRenderer.color = isActive ? new Color(1, 1, 1, 1) : new Color(0.3f, 0.3f, 0.3f, 1);
        }

        /// <summary>
        /// 死亡
        /// </summary>
        public void OnDefeated()
        {
            //TODO 死亡処理
            IsAlive = true;
            canvasGroup.DOFade(0, 1f)
                .SetEase(Ease.OutCubic)
                .SetLink(gameObject)
                .OnComplete(() => gameObject.SetActive(false));
        }
    }
}