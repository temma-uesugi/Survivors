#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using App.AppEditor.StageEditor.Records;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// ItemData管理
    /// </summary>
    public class ItemDataManager
    {
        private int seqId = 0;
        private readonly Dictionary<int, ItemData> itemMap = new();
        public IEnumerable<ItemData> AllItem => itemMap.Values;

        /// <summary>
        /// Reset
        /// </summary>
        public void Reset()
        {
            seqId = 0;
            itemMap.Clear();
        }

        /// <summary>
        /// アイテム追加
        /// </summary>
        public ItemData AddItem(SettingRecord settingRecord, int x, int y)
        {
            var record = settingRecord.IsPreset
                ? settingRecord //プリセットなのでインスタンスを紐づける
                : settingRecord with {}; //プリセットではないのでcloneする
            seqId++;
            var itemData = new ItemData(seqId, x, y, record);
            itemMap.Add(seqId, itemData);
            return itemData;
        }

        /// <summary>
        /// 指定グリッドのアイテムを取得
        /// </summary>
        public List<ItemData> GetAllByGrid(int x, int y)
        {
            return itemMap.Values.Where(val => val.X == x && val.Y == y).ToList();
        }

        /// <summary>
        /// 指定グリッドの全アイテムてを削除
        /// </summary>
        public List<ItemData> RemoveGridAll(int x, int y)
        {
            var gridItems = GetAllByGrid(x, y);
            foreach (var itemData in gridItems)
            {
                itemMap.Remove(itemData.Id);
            }
            return gridItems;
        }

        /// <summary>
        /// 除去
        /// </summary>
        public void Remove(int id)
        {
            if (itemMap.ContainsKey(id))
            {
                itemMap.Remove(id);
            }
        }

        /// <summary>
        /// プリセットに交換
        /// </summary>
        public void SwitchPresetRecord(int id, SettingRecord presetRecord)
        {
            if (itemMap.TryGetValue(id, out var itemData))
            {
                itemData.Record = presetRecord;
            }
        }

        /// <summary>
        /// 該当プリセットとのリンクを全解除
        /// </summary>
        public List<(int id, SettingRecord record)> UnlinkPresetAll(SettingRecord presetRecord)
        {
            var list = new List<(int id, SettingRecord record)>();
            var keys = itemMap.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                var id = keys[i];
                if (itemMap[id].Record != presetRecord)
                {
                    continue;
                }
                var cloneRecord = UnlinkPreset(id);
                list.Add((id, cloneRecord));
            }
            return list;
        }

        /// <summary>
        /// プリセットの解除
        /// </summary>
        public SettingRecord UnlinkPreset(int itemId)
        {
            if (!itemMap.TryGetValue(itemId, out var itemData))
            {
                return null;
            }
            var cloneRecord = itemData.Record with { PresetId = Define.InvalidPresetId };
            itemData.Record = cloneRecord;
            itemMap[itemId] = itemData;
            return cloneRecord;
        }
    }
}
#endif
