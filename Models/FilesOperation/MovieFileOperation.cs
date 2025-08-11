using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace WpfScheduledApp20250729.Models.FilesOperation
{
    public class MovieFileOperation
    {
        /// <summary>
        /// 動画ファイルを指定の時間から開く
        /// </summary>
        /// <param name="fileName">動画ファイル名</param>
        /// <param name="startTimeSeconds">開始時間（秒）</param>
        public void OpenMovieFileWithSpecificTime(string fileName, int startTimeSeconds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    MessageBox.Show("動画ファイル名が指定されていません。", "🎮 MOVIE ERROR");
                    return;
                }

                // ファイルが存在するかチェック（複数の拡張子で試す）
                string filePath = FindMovieFile(fileName);
                
                if (string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show($"動画ファイル '{fileName}' が見つかりません。", "🎮 MOVIE NOT FOUND");
                    return;
                }

                // VLCプレイヤーで特定時間から開く（VLCがインストールされている場合）
                if (TryOpenWithVLC(filePath, startTimeSeconds))
                {
                    return;
                }

                // Windows Media Playerまたはデフォルトプレイヤーで開く
                OpenWithDefaultPlayer(filePath, startTimeSeconds);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"動画ファイルを開くときにエラーが発生しました: {ex.Message}", "🎮 MOVIE ERROR");
            }
        }

        /// <summary>
        /// 動画ファイルを探す（複数の場所と拡張子で）
        /// </summary>
        private string FindMovieFile(string fileName)
        {
            // 検索する拡張子
            string[] extensions = { ".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm" };
            
            // 検索するフォルダ
            string[] searchFolders = {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Movies"),
                Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                AppDomain.CurrentDomain.BaseDirectory
            };

            foreach (var folder in searchFolders)
            {
                if (!Directory.Exists(folder)) continue;

                // 拡張子なしのファイル名の場合、各拡張子を試す
                if (!Path.HasExtension(fileName))
                {
                    foreach (var ext in extensions)
                    {
                        string fullPath = Path.Combine(folder, fileName + ext);
                        if (File.Exists(fullPath))
                        {
                            return fullPath;
                        }
                    }
                }
                else
                {
                    // 拡張子ありの場合、そのまま検索
                    string fullPath = Path.Combine(folder, fileName);
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// VLCプレイヤーで特定時間から開く
        /// </summary>
        private bool TryOpenWithVLC(string filePath, int startTimeSeconds)
        {
            try
            {
                // VLCの一般的なインストールパス
                string[] vlcPaths = {
                    @"C:\Program Files\VideoLAN\VLC\vlc.exe",
                    @"C:\Program Files (x86)\VideoLAN\VLC\vlc.exe"
                };

                foreach (var vlcPath in vlcPaths)
                {
                    if (File.Exists(vlcPath))
                    {
                        // VLCで特定時間から再生
                        var startInfo = new ProcessStartInfo
                        {
                            FileName = vlcPath,
                            Arguments = $"\"{filePath}\" --start-time={startTimeSeconds}",
                            UseShellExecute = false
                        };

                        Process.Start(startInfo);
                        MessageBox.Show($"🎮 VLC起動: {startTimeSeconds}秒から再生開始", "MOVIE PLAYER");
                        return true;
                    }
                }
            }
            catch
            {
                // VLCでの起動に失敗した場合はfalseを返す
            }

            return false;
        }

        /// <summary>
        /// デフォルトプレイヤーで開く
        /// </summary>
        private void OpenWithDefaultPlayer(string filePath, int startTimeSeconds)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });

                if (startTimeSeconds > 0)
                {
                    MessageBox.Show($"🎮 動画を開きました。\n手動で {startTimeSeconds}秒 の位置に移動してください。", 
                                  "MOVIE PLAYER");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"デフォルトプレイヤーで動画を開けませんでした: {ex.Message}", "🎮 PLAYER ERROR");
            }
        }

        /// <summary>
        /// 時間を秒に変換するヘルパーメソッド（MM:SSフォーマットから）
        /// </summary>
        public int ConvertTimeToSeconds(string timeString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(timeString)) return 0;

                if (timeString.Contains(":"))
                {
                    var parts = timeString.Split(':');
                    if (parts.Length == 2)
                    {
                        int minutes = int.Parse(parts[0]);
                        int seconds = int.Parse(parts[1]);
                        return minutes * 60 + seconds;
                    }
                    else if (parts.Length == 3)
                    {
                        int hours = int.Parse(parts[0]);
                        int minutes = int.Parse(parts[1]);
                        int seconds = int.Parse(parts[2]);
                        return hours * 3600 + minutes * 60 + seconds;
                    }
                }

                // 数値のみの場合は秒として扱う
                return int.Parse(timeString);
            }
            catch
            {
                return 0;
            }
        }
    }
}