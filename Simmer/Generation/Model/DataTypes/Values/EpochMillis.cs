namespace Simmer.Model.DataTypes.Values;

public class EpochMillis : DataTypeBase
{
    public const string Identifier = "epochmillis";
    
    public EpochMillisProgression Progression { get; set; } = EpochMillisProgression.Now;
    
    public long Increment { get; set; } = 1000;
    
    public string Start
    {
        get => _start.ToString();
        set => _start = value switch {
           "now" => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
           _ => long.TryParse(value, out var val) ? val : throw new ArgumentException($"{value} is not a valid {nameof(Start)}. Use a number or 'now'")
        }; 
    }
    
    private long _start = 0;
    private long _currentIteration = 0;

    public EpochMillis()
    {
        
    }
    
    public EpochMillis(long start)
    {
        _start = start;
    }
    
    public override Func<dynamic> GetGenerator()
    {
        return Progression switch
        {
            EpochMillisProgression.Now => () => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            EpochMillisProgression.Linear => () => _start + Increment * _currentIteration++,
            _ => throw new ArgumentOutOfRangeException($"Epoch Millis progression had an invalid value: {Progression}")
        };
    }

    protected override bool CanGenerate(object? value)
    {
        return value is long or int;
    }

    public enum EpochMillisProgression
    {
        Now,
        Linear
    }
}