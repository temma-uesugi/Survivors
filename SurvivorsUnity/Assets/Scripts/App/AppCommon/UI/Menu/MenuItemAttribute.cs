using System;

namespace App.AppCommon.UI.Menu
{
    /// <summary>
    /// MenuItemとして登録しない
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class IgnoreMenuItemAttribute : Attribute
    {
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class OrderAttribute : Attribute
    {
        public int Order { get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OrderAttribute(int order)
        {
            Order = order; 
        }
    }
}