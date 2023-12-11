namespace Simmer.Generation.Model.DataTypes.Structures;

public class Sequence : DataTypeBase
{
   public List<DataTypeBase> Content { get; set; } = new();
    private List<Func<dynamic>> _generators = new();
    private int _iteration = 0;
    
    public Sequence()
    {
    }
    
    public override Func<dynamic> GetGenerator()
    {
        _generators = Content.Select(x => x.GetGenerator()).ToList();
        return () => _generators[_iteration++ % Content.Count]();
    }

    protected override bool CanGenerate(object? value)
    {
        return false; // TODO: This is not possible with single-value input, maybe if we provide multiple
    }
}