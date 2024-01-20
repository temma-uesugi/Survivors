using App.Battle2.Map.Cells;

namespace App.Battle2.Objects
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