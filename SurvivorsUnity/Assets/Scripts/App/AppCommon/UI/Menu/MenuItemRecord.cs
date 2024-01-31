using System;

namespace App.AppCommon.UI
{
    /// <summary>
    /// MenuItem
    /// </summary>
    public record MenuItemRecord<T>(T ItemType) where T : Enum;
}