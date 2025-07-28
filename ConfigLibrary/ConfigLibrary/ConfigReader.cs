// ConfigLibrary.cs
using System;
using System.Collections.Generic;
using System.IO;

namespace ConfigLibrary
{
    public class ConfigReader
    {
        private readonly Dictionary<string, string> _configurations;

        public ConfigReader(string filePath)
        {
            _configurations = new Dictionary<string, string>();
            LoadConfigurations(filePath);
        }

        private void LoadConfigurations(string filePath)
        {
            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    _configurations[parts[0].Trim()] = parts[1].Trim();
                }
            }
        }

        public string GetValue(string key)
        {
            return _configurations.TryGetValue(key, out var value) ? value : null;
        }
    }
}