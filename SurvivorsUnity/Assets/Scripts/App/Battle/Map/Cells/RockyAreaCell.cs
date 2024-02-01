using Master.Constants;

namespace Master.Battle.Map.Cells
{
    /// <summary>
    /// 岩場Cell
    /// </summary>
    public class RockyAreaCell : HexCell
    {
        public override MapCellType CellType => MapCellType.RockyArea;
    }
}