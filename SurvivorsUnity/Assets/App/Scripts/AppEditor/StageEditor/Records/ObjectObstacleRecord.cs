#if UNITY_EDITOR
using System;
using UnityEngine;

namespace App.AppEditor.StageEditor.Records
{
    /// <summary>
    /// 障害物設定レコード
    /// </summary>
    [Serializable]
    public record ObjectObstacleRecord(string ItemLabel, Color IconColor, int PresetId, string ItemDesc, int Param1, int GetPoint)
        : ObjectRecord(Define.QuestObjectType.Obstacle, ItemLabel, IconColor, PresetId, ItemDesc, Param1, GetPoint);
}
#endif
