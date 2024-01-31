using System;

namespace App.Localization
{
    /// <summary>
    /// ローカライゼーションの属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class L10NAttribute : Attribute
    {
        public Type CategoryType { get; private set; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public L10NAttribute(Type categoryType)
        {
            CategoryType = categoryType;
        }
    }
}