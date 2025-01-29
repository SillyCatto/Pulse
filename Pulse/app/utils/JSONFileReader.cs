using System.Text.Json;

namespace Pulse.utils;

public class JSONFileReader : IFileReader
{
    private readonly string _filePath;

    public JSONFileReader(string filePath)
    {
        _filePath = filePath;
    }


    public Dictionary<string, object>? Read()
    {
        if (!File.Exists(_filePath))
        {
            return new Dictionary<string, object>(); //return empty dict
        }

        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
    }

    public object? GetValue(string keyPath)
    {
        var data = Read();
        if (data == null) return null;

        string[] keys = keyPath.Split('.');
        object? current = data;

        foreach (var key in keys)
        {
            if (current is Dictionary<string, object> dict && dict.TryGetValue(key, out var next))
            {
                current = next;
            }
            else
            {
                return null;
            }
        }

        return current;
    }
}