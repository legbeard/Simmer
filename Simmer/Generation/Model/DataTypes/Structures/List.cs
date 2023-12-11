using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using Simmer.Deserialization;
using YamlDotNet.Serialization;

namespace Simmer.Model.DataTypes.Structures;

public class List : DataTypeBase
{
    public ListRoot Content { get; set; } = new();
    
    [DefaultValue(null)]
    public int? Repeat { get; set; } = null;
    
    [DefaultValue(null)]
    public int? MinRepeat { get; set; } = null;
    
    [DefaultValue(null)]
    public int? MaxRepeat { get; set; } = null;

    private bool _repeated = false;
    private List<ListRoot> _repeats = new();

    public override Func<dynamic> GetGenerator()
    {
        if (!_repeated)
        {
            _repeats.AddRange(Enumerable.Range(0, GetMaxRepeats()).Select(_ => DeepCopy(Content) ?? throw new ApplicationException("Unable to DeepCopy content")));
            _repeated = true;
        }
        return () =>
        {
            var content = new ListRoot();
            var additionalRepeats = _repeats.Take(GetRepeats());
            
            foreach(var additionalRepeat in additionalRepeats)
            {
                content.AddRange(additionalRepeat);
            }
            
            // Get the generator for the root
            var generator = content.GetGenerator();
            // Call the generator
            return generator();
        };
    }
    
    protected override bool CanGenerate(object? value)
    {
        return value is IList;
    }

    private int GetMaxRepeats()
    {
        return MaxRepeat ?? Repeat ?? 1;
    }
    
    private int GetRepeats()
    {
        if (Repeat.HasValue)
        {
            return Repeat.Value;
        }
        
        if(MinRepeat.HasValue && MaxRepeat.HasValue)
        {
            var val = Faker.Random.Int(MinRepeat.Value, MaxRepeat.Value);
            Console.WriteLine("Repeating {0} times", val);
            return val;
        }

        return 1;
    }
    
    public static ListRoot DeepCopy(ListRoot source)
    {
        return YamlSerializer.Deserialize(
            YamlSerializer.Serialize(source)
            ) as ListRoot ?? throw new ApplicationException("Can't deserialize to ListRoot");
    }
}