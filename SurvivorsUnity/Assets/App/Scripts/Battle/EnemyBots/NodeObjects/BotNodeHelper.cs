using System.IO;
using App.AppCommon;
using App.AppCommon.Core;

namespace App.Battle.EnemyBots.NodeObjects
{
    /// <summary>
    /// BotNodeのHelper
    /// </summary>
    public static class BotNodeHelper
    {
        public static bool TryGetBotNodeRoot(string id, out BotNodeObject.BotNodeRoot  root)
        {
            root = null;
            var file = Path.Combine(GameConst.EnemyBotNodeDirPath, $"{id}.yaml");
            if (!File.Exists(file))
            {
                Log.Error($"Not Exists File [{id}]");
                return false;
            }
            if (!YamlHelper.TryDeserialize<BotNodeObject.BotNodeRoot>(file, out root))
            {
                var fileName = Path.GetFileName(file);
                Log.Error($"Reserialize Error [{fileName}]");
                return false;
            }

            return true;
        }
    }
}