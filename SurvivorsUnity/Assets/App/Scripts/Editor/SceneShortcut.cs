using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace App.Editor
{
    /// <summary>
    /// シーンのショートカット
    /// </summary>
    public static class SceneShortcut
    {
        [MenuItem("Survivors/Scene/Battle", priority = 101)]
        private static bool OpenBattle() => OpenScene("Battle");
       
        /// <summary>
        /// シーンを開く
        /// </summary>
        private static bool OpenScene(string sceneName)
        {
            bool isSuccess = false;
            try
            {
                //変更チェック
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    // シーンアセットから検索
                    foreach (var guid in AssetDatabase.FindAssets("t:Scene"))
                    {
                        var path = AssetDatabase.GUIDToAssetPath(guid);
                        if (path.Contains(sceneName + ".unity"))
                        {
                            EditorSceneManager.OpenScene(path);
                            isSuccess = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
                Debug.LogError($"シーンが見つからない: {sceneName}");
            }
            return isSuccess;
        }
    }
}