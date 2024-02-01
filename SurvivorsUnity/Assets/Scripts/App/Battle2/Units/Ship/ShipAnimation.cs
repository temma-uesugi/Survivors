using App.AppCommon;
using UnityEngine;
using Constants;

namespace App.Battle2.Units.Ship
{
    [RequireComponent(typeof(Animator))]
    public class ShipAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private static readonly int RightStateHash = Animator.StringToHash("ShipRight");
        private static readonly int TopRightStateHash = Animator.StringToHash("ShipTopRight");
        private static readonly int TopLeftStateHash = Animator.StringToHash("ShipTopLeft");
        private static readonly int LeftStateHash = Animator.StringToHash("ShipLeft");
        private static readonly int BottomLeftStateHash = Animator.StringToHash("ShipBottomLeft");
        private static readonly int BottomRightStateHash = Animator.StringToHash("ShipBottomRight");

        /// <summary>
        /// 方向アニメセット
        /// </summary>
        public void SetDirectionAnim(DirectionType dir)
        {
            var state = dir switch
            {
                DirectionType.Right => RightStateHash,
                DirectionType.TopRight => TopRightStateHash,
                DirectionType.TopLeft => TopLeftStateHash,
                DirectionType.Left => LeftStateHash,
                DirectionType.BottomLeft => BottomLeftStateHash,
                DirectionType.BottomRight => BottomRightStateHash,
                _ => 0,
            };
            animator.Play(state, 0, 0);
        }

    }
}