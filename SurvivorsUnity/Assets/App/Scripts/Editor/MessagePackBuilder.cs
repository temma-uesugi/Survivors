// using System.Diagnostics;
// using System.Threading.Tasks;
// using App.AppCommon.Core;
// using UnityEditor;
// using UnityEngine;
//
// namespace App.Editor
// {
//     /// <summary>
//     /// メッセージパックビルダー
//     /// </summary>
//     public static class MessagePackBuilder
//     {
//         /// <summary>
//         /// ビルド
//         /// </summary>
//         [MenuItem("CodeGenerate/MessagePack")]
//         private static void Build()
//         {
//         
//             // Task.WhenAll
//             // (
//             //     // MessagePack
//             //     execute("mpc", "Scripts/ServerShared", "Generated/MessagePack.Generated"),
//             // ).Wait();
//             
//             var args = $"mpc -i {Application.dataPath}/App/Scripts/Master -o {Application.dataPath}/App/Scripts/Generated/MessagePack";
//             var result = ProcessUtility.Run(command: DotNetCommand, args, assetsPath);
//             
//             Task execute(string toolName, string input, string output, string additionalOption = "")
//             {
//                 var assetsPath = Application.dataPath;
//                 return Task.Run(() =>
//                 {
//                     var inputPath = Path.Combine(assetsPath, input);
//                     var outputDir = Path.Combine(assetsPath, $"Scripts/{output}");
//
//                     // csファイルを一旦削除
//                     foreach (var file in Directory.EnumerateFiles(outputDir, "*.cs", SearchOption.AllDirectories))
//                     {
//                         File.Delete(file);
//                     }
//
//                     var args = $"{toolName} -i {inputPath} -o {outputDir} {additionalOption}";
//                     var result = ProcessUtility.Run(command: DotNetCommand, args, assetsPath);
//
//                     // 何か出力されていれば成功と判定
//                     bool success = Directory.EnumerateFiles(outputDir, "*.cs", SearchOption.AllDirectories).Any();
//
//                     var log = $"[Run IL2CPP Resolver]\n{DotNetCommand} {args}\n\n{result.output}";
//                     if (success)
//                     {
//                         Debug.Log(log);
//                     }
//                     else
//                     {
//                         // 失敗した場合はgitで元に戻す
//                         var gitResult = ProcessUtility.Run(command: "git", $"checkout head {outputDir}");
//                         Debug.LogError(log + "\n" + result.errorOutput);
//                         if (!string.IsNullOrEmpty(gitResult.errorOutput)) Debug.LogError(gitResult.errorOutput);
//                     }
//                 });
//             }
//
// //             var exProcess = new Process();
// //
// //             var rootPath = Application.dataPath + "/..";
// //             var filePath = rootPath + "/GeneratorTools/mpc";
// //             var exeFileName = "";
// // #if UNITY_EDITOR_WIN
// //         exeFileName = "/win-x64/mpc.exe";
// // #elif UNITY_EDITOR_OSX
// //             exeFileName = "/osx-x64/mpc";
// // #elif UNITY_EDITOR_LINUX
// //         exeFileName = "/linux-x64/mpc";
// // #else
// //         return;
// // #endif 
// //             
// //             var psi = new ProcessStartInfo()
// //             {
// //                 CreateNoWindow = true,
// //                 WindowStyle = ProcessWindowStyle.Hidden,
// //                 RedirectStandardOutput = true,
// //                 RedirectStandardError = true,
// //                 UseShellExecute = false,
// //                 FileName = filePath + exeFileName,
// //                 Arguments =
// //                     $@"-i ""{Application.dataPath}/App/Scripts/Master"" -o ""{Application.dataPath}/App/Scripts/Generated/MessagePack""",
// //             };
// //
// //             var p = Process.Start(psi);
// //
// //             p.EnableRaisingEvents = true;
// //             p.Exited += (object sender, System.EventArgs e) =>
// //             {
// //                 var data = p.StandardOutput.ReadToEnd();
// //                 UnityEngine.Debug.Log($"{data}");
// //                 p.Dispose();
// //                 p = null;
// //             };  
//             
//         }
//     }
// }