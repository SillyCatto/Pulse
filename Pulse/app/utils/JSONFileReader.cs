using System.Text.Json;
using Pulse.app.utils;

namespace Pulse.utils;

public class JSONFileReader : IFileReader
    {
        private readonly string _filePath;

        public JSONFileReader(string filePath)
        {
            _filePath = filePath;
        }
        
        public Dictionary<string, List<string>>? Read()
        {
            if (!File.Exists(_filePath))
            {
                return new Dictionary<string, List<string>>(); // return an empty dict
            }

            string json = File.ReadAllText(_filePath);
            var jsonData = JsonSerializer
                .Deserialize<Dictionary<string, JsonElement>>(json) ?? new Dictionary<string, JsonElement>();

            return ConvertJsonElements(jsonData);
        }
        
        public object? GetValue(string key)
        {
            var data = Read();
            if (data == null || !data.ContainsKey(key)) return null;

            return data[key];
        }
        
        private Dictionary<string, List<string>> ConvertJsonElements(Dictionary<string, JsonElement> dict)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var kvp in dict)
            {
                var list = new List<string>();
                if (kvp.Value.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in kvp.Value.EnumerateArray())
                    {
                        if (item.ValueKind == JsonValueKind.String)
                        {
                            list.Add(item.GetString()!);
                        }
                        else
                        {
                            list.Add(item.ToString()); // convert other to string
                        }
                    }
                }
                else if (kvp.Value.ValueKind == JsonValueKind.String)
                {
                    list.Add(kvp.Value.GetString()!);
                }
                else
                {
                    list.Add(kvp.Value.ToString()); // handle unexpected types
                }
                result[kvp.Key] = list;
            }

            return result;
        }
    }