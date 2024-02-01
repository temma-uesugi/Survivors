#if UNITY_EDITOR

using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor.MasterMemory
{
    //Note:
    //別途 mastermemory.generator, messagepack.generator のインストールが必要
    //.NETを入れてから
    //dotnet tool install --global MasterMemory.Generator
    //dotnet tool install --global MessagePack.Generator
    
    /// <summary>
    /// MasterMemoryの生成
    /// </summary>
    public static class Generator
    {
        private static readonly string MasterDir = Path.Combine(Application.dataPath, "Scripts", "Master");
        private static readonly string InputDir = Path.Combine(MasterDir, "Tables");
        private static readonly string GeneratedDir = Path.Combine(MasterDir, "Generated");
        private static readonly string MasterMemoryGeneratedDir = Path.Combine(GeneratedDir, "MasterMemory");
        private static readonly string MessagePackGeneratedDir = Path.Combine(GeneratedDir, "MessagePack");

        private static readonly string CopyDir = Path.Combine(Application.dataPath, "../", "GeneratedCopy");
        
        /// <summary>
        /// ビルド
        /// </summary>
        [MenuItem("Survivors/MasterMemory/Generate", priority = 1001)]
        private static async UniTask BuildAsync()
        {
            //先に現状のものをコピーしておく
            if (!Directory.Exists(CopyDir))
            {
                Directory.CreateDirectory(CopyDir);
            }
            FileUtil.ClearDirectory(CopyDir);
            FileUtil.CopyDirectory(GeneratedDir, CopyDir, true);
            
            await ProcessHelper.InvokeAsync("dotnet-mmgen", $"-i {InputDir}", $"-o {MasterMemoryGeneratedDir}", "-n App.MD");
            await ProcessHelper.InvokeAsync("mpc", $"-i {InputDir}", $"-o {MessagePackGeneratedDir}", "-c");
            Debug.Log("Complete!");
        }

        /// <summary>
        /// データ作成
        /// </summary>
        [MenuItem("Survivors/MasterMemory/CreateData", priority = 1002)]
        private static void CreateData()
        {
            DataCreator.Create();
            Debug.Log("Complete!");
        }
        
        /// <summary>
        /// 生成済みを戻す
        /// </summary>
        [MenuItem("Survivors/MasterMemory/RestoreGenerated", priority = 1009)]
        private static void RestoreGenerated()
        {
            FileUtil.ClearDirectory(GeneratedDir);
            FileUtil.CopyDirectory(CopyDir, GeneratedDir, true);
            Debug.Log("Complete!");
        }
    }
}

#endif