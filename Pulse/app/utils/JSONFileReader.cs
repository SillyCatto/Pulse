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
        throw new NotImplementedException();
    }
}