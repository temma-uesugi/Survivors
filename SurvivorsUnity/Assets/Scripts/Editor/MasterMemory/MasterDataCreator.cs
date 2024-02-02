#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using App.AppCommon.Core;
using App.Battle2.Units.Enemy;
using App.MD;
using Master;
using Master.Constants;
using MasterMemory;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = System.Object;

namespace Editor.MasterMemory
{
    /// <summary>
    /// Data作成
    /// </summary>
    public class MasterDataCreator
    {
        /// <summary>
        /// SheetのId
        /// </summary>
        private static readonly Dictionary<string, string> SheetIdMap = new()
        {
            { "Enemy", "1fVXOhIdj8rI7_WES23dh6_n4_hW897_w3UIJmVC72qY" },
            { "Hero", "1PgmBKNvdJ4d-D0v1A3aS50lsSCF5gKNGuW_63V1qKWs" }
        };

        private static readonly string KeyPath =
            Path.Combine(Application.dataPath, "Scripts", "Editor", "MasterMemory", "key.json");

        private static readonly string AssetPath = Path.Combine(Application.dataPath, "Resources", "master-data.bytes");

        private readonly Assembly _masterAssembly;
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MasterDataCreator()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            _masterAssembly = assemblies.FirstOrDefault(x => x.FullName.StartsWith("Master,"));
        }
        
        /// <summary>
        /// 作成
        /// </summary>
        public void Create()
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
            catch (Exception e)
            {
                Debug.LogError($"Error {e.Message}");
            }

            var ssGetter = new SpreadSheetGetter(KeyPath, SheetIdMap);
            var builder = new DatabaseBuilder();
            
            //Genericメソッド
            MethodInfo createMethod = GetType().GetMethod("Create", BindingFlags.NonPublic | BindingFlags.Instance);
            if (createMethod == null)
            {
                throw new Exception("CreateMethod is Not Exists");
            }
            
            foreach (var type in _masterAssembly.GetTypes())
            {
                var attr = Attribute.GetCustomAttribute(type, typeof(MemoryTableAttribute));
                if (attr == null) continue;

                var splits = type.ToString().Split("."); 
                if (splits.Length < 2) continue;
                var className = splits[^1];
                var classCategory = splits[^2];
                
                var genericCreateMethod = createMethod.MakeGenericMethod(type);
                var appendMethod = typeof(DatabaseBuilder).GetMethod("Append", new[] { typeof(IEnumerable<>).MakeGenericType(type) });
                if (appendMethod == null)
                {
                    Log.Error($"AppendMethod is Not Exists [{type.ToString()}]");
                    continue;
                }
                
                var values = ssGetter.Get(classCategory, className);
                var res = genericCreateMethod.Invoke(this, new object[]{ values });
                appendMethod.Invoke(builder, new[] { res });
            }
            
            byte[] data = builder.Build();

            using var fs = new FileStream(AssetPath, FileMode.Create);
            fs.Write(data, 0, data.Length);
        }

        /// <summary>
        /// 作成
        /// </summary>
        private T[] Create<T>(Dictionary<string, string>[] datas)
        {
            var classFullName = typeof(T).FullName;
            if (string.IsNullOrEmpty(classFullName)) return null;
            
            T[] resultArray = new T[datas.Length];

            for (var i = 0; i < datas.Length; i++)
            {
                var valueList = new List<Object>();
                foreach (var kvp in datas[i])
                {
                    var property = typeof(T).GetProperty(kvp.Key);
                    if (property != null)
                    {
                        if (property.PropertyType.IsEnum)
                        {
                            var value = Enum.Parse(property.PropertyType, kvp.Value);
                            valueList.Add(value);
                        }
                        else
                        {
                            var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                            valueList.Add(value);
                        }
                    }
                }
               
                var instance = (T)_masterAssembly.CreateInstance(
                    classFullName,
                    false,
                    BindingFlags.CreateInstance,
                    null,
                    valueList.ToArray(),
                    null,
                    null
                );
                resultArray[i] = instance;
            }

            Debug.Log($"Date Created {typeof(T)}, レコード数: {datas.Length}");
            return resultArray;
        }
    }
}

#endif