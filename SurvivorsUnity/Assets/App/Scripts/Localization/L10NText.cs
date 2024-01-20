using System;
using System.Linq;
using App.AppLibs;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace App.Localization
{
    /// <summary>
    /// ローカライズText
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class L10NText : MonoBehaviour
    {
        [SerializeField, Disabled] public string category;
        [SerializeField, Disabled] public string key;
        [SerializeField] public string[] param;

        private TextMeshProUGUI _text;

        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }
        
        /// <summary>
        /// テキスト更新
        /// </summary>
        public void UpdateText(params object[] updateParams)
        {
            _text.SetText(L10NConverter.Instance.GetString(category, key, updateParams));
        }
    }
    
#if UNITY_EDITOR
    /// <summary>
    /// ローカライズText
    /// </summary>
    [CustomEditor(typeof(L10NText))]
    public class L10NTextEditor : Editor
    {
        private L10NText _target;
        private TextMeshProUGUI _text;

        //カテゴリ
        private string[] _categoryValues;
        private int _categoryIndex = 0; 
        //Key 
        private string[] _keyValues;
        private int _keyIndex = 0;
       
        private static readonly string NoSelection = "-";
        
        /// <summary>
        /// OnEnable
        /// </summary>
        public void OnEnable()
        {
            _target = (L10NText) target;
            _text = _target.GetComponent<TextMeshProUGUI>();
            _categoryValues = L10NConverter.Instance.Categories.Prepend(NoSelection).ToArray();

            var catIndex = Array.IndexOf(_categoryValues, _target.category);
            _categoryIndex = catIndex < 0 ? 0 : catIndex;
            var initCat = _categoryValues[_categoryIndex];
            _keyValues = L10NConverter.Instance.Keys(initCat).Prepend(NoSelection).ToArray();
            var keyIndex = Array.IndexOf(_keyValues, _target.key);
            _keyIndex = keyIndex < 0 ? 0 : keyIndex;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            //オプションの設定
            EditorGUI.BeginChangeCheck();
            int newCategoryIndex = EditorGUILayout.Popup("カテゴリ選択", _categoryIndex, _categoryValues);
            if (EditorGUI.EndChangeCheck())
            {
                if (newCategoryIndex != _categoryIndex)
                {
                    _categoryIndex = newCategoryIndex;
                    var cat = _categoryValues[_categoryIndex];
                    _target.category = cat;
                    _keyValues = L10NConverter.Instance.Keys(cat).Prepend(NoSelection).ToArray();
                    _keyIndex = 0;
                    var key = _keyValues[_keyIndex];
                    _target.key = key;
                    _text.text = "";
                    EditorUtility.SetDirty(_target);
                }
            }

            int newKeyIndex = EditorGUILayout.Popup("Key選択", _keyIndex, _keyValues);
            if (EditorGUI.EndChangeCheck())
            {
                if (newKeyIndex != _keyIndex)
                {
                    _keyIndex = newKeyIndex;
                    var key = _keyValues[_keyIndex];
                    _target.key = key;
            
                    var cat = _categoryValues[_categoryIndex];
                    var param = _target.param;
                    var str = string.IsNullOrWhiteSpace(key) || key == NoSelection
                        ? ""
                        : L10NConverter.Instance.GetString(cat, key, param);
                    _text.text = str;
                    EditorUtility.SetDirty(_target);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
            }

            EditorGUI.EndChangeCheck();
        }
    }
#endif
}