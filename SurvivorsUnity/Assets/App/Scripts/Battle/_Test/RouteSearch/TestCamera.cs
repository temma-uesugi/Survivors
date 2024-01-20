using UnityEngine;

namespace App.Battle._Test.RouteSearch
{
    public class TestCamera : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        /// <summary>
        /// ポジションのセット
        /// </summary>
        public void SetPosition(Vector3 pos)
        {
            pos.z = -1;
            camera.transform.position = pos;
        }
    }
}