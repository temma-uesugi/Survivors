using Master.Constants;

namespace App.Battle.Map
{
    /// <summary>
    /// 砂漠Cell
    /// </summary>
    public class DesertCell : HexCell
    {
        public override MapCellType CellType => MapCellType.Desert;
    }
}