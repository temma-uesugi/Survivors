#if UNITY_EDITOR
using System;
using UnityEngine;

namespace App.AppEditor.StageEditor.Records
{
    /// <summary>
    /// 中継地点設定レコード
    /// </summary>
    [Serializable]
    public record ObjectRelayPointRecord(string ItemLabel, Color IconColor, int PresetId, string ItemDesc, int Param1, int GetPoint)
        : ObjectRecord(Define.QuestObjectType.RelayPoint, ItemLabel, IconColor, PresetId, ItemDesc, Param1, GetPoint)
    {

    }
}
#endif
