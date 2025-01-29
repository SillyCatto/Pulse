namespace Pulse.utils;

public interface IFileReader
{
    Dictionary<string, object>? Read();
    object? GetValue(string keyPath);
}