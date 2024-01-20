using System;
using MessagePack;
using UnityEngine;

namespace App.AppEditor.StageEditor
{
    /// <summary>
    /// プリセットデータ
    /// </summary>
    [Serializable]
    public class PresetData
    {
        public int Id { get; set; }
        public string Json { get; set; }
    }

    /// <summary>
    /// 設定データ基本
    /// </summary>
    [MessagePackObject(true)]
    [Union(0, typeof(ObjectSettingData))]
    [Union(1, typeof(EnemySettingData))]
    public abstract class SettingData
    {
        public int PresetId { get; set; }
        public string ItemLabel { get; set; }
        public Color IconColor { get; set; }
        public string IconDesc { get; set; }
    }

    /// <summary>
    /// Objectの設定データ
    /// </summary>
    [MessagePackObject(true)]
    public class ObjectSettingData : SettingData
    {
        public Define.QuestObjectType ObjectType { get; set; }
        public int ObjectParam1 { get; set; }
        public int GetPoint { get; set; }
    }

    /// <summary>
    /// Enemyのプリセットデータ
    /// </summary>
    [MessagePackObject(true)]
    public class EnemySettingData : SettingData
    {
        public Define.QuestEnemyType EnemyType { get; set; }
        public int EnemyParam1 { get; set; }
        public int EnemyParam2 { get; set; }
        public int Level { get; set; }
        public int GetPoint { get; set; }
        public int SpawnGameTime { get; set; }
        public bool IsBoss { get; set; }
        public bool IsStealth { get; set; }
    }
}