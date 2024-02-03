using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace App.AppCommon.ResourceMaps
{
    /// <summary>
    /// EnumとImageの対応
    /// </summary>
    [Serializable]
    public class ResourceMapItem<T, TO> where T : Enum where TO : Object
    {
        public T Type;
        public TO Resource;
    }
    
    /// <summary>
    /// EnumとItemのマッピング
    /// </summary>
    public class ResourceMapBase<T, TO> : ScriptableObject where T : Enum where TO : Object
    {
        [SerializeField] private ResourceMapItem<T, TO>[] items;

        /// <summary>
        /// Object取得
        /// </summary>
        public TO GetObject(T type)
        {
            var data = items.FirstOrDefault(x => x.Type.Equals(type));
            if (data == null || data.Resource == null)
            {
                Debug.LogError($"イメージが存在しません : {type}");
                return null;
            }
            return data.Resource;
        }
    }
    
    /// <summary>
    /// EnumとImageの対応
    /// </summary>
    [Serializable]
    public class ResourceMapItem<T1, T2, TO> where T1 : Enum where TO : Object
    {
        public T1 Type;
        public T2 Value;
        public TO Resource;
    }

    /// <summary>
    /// EnumとItemのマッピング
    /// </summary>
    public class ResourceMapBase<T1, T2, TO> : ScriptableObject where T1 : Enum where TO : Object
    {
        [SerializeField] private ResourceMapItem<T1, T2, TO>[] items;

        /// <summary>
        /// Object取得
        /// </summary>
        public TO GetObject(T1 type, T2 value)
        {
            var data = items.FirstOrDefault(x => x.Type.Equals(type) && x.Value.Equals(value));
            if (data == null || data.Resource == null)
            {
                Debug.LogError($"イメージが存在しません : {type}");
                return null;
            }
            return data.Resource;
        }
    }

}