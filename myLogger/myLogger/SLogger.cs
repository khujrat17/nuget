using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLogger
{
    public class SLogger
    {
        private readonly string _logFilePath;

        public SLogger(string logFilePath)
        {
            _logFilePath = logFilePath;

        }

        // Method to ensure the log file exists
        public void EnsureLogFileExists(string logFilePath)
        {
            if (!File.Exists(logFilePath))
            {
                // Create the file and close it immediately
                using (File.Create(logFilePath)) { }
            }
        }

        public void Log(string message)
        {
            // Prepare the log message
            var logMessage = $"{DateTime.Now}: {message}";

            // Append the log message to the file
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }

        public void LogError(string message)
        {
            Log($"ERROR: {message}");
        }

        public void LogInfo(string message)
        {
            Log($"INFO: {message}");
        }

        public void LogWarning(string message)
        {
            Log($"WARNING: {message}");
        }
    }
}
