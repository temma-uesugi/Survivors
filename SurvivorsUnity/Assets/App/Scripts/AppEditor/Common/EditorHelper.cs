using UnityEngine;

namespace App.AppEditor.Common
{
    /// <summary>
    /// Editor用Helper
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// 区切りを入れる
        /// </summary>
        public static void Separate()
        {
            GUILayout.Space(4);
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));
            GUILayout.Space(4);
        }
    }
}