using App.Battle.Map;

namespace App.Battle.Units
{
    public record HeroCreateParam(uint HeroUnitId, int Index, HexCell InitCell, string Label);
}