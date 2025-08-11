using System;
using System.IO;

namespace WpfScheduledApp20250729.Helpers
{
    internal static class FileHelper
    {
        /// <summary>
        /// ファイルの拡張子を取得
        /// </summary>
        /// <param name="filePath">ファイル名またはパス</param>
        /// <returns>拡張子（ドット含む）、拡張子がない場合はnull</returns>
        public static string? GetFileExtension(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            // 最後のバックスラッシュまたはスラッシュ以降のファイル名部分を取得
            string fileName = Path.GetFileName(filePath);

            // 最後のドットの位置を探す
            int lastDotIndex = fileName.LastIndexOf('.');

            // ドットが見つからない、または最初の文字がドット（隠しファイル）の場合
            if (lastDotIndex == -1 || lastDotIndex == 0)
                return null;

            // 拡張子を返す（ドット含む）
            return fileName.Substring(lastDotIndex);
        }

        /// <summary>
        /// プロジェクト相対パスを絶対パスに変換
        /// Debug時とRelease時両方に対応
        /// </summary>
        /// <param name="relativePath">プロジェクト基準の相対パス（例: "/arc/sample.txt"）</param>
        /// <returns>絶対パス</returns>
        private static string GetProjectPath(string relativePath)
        {
            // プロジェクトルートディレクトリを取得
            string projectRoot = GetProjectRootDirectory();

            // 先頭のスラッシュを削除
            if (relativePath.StartsWith("/") || relativePath.StartsWith("\\"))
                relativePath = relativePath.Substring(1);

            return Path.Combine(projectRoot, relativePath);
        }

        /// <summary>
        /// プロジェクトルートディレクトリを取得
        /// </summary>
        /// <returns>プロジェクトルートの絶対パス</returns>
        private static string GetProjectRootDirectory()
        {
            // 実行中のアセンブリの場所から開始
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // bin フォルダから上位に遡ってプロジェクトルートを探す
            DirectoryInfo? directory = new DirectoryInfo(currentDirectory);

            while (directory != null)
            {
                // .csproj ファイルがあるディレクトリがプロジェクトルート
                if (directory.GetFiles("*.csproj").Length > 0)
                {
                    return directory.FullName;
                }
                directory = directory.Parent;
            }

            // 見つからない場合は現在のディレクトリを返す
            return currentDirectory;
        }

        /// <summary>
        /// ファイルが存在するかチェック（プロジェクト相対パス）
        /// </summary>
        /// <param name="relativePath">プロジェクト基準の相対パス</param>
        /// <returns>ファイルが存在するかどうか</returns>
        private static bool FileExists(string relativePath)
        {
            string fullPath = GetProjectPath(relativePath);
            return File.Exists(fullPath);
        }

 
    }
}