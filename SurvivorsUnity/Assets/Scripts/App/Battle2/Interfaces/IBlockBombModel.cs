using App.Battle2.Map.Cells;
using App.Battle2.ValueObjects;
using UniRx;
using UnityEngine;

namespace App.Battle2.Interfaces
{
    /// <summary>
    /// 砲撃を阻害するもののInterface
    /// </summary>
    public interface IBlockBombModel
    {
        /// <summary>
        /// Hexセル位置
        /// </summary>
        IReadOnlyReactiveProperty<HexCell> Cell { get; }

        /// <summary>
        /// Grid
        /// </summary>
        GridValue Grid { get; }

        /// <summary>
        /// Position
        /// </summary>
        Vector3 Position { get; }
       
        /// <summary>
        /// ID
        /// </summary>
        uint Id { get; }
    }
}