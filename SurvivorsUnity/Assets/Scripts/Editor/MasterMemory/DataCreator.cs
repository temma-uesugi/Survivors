using System;
using System.Collections.Generic;
using System.IO;
using App.MD;
using Master.Tables.Enemy;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor.MasterMemory
{
    /// <summary>
    /// スプレッドシート設定
    /// </summary>
    public class SpreadSheetSetting
    {
        public string KeyFile { get; set; }
        public string ApplicationName { get; set; }
        public Dictionary<string, string> SpreadsheetId { get; set; }
    }

    /// <summary>
    /// Data作成
    /// </summary>
    public static class DataCreator
    {
        /// <summary>
        /// 設定
        /// </summary>
        private static readonly SpreadSheetSetting Setting = new SpreadSheetSetting
        {
            KeyFile = "./lively-vim-404818-006ad04a879e.json",
            // ApplicationName = "App",
            SpreadsheetId = new()
            {
                { "Enemy", "1fVXOhIdj8rI7_WES23dh6_n4_hW897_w3UIJmVC72qY" }
            }
        };

        /// <summary>
        /// SheetのId
        /// </summary>
        private static readonly Dictionary<string, string> SheetIdMap = new()
        {
            { "Enemy", "1fVXOhIdj8rI7_WES23dh6_n4_hW897_w3UIJmVC72qY" }
        };

        private static readonly string KeyPath = Path.Combine(Application.dataPath, "Scripts", "Editor", "MasterMemory", "key.json");
        private static readonly string AssetPath = Path.Combine(Application.dataPath, "Resources", "master-data.bytes");

        /// <summary>
        /// 作成
        /// </summary>
        public static void Create()
        {
            try
            {
                StaticCompositeResolver.Instance.Register
                (
                    new IFormatterResolver[]
                    {
                        MasterMemoryResolver.Instance,
                        GeneratedResolver.Instance,
                        StandardResolver.Instance,
                    }
                );
                var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
                MessagePackSerializer.DefaultOptions = options;
            }
            catch
            {
                Debug.LogError("Error");
            }

            var ssGetter = new SpreadSheetGetter(KeyPath, SheetIdMap);
            var builder = new DatabaseBuilder();
            
            //Note
            //Table毎に書く必要あり。refrectionではできなかった
            //敵
            builder.Append(Create<EnemyBase>(ssGetter.Get("Enemy", "EnemyBase")));
            builder.Append(Create<EnemyLevelStatus>(ssGetter.Get("Enemy", "EnemyLevelStatus")));
            builder.Append(Create<EnemySkillSet>(ssGetter.Get("Enemy", "EnemySkillSet")));
            builder.Append(Create<EnemySkill>(ssGetter.Get("Enemy", "EnemySkill")));
            builder.Append(Create<EnemySkillEffect>(ssGetter.Get("Enemy", "EnemySkillEffect")));
            
            byte[] data = builder.Build();
            
            using var fs = new FileStream(AssetPath, FileMode.Create);
            fs.Write(data, 0, data.Length);
        }

        /// <summary>
        /// 作成
        /// </summary>
        public static T[] Create<T>(Dictionary<string, string>[] datas)
        {
            T[] resultArray = new T[datas.Length];

            for (var i = 0; i < datas.Length; i++)
            {
                var instance = Activator.CreateInstance<T>();

                foreach (var kvp in datas[i])
                {
                    var property = typeof(T).GetProperty(kvp.Key);
                    if (property != null)
                    {
                        if (property.PropertyType.IsEnum)
                        {
                            var value = Enum.Parse(property.PropertyType, kvp.Value);
                            property.SetValue(instance, value);
                        }
                        else
                        {
                            var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                            property.SetValue(instance, value);
                        }
                    }
                }

                resultArray[i] = instance;
            }

            Debug.Log($"Date Created {typeof(T)}, レコード数: {datas.Length}");
            return resultArray;
        }
    }
}