#if UNITY_EDITOR
namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// ステージ保存データ
    /// </summary>
    public class StageSaveData
    {
        public uint Width { get; set; }
        public uint Height { get; set; }
        public uint QuestId { get; set; }
        public uint StageNo { get; set; }
        public uint FieldId { get; set; }
        public uint TimeLimitSec { get; set; }
        public uint NeedPoint { get; set; }
        public int MotherBasePositionX { get; set; }
        public int MotherBasePositionY { get; set; }
        public ItemSaveData[] Objects { get; set; }
        public ItemSaveData[] Enemies { get; set; }
    }

    /// <summary>
    /// アイテム保存データ
    /// </summary>
    public class ItemSaveData
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string DataJson { get; set; }
    }

}
#endif
