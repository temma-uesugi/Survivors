using App.AppCommon;
using UniRx;
using UnityEngine;

namespace App.Battle.UI.HexButtons
{
    /// <summary>
    /// Hex用ボタン
    /// </summary>
    public class HexButton : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;

        /// <summary>
        /// Awake
        /// </summary>
        protected virtual void Awake()
        {
            gameObject.SetActive(false);
            BattleCamera.Instance.CameraSizeRatio.Subscribe(OnUpdateCameraSizeRatio).AddTo(this);
        }
        
        /// <summary>
        /// カメラサイズのUpdate
        /// </summary>
        protected virtual void OnUpdateCameraSizeRatio(float ratio)
        {
            rectTransform.localScale = GameConst.DefaultHexImageScale * Vector3.one * ratio;
        }
        
        /// <summary>
        /// 活性化
        /// </summary>
        public void SetPosition(Vector3 position)
        {
            rectTransform.position = position;
        }

        /// <summary>
        /// Activeのセット
        /// </summary>
        public virtual void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}