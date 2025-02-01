namespace Pulse.app.utils;

public interface IFileReader
{
    Dictionary<string, List<string>>? Read();
    object? GetValue(string key);
}