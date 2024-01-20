#if UNITY_EDITOR
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

namespace App.AppEditor.Common
{
    /// <summary>
    /// VisualElementの拡張
    /// </summary>
    public static class VisualElementExtensions
    {
        /// <summary>
        /// 複数classを付与
        /// </summary>
        public static void AddToClassesList(this VisualElement self, string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                return;
            }
            foreach (var cls in Regex.Split(className, "\\s+"))
            {
                self.AddToClassList(cls);
            }
        }

        /// <summary>
        /// 属性を付与
        /// </summary>
        public static VisualElement Attr(this VisualElement self, string key, string value)
        {
            if (self.userData == null)
            {
                self.userData = "";
            }
            self.userData += $"{key}={value};";
            return self;
        }

        /// <summary>
        /// 属性を取得
        /// </summary>
        public static string Attr(this VisualElement self, string key)
        {
            var res = "";
            if (self?.userData == null)
            {
                return res;
            }

            var attrStrList = self.userData.ToString().Split(";");
            foreach (var attrStr in attrStrList)
            {
                var attr = attrStr.Split("=");
                if (attr[0] == key && !string.IsNullOrEmpty(attr[1]))
                {
                    res = attr[1];
                    break;
                }
            }
            return res;
        }

        /// <summary>
        /// テキストありのElementをAppend
        /// </summary>
        public static VisualElement AppendWithText<T>(this VisualElement self, string text, params (string key, string val)[] attrs) where T : VisualElement, new()
        {
            return self.AppendWithText<T>(text, id: "", className: "", attrs);
        }

        /// <summary>
        /// テキストありのElementをAppend
        /// </summary>
        public static VisualElement AppendWithText<T>(this VisualElement self, string text, string className, params (string key, string val)[] attrs) where T : VisualElement, new()
        {
            return self.AppendWithText<T>(text, id: "", className: className, attrs);
        }
        
        /// <summary>
        /// テキストありのElementをAppend
        /// </summary>
        public static VisualElement AppendWithText<T>(this VisualElement self, string text, string id, string className, params (string key, string val)[] attrs) where T : VisualElement, new()
        {
            var elem = new T();
            var textElem = elem.Q<TextElement>();
            if (textElem == null)
            {
                textElem = new TextElement();
                elem.Add(textElem);
            }
            textElem.text = text;
            if (!string.IsNullOrEmpty(id))
            {
                elem.name = id;
            }
            elem.AddToClassesList(className);
            foreach (var attr in attrs)
            {
                elem.Attr(attr.key, attr.val);
            }
            self.Add(elem);
            return self;
        }

        /// <summary>
        /// ElementをAppend
        /// </summary>
        public static VisualElement Append<T>(this VisualElement self, out T appended) where T : VisualElement, new()
        {
            return self.Append(id: null, className: null, out appended);
        }
        
        /// <summary>
        /// ElementをAppend
        /// </summary>
        public static VisualElement Append<T>(this VisualElement self, string className, out T appended) where T : VisualElement, new()
        {
            return self.Append(id: null, className: className, out appended);
        }

        /// <summary>
        /// ElementをAppend
        /// </summary>
        public static VisualElement Append<T>(this VisualElement self, string id, string className, out T appended) where T : VisualElement, new()
        {
            var elem = new T();
            if (!string.IsNullOrEmpty(id))
            {
                elem.name = id;
            }
            elem.AddToClassesList(className);
            self.Add(elem);
            appended = elem;
            return self;
        }

        /// <summary>
        /// 要素の後ろ(兄弟要素として)に追加
        /// </summary>
        public static VisualElement After<T>(this VisualElement self, string className, out T added)
            where T : VisualElement, new()
        {
            return self.After(id: null, className, out added);
        }
        
        /// <summary>
        /// 要素の後ろ(兄弟要素として)に追加
        /// </summary>
        public static VisualElement After<T>(this VisualElement self, string id, string className, out T added)
            where T : VisualElement, new()
        {
            added = new T();
            if (!string.IsNullOrEmpty(id))
            {
                added.name = id;
            }
            added.AddToClassesList(className);
            var selfIndex = self.parent.IndexOf(self);
            self.parent.Insert(selfIndex + 1, added);
            return self;
        }

        /// <summary>
        /// TableをAppend
        /// </summary>
        public static VisualElement[][] AppendTable(this VisualElement self, int rowNum, int colNum)
        {
            self.Append<Box>(id: null, className: "table", out var table);
            var colMap = new VisualElement[rowNum][];
            for (int r = 0; r < rowNum; r++)
            {
                table.Append<Box>(id: null, className: "row", out var row);
                row.Attr("row", r.ToString());
                colMap[r] = new VisualElement[colNum];
                for (int c = 0; c < colNum; c++)
                {
                    row.Append<Box>(id: null, className: "col", out var col);
                    col.Attr("col", c.ToString());
                    colMap[r][c] = col;
                }
            }
            return colMap;
        }
    }
}
#endif