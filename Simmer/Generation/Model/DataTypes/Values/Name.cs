namespace Simmer.Model.DataTypes.Values;

public class Name : DataTypeBase
{
    public Name()
    {
        
    }
    
    public Name(string value)
    {
        // Noop, we don't need the value, but need this ctor for reflection
    }
    
    public override Func<dynamic> GetGenerator()
    {        
        return () => Faker.Name.FullName();
    }
    
    protected override bool CanGenerate(object? value)
    {
        return value is string str && str.Split(' ').Length >= 2;
    }
}