#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using App.AppCommon.Core;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.AppEditor.BotNodeEditor
{
    /// <summary>
    /// BotNodeEditorで使うHelper
    /// </summary>
    public static class BotNodeEditorHelper
    {
        //Editor上のディレクトリ
        public const string EditorDir = "Assets/App/Scripts/AppEditor/BotNodeEditor/";
        //Exportするディレクトリ
        public const string ExportPath = "Assets/App/Scripts/Battle/BotBehaviourTree/Data";
        
        private static MethodInfo _addComponentMethodInfo = null;

        /// <summary>
        /// targetをGameObjectに
        /// </summary>
        public static GameObject TargetToGameObject<T>(Object obj) where T : MonoBehaviour
        {
            return (obj as T)?.gameObject;
        }

        /// <summary>
        /// Rootを追加
        /// </summary>
        [MenuItem("Trafalgar/BotNode/CreateRootNode", priority = 2001)]
        public static void AddRoot()
        {
            Log.Debug("AddRoot");
            var root = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(EditorDir + "RootNode.prefab")) as GameObject;
            if (root == null)
            {
                Log.Debug("AddRoot Failed");
                return;
            }
            root.transform.SetAsLastSibling();
            PrefabUtility.UnpackPrefabInstance(root, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            BotNodeEditorSystem.Focus(root.GetInstanceID());
        }

        /// <summary>
        /// Actionの追加
        /// </summary>
        public static void AddAction(GameObject gameObject)
        {
            var action = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(EditorDir + "ActionNode.prefab")) as GameObject;
            if (action == null || gameObject == null)
            {
                Debug.Log("AddAction Failed");
                return;
            }
            action.transform.SetParent(gameObject.transform);
            //prefabをUnpack
            PrefabUtility.UnpackPrefabInstance(action, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            //ActionのComponentをアタッチ
            // AddComponent(action, type);
            BotNodeEditorSystem.RerenderView();
            BotNodeEditorSystem.Focus(action.GetInstanceID());
        }

        /// <summary>
        /// AddComponent<T>の MethodInfoを取得
        /// </summary>
        private static MethodInfo GetAddComponentMethodInfo()
        {
            if (_addComponentMethodInfo == null)
            {
                foreach (var mi in typeof(GameObject).GetMethods(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (mi.Name == "AddComponent" && mi.IsGenericMethod)
                    {
                        _addComponentMethodInfo = mi;
                        break;
                    }
                }
            }
            return _addComponentMethodInfo;
        }

        /// <summary>
        /// AddComponent
        /// </summary>
        private static void AddComponent(GameObject gameObject, Type type)
        {
            GetAddComponentMethodInfo()?.MakeGenericMethod(type).Invoke(gameObject, new object[]{});
        }
    }
}
#endif
