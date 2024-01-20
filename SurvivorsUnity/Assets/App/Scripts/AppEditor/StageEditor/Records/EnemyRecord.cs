#if UNITY_EDITOR
using UnityEngine;

namespace App.AppEditor.StageEditor.Records
{
    /// <summary>
    /// 敵レコード基本
    /// </summary>
    public record EnemyRecord(Define.QuestEnemyType EnemyType, string ItemLabel, Color IconColor, int PresetId, string ItemDesc,
        int EnemyParam1, int EnemyParam2, int Level, int GetPoint, int SpawnGameTime, bool IsBoss, bool IsStealth)
        : SettingRecord(ItemLabel, IconColor, PresetId, ItemDesc)
    {
        public int EnemyParam1 { get; set; } = EnemyParam1;
        public int EnemyParam2 { get; set; } = EnemyParam2;
        public int Level { get; set; } = Level;
        public int GetPoint { get; set; } = GetPoint;
        public int SpawnGameTime { get; set; } = SpawnGameTime;
        public bool IsBoss { get; set; } = IsBoss;
        public bool IsStealth { get; set; } = IsStealth;
    }
}
#endif
