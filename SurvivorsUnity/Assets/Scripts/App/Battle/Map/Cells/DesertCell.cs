using Master.Constants;

namespace Master.Battle.Map.Cells
{
    /// <summary>
    /// 砂漠Cell
    /// </summary>
    public class DesertCell : HexCell
    {
        public override MapCellType CellType => MapCellType.Desert;
    }
}