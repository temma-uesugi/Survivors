// using UnityEngine;
//
// namespace App.AppCommon.Libs
// {
//     /// <summary>
//     /// Singleton のMonoBehaviour
//     /// </summary>
//     public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
//     {
//
//         private static T _instance;
//         public static T Instance
//         {
//             get
//             {
//                 if (_instance == null)
//                 {
//                     _instance = (T)FindObjectOfType(typeof(T));
//
//                     if (_instance == null)
//                     {
//                         Debug.LogWarning(typeof(T) + "is nothing");
//                     }
//                 }
//
//                 return _instance;
//             }
//         }
//
//
//         /// <summary>
//         /// Awake
//         /// </summary>
//         protected virtual void Awake()
//         {
//             CheckInstance();
//         }
//
//         /// <summary>
//         /// Instanceのチェック
//         /// </summary>
//         /// <returns></returns>
//         protected bool CheckInstance()
//         {
//             if (_instance == null)
//             {
//                 _instance = (T)this;
//                 return true;
//             }
//             else if (Instance == this)
//             {
//                 return true;
//             }
//
//             Destroy(this);
//             return false;
//         }
//
//     }
// }