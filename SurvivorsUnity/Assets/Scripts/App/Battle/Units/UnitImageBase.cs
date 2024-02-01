using App.AppCommon.Core;
using UnityEngine;

namespace Master.Battle.Units
{
    /// <summary>
    /// UnitImage
    /// </summary>
    public abstract class UnitImageBase : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenter;
        [SerializeField] private Animator animator;
        
        private static readonly int IdleStateHash = Animator.StringToHash("Idle");
       
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            animator.Play(IdleStateHash, 0, 0);
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