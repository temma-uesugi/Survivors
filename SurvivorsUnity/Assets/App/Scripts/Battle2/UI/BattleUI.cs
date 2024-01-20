using App.Battle2.Core;
using UnityEngine;

namespace App.Battle2.UI
{
    /// <summary>
    /// BattleUI
    /// </summary>
    [ContainerRegisterMonoBehaviourAttribute2(typeof(BattleUI))]
    public class BattleUI : MonoBehaviour
    {
        [field: SerializeField] public RectTransform RectTrans { get; private set; }
        [SerializeField] private RectTransform safeArea;

        [SerializeField] bool adjustLeft;
        [SerializeField] bool adjustRight;
        [SerializeField] bool adjustTop;
        [SerializeField] bool adjustBottom;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            var area = Screen.safeArea;

            var anchorMin = area.position;
            var anchorMax = area.position + area.size;
            anchorMin.x = adjustLeft ? anchorMin.x / Screen.width : 0;
            anchorMax.x = adjustRight ? anchorMax.x / Screen.width : 1;
            anchorMin.y = adjustBottom ? anchorMin.y / Screen.height : 0;
            anchorMax.y = adjustTop ? anchorMax.y / Screen.height : 1;
            safeArea.anchorMin = anchorMin;
            safeArea.anchorMax = anchorMax;
        }
    }
}