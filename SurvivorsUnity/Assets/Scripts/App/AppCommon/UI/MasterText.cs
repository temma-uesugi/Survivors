using UnityEngine;
using UnityEngine.UI;

namespace App.AppCommon.UI
{
    /// <summary>
    /// マスタテキスト
    /// </summary>
    [RequireComponent(typeof(Text))]
    public class MasterText : MonoBehaviour
    {
        [SerializeField] private MasterStringType[] stringType;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            var text = GetComponent<Text>();
            text.text = Lang.GetText(text.text, stringType);
        }

    }
}