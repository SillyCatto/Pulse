namespace Pulse.app.models;

public class TodoModel : IModelAdapter
{
    private Dictionary<string, List<string>> _todoRecords;

    public TodoModel()
    {
        _todoRecords = new Dictionary<string, List<string>>();
    }

    public Dictionary<string, List<string>> ToDict()
    {
        return new Dictionary<string, List<string>>(_todoRecords);
    }

    public void AddRecord(string index, List<string> value)
    {
        _todoRecords[index] = value;
    }

    public bool RemoveRecord(string key)
    {
        return _todoRecords.Remove(key);
    }

    public bool Update(string key, List<string> newValue)
    {
        if (!_todoRecords.ContainsKey(key)) return false;
        _todoRecords[key] = newValue;
        return true;
    }

    public static List<string> GetAllStatus()
    {
        return
        [
            ":memo: Planned",
            ":hourglass_not_done: InProgress",
            ":pause_button:  Paused",
            ":check_mark:  Done",
            ":cross_mark: Cancelled"
        ];
    }
    
    public void SetData(Dictionary<string, List<string>> newRecords)
    {
        _todoRecords = new Dictionary<string, List<string>>(newRecords);
    }
}