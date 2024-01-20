using App.AppCommon;

namespace App.Battle.Map
{
    /// <summary>
    /// 茂みCell
    /// </summary>
    public class BushesCell : HexCell
    {
        public override MapCellType CellType => MapCellType.Bushes;
    }
}