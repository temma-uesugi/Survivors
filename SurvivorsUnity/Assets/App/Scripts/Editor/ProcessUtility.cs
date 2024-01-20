using System;
using System.Diagnostics;
using System.Text;

namespace App.Editor
{
    /// <summary>
    /// 外部プロセスを扱うユーティリティ
    /// </summary>
    public static class ProcessUtility
    {
        /// <summary>
        /// 外部プロセスを起動する
        /// </summary>
        /// <param name="command">実行コマンド</param>
        /// <param name="args">コマンド引数</param>
        /// <returns>プロセスの出力結果</returns>
        public static (string output, string errorOutput) Run(string command, string args, string workingDirectory = "")
        {
            var psi = CreateProcessStartInfo(command, args, workingDirectory);

            try
            {
                var process = Process.Start(psi);
                var readOutput = process?.StandardOutput.ReadToEnd();
                var readError = process?.StandardError.ReadToEnd();
                var success = string.IsNullOrEmpty(readError);

                return (readOutput, readError);
            }
            catch (Exception e)
            {
                return (e.Message, string.Empty);
            }
        }

        /// <summary>
        /// プロセスの開始情報を生成する
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="args">コマンド引数</param>
        /// <returns>プロセスの開始情報</returns>
        private static ProcessStartInfo CreateProcessStartInfo(string command, string args, string workingDirectory)
        {
            return new ProcessStartInfo
            {
                FileName = command,
                Arguments = args,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory,
                WindowStyle = ProcessWindowStyle.Hidden,
                StandardOutputEncoding = Encoding.UTF8,
            };
        }
    }
}