using Master.Constants;

namespace Master.Battle.Map.Cells
{
    /// <summary>
    /// 森Cell
    /// </summary>
    public class ForestCell : HexCell
    {
        public override MapCellType CellType => MapCellType.Forest;
    }
}