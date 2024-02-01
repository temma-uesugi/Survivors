using Master.Constants;

namespace Master.Battle.Map.Cells
{
    /// <summary>
    /// 川Cell
    /// </summary>
    public class RiverCell : HexCell
    {
        public override MapCellType CellType => MapCellType.River;
    }
}