using System;

namespace App.AppCommon.UI.Menu
{
    /// <summary>
    /// MenuItem
    /// </summary>
    public record MenuItemRecord<T>(T ItemType) where T : Enum;
}