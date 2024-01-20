using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
#if UNITY_EDITOR
using UnityEngine;
#endif

namespace App.AppCommon.Core
{
    /// <summary>
    /// Yamlのヘルパー
    /// </summary>
    public static class YamlHelper
    {
        /// <summary>
        /// デシリアライズ
        /// </summary>
        public static bool TryDeserialize<T>(string path, out T obj) where T : class
        {
            obj = null;
            try
            {
                using var reader = new StreamReader(path);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();
                obj = deserializer.Deserialize<T>(reader);
            }
            catch (Exception e)
            {
                #if UNITY_EDITOR
                Debug.Log(e.Message);
                #endif
                return false;
            }
            return true;
        }

        /// <summary>
        /// シリアライズ
        /// </summary>
        public static bool TrySerialize<T>(string path, T obj)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            using var writer = new StreamWriter(path);
            try
            {
                serializer.Serialize(writer, obj);
            }
            catch (Exception e)
            {
                #if UNITY_EDITOR
                Debug.Log(e.Message);
                #endif
                return false;
            }
            return true;
        }
    }
}