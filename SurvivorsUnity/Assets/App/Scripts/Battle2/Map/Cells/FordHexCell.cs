namespace App.Battle2.Map.Cells
{
    /// <summary>
    /// 浅瀬のセル
    /// </summary>
    public class FordHexCell : HexCell
    {
        public override Type CellType => Type.Sea;
    }
}