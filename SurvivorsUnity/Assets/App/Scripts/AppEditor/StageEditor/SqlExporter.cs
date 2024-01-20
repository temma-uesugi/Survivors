#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using App.AppEditor.StageEditor.Records;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// Sql吐き出し
    /// </summary>
    public static class SqlExporter
    {
        //QuestStages Delete
        private static readonly string DeleteStageQuery = "delete from QuestStages where QuestStageId = {0};";
        //挿入
        private static readonly string InsertStageQuery = "insert into QuestStages {0} values \n{1};";

        /// <summary>
        /// QuestStages挿入カラム
        /// </summary>
        private static readonly string InsertStageColumn = @"
(
QuestStageId,
QuestId,
StageNo,
FieldId,
TimeLimitSec,
NeedPoint,
MotherBasePositionX,
MotherBasePositionY,
IsActiveRecord
)";
        /// <summary>
        /// 設定情報をStageデータのインサートValueに
        /// </summary>
        private static string SettingToStageInsertValue(StageEditorSetting setting, int initPosX, int initPosY)
        {
            return InsertStageColumn //
                .Replace("QuestStageId", setting.QuestStageId.ToString()) //
                .Replace("QuestId", setting.QuestId.ToString()) //
                .Replace("StageNo", setting.StageNo.ToString()) //
                .Replace("FieldId", setting.FieldId.ToString()) //
                .Replace("TimeLimitSec", setting.TimeLimitSec.ToString()) //
                .Replace("NeedPoint", setting.NeedPoint.ToString()) //
                .Replace("MotherBasePositionX", StageEditorUtil.ToPositionXStr(setting.Width, initPosX)) //
                .Replace("MotherBasePositionY", StageEditorUtil.ToPositionXStr(setting.Height, initPosY)) //
                .Replace("IsActiveRecord", "1");
        }

        private static int objId = 0;
        //QuestStageObjects Delete
        private static readonly string DeleteStageObjectQuery = "delete from QuestStageObjects where QuestStageId = {0};";
        //挿入
        private static readonly string InsertStageObjectQuery = "insert into QuestStageObjects {0} values \n{1};";
        //Preset削除
        private static readonly string DeletePresetStageObejctQuery = "update QuestStageObjects set PresetId = 0 where PresetId = {0};";

        /// <summary>
        /// QuestStageObjects挿入カラム
        /// </summary>
        private static readonly string InsertStageObjectColumn = @"
(
QuestStageObjectId,
QuestStageId,
ObjectType,
ObjectParam1,
PositionX,
PositionY,
GetPoint,
IsActiveRecord,
PresetId
)";

        /// <summary>
        /// オブジェクトRecordをObjectデータのインサートValueに
        /// </summary>
        private static string ObjectRecordToInsertValue(StageEditorSetting setting, ObjectRecord objRecord, int x, int y)
        {
            objId++;
            var id = $"{setting.QuestStageId}{objId:D3}";
            return InsertStageObjectColumn //
                .Replace("QuestStageObjectId", id) //
                .Replace("QuestStageId", setting.QuestStageId.ToString()) //
                .Replace("ObjectType", ((int)objRecord.ObjectType).ToString()) //
                .Replace("ObjectParam1", objRecord.ObjectParam1.ToString()) //
                .Replace("PositionX", StageEditorUtil.ToPositionXStr(setting.Width, x)) //
                .Replace("PositionY", StageEditorUtil.ToPositionXStr(setting.Height, y)) //
                .Replace("GetPoint", objRecord.GetPoint.ToString()) //
                .Replace("IsActiveRecord", "1") //
                .Replace("PresetId", objRecord.PresetId.ToString())  //
                + ","; //カンマを入れる、、最後の行はStringBuilder展開前に削除する
        }

        /// <summary>
        /// ObjectのPresetRecordをUpdate文に
        /// </summary>
        private static string ObjectPresetRecordToUpdateQuery(ObjectRecord objectRecord)
        {
            var sql =  @$"update QuestStageObjects set
ObjectParam1 = {objectRecord.ObjectParam1},
GetPoint = {objectRecord.GetPoint}
where PresetId = {objectRecord.PresetId}
;";
            return BreakToSpace(sql);
        }

        private static int enemyId = 0;
        //QuestStageEnemys Delete
        private static readonly string DeleteStageEnemyQuery = "delete from QuestStageEnemys where QuestStageId = {0};";
        //挿入
        private static readonly string InsertStageEnemyQuery = "insert into QuestStageEnemys {0} values \n{1};";
        //Preset削除
        private static readonly string DeletePresetStageEnemyQuery = "update QuestStageEnemys set PresetId = 0 where PresetId = {0};";

        /// <summary>
        /// QuestStageEnemys挿入カラム
        /// </summary>
        private static readonly string InsertStageEnemyColumn = @"
(
QuestEnemyId,
QuestStageId,
EnemyType,
EnemyParam1,
EnemyParam2,
Level,
GetPoint,
SpawnPointX,
SpawnPointY,
SpawnGameTime,
IsBoss,
IsStealth,
IsActiveRecord,
PresetId
)";

        /// <summary>
        /// 敵RecordをEnemyデータのインサートValueに
        /// </summary>
        private static string EnemyRecordToInsertValue(StageEditorSetting setting, EnemyRecord enemyRecord, int x, int y)
        {
            enemyId++;
            var id = $"{setting.QuestStageId}{enemyId:D3}";
            return InsertStageEnemyColumn //
                .Replace("QuestEnemyId", id) //
                .Replace("QuestStageId", setting.QuestStageId.ToString()) //
                .Replace("EnemyType", ((int)enemyRecord.EnemyType).ToString()) //
                .Replace("EnemyParam1", enemyRecord.EnemyParam1.ToString()) //
                .Replace("EnemyParam2", enemyRecord.EnemyParam2.ToString()) //
                .Replace("Level", enemyRecord.Level.ToString()) //
                .Replace("GetPoint", enemyRecord.GetPoint.ToString()) //
                .Replace("SpawnPointX", StageEditorUtil.ToPositionXStr(setting.Width, x)) //
                .Replace("SpawnPointY", StageEditorUtil.ToPositionXStr(setting.Height, y)) //
                .Replace("IsBoss", enemyRecord.IsBoss ? "1" : "0") //
                .Replace("IsStealth", enemyRecord.IsStealth ? "1" : "0") //
                .Replace("IsActiveRecord", "1")  //
                .Replace("PresetId", enemyRecord.PresetId.ToString())  //
                + ","; //カンマを入れる、最後の行はStringBuilder展開前に削除する
        }

        /// <summary>
        /// ObjectのPresetRecordをUpdate文に
        /// </summary>
        private static string EnemyPresetRecordToUpdateQuery(EnemyRecord enemyRecord)
        {
            var sql = @$"update QuestStageEnemys set
EnemyParam1 = {enemyRecord.EnemyParam1},
EnemyParam2 = {enemyRecord.EnemyParam2},
Level = {enemyRecord.Level},
GetPoint = {enemyRecord.GetPoint},
SpawnGameTime = {enemyRecord.SpawnGameTime},
IsBoss = {(enemyRecord.IsBoss ? "1" : "0")},
IsStealth = {(enemyRecord.IsStealth ? "1" : "0")}
where PresetId = {enemyRecord.PresetId}
;";
            return BreakToSpace(sql);
        }

        /// <summary>
        /// 挿入SQLの吐き出し
        /// </summary>
        public static StringBuilder CreateInsertSql(StageEditorSetting setting, int initPosX, int initPosY, ItemData[] items)
        {
            objId = 0;
            enemyId = 0;

            var builder = new StringBuilder();
            //Delete文
            builder.AppendLine("#Stage設定削除");
            builder.AppendLine(string.Format(DeleteStageQuery, setting.QuestStageId));
            //ステージInsert文
            builder.AppendLine("#Stage設定挿入");
            var stageValueStr = SettingToStageInsertValue(setting, initPosX, initPosY);
            builder.AppendLine(string.Format(InsertStageQuery, NoBreak(InsertStageColumn), NoBreak(stageValueStr)));

            //オブジェクトと敵のInsert文準備
            var objBuilder = new StringBuilder();
            var enemyBuilder = new StringBuilder();
            foreach (var item in items)
            {
                if (item.Record is ObjectRecord objectRecord)
                {
                    var objStr = ObjectRecordToInsertValue(setting, objectRecord, item.X, item.Y);
                    objBuilder.AppendLine(NoBreak(objStr));
                }
                else if (item.Record is EnemyRecord enemyRecord)
                {
                    var enemyStr = EnemyRecordToInsertValue(setting, enemyRecord, item.X, item.Y);
                    enemyBuilder.AppendLine(NoBreak(enemyStr));
                }
            }

            //ObjectInsert文
            builder.AppendLine(string.Empty);
            builder.AppendLine("#StageObject削除");
            builder.AppendLine(string.Format(DeleteStageObjectQuery, setting.QuestStageId));
            if (objBuilder.Length > 0)
            {
                builder.AppendLine("#StageObject挿入");
                objBuilder.Remove(objBuilder.Length - 2, 1); //最後の1文字を削除
                builder.AppendLine(string.Format(InsertStageObjectQuery, NoBreak(InsertStageObjectColumn), objBuilder));
            }

            //敵Insert文
            builder.AppendLine(string.Empty);
            builder.AppendLine("#StageEnemy削除");
            builder.AppendLine(string.Format(DeleteStageEnemyQuery, setting.QuestStageId));
            if (enemyBuilder.Length > 0)
            {
                builder.AppendLine("#StageEnemy挿入");
                enemyBuilder.Remove(enemyBuilder.Length - 2, 1); //最後の1文字を削除
                builder.AppendLine(string.Format(InsertStageEnemyQuery, NoBreak(InsertStageEnemyColumn), enemyBuilder));
            }

            return builder;
        }

        /// <summary>
        /// 挿入SQLの吐き出し
        /// </summary>
        public static StringBuilder CreateUpdatePresetSql(HashSet<SettingRecord> updateRecords, HashSet<SettingRecord> removeRecords)
        {
            var builder = new StringBuilder();
            if (removeRecords.Any())
            {
                builder.AppendLine(string.Empty);
                builder.AppendLine("#Preset削除対応");
            }
            foreach (var removeRecord in removeRecords)
            {
                if (removeRecord is ObjectRecord)
                {
                    builder.AppendLine(string.Format(DeletePresetStageObejctQuery, removeRecord.PresetId));
                }
                else if (removeRecord is EnemyRecord)
                {
                    builder.AppendLine(string.Format(DeletePresetStageEnemyQuery, removeRecord.PresetId));
                }
            }
            if (updateRecords.Any())
            {
                builder.AppendLine(string.Empty);
                builder.AppendLine("#Preset更新対応");
            }
            foreach (var updateRecord in updateRecords)
            {
                if (updateRecord is ObjectRecord objRecord)
                {
                    builder.AppendLine(ObjectPresetRecordToUpdateQuery(objRecord));
                }
                else if (updateRecord is EnemyRecord enemyRecord)
                {
                    builder.AppendLine(EnemyPresetRecordToUpdateQuery(enemyRecord));
                }
            }
            return builder;
        }

        /// <summary>
        /// PresetRecordによる更新Sql文の作成
        /// </summary>
        public static StringBuilder CreateUpdatePresetSql(IEnumerable<SettingRecord> records)
        {
            var builder = new StringBuilder();
            foreach (var updateRecord in records)
            {
                if (updateRecord is ObjectRecord objRecord)
                {
                    builder.AppendLine(ObjectPresetRecordToUpdateQuery(objRecord));
                }
                else if (updateRecord is EnemyRecord enemyRecord)
                {
                    builder.AppendLine(EnemyPresetRecordToUpdateQuery(enemyRecord));
                }
            }
            return builder;
        }

        /// <summary>
        /// 改行なしに
        /// </summary>
        private static string NoBreak(string str) => Regex.Replace(str, @"\r\n|\n|\r", "");

        /// <summary>
        /// 改行をスペースに
        /// </summary>
        private static string BreakToSpace(string str) => Regex.Replace(str, @"\r\n|\n|\r", " ");

    }
}
#endif
