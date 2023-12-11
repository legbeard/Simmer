using System.Collections;
using System.ComponentModel;
using Simmer.Generation.Model.Roots;

namespace Simmer.Generation.Model.DataTypes.Structures;

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
    private List<Func<dynamic>> _repeatedGenerators = new();

    public override Func<dynamic> GetGenerator()
    {
        if (!_repeated)
        {
            _repeatedGenerators.AddRange(Enumerable.Range(0, GetMaxRepeats()).Select(_ => DeepCopy(Content).GetGenerator()));
            _repeated = true;
        }
        return () =>
        {
            List<dynamic> content = new();
            
            foreach(var generator in _repeatedGenerators.Take(GetRepeats()))
            {
                content.AddRange(GetGeneratedValues(generator));
            }

            return content;
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
            return Faker.Random.Int(MinRepeat.Value, MaxRepeat.Value);;
        }

        return 1;
    }
    
    public IEnumerable<dynamic> GetGeneratedValues(Func<dynamic> generator)
    {
        var value = generator();
        if (value is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }
        }
        else
        {
            yield return value;
        }
    }
    
    public static ListRoot DeepCopy(ListRoot source)
    {
        return YamlSerializer.Deserialize(
            YamlSerializer.Serialize(source)
            ) as ListRoot ?? throw new ApplicationException("Can't deserialize to ListRoot");
    }
}