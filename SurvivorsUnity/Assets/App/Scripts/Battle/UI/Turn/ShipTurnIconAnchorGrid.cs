using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Battle.UI.Turn
{
    /// <summary>
    /// 船ターンアイコンのAnchor内Grid
    /// </summary>
    public class ShipTurnIconAnchorGrid : MonoBehaviour
    {
        [SerializeField] private RectTransform[] transforms;

        private Vector3[] _positions;
        
        /// <summary>
        /// SetupAsync
        /// </summary>
        public async UniTask SetupAsync()
        {
            await UniTask.Yield();
            _positions = transforms.Select(x => x.position).ToArray();
            foreach (var trans in transforms)
            {
                trans.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// インデクサ
        /// </summary>
        public Vector3 this[int index] => _positions[index];
    }
}