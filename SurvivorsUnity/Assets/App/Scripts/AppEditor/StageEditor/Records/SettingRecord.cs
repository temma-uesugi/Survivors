#if UNITY_EDITOR
using UnityEngine;

namespace App.AppEditor.StageEditor.Records
{
    /// <summary>
    /// クエストで使う設定
    /// </summary>
    public record SettingRecord(string ItemLabel, Color IconColor, int PresetId, string ItemDesc)
    {
        public string ItemLabel { get; set; } = ItemLabel;
        public Color IconColor { get; set; } = IconColor;
        public string ItemDesc { get; set; } = ItemDesc;
        
        public bool IsPreset => PresetId != Define.InvalidPresetId;
    }
}
#endif
