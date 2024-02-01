using Master.Battle.Map.Cells;

namespace Master.Battle.Units.Enemys
{
    /// <summary>
    /// 敵作成パラメータ
    /// </summary>
    public record EnemyCreateParam(uint EnemyUnitId, uint EnemyId, int Level, HexCell InitCell, string Label);
}
