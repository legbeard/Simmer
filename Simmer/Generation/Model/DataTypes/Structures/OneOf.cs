namespace Simmer.Generation.Model.DataTypes.Structures;

public class OneOf : DataTypeBase
{
    public const string Identifier = "oneof";
    
    public Dictionary<string, DataTypeBase> Options { get; set; }

    private readonly List<KeyValuePair<string, Func<dynamic>>> _generators = new();
    public override Func<dynamic> GetGenerator()
    {
        // Populate generators if empty (only first time)
        if (!_generators.Any())
        {
            foreach (var kvp in Options)
            {
                _generators.Add(new KeyValuePair<string, Func<dynamic>>(kvp.Key, kvp.Value.GetGenerator()));
            }
        }
        
        return () =>
        {
            var idx = Faker.Random.Int(0, Options.Count - 1);
            var keyValuePair = _generators[idx];
            return new KeyValuePair<string, dynamic>(keyValuePair.Key, keyValuePair.Value());
        };
    }

    protected override bool CanGenerate(object? value)
    {
        return false; // TODO: This is not possible with single-value input, maybe if we provide multiple
    }
}