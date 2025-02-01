namespace Pulse.app.models;

public interface IModelAdapter
{
    // void SetDefaults(); // Initialize default values (optional)
    bool Validate();
    Dictionary<string, List<string>> ToDict();
    void AddRecord(string key, List<string> value);
    bool RemoveRecord(string key);
    bool Update(string key, List<string> newValue);
}