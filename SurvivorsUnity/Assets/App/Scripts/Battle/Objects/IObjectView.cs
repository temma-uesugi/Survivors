using App.Battle.Map.Cells;
using UnityEngine;

namespace App.Battle.Objects
{
    /// <summary>
    /// ObjectViewにつくインターフェイス
    /// </summary>
    public interface IObjectView
    {
        /// <summary>
        /// ObjectId
        /// </summary>
        uint ObjectId { get; }

        /// <summary>
        /// Cell
        /// </summary>
        HexCell Cell { get; } 
    }
}