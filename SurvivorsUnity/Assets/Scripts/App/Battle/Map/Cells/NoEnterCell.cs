using Master.Constants;

namespace Master.Battle.Map.Cells
{
    /// <summary>
    /// 侵入不可Cell
    /// </summary>
    public class NoEnterCell : HexCell
    {
        public override MapCellType CellType => MapCellType.NoEnter;
    }
}