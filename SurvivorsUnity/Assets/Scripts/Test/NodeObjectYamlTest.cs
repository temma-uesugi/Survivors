// using System.IO;
// using App.AppCommon;
// using App.AppCommon.Core;
// using App.Battle2.EnemyBots.NodeObjects;
// using NUnit.Framework;
//
// namespace App.Test
// {
//     /// <summary>
//     /// NodeYamlのテスト
//     /// </summary>
//     public class NodeObjectYamlTest
//     {
//         [Test]
//         public void CheckNodeYaml()
//         {
//             var files = Directory.GetFiles(GameConst.EnemyBotNodeDirPath, "*.yaml");
//             foreach (var file in files)
//             {
//                 if (!YamlHelper.TryDeserialize<BotNodeObject.BotNodeRoot>(file, out var root))
//                 {
//                     var fileName = Path.GetFileName(file);
//                     Assert.Fail($"Reserialize Error [{fileName}]"); 
//                 }
//             }
//             Assert.Pass("OK");
//         }
//     }
// }