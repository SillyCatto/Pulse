namespace Pulse.utils;

public interface IFileWriter
{
    void Write(Dictionary<string, object> data);
    void UpdateValue(string keyPath, object newValue);
}