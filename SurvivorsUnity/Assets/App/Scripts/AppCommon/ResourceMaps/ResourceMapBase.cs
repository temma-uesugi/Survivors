using System;
using UnityEngine;
using System.Linq;

namespace App.AppCommon
{
    /// <summary>
    /// EnumとImageの対応
    /// </summary>
    [Serializable]
    public class ResourceMapItem<T> where T : Enum
    {
        public T Type;
        public Sprite Resource;
    }
    
    /// <summary>
    /// EnumとItemのマッピング
    /// </summary>
    public class ResourceMapBase<T> : ScriptableObject where T : Enum
    {
        [SerializeField] private ResourceMapItem<T>[] items;

        /// <summary>
        /// Sprite取得
        /// </summary>
        public Sprite GetSprite(T type)
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
    public class ResourceMapItem<T1, T2> where T1 : Enum
    {
        public T1 Type;
        public T2 Value;
        public Sprite Resource;
    }

    /// <summary>
    /// EnumとItemのマッピング
    /// </summary>
    public class ResourceMapBase<T1, T2> : ScriptableObject where T1 : Enum
    {
        [SerializeField] private ResourceMapItem<T1, T2>[] items;

        /// <summary>
        /// Sprite取得
        /// </summary>
        public Sprite GetSprite(T1 type, T2 value)
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