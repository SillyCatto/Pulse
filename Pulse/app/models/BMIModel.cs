using Pulse.models.record;

namespace Pulse.models;

public class BMIModel : IModelAdapter
{
    private Dictionary<string, BMIRecord> _bmiRecords;

    public BMIModel()
    {
        _bmiRecords = new Dictionary<string, BMIRecord>();
    }

    public bool Validate()
    {
        foreach (var entry in _bmiRecords)
        {
            if (entry.Value.Value < 10 || entry.Value.Value > 50) // realistic BMI values
                return false;
        }
        return true;
    }

    public Dictionary<string, object> ToDict()
    {
        var result = new Dictionary<string, object>();
        foreach (var entry in _bmiRecords)
        {
            result[entry.Key] = entry.Value;
        }
        return result;
    }
    

    public void AddRecord(string date, object value)
    {
        if (_bmiRecords.ContainsKey(date))
            throw new InvalidOperationException($"BMI record for {date} already exists.");
        
        if (value is BMIRecord record)
        {
            _bmiRecords[date] = record;
        }
        else if (value is double bmiValue)
        {
            _bmiRecords[date] = new BMIRecord(bmiValue);
        }
        else
        {
            throw new ArgumentException("BMI value must be a double or a BMIRecord.");
        }
    }

    public bool RemoveRecord(string date)
    {
        return _bmiRecords.Remove(date);
    }

    public bool Update(string date, object newValue)
    {
        if (!(newValue is double bmiValue))
            throw new ArgumentException("BMI value must be a double.");

        if (_bmiRecords.ContainsKey(date))
        {
            _bmiRecords[date] = new BMIRecord(bmiValue);
            return true;
        }
        return false;
    }
}