using System.Text.Json;
using Pulse.app.utils;

namespace Pulse.utils;

public class JSONFileWriter : IFileWriter
{
    private readonly string _filePath;

    public JSONFileWriter(string filePath)
    {
        _filePath = filePath;
    }
    
    public void Write(Dictionary<string, List<string>> data)
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

    public void UpdateValue(string key, List<string> record)
    {
        var data = new JSONFileReader(_filePath).Read();

        if (data == null)
        {
            data = new Dictionary<string, List<string>>();
        }
        
        data[key] = record;
        Write(data);
    }
}