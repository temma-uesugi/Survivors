using App.AppCommon;

namespace App.Battle.Map
{
    /// <summary>
    /// 侵入不可Cell
    /// </summary>
    public class NoEnterCell : HexCell
    {
        public override MapCellType CellType => MapCellType.NoEnter;
    }
}