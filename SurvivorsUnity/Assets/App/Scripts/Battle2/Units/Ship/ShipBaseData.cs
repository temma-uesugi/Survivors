namespace App.Battle2.Units.Ship
{
    /// <summary>
    /// 船のデータ
    /// </summary>
    public record ShipBaseData
    {
        //移動速度(=移動距離)
        public int MoveSpeed { get; init; }
        public int MaxHp { get; init; }
        public int MaxCp { get; init; }
    }
}