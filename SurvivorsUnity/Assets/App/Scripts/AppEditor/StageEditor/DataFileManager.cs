#if UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Text;
using App.AppEditor.StageEditor.Records;
using Newtonsoft.Json;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// データファイル管理
    /// </summary>
    public static class DataFileManager
    {
        private static readonly string DirPath = "Assets/Scenes/QuestStageEditor/Data/Stages";

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save(StageEditorSetting setting, int initPosX, int initPosY, ItemData[] items)
        {
            var filePath = Path.Combine(DirPath, $"{setting.QuestId}_{setting.StageNo}.json");
            var data = new StageSaveData
            {
                Width = setting.Width,
                Height = setting.Height,
                QuestId = setting.QuestId,
                StageNo = setting.StageNo,
                FieldId = setting.FieldId,
                TimeLimitSec = setting.TimeLimitSec,
                NeedPoint = setting.NeedPoint,
                MotherBasePositionX = initPosX,
                MotherBasePositionY = initPosY,
                Objects = items //
                    .Where(x => x.Record is ObjectRecord) //
                    .Select(val => new ItemSaveData //
                    {
                        Id = val.Id,
                        X = val.X,
                        Y = val.Y,
                        DataJson = StageEditorUtil.RecordToJsonStr(val.Record),
                    }) //
                    .ToArray(),
                Enemies = items //
                    .Where(x => x.Record is EnemyRecord) //
                    .Select(val => new ItemSaveData //
                    {
                        Id = val.Id,
                        X = val.X,
                        Y = val.Y,
                        DataJson = StageEditorUtil.RecordToJsonStr(val.Record),
                    }) //
                    .ToArray(),
            };
            var jsonStr = JsonConvert.SerializeObject(data, Formatting.Indented);
            using StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8);
            sw.Write(jsonStr);
        }

        /// <summary>
        /// データ取得
        /// </summary>
        public static StageSaveData GetData(uint questId, uint stageNo)
        {
            var filePath = Path.Combine(DirPath, $"{questId}_{stageNo}.json");
            if (!File.Exists(filePath))
            {
                return null;
            }
            using StreamReader sr = new StreamReader(filePath, Encoding.UTF8);
            var json = sr.ReadToEnd();
            var data = JsonConvert.DeserializeObject<StageSaveData>(json);
            return data;
        }

    }
}
#endif
