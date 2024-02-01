using Master.Battle.Map.Cells;

namespace Master.Battle.Units.Heroes
{
    public record HeroCreateParam(uint HeroUnitId, int Index, HexCell InitCell, string Label);
}