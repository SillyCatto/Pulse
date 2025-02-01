namespace Pulse.app.models.record;

public class BMIRecord : IRecord
{
    public double Value { get; }
    public string Verdict { get; }

    public BMIRecord(double bmiVal)
    {
        Value = bmiVal;
        Verdict = GetVerdict(Value);
    }

    private static string GetVerdict(double bmi)
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

    public List<string> GetValues()
    {
        return new List<string> { Value.ToString("F2"), Verdict };
    }
}