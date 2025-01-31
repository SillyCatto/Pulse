namespace Pulse.models;

public interface IModelAdapter
{
    // void SetDefaults(); // Initialize default values (optional)
    bool Validate();
    Dictionary<string, object> Load();
    void Save();
    string ToJson();
    void AddRecord(string key, object value);
    bool RemoveRecord(string key);
    bool Update(string key, object newValue);
}