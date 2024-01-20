using System.IO;
using App.AppCommon;
using App.AppCommon.Core;
using App.Battle.EnemyBots.NodeObjects;

namespace App.AppEditor.BotNodeEditor
{
    /// <summary>
    /// BotNodeの作成
    /// </summary>
    public static class BotNodeCreator
    {
        /// <summary>
        /// 作成
        /// </summary>
        public static void Create(RootNodeComponent rootNode, string rootName)
        {
            var root = rootNode.ConvertBotNodeRoot();
            var path = Path.Combine(GameConst.EnemyBotNodeDirPath, rootName + ".yaml");
            if (!YamlHelper.TrySerialize<BotNodeObject.BotNodeRoot>(path, root))
            {
                return;  
            }
            Log.Debug("OK");
        }
    }
}