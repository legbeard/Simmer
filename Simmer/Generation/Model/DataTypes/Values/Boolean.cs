using Bogus;

namespace Simmer.Model.DataTypes.Values;

public class Boolean : DataTypeBase
{
    public const string Identifier = "bool";
    private readonly Faker _faker = new ();
    
    public bool? Value { get; set; } = null;

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