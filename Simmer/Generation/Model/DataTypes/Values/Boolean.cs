using Bogus;

namespace Simmer.Generation.Model.DataTypes.Values;

public class Boolean : DataTypeBase
{
    private readonly Faker _faker = new ();
    
    public bool? Value { get; init; } = null;

    public Boolean()
    {
        
    }

    public Boolean(bool value)
    {
        Value = value;
    }
    
    public override Func<dynamic> GetGenerator()
    {
        return Value != null 
            ? () => Value 
            : () => _faker.Random.Bool();
    }

    protected override bool CanGenerate(object? value)
    {
        return value is bool;
    }
}