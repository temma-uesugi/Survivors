using App.AppCommon;

namespace App.Battle.Map
{
    /// <summary>
    /// 岩場Cell
    /// </summary>
    public class RockyAreaCell : HexCell
    {
        public override MapCellType CellType => MapCellType.RockyArea;
    }
}