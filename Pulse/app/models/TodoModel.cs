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
        throw new NotImplementedException();
    }

    public void AddRecord(string key, List<string> value)
    {
        throw new NotImplementedException();
    }

    public bool RemoveRecord(string key)
    {
        throw new NotImplementedException();
    }

    public bool Update(string key, List<string> newValue)
    {
        throw new NotImplementedException();
    }
}