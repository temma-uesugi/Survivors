using Master.Constants;

namespace App.Battle.Map
{
    /// <summary>
    /// 川Cell
    /// </summary>
    public class RiverCell : HexCell
    {
        public override MapCellType CellType => MapCellType.River;
    }
}