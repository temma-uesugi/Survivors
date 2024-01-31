using App.AppCommon;
using App.Battle2.Map.Cells;

namespace App.Battle2.Units.Ship
{
    /// <summary>
    /// 船の作成パラメータ
    /// </summary>
    public record ShipCreateParam(uint ShipUnitId, int Index, ShipStatus Status, HexCell InitCell, DirectionType InitDirection, string Label);
}