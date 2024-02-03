using App.Battle.Map;

namespace App.Battle.Units
{
    /// <summary>
    /// 敵作成パラメータ
    /// </summary>
    public record EnemyCreateParam(uint EnemyUnitId, uint EnemyId, int Level, HexCell InitCell, string Label);
}
