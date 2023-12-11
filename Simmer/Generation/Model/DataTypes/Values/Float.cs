
using System.ComponentModel;

namespace Simmer.Generation.Model.DataTypes.Values;

public class Float : DataTypeBase
{

    [DefaultValue(null)]
    public double? Value { get; set; } = null;
    public double? Increment { get; set; } = null;

    public double? Min { get; set; } = null;
    public double? Max { get; set; } = null;

    private int _currentIteration = 0;
    
    public Float()
    {
    }

    public Float(double value) : this()
    {
        
    }
    
    public override Func<dynamic> GetGenerator()
    {
        if (Value.HasValue)
        {
            if (Increment.HasValue)
            {
                return () => Value + Increment * _currentIteration++;
            }
            return () => Value;
        }
        
        return () => Faker.Random.Double(Min ?? double.MinValue, Max ?? double.MaxValue);
    }

    protected override bool CanGenerate(object? value)
    {
        return value is double or float;
    }
}