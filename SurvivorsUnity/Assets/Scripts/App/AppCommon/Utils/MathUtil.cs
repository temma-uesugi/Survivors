using UnityEngine;

namespace App.AppCommon.Utils
{
    /// <summary>
    /// 算術Util
    /// </summary>
    public static class MathUtil
    {
        /// <summary>
        /// Vector2をAngleに変換
        /// </summary>
        public static float Vector2ToAngle(Vector2 vector2)
        {
            float angle = Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
            if (angle < 0)
            {
                angle += 360;
            }

            return angle;
        }
    }
}