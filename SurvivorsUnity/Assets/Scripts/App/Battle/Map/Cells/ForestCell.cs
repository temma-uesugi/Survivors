using App.AppCommon;

namespace App.Battle.Map
{
    /// <summary>
    /// 森Cell
    /// </summary>
    public class ForestCell : HexCell
    {
        public override MapCellType CellType => MapCellType.Forest;
    }
}