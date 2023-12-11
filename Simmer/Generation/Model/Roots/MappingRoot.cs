using Simmer.Model.DataTypes;

namespace Simmer.Model;

/// <summary>
/// Used when the root of the model yaml is a mapping
/// </summary>

public class MappingRoot : Dictionary<string, DataTypeBase>, IPayloadGeneratorFactory
{
    public Func<dynamic> GetGenerator()
    {
        var subGenerators = this
            .ToDictionary(
                x => x.Key,
                x => x.Value.GetGenerator()
            );

        return GetGenerator(subGenerators);
    }
    
    private static Func<dynamic> GetGenerator(Dictionary<string, Func<dynamic>> generators)
    {
        return generators.Count switch
        {
            < 1 => throw new ArgumentException("Expected input yaml to end up as at least 1 subgenerator"),
            1 => GetSingleGenerator(generators),
            > 1 => GetMultipleGenerators(generators)
        };
    }

    private static Func<dynamic> GetSingleGenerator(Dictionary<string, Func<dynamic>> generators) => 
        () => {
            var value = generators.First().Value();
            return value is KeyValuePair<string, dynamic> kvp ? kvp.Value : value;
        };

    private static Func<dynamic> GetMultipleGenerators(Dictionary<string, Func<dynamic>> generators) =>
        () => generators
            .Select(x => UnpackAndReplaceKeyIfKeyValuePair(x))
            .ToDictionary(x => x.Key, x => x.Value);

    private static KeyValuePair<string, dynamic> UnpackAndReplaceKeyIfKeyValuePair(KeyValuePair<string, Func<dynamic>> input)
    {
        var result = input.Value();
        return result switch
        {
            KeyValuePair<string, object> kvp => new KeyValuePair<string, dynamic>(kvp.Key, kvp.Value),
            _ => new KeyValuePair<string, dynamic>(input.Key, result)
        };
    }
}