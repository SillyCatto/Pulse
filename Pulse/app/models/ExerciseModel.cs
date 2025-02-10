namespace Pulse.app.models;

public class ExerciseModel : IModelAdapter
{
    private Dictionary<string, List<string>> _exerciseRecords;

    public ExerciseModel()
    {
        _exerciseRecords = new Dictionary<string, List<string>>();
    }

    public Dictionary<string, List<string>> ToDict()
    {
        return new Dictionary<string, List<string>>(_exerciseRecords);
    }

    public void AddRecord(string index, List<string> value)
    {
        _exerciseRecords[index] = value;
    }

    public bool RemoveRecord(string key)
    {
        return _exerciseRecords.Remove(key);
    }

    public bool Update(string key, List<string> newValue)
    {
        if (!_exerciseRecords.ContainsKey(key)) return false;
        _exerciseRecords[key] = newValue;
        return true;
    }
    
    public void SetData(Dictionary<string, List<string>> newRecords)
    {
        _exerciseRecords = new Dictionary<string, List<string>>(newRecords);
    }
}