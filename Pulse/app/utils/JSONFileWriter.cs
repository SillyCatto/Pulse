using System.Text.Json;

namespace Pulse.utils;

public class JSONFileWriter : IFileWriter
{
    private readonly string _filePath;

    public JSONFileWriter(string filePath)
    {
        _filePath = filePath;
    }
    
    public void Write(Dictionary<string, object> data)
    {
        string dir = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "{}");
        }
        
        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
        
    }

    public void UpdateValue(string keyPath, object newValue)
    {
        var data = new JSONFileReader(_filePath).Read();

        string[] keys = keyPath.Split('.');
        Dictionary<string, object> current = data;

        for (int i = 0; i < keys.Length - 1; i++)
        {
            string key = keys[i];

            if (!current.ContainsKey(key) || current[key] is not Dictionary<string, object>)
            {
                current[key] = new Dictionary<string, object>();
            }

            current = (Dictionary<string, object>)current[key];
        }

        current[keys[^1]] = newValue;
        Write(data);
    }
}