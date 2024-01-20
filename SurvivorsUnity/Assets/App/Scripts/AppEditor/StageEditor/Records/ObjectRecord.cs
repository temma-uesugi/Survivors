#if UNITY_EDITOR
using UnityEngine;

namespace App.AppEditor.StageEditor.Records
{
    /// <summary>
    /// クエストで使うObjectの設定
    /// </summary>
    public record ObjectRecord(Define.QuestObjectType ObjectType, string ItemLabel, Color IconColor, int PresetId, string ItemDesc,
        int ObjectParam1, int GetPoint) : SettingRecord(ItemLabel, IconColor, PresetId, ItemDesc)
    {
        public int ObjectParam1 { get; set; } = ObjectParam1;
        public int GetPoint { get; set; } = GetPoint;
    }
}
#endif
