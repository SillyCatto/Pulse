namespace Pulse.app.utils;

public interface IFileWriter
{
    void Write(Dictionary<string, List<string>> data);
    void UpdateValue(string keyPath, List<string> record);
}