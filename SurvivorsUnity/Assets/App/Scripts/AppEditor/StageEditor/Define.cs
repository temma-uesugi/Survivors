#if UNITY_EDITOR
using App.AppEditor.StageEditor.Records;
using FastEnumUtility;
using UnityEngine;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// 定義
    /// </summary>
    public static class Define
    {
        public static int InvalidItemId = 0;
        public static int InvalidPresetId = 0;

        public static Color ObjectDefaultColor = Color.blue;
        public static Color EnemyDefaultColor = Color.green;

        /// <summary>
        /// モード
        /// </summary>
        public enum ModeType
        {
            [Label("選択")]
            Select = 1,
            [Label("追加")]
            Add = 2,
            [Label("削除")]
            Remove = 3,
            [Label("開始位置設定")]
            InitPos = 4,
        }

        /// <summary>
        /// 設定データのタイプ
        /// </summary>
        public enum SettingDataType
        {
            [Label("中継地点")]
            ObjectRelayPoint = 1,
            [Label("障害物")]
            ObjectObstacle = 2,
            [Label("浮遊アイテム")]
            ObjectFloatingItem = 3,
            [Label("敵Actor")]
            EnemyActor = 11,
            [Label("敵チーム")]
            EnemyTeam = 12,
            [Label("ちびドラの巣")]
            EnemyMobDraNest = 13,
            [Label("敵グループ")]
            EnemyQuestEnemyGroup = 14,
        }

        /// <summary>
        /// 設定データのタイプからObjectTypeのint値を取得
        /// </summary>
        public static int GetSettingTypeInt(this SettingDataType type) => type switch
        {
            SettingDataType.ObjectRelayPoint => QuestObjectType.RelayPoint.ToInt32(),
            SettingDataType.ObjectObstacle => QuestObjectType.Obstacle.ToInt32(),
            SettingDataType.ObjectFloatingItem => QuestObjectType.FloatingItem.ToInt32(),
            SettingDataType.EnemyActor => QuestEnemyType.Actor.ToInt32(),
            SettingDataType.EnemyTeam => QuestEnemyType.Team.ToInt32(),
            SettingDataType.EnemyMobDraNest => QuestEnemyType.MobDraNest.ToInt32(),
            SettingDataType.EnemyQuestEnemyGroup => QuestEnemyType.QuestEnemyGroup.ToInt32(),
            _ => 0,
        };

        /// <summary>
        /// 設定データのタイプからObjectTypeのint値を取得
        /// </summary>
        public static SettingRecord GetDefaultRecord(this SettingDataType type) => type switch
        {
            SettingDataType.ObjectRelayPoint => new ObjectRelayPointRecord(type.GetLabel(), ObjectDefaultColor, InvalidPresetId, "", 0, 0),
            SettingDataType.ObjectObstacle => new ObjectObstacleRecord(type.GetLabel(), ObjectDefaultColor, InvalidPresetId, "", 0, 0),
            SettingDataType.ObjectFloatingItem => new ObjectFloatingItemRecord(type.GetLabel(), ObjectDefaultColor, InvalidPresetId, "", 0, 0),
            SettingDataType.EnemyActor => new EnemyActorRecord(type.GetLabel(), EnemyDefaultColor, InvalidPresetId, "", 0, 0, 0, 0, 0, false, false),
            SettingDataType.EnemyTeam => new EnemyTeamRecord(type.GetLabel(), EnemyDefaultColor, InvalidPresetId, "", 0, 0, 0, 0, 0, false, false),
            SettingDataType.EnemyMobDraNest => new EnemyMobDraNestRecord(type.GetLabel(), EnemyDefaultColor, InvalidPresetId, "", 0, 0, 0, 0, 0, false, false),
            SettingDataType.EnemyQuestEnemyGroup => new EnemyQuestEnemyGroupRecord(type.GetLabel(), EnemyDefaultColor, InvalidPresetId, "", 0, 0, 0, 0, 0, false, false),
            _ => null,
        };
        
        /// <summary>クエストのオブジェクトタイプ</summary>
        public enum QuestObjectType
        {
            /// <summary>中継地点</summary>
            RelayPoint = 1,
            /// <summary>障害物</summary>
            Obstacle = 2,
            /// <summary>浮遊アイテム</summary>
            FloatingItem = 3,
        }

        /// <summary>クエストの敵タイプ</summary>
        public enum QuestEnemyType
        {
            /// <summary>指定なし</summary>
            None = 0,
            /// <summary>Actor</summary>
            Actor = 1,
            /// <summary>浮島艦とクルーの単位であるチーム</summary>
            Team = 2,
            /// <summary>モブドラの巣</summary>
            MobDraNest = 3,
            /// <summary>クエスト用敵の群れ</summary>
            QuestEnemyGroup = 4,
        }
    }
}
#endif
