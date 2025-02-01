using System.Text.Json;
using Pulse.app.models;
using Pulse.utils;

namespace Pulse.models;

public class JSONModelHandler<T> where T : IModelAdapter, new()
{
    private readonly string _filePath;
    private readonly JSONFileReader _reader;
    private readonly JSONFileWriter _writer;
    private readonly T _model;

    public JSONModelHandler(string filePath)
    {
        _filePath = filePath;
        _reader = new JSONFileReader(_filePath);
        _writer = new JSONFileWriter(_filePath);
        _model = new T();

        Load();
    }

    public void Load()
    {
        var data = _reader.Read();
        if (data != null)
        {
            foreach (var entry in data)
            {
                _model.AddRecord(entry.Key, entry.Value);
            }
        }
    }

    public void Save()
    {
        _writer.Write(_model.ToDict());
    }

    public void AddRecord(string key, List<string> value)
    {
        _model.AddRecord(key, value);
    }
    

    public bool RemoveRecord(string key)
    {
        bool isRemoved = _model.RemoveRecord(key);
        return isRemoved;
    }

    public bool Update(string key, List<string> newValue)
    {
        bool updated = _model.Update(key, newValue);
        return updated;
    }

    public Dictionary<string, List<string>> ToDict()
    {
        return _model.ToDict();
    }
    
    public T AsModel()
    {
        return _model;
    }
}

