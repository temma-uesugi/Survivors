#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UniRx;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace App.AppEditor.Common
{
    /// <summary>
    /// Objectの検索window
    /// </summary>
    public class ObjectSearchWindowProvider<T> : ScriptableObject, ISearchWindowProvider
    {
        private Texture2D icon;

        private readonly Subject<Type> onSelected = new Subject<Type>();
        public IObservable<Type> OnSelected => onSelected;

        /// <summary>
        /// 検索結果のTree表示取得
        /// </summary>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (icon == null)
            {
                icon = new Texture2D(1, 1);
            }
            icon.SetPixel(0, 0, new Color(0, 0, 0, 0));
            icon.Apply();
            var entries = new List<SearchTreeEntry>();
            entries.Add(new SearchTreeGroupEntry(new GUIContent(typeof(T).Name)) { level = 0 });
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsAbstract && (type.IsSubclassOf(typeof(T)) || typeof(T).IsAssignableFrom(type)))
                    {
                        entries.Add(new SearchTreeEntry(new GUIContent(type.Name, icon)) { level = 1, userData = type });
                    }

                }
            }
            return entries;
        }

        /// <summary>
        /// OnSelectEntry
        /// </summary>
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var type = searchTreeEntry.userData as Type;
            onSelected.OnNext(type);
            return true;
        }

        /// <summary>
        /// OnDestroy
        /// </summary>
        void OnDestroy()
        {
            if (icon != null)
            {
                DestroyImmediate(icon);
                icon = null;
            }
        }
    }
}
#endif
