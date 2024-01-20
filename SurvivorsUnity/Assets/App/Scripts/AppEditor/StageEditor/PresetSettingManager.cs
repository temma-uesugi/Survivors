#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using App.AppEditor.StageEditor.Records;
using Newtonsoft.Json;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// プリセット設定管理
    /// </summary>
    public class PresetSettingManager
    {
        private static readonly string JsonPath = "Assets/Scenes/QuestStageEditor/Data/preset.json";

        private int seqId = 0;
        private readonly Dictionary<int, SettingRecord> recordMap = new();
        public IEnumerable<SettingRecord> AllRecords => recordMap.Values;
        public SettingRecord this[int id] => recordMap.TryGetValue(id, out var rec) ? rec : null;

        public HashSet<SettingRecord> UpdatedRecords { get; private set; } = new();
        public HashSet<SettingRecord> RemovedRecords { get; private set; } = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PresetSettingManager()
        {
            //jsonファイル取得
            if (!File.Exists(JsonPath))
            {
                return;
            }
            using StreamReader sr = new StreamReader(JsonPath, Encoding.UTF8);
            var json = sr.ReadToEnd();
            var jsonObj = JsonConvert.DeserializeObject<PresetData[]>(json);
            if (jsonObj == null)
            {
                return;
            }
            foreach (var obj in jsonObj)
            {
                try
                {
                    var settingData = StageEditorUtil.JsonToSettingData(obj.Json);
                    recordMap.Add(obj.Id, StageEditorUtil.SettingDataToRecord(settingData));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            seqId = recordMap.Max(x => x.Key);
        }

        /// <summary>
        /// プリセット登録
        /// </summary>
        public SettingRecord SetPreset(SettingRecord record)
        {
            seqId++;
            var presetRecord = record with { PresetId = seqId };
            recordMap.Add(seqId, presetRecord);
            WriteJson();
            return presetRecord;
        }

        /// <summary>
        /// TryGet
        /// </summary>
        public bool TryGet(int id, out SettingRecord value)
        {
            if (recordMap.TryGetValue(id, out value))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Json書き出し
        /// </summary>
        private void WriteJson()
        {
            var dataList = recordMap //
                .Select(x => (id: x.Key, json: StageEditorUtil.RecordToJsonStr(x.Value))) //
                .Where(x => !string.IsNullOrEmpty(x.json)) //
                .Select(x => new PresetData //
                {
                    Id = x.id,
                    Json = x.json
                })//
                .ToArray();
            var jsonStr = JsonConvert.SerializeObject(dataList, Formatting.Indented);
            using StreamWriter sw = new StreamWriter(JsonPath, false, Encoding.UTF8);
            sw.Write(jsonStr);
        }

        /// <summary>
        /// プリセットの除去
        /// </summary>
        public void RemovePreset(SettingRecord record)
        {
            RemovedRecords.Add(record);
        }

        /// <summary>
        /// プリセットの更新
        /// </summary>
        public void UpdatePreset(SettingRecord record)
        {
            UpdatedRecords.Add(record);
        }

        /// <summary>
        /// 保存してリセット
        /// </summary>
        public void SaveAndReset()
        {
            foreach (var removeRecord in RemovedRecords)
            {
                if (recordMap.ContainsKey(removeRecord.PresetId))
                {
                    recordMap.Remove(removeRecord.PresetId);
                }
            }
            WriteJson();
            UpdatedRecords.Clear();
            RemovedRecords.Clear();
        }
    }
}
#endif
