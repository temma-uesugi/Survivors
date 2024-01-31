using App.AppCommon.Core;
using UnityEngine;

namespace App.Battle2.Units.Enemy
{
    /// <summary>
    /// 敵Image
    /// </summary>
    public class EnemyImage2 : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenter;
        [SerializeField] private Animator animator;

        private static readonly int IdleStageHash = Animator.StringToHash("Idle");
        
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            animator.Play(IdleStageHash, 0, 0);
        }

        //TODO デバッグ用
        public string Label;
        
        /// <summary>
        /// Activeセット
        /// </summary>
        public void SetActive(bool isActive)
        {
            spriteRenter.color = isActive ? new Color(1, 1, 1, 1) : new Color(0.3f, 0.3f, 0.3f, 1);
            
            //TODO デバッグ用ログ
            Log.Debug("EnemyImage.SetActive", isActive, Label);
        }
    }
}