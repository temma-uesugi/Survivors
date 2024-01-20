using UnityEditor;
using UnityEngine;

namespace App.Editor
{
    public class CreateEmptyScriptableObject
    {
        /// <summary>
        /// 空のScriptableObjectを作成する
        /// </summary>
        [MenuItem("Assets/Create/ScriptableObjects/Empty", priority = 1)]
        private static void Create()
        {
            ScriptableObject obj = ScriptableObject.CreateInstance<ScriptableObject>();

            string defaultName = "EmptyScriptableObject.asset";

            ProjectWindowUtil.CreateAsset(obj, defaultName);
        }
    }
}