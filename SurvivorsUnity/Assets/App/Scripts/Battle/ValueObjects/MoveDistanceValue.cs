using App.AppCommon.Core;

namespace App.Battle.ValueObjects
{
    /// <summary>
    /// 移動距離ValueObject
    /// </summary>
    public class MoveDistanceValue
    {
        public int MaxDistance { get; init; }
        public int BaseDistance { get; init; }
        public int MinDistance { get; init; }

        public float RemainingDistance { get; private set; }

        public bool IsOverMin => MaxDistance - RemainingDistance > MinDistance;
        public bool IsOverBase => MaxDistance - RemainingDistance > BaseDistance;
        public bool IsOver => RemainingDistance <= 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MoveDistanceValue(int baseDistance, float brakingRate, float expandRate)
        {
            BaseDistance = baseDistance;
            MinDistance = (int)(baseDistance * brakingRate);
            MaxDistance = (int)(baseDistance * expandRate);
            RemainingDistance = MaxDistance;
            Log.Debug("MoveDistanceValue", baseDistance, brakingRate, expandRate, MinDistance, MaxDistance);
        }

        /// <summary>
        /// 残り距離をセット
        /// </summary>
        public float SetRemainingDistance(float remainingDistance)
        {
            Log.Debug("SetRemainingDistance", remainingDistance);
            var res = RemainingDistance - remainingDistance;
            RemainingDistance = remainingDistance;
            return res;
        }

    }
}