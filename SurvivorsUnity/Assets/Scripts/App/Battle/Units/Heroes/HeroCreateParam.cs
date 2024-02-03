using App.Battle.Map;

namespace App.Battle.Units
{
    public record HeroCreateParam(uint HeroUnitId, int FormationIndex, HexCell InitCell, string Label);
}