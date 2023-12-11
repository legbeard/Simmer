namespace Simmer.Model.DataTypes.Structures;

public class Object : DataTypeBase
{

    public MappingRoot Content { get; set; } = new();

    public override Func<dynamic> GetGenerator()
    {
        return Content.GetGenerator();
    }

    protected override bool CanGenerate(object? value)
    {
        return value is Dictionary<string, object?>;
    }
}