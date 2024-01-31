using System.IO;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace App.Editor.MasterMemory
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
        private static readonly string InputDir = Path.Combine(Application.dataPath, "App", "Scripts", "Master", "Tables");
        private static readonly string GeneratedDir = Path.Combine(Application.dataPath, "App", "Scripts", "Generated");
        private static readonly string MasterMemoryGeneratedDir = Path.Combine(GeneratedDir, "MasterMemory");
        private static readonly string MessagePackGeneratedDir = Path.Combine(GeneratedDir, "MessagePack");
        
        /// <summary>
        /// ビルド
        /// </summary>
        [MenuItem("Survivors/MasterMemory/Generate", priority = 1001)]
        private static async UniTask BuildAsync()
        {
            await ProcessHelper.InvokeAsync("dotnet-mmgen", $"-i {InputDir}", $"-o {MasterMemoryGeneratedDir}", "-n App.MD");
            await ProcessHelper.InvokeAsync("mpc", $"-i {InputDir}", $"-o {MessagePackGeneratedDir}");
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
    }
}