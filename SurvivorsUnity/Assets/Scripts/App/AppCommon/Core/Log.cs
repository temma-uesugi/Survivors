using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace App.AppCommon.Core
{
    /// <summary>
    /// ログ出力
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Server用にログ文章を作成
        /// </summary>
        private static string MakeLog(string name, params object[] objects)
        {
            var builder = new StringBuilder();
            builder.Append("[DebugLog] ");
            builder.Append(DateTimeOffset.Now.ToString("yyyy/MM/dd HH:mm:ss.fff "));

            StackTrace st = new StackTrace(2, true);
            builder.Append(Path.GetFileName(st.GetFrame(0)?.GetFileName()));
            builder.Append(": " );
            builder.Append(st.GetFrame(0)?.GetFileLineNumber());
            if (!string.IsNullOrEmpty(name))
            {
                builder.Append(", ");
                builder.Append(name);
            }
            foreach (var obj in objects)
            {
                try
                {
                    builder.Append(", ");
                    builder.Append(JsonConvert.SerializeObject(obj, Formatting.None));
                }
                catch (Exception)
                {
                    builder.Append($"{obj}");
                }
            }
            builder.Append(", ");

            return builder.ToString();
        }

        /// <summary>
        /// Debug出力
        /// </summary>
        public static void Debug(string name, params object[] objects)
        {
#if (NETCOREAPP && DEBUG)
            Console.WriteLine(MakeLog(name, objects));
#elif UNITY_EDITOR
            UnityEngine.Debug.Log(MakeLog(name, objects));
#endif
        }

        /// <summary>
        /// Debug出力
        /// </summary>
        public static void Debug(params object[] objects)
        {
#if (NETCOREAPP && DEBUG)
            Console.WriteLine(MakeLog("", objects));
#elif UNITY_EDITOR
            UnityEngine.Debug.Log(MakeLog("", objects));
#endif
        }

        /// <summary>
        /// エラーログ出力 (サーバーでは普通のログ)
        /// </summary>
        public static void Error(params object[] objects)
        {
#if (NETCOREAPP && DEBUG)
            Console.WriteLine(MakeLog(null, objects));
#elif UNITY_EDITOR
            UnityEngine.Debug.LogError(MakeLog("", objects));
#endif
        }
        
        /// <summary>
        /// エラーログ出力 (サーバーでは普通のログ)
        /// </summary>
        public static void Warning(params object[] objects)
        {
#if (NETCOREAPP && DEBUG)
            Console.WriteLine(MakeLog(null, objects));
#elif UNITY_EDITOR
            UnityEngine.Debug.LogWarning(MakeLog("", objects));
#endif
        }
    }
}