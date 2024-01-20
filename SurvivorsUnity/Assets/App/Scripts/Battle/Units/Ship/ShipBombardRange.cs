using App.AppCommon;
using App.Battle.Common;
using App.Battle.Map;
using UnityEngine;

namespace App.Battle.Units.Ship
{
    /// <summary>
    /// 船の砲撃範囲
    /// </summary>
    public class ShipBombardRange : MonoBehaviour
    {
        [SerializeField] private BombardRangeDraw leftRange;
        [SerializeField] private BombardRangeDraw rightRange;

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(BombStatus leftStatus, BombStatus rightStatus)
        {
            leftRange.Setup(BombSide.Left, leftStatus);
            rightRange.Setup(BombSide.Right, rightStatus);
        }

        /// <summary>
        /// Statusの更新
        /// </summary>
        public void UpdateStatus(BombSide side, BombStatus status)
        {
            if (side == BombSide.Left)
            {
                leftRange.UpdateStatus(status); 
            }
            else if (side == BombSide.Right)
            {
                rightRange.UpdateStatus(status); 
            }
        }
        
        /// <summary>
        /// 方向をセット
        /// </summary>
        public void SetDirection(DirectionType dir)
        {
            var dig = HexUtil.GetDegByDir(dir);
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, dig));
        }

        /// <summary>
        /// SetActive
        /// </summary>
        public void SetActive(bool isActive)
        {
            leftRange.SetActive(isActive);
            rightRange.SetActive(isActive);
        }
    }
}