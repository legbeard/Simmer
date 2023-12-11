namespace Simmer.Model.DataTypes.Values;

public class Null : DataTypeBase
{
    public Null()
    {
        
    }
    
    public Null(object value)
    {
        // Noop, we don't need the value, but need this ctor for reflection
    }

    public override Func<dynamic> GetGenerator()
    {
        return () => null!;
    }

    protected override bool CanGenerate(object? value)
    {
        return value is null;
    }
}