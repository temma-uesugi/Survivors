namespace App.Battle.Map.Cells
{
    /// <summary>
    /// 海のセル
    /// </summary>
    public class SeaHexCell : HexCell
    {
        public override Type CellType => Type.Sea;
        
        public int WaveHeight { get; private set; }

        /// <summary>
        /// Setup
        /// </summary>
        public void Setup(int x, int y, int waveHeight)
        {
            Setup(x, y); 
            UpdateHeight(waveHeight);
        }

        /// <summary>
        /// 並の高さ変更
        /// </summary>
        public void UpdateHeight(int waveHeight)
        {
            WaveHeight = waveHeight;
        }
    }
}