namespace Pulse.app.models;

public interface IModelAdapter
{
    Dictionary<string, List<string>> ToDict();
    void AddRecord(string key, List<string> value);
    bool RemoveRecord(string key);
    bool Update(string key, List<string> newValue);
    void SetData(Dictionary<string, List<string>> newRecords);
}