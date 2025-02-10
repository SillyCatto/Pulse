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
            ":neutral_face: Neutral",
            ":grinning_face_with_smiling_eyes: Happy",
            ":disappointed_face: Sad",
            ":crying_face: Depressed",
            ":anxious_face_with_sweat: Anxious",
            ":fearful_face: Scared",
            ":partying_face: Excited",
            ":relieved_face: Relaxed",
            ":angry_face: Angry",
            ":pensive_face: Stressed",
            ":sleeping_face: Tired"
        ];
    }
    
    public void SetData(Dictionary<string, List<string>> newRecords)
    {
        _moodRecords = new Dictionary<string, List<string>>(newRecords);
    }
}