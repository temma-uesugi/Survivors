using App.AppEditor.StageEditor.Records;

#if UNITY_EDITOR
namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// ItemData
    /// </summary>
    public record ItemData(int Id, int X, int Y, SettingRecord Record)
    {
        public SettingRecord Record { get; set; } = Record;
    }
}
#endif
