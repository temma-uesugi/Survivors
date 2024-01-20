using App.AppCommon;
using App.Battle.Units.Ship;
using FastEnumUtility;
using UnityEngine;

namespace App.Battle.Common
{
    /// <summary>
    /// 攻撃範囲の描画
    /// </summary>
    public class BombardRangeDraw : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        //マテリアルを直接指定
        //Note: シェーダーのことをよくわからない
        [SerializeField] private Material narrowMaterial;
        [SerializeField] private Material middleMaterial;
        [SerializeField] private Material wideMaterial;

        private const string ColorProperty = "_Color";
        private const string DegProperty = "_Degree";

        private Transform _trans;
        private BombSide _side;
         
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            // Setup(Side.Right, GameConst.DefaultBombardRange, BombRangeType.Middle);
        }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(BombSide side, BombStatus status)
        {
            _trans = transform;
            _side = side;
            UpdateStatus(status);
        }

        /// <summary>
        /// ステータスをUpdate
        /// </summary>
        public void UpdateStatus(BombStatus status)
        {
            var material = status.RangeType switch
            {
                BombRangeType.Narrow => narrowMaterial,
                BombRangeType.Middle => middleMaterial,
                BombRangeType.Wide => wideMaterial,
                _=> middleMaterial,
            };
            spriteRenderer.material = material;
            
            var scale = _side.ToInt32() * status.RangeDistance;
            _trans.localScale = new Vector3(scale * 2, scale * 2, scale * 2);
            _trans.rotation = Quaternion.Euler(new Vector3(0, 0, status.RangeType.ToInt32() / 2f));
        }
        
        /// <summary>
        /// SetActive
        /// </summary>
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}