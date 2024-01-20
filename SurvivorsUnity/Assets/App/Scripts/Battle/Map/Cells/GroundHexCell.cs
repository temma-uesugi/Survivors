namespace App.Battle.Map.Cells
{
    /// <summary>
    /// 地面のセル
    /// </summary>
    public class GroundHexCell : HexCell
    {
        public override Type CellType => Type.Sea;
    }
}