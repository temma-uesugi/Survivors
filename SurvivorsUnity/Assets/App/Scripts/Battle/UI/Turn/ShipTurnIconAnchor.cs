using Cysharp.Threading.Tasks;
using UnityEngine;

namespace App.Battle.UI.Turn
{
    /// <summary>
    /// 船ターンアイコンのAnchor
    /// </summary>
    public class ShipTurnIconAnchor : MonoBehaviour
    {
        [SerializeField] private ShipTurnIconAnchorGrid[] grids;

        /// <summary>
        /// Setup
        /// </summary>
        public async UniTask SetupAsync()
        {
            await UniTask.Yield();
            foreach (var grid in grids)
            {
                await grid.SetupAsync();
            }
        }
        
        /// <summary>
        /// インデクサ
        /// </summary>
        public Vector3 this[int index, int gridIndex] => grids[index][gridIndex];
    }
}