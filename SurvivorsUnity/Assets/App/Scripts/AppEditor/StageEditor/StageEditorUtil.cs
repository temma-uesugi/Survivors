using App.AppEditor.StageEditor.Records;

#if UNITY_EDITOR
namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// Util関数
    /// </summary>
    public static class StageEditorUtil
    {
        /// <summary>
        /// GridXを位置Xに
        /// </summary>
        public static float ToPositionX(uint width, int x) => (x - width / 2f - 0.5f);

        /// <summary>
        /// GridXを位置Xの文字列に
        /// </summary>
        public static string ToPositionXStr(uint width, int x) => ToPositionX(width, x).ToString("0.#");

        /// <summary>
        /// GridYを位置Yに
        /// </summary>
        public static float ToPositionY(uint height, int y) => (y - height / 2f - 0.5f);

        /// <summary>
        /// GridYを位置Yの文字列に
        /// </summary>
        public static string ToPositionYStr(uint height, int y) => ToPositionY(height, y).ToString("0.#");

        /// <summary>
        /// レコードをJson文字列に
        /// </summary>
        public static string RecordToJsonStr(SettingRecord record)
        {
            if (record is ObjectRecord objRecord)
            {
                var objPresetData = new ObjectSettingData
                {
                    PresetId = record.PresetId,
                    ItemLabel = record.ItemLabel,
                    IconColor = record.IconColor,
                    IconDesc = record.ItemDesc,
                    ObjectType = objRecord.ObjectType,
                    ObjectParam1 = objRecord.ObjectParam1,
                    GetPoint = objRecord.GetPoint,
                };
                var objBytes = MessagePack.MessagePackSerializer.Serialize((SettingData)objPresetData);
                var objPresetJson = MessagePack.MessagePackSerializer.ConvertToJson(objBytes);
                return objPresetJson;
            }
            if (record is EnemyRecord enemyRecord)
            {
                var enemyPresetData = new EnemySettingData
                {
                    PresetId = record.PresetId,
                    ItemLabel = record.ItemLabel,
                    IconColor = record.IconColor,
                    IconDesc = record.ItemDesc,
                    EnemyType = enemyRecord.EnemyType,
                    EnemyParam1 = enemyRecord.EnemyParam1,
                    EnemyParam2 = enemyRecord.EnemyParam2,
                    Level = enemyRecord.Level,
                    GetPoint = enemyRecord.GetPoint,
                    SpawnGameTime = enemyRecord.SpawnGameTime,
                    IsBoss = enemyRecord.IsBoss,
                    IsStealth = enemyRecord.IsStealth,
                };
                var enemyBytes = MessagePack.MessagePackSerializer.Serialize((SettingData)enemyPresetData);
                var enemyPresetJson = MessagePack.MessagePackSerializer.ConvertToJson(enemyBytes);
                return enemyPresetJson;
            }
            return null;
        }

        /// <summary>
        /// JsonををSettingDataに
        /// </summary>
        public static SettingData JsonToSettingData(string json)
        {
            var bytes = MessagePack.MessagePackSerializer.ConvertFromJson(json);
            var settingData = MessagePack.MessagePackSerializer.Deserialize<SettingData>(bytes);
            return settingData;
        }

        /// <summary>
        /// SettingDataをSettingRecordに
        /// </summary>
        public static SettingRecord SettingDataToRecord(SettingData presetData)
        {
            if (presetData is ObjectSettingData objData)
            {
                return objData.ObjectType switch
                {
                    Define.QuestObjectType.RelayPoint => new ObjectRelayPointRecord(objData.ItemLabel, objData.IconColor,
                        objData.PresetId, objData.IconDesc, objData.ObjectParam1, objData.GetPoint),
                    Define.QuestObjectType.Obstacle => new ObjectObstacleRecord(objData.ItemLabel, objData.IconColor,
                        objData.PresetId, objData.IconDesc, objData.ObjectParam1, objData.GetPoint),
                    Define.QuestObjectType.FloatingItem => new ObjectFloatingItemRecord(objData.ItemLabel, objData.IconColor,
                        objData.PresetId, objData.IconDesc, objData.ObjectParam1, objData.GetPoint),
                    _ => null,
                };
            }
            if (presetData is EnemySettingData enemyData)
            {
                return enemyData.EnemyType switch
                {
                    Define.QuestEnemyType.Actor => new EnemyActorRecord(enemyData.ItemLabel, enemyData.IconColor,
                        enemyData.PresetId, enemyData.IconDesc, enemyData.EnemyParam1, enemyData.EnemyParam2, enemyData.Level,
                        enemyData.GetPoint, enemyData.SpawnGameTime, enemyData.IsBoss, enemyData.IsStealth),
                    Define.QuestEnemyType.Team => new EnemyTeamRecord(enemyData.ItemLabel, enemyData.IconColor,
                        enemyData.PresetId, enemyData.IconDesc, enemyData.EnemyParam1, enemyData.EnemyParam2, enemyData.Level,
                        enemyData.GetPoint, enemyData.SpawnGameTime, enemyData.IsBoss, enemyData.IsStealth),
                    Define.QuestEnemyType.MobDraNest => new EnemyMobDraNestRecord(enemyData.ItemLabel, enemyData.IconColor,
                        enemyData.PresetId, enemyData.IconDesc, enemyData.EnemyParam1, enemyData.EnemyParam2, enemyData.Level,
                        enemyData.GetPoint, enemyData.SpawnGameTime, enemyData.IsBoss, enemyData.IsStealth),
                    Define.QuestEnemyType.QuestEnemyGroup => new EnemyQuestEnemyGroupRecord(enemyData.ItemLabel, enemyData.IconColor,
                        enemyData.PresetId, enemyData.IconDesc, enemyData.EnemyParam1, enemyData.EnemyParam2, enemyData.Level,
                        enemyData.GetPoint, enemyData.SpawnGameTime, enemyData.IsBoss, enemyData.IsStealth),
                    _ => null,
                };
            }
            return null;
        }
    }
}
#endif
