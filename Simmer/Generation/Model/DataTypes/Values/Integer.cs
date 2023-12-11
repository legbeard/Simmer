using System.ComponentModel;

namespace Simmer.Model.DataTypes.Values;

public class Integer : DataTypeBase
{

    [DefaultValue(Int32.MinValue)]
    public int? Min { get; set; } = Int32.MinValue;
    
    [DefaultValue(Int32.MinValue)]
    public int? Max { get; set; } = Int32.MaxValue;
    
    [DefaultValue(null)]
    public int? Value { get; set; }

    public Integer()
    {
        
    }
    
    public Integer(int value)
    {
        Value = value;
    }

    public override Func<dynamic> GetGenerator()
    {
        return Value != null 
            ? () => Value
            : () => Faker.Random.Int(Min ?? int.MinValue, Max ?? int.MaxValue);
    }

    protected override bool CanGenerate(object? value)
    {
        return value is long or int;
    }
}