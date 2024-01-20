using App.AppCommon;
using App.Battle.Map.Cells;
using App.Battle.ValueObjects;

namespace App.Battle.Units.Ship
{
    /// <summary>
    /// 船の作成パラメータ
    /// </summary>
    public record ShipCreateParam(uint ShipUnitId, int Index, ShipStatus Status, HexCell InitCell, DirectionType InitDirection, string Label);
}