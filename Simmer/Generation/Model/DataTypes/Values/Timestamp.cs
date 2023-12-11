using System.ComponentModel;
using System.Globalization;

namespace Simmer.Generation.Model.DataTypes.Values;

public class Timestamp : DataTypeBase
{
    [DefaultValue("o")]
    public string Format { get; set; } = "yyyy-MM-ddTHH:mm:ss.fffffffZ";

    [DefaultValue(null)]
    public DateTime? StartAt { get; set; } = null;

    [DefaultValue(null)] 
    public TimeSpan? Increment { get; set; } = null;

    private DateTime? _current;

    public Timestamp()
    {
        if (StartAt.HasValue && !Increment.HasValue)
        {
            throw new ArgumentException("Must supply 'increment' when supplying 'startAt'");
        }
    }

    public override Func<dynamic> GetGenerator()
    {
        if (!StartAt.HasValue)
        {
            return () => GetFormattedTimestamp(DateTime.Now.ToUniversalTime());
        }
        
        _current ??= StartAt ?? DateTime.UtcNow;
        return () =>
        {
            var current = _current;
            _current += Increment;
            return GetFormattedTimestamp(current.Value.ToUniversalTime());
        };
    }

    protected override bool CanGenerate(object? value)
    {
        return value switch
        {
            string s => DateTimeOffset.TryParse(s, out var _),
            DateTimeOffset or DateTimeOffset => true,
            _ => false
        };
    }
    
    private string GetFormattedTimestamp(DateTimeOffset timestamp)
    {
        return timestamp.ToString(Format, CultureInfo.InvariantCulture);
    }
}