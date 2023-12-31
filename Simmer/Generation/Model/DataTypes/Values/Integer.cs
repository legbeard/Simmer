﻿using System.ComponentModel;

namespace Simmer.Generation.Model.DataTypes.Values;

public class Integer : DataTypeBase
{
    [DefaultValue(null)]
    public int? Value { get; init; }

    [DefaultValue(null)] 
    public int? Increment { get; init; } = null;

    [DefaultValue(null)] 
    public int? Min { get; init; } = null;
    
    [DefaultValue(null)]
    public int? Max { get; init; } = null;
    
    private int _currentIteration = 0;

    public Integer()
    {
        
    }
    
    public Integer(int value)
    {
        Value = value;
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
        return value is long or int;
    }
}