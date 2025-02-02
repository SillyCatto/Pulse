namespace Pulse.app.models;

public class BMIModel : IModelAdapter
{
    private Dictionary<string, List<string>> _bmiRecords;

    public BMIModel()
    {
        _bmiRecords = new Dictionary<string, List<string>>();
    }

    public bool Validate()
    {
        foreach (var entry in _bmiRecords)
        {
            if (entry.Value is List<string> bmiRecord && bmiRecord.Count > 0)
            {
                if (double.TryParse(bmiRecord[0], out double bmiValue))
                {
                    if (bmiValue < 10 || bmiValue > 50)
                        return false;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    public Dictionary<string, List<string>> ToDict()
    {
        var result = new Dictionary<string, List<string>>();
        foreach (var entry in _bmiRecords)
        {
            result[entry.Key] = entry.Value;
        }
        return result;
    }

    public void AddRecord(string date, List<string> value)
    {
        if (_bmiRecords.ContainsKey(date))
            throw new InvalidOperationException($"BMI record for {date} already exists.");
        
        if (value is List<string> bmiRecord)
        {
            // simply add the List<string> to the records
            _bmiRecords[date] = bmiRecord;
        }
        else
        {
            throw new ArgumentException("BMI record must be a List<string>.");
        }
    }

    public bool RemoveRecord(string date)
    {
        return _bmiRecords.Remove(date);
    }

    public bool Update(string date, List<string> newValue)
    {
        if (newValue is List<string> bmiRecord)
        {
            _bmiRecords[date] = bmiRecord;
            return true;
        }

        throw new ArgumentException("BMI value must be a double or a List<string>.");
    }
    
    public static string GetVerdict(double bmi)
    {
        return bmi switch
        {
            < 18.5 => "Underweight",
            < 25.0 => "Normal weight",
            < 30.0 => "Pre-obesity",
            < 35.0 => "Obesity class I",
            < 40.0 => "Obesity class II",
            _ => "Obesity class III"
        };
    }

    
}