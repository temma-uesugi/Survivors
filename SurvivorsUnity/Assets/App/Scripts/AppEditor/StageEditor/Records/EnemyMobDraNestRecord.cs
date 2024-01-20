#if UNITY_EDITOR
using System;
using UnityEngine;

namespace App.AppEditor.StageEditor.Records
{
    /// <summary>
    /// ちびドラの巣レコード
    /// </summary>
    [Serializable]
    public record EnemyMobDraNestRecord(string ItemLabel, Color IconColor, int PresetId, string ItemDesc,
        int EnemyParam1, int EnemyParam2, int Level,int GetPoint, int SpawnGameTime, bool IsBoss, bool IsStealth)
        : EnemyRecord(Define.QuestEnemyType.MobDraNest, ItemLabel, IconColor, PresetId, ItemDesc,
        EnemyParam1, EnemyParam2, Level, GetPoint, SpawnGameTime, IsBoss, IsStealth);
}
#endif
