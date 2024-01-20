using App.Battle2.Map.Cells;

namespace App.Battle2.Units.Enemy
{

    /// <summary>
    /// 敵作成パラメータ
    /// </summary>
    public record EnemyCreateParam(uint EnemyUnitId, uint EnemyId, int Level, HexCell InitCell, string Label);
}