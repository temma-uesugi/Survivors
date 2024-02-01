#if UNITY_EDITOR

using System.IO;

namespace Editor
{
    /// <summary>
    /// FileのUtility
    /// </summary>
    public static class FileUtil
    {
        /// <see cref="https://learn.microsoft.com/ja-jp/dotnet/standard/io/how-to-copy-directories" />
        /// <summary>
        /// ディレクトリコピー
        /// </summary>
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        /// <summary>
        /// ディレクトリ内を削除
        /// </summary>
        public static void ClearDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                return;
            }

            //ディレクトリ以外の全ファイルを削除
            string[] filePaths = Directory.GetFiles(dirPath);
            foreach (string filePath in filePaths)
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }

            //ディレクトリの中のディレクトリも再帰的に削除
            string[] directoryPaths = Directory.GetDirectories(dirPath);
            foreach (string directoryPath in directoryPaths)
            {
                ClearDirectory(directoryPath);
            }
        }
    }
}

#endif