using App.AppCommon;
using App.Battle.Map.Cells;
using App.Battle.ValueObjects;
using UniRx;
using UnityEngine;

namespace App.Battle.Interfaces
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