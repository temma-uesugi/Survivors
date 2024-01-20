using System;
using System.IO;
using UnityEngine;

namespace App.AppCommon
{
    /// <summary>
    /// ゲーム内定数
    /// </summary>
    public static class GameConst
    {
        public const DirectionType PlayerDefaultDir = DirectionType.Right;
        public const DirectionType EnemyDefaultDir = DirectionType.Left;

        public const uint InvalidUnitId = 0;
        public const uint PlayerUserId = 1;
        public const uint EnemyUserId = 2;

        //予報されているターン数
        public const int PredictedTurnAmount = 8;

        //風向き抽出テーブルのデフォルトの重み
        public const int WindDirDrawDefaultWeight = 10;

        //砲撃レンジの初期値(距離)
        public const int DefaultBombardRangeDistance = 3;

        //敵攻撃レンジの初期値(距離)
        public const int DefaultEnemyAttackRangeDistance = 1;

        //カメラのデフォルトサイズ
        public const float DefaultBattleCameraSize = 5;

        //HexImageのデフォルトスケール
        public const float DefaultHexImageScale = 0.22f;

        public const int MapWidth = 40;
        public const int MapHeight = 25;
        
        //最大カメラサイズ
        // public const float BattleCameraSizeLimit = 10;
        public const float BattleCameraSizeLimit = 12;

        //ObjectIdのOffset
        public const uint ObjectIdOffset = 10_000;
        
        private static readonly string DataDir = "App/Data";
        public static string RootDataPath =>
#if UNITY_EDITOR
            Path.Combine(Application.dataPath, DataDir);
#else
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\'), RootDataPath);
#endif
        //敵BotNodeのパス
        public static readonly string EnemyBotNodeDirPath = Path.Combine(GameConst.RootDataPath, "EnemyBotNodes");
    }
}