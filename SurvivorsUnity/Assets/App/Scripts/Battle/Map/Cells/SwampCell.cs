using App.AppCommon;

namespace App.Battle.Map
{
    /// <summary>
    /// 沼Cell
    /// </summary>
    public class SwampCell : HexCell
    {
        public override MapCellType CellType => MapCellType.Swamp;
    }
}