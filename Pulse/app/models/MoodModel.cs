namespace Pulse.app.models;

public class MoodModel : IModelAdapter
{
    private Dictionary<string, List<string>> _moodRecords;

    public MoodModel()
    {
        _moodRecords = new Dictionary<string, List<string>>();
    }

    public Dictionary<string, List<string>> ToDict()
    {
        return new Dictionary<string, List<string>>(_moodRecords);
    }

    public void AddRecord(string index, List<string> value)
    {
        _moodRecords[index] = value;
    }

    public bool RemoveRecord(string key)
    {
        return _moodRecords.Remove(key);
    }

    public bool Update(string key, List<string> newValue)
    {
        if (!_moodRecords.ContainsKey(key)) return false;
        _moodRecords[key] = newValue;
        return true;
    }

    public static List<string> GetAllMoods()
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
        _moodRecords = new Dictionary<string, List<string>>(newRecords);
    }
}