using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using App.AppCommon.Core;
using App.AppCommon.Libs;

namespace App.Localization
{
    /// <summary>
    /// ローカイライズ変換
    /// </summary>
    public class L10NConverter : SingletonBase<L10NConverter>
    {
        //KeyMap
        private readonly Dictionary<string, string[]> _keyMap = new();
        public IEnumerable<string> Categories => _keyMap.Keys;
        public IEnumerable<string> Keys(string cat) =>
            _keyMap.TryGetValue(cat, out var keys) ? keys : Enumerable.Empty<string>();

        //cat-key-文字列[]
        private readonly Dictionary<string, Dictionary<string, string[]>> _catEnumMap = new();

        private int _langIndex = 0;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public L10NConverter()
        {
            Type keyType = typeof(L10NKey);
            var types = keyType.GetNestedTypes(BindingFlags.Public | BindingFlags.Static);
            foreach (var type in types)
            {
                if (type.IsEnum)
                {
                    Array enumValues = Enum.GetValues(type);
                    string[] enumNames = new string[enumValues.Length];
                    for (int i = 0; i < enumValues.Length; i++)
                    {
                        enumNames[i] = enumValues.GetValue(i).ToString();
                    }
                    
                    _keyMap[type.Name] = enumNames;
                }
            }
           
            L10NString ins = new L10NString();
            Type stringType = typeof(L10NString);
            PropertyInfo[] properties = stringType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                L10NAttribute attr = property.GetCustomAttribute<L10NAttribute>(false);
                if (attr != null)
                {
                    var stringMap = new Dictionary<string, string[]>();
                    IDictionary<string, string[]> dictionary = (IDictionary<string, string[]>)property.GetValue(ins);
                    foreach (var key in dictionary.Keys)
                    {
                        stringMap.Add(key, dictionary[key]);
                    }
                    _catEnumMap.Add(attr.CategoryType.Name, stringMap); 
                }
            }
        }

        /// <summary>
        /// 言語Indexをセット
        /// </summary>
        public void SetLangIndex(int index) => _langIndex = index;
        
        /// <summary>
        /// 文字列取得
        /// </summary>
        public string GetString(string cat, string key, params string[] param)
        {
            object[] paramConverted = param.Select(x =>
            {
                if (int.TryParse(x, out var intRes)) return (object)intRes;
                if (float.TryParse(x, out var floatRes)) return (object)floatRes;
                if (double.TryParse(x, out var doubleRes)) return (object)doubleRes;
                if (Decimal.TryParse(x, out var decimalRes)) return (object)decimalRes;
                return x;
            }).ToArray();
            return GetString(cat, key, paramConverted);
            // if (!_catEnumMap.TryGetValue(cat, out var enumMap) || !enumMap.TryGetValue(key, out var strings))
            // {
            //     Log.Error("Localization String Error");
            //     return "";
            // }
            // return string.Format(strings[_langIndex], paramConverted);
        }

        /// <summary>
        /// 文字列取得
        /// </summary>
        public string GetString(string cat, string key, params object[] param)
        {
            if (!_catEnumMap.TryGetValue(cat, out var enumMap) || !enumMap.TryGetValue(key, out var strings))
            {
                Log.Error("Localization String Error");
                return "";
            }
            return string.Format(strings[_langIndex], param);
        }

    }
}