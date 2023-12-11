namespace Simmer.Generation.Model.DataTypes.Values;

public class EpochMillis : DataTypeBase
{
    public long? Increment { get; init; } = null;

    public long? StartAt { get; init; } = null;
    
    private long _currentIteration = 0;

    public EpochMillis()
    {
        if (StartAt.HasValue && !Increment.HasValue)
        {
            throw new ArgumentException("Must supply 'increment' when supplying 'startAt'");
        }
    }
    
    public EpochMillis(long startAt)
    {
        // For now, noop, since we can't provide increment.
        // This makes all generated models with epoch millis use "now";
    }
    
    public override Func<dynamic> GetGenerator()
    {
        if (StartAt.HasValue && Increment.HasValue)
        {
            return () => StartAt.Value! + Increment.Value! * _currentIteration++;
        }

        return () => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    protected override bool CanGenerate(object? value)
    {
        return value is long or int;
    }
}