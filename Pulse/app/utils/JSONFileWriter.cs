using System.Text.Json;
using Pulse.app.utils;

namespace Pulse.utils;

public class JSONFileWriter : IFileWriter
{
    private readonly string _filePath;
    private readonly JSONFileReader _reader;

    public JSONFileWriter(string filePath)
    {
        _filePath = filePath;
        _reader = new JSONFileReader(_filePath);
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
        var data = _reader.Read() ?? new Dictionary<string, List<string>>();
        
        data[key] = record;
        Write(data);
    }
}