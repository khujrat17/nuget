using System;
using System.IO;
using System.Text;

namespace myLogger
{
    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public class SLogger
    {
        private readonly string _logDirectory;
        private readonly string _logFileName;
        private readonly object _lockObj = new object();
        private readonly int _retentionDays;

        public SLogger(string logDirectory, string logFileName = "log.txt", int retentionDays = 30)
        {
            _logDirectory = logDirectory;
            _logFileName = logFileName;
            _retentionDays = retentionDays;

            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);

            CleanupOldLogs();
        }

        /// <summary>
        /// Logs a message with optional log level and console output.
        /// </summary>
        public void Log(string message, LogLevel level = LogLevel.Info, bool writeToConsole = false)
        {
            WriteLog(message, level, writeToConsole);
        }

        /// <summary>
        /// Logs an exception with full details.
        /// </summary>
        public void LogException(Exception ex, LogLevel level = LogLevel.Error, bool writeToConsole = true)
        {
            if (ex == null) return;

            var sb = new StringBuilder();
            sb.AppendLine("Exception Occurred:");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"StackTrace: {ex.StackTrace}");

            // Include inner exceptions if any
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                sb.AppendLine("Inner Exception:");
                sb.AppendLine($"Message: {inner.Message}");
                sb.AppendLine($"StackTrace: {inner.StackTrace}");
                inner = inner.InnerException;
            }

            WriteLog(sb.ToString(), level, writeToConsole);
        }

        #region Backward-Compatible 1.0 Methods
        public void LogInfo(string message) => Log(message, LogLevel.Info);
        public void LogWarning(string message) => Log(message, LogLevel.Warning);
        public void LogError(string message) => Log(message, LogLevel.Error);
        #endregion

        /// <summary>
        /// Clears all logs in the directory.
        /// </summary>
        public void ClearLogs()
        {
            lock (_lockObj)
            {
                foreach (var file in Directory.GetFiles(_logDirectory, $"*_{_logFileName}"))
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Deletes old logs beyond retention period.
        /// </summary>
        private void CleanupOldLogs()
        {
            lock (_lockObj)
            {
                var files = Directory.GetFiles(_logDirectory, $"*_{_logFileName}");
                foreach (var file in files)
                {
                    try
                    {
                        var creationTime = File.GetCreationTime(file);
                        if ((DateTime.Now - creationTime).TotalDays > _retentionDays)
                            File.Delete(file);
                    }
                    catch { /* Ignore deletion errors */ }
                }
            }
        }

        /// <summary>
        /// Internal helper to write log messages to file and optionally console.
        /// </summary>
        private void WriteLog(string message, LogLevel level, bool writeToConsole)
        {
            string datedFile = Path.Combine(_logDirectory, $"{DateTime.Now:yyyy-MM-dd}_{_logFileName}");
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}";

            lock (_lockObj)
            {
                File.AppendAllText(datedFile, logMessage + Environment.NewLine);
            }

            if (writeToConsole)
                Console.WriteLine(logMessage);
        }
    }
}
