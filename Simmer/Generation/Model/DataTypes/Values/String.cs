using System.ComponentModel;
using Bogus;

namespace Simmer.Generation.Model.DataTypes.Values;

public class String : DataTypeBase
{
    [DefaultValue(StringValueGeneration.Always)]
    public StringValueGeneration Generate { get; set; } = StringValueGeneration.Always;
    
    [DefaultValue(StringValueFormat.Constant)]
    public StringValueFormat Format { get; set; } = StringValueFormat.Constant;
    
    [DefaultValue(null)]
    public string? Value { get; set; }

    [DefaultValue(0)]
    public int MinLength { get; set; } = 0;

    [DefaultValue(100)]
    public int MaxLength { get; set; } = 100;
    
    [DefaultValue(null)]
    public int? Length { get; set; }
    
    private int GetLength()
    {
        return Length ?? Faker.Random.Int(MinLength, MaxLength);
    }

    public String()
    {
        
    }

    public String(string value)
    {
        Value = value;
    }
    
    public override Func<dynamic> GetGenerator()
    {
        var generatorFunc = Format switch
        {
            StringValueFormat.Address => () => Faker.Address.FullAddress(),
            StringValueFormat.AlphaNumeric => () => Faker.Random.AlphaNumeric(GetLength()),
            StringValueFormat.Constant => GetConstantGenerator(),
            StringValueFormat.Email => () => Faker.Internet.Email(),
            StringValueFormat.Guid => () => Faker.Random.Guid().ToString(),
            StringValueFormat.Hexadecimal => () => Faker.Random.Hexadecimal(GetLength()),
            StringValueFormat.Lorem => () => Faker.Lorem.Sentence(GetLength()),
            StringValueFormat.Letters => () => Faker.Random.String2(GetLength()),
            StringValueFormat.Url => () => Faker.Internet.Url(),
            StringValueFormat.Uuid => () => Faker.Random.Uuid().ToString(),
            _ => throw new ArgumentOutOfRangeException(nameof(Format), "Invalid string format. Please use one of the following: string, guid, email, url, hexadecimal, alphanumeric, uuid")
        };

        switch (Generate)
        {
            case StringValueGeneration.Once:
                var value = generatorFunc();
                return () => value;
            case StringValueGeneration.Always:
                return generatorFunc;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    protected override bool CanGenerate(object? value)
    {
        return value is string; // TODO: When we get to using multiple, determine if it's possible to figure out which type of randomizer to use
    }

    private Func<dynamic> GetConstantGenerator()
    {
        return !string.IsNullOrWhiteSpace(Value) ? () => Value : throw new ArgumentException("Please supply a 'value' for constant string generation.");
    }
}

public enum StringValueFormat
{
    Address,
    AlphaNumeric,
    Constant,
    Email,
    Guid,
    Hexadecimal,
    Lorem,
    Letters,
    Url,
    Uuid, 
}

public enum StringValueGeneration
{
    Once,
    Always
}