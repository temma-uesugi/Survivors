using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.AppCommon.Core;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace App.Editor
{
    /// <summary>
    /// MasterMemoryの生成
    /// </summary>
    public static class MasterMemoryGenerator
    {
        /// <summary>
        /// ビルド
        /// </summary>
        [MenuItem("CodeGenerate/MasterMemory")]
        private static void Build()
        {
            //TODO 動かないので、直接mpc.sh を指定する
            ExecuteShellScript($"{Application.dataPath}/../GeneratorTools/mpc.sh");
        }
        
        /// <summary>
        /// シェルの実行
        /// </summary>
        private static void ExecuteShellScript(string filePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = filePath,
            };

            Process process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            process.StandardInput.WriteLine($"chmod +x {filePath}");

            // シェルスクリプトのパスを実行
            process.WaitForExit();
            process.EnableRaisingEvents = true;
            string output = process.StandardOutput.ReadToEnd();
            Log.Debug("output", output);
            process.Close(); 
        }
    }
}