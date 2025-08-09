using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WpfScheduledApp20250729.Utils
{
    public class Logger
    {
        private static readonly object _lockObject = new object();
        private static readonly string _logDirectory = GetLogDirectory();
        private const string _projectName = "WpfScheduledApp20250729";

        private static string GetLogDirectory()
        {
            // 本番環境とデバッグ環境で適切なパスを返す
            string baseDirectory;
            
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // デバッグ時: プロジェクトルートのLogsフォルダ
                var projectRoot = FindProjectRoot();
                baseDirectory = projectRoot != null ? projectRoot : AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                // 本番環境: 実行ファイルと同じディレクトリのLogsフォルダ
                baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            
            return Path.Combine(baseDirectory, "Logs");
        }

        private static string? FindProjectRoot()
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            var directory = new DirectoryInfo(currentDir);
            
            // .csprojファイルまたは.slnファイルを探してプロジェクトルートを特定
            while (directory != null)
            {
                if (directory.GetFiles("*.csproj").Length > 0 || 
                    directory.GetFiles("*.sln").Length > 0)
                {
                    return directory.FullName;
                }
                directory = directory.Parent;
            }
            
            return null;
        }

        public enum LogLevel
        {
            Info,
            Warning,
            Error,
            Debug
        }

        static Logger()
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
            
            // ログディレクトリの場所をデバッグ出力（初回のみ）
            System.Diagnostics.Debug.WriteLine($"Logger initialized. Log directory: {_logDirectory}");
            Console.WriteLine($"Logger initialized. Log directory: {_logDirectory}");
        }

        public static void Log(string message, LogLevel level = LogLevel.Info, string methodName = "", string className = "")
        {
            Task.Run(() => WriteLogAsync(message, level, methodName, className));
        }

        public static void LogInfo(string message, string methodName = "", string className = "")
        {
            Log(message, LogLevel.Info, methodName, className);
        }

        public static void LogWarning(string message, string methodName = "", string className = "")
        {
            Log(message, LogLevel.Warning, methodName, className);
        }

        public static void LogError(string message, string methodName = "", string className = "")
        {
            Log(message, LogLevel.Error, methodName, className);
        }

        public static void LogError(Exception exception, string methodName = "", string className = "")
        {
            var message = $"Exception: {exception.Message}\nStackTrace: {exception.StackTrace}";
            Log(message, LogLevel.Error, methodName, className);
        }

        public static void LogDebug(string message, string methodName = "", string className = "")
        {
            Log(message, LogLevel.Debug, methodName, className);
        }

        private static async Task WriteLogAsync(string message, LogLevel level, string methodName, string className)
        {
            await Task.Run(() =>
            {
                lock (_lockObject)
                {
                    try
                    {
                        var timestamp = DateTime.Now;
                        var dateString = timestamp.ToString("yyyyMMdd");
                        var logFileName = $"{_projectName}_{dateString}.log";
                        var logFilePath = Path.Combine(_logDirectory, logFileName);

                        var logEntry = FormatLogEntry(timestamp, level, message, methodName, className);

                        File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        // ログ出力でエラーが発生した場合は、イベントログまたはコンソールに出力
                        Console.WriteLine($"Logger Error: {ex.Message}");
                    }
                }
            });
        }

        private static string FormatLogEntry(DateTime timestamp, LogLevel level, string message, string methodName, string className)
        {
            var timeString = timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var levelString = level.ToString().ToUpper();
            
            var location = "";
            if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(methodName))
            {
                location = $"[{className}.{methodName}] ";
            }
            else if (!string.IsNullOrEmpty(className))
            {
                location = $"[{className}] ";
            }
            else if (!string.IsNullOrEmpty(methodName))
            {
                location = $"[{methodName}] ";
            }

            return $"{timeString} [{levelString}] {location}{message}";
        }

        public static void LogWithContext(string message, LogLevel level = LogLevel.Info, [System.Runtime.CompilerServices.CallerMemberName] string methodName = "", [System.Runtime.CompilerServices.CallerFilePath] string filePath = "")
        {
            var className = Path.GetFileNameWithoutExtension(filePath);
            Log(message, level, methodName, className);
        }
    }
}