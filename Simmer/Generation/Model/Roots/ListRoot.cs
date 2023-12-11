using Simmer.Generation.Model.DataTypes;

namespace Simmer.Generation.Model.Roots;

/// <summary>
/// Used when the root of the model yaml is a list
/// </summary>
public class ListRoot : List<DataTypeBase>, IPayloadGeneratorFactory
{
    public Func<dynamic> GetGenerator()
    {
        var subGenerators = this.Select(x => x.GetGenerator()).ToList();
        return GetCombinedGenerator(subGenerators);
    }
    
    private static Func<dynamic> GetCombinedGenerator(List<Func<dynamic>> generators)
    {
        return () => generators
            .Select(x => UnpackIfKeyValuePair(x))
            .ToList();
    }

    private static dynamic UnpackIfKeyValuePair(Func<dynamic> input)
    {
        var result = input();
        return result switch
        {
            KeyValuePair<string, object> kvp => kvp.Value,
            _ => result
        };
    }
}