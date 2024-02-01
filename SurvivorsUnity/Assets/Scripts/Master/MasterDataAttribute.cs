using System;

namespace Master
{
    /// <summary>
    /// マスタ属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MasterDataAttribute : Attribute
    {
        public string Category { get; }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MasterDataAttribute(string category)
        {
            Category = category;
        }
    }
}