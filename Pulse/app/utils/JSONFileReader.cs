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
            return new Dictionary<string, object>(); // return empty dict
        }

        string json = File.ReadAllText(_filePath);
        var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();

        return ConvertJsonElements(jsonData);
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
    
    private Dictionary<string, object> ConvertJsonElements(Dictionary<string, object> dict)
    {
        var result = new Dictionary<string, object>();

        foreach (var kvp in dict)
        {
            if (kvp.Value is JsonElement element)
            {
                result[kvp.Key] = ConvertJsonElement(element);
            }
            else
            {
                result[kvp.Key] = kvp.Value;
            }
        }

        return result;
    }
    
    private object ConvertJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => ConvertJsonElements(JsonSerializer.Deserialize<Dictionary<string, object>>(element.GetRawText())),
            JsonValueKind.Array => element.EnumerateArray().Select(ConvertJsonElement).ToList(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number => element.TryGetInt64(out long l) ? l : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            _ => null
        };
    }
}