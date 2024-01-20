#if UNITY_EDITOR
using System;
using UnityEngine;

namespace App.AppEditor.StageEditor.Records
{
    /// <summary>
    /// 浮遊アイテム設定レコード
    /// </summary>
    [Serializable]
    public record ObjectFloatingItemRecord(string ItemLabel, Color IconColor, int PresetId, string ItemDesc, int Param1, int GetPoint)
        : ObjectRecord(Define.QuestObjectType.FloatingItem, ItemLabel, IconColor, PresetId, ItemDesc, Param1, GetPoint);
}
#endif
