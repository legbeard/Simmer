﻿using Bogus;
using Simmer.Extensions;
using YamlDotNet.Serialization;

namespace Simmer.Generation.Model.DataTypes;

public abstract class DataTypeBase : IPayloadGeneratorFactory
{
    protected static readonly Faker Faker = new ();
    
    [YamlMember(Order = -1)] // Used to make sure that the type is always the first property in the yaml when serializing the model
    public string Type
    {
        get => GetType().Name.ToLower();
        set => _ = value;
    }

    public abstract Func<dynamic> GetGenerator();
    protected abstract bool CanGenerate(object? value);

    // This will expand forever, based on number of possible data types to produce.
    // Depending on how annoying this gets, find a better way to do this, reflection might be an option.
    public static Dictionary<string, Type> GetTypeMappings() => typeof(DataTypeBase).GetDerivedTypes().ToDictionary(x => x.Name.ToLower(), x => x);

    public static Type GetGeneratingType(object? value)
    {
        // TODO: Make CanGenerate return a tuple of (bool, object[]), where the object[] is the arguments to the ctor
        // TODO: Make this able to take an array of values and evaluate matches based on whether all values match
        // TODO: Implement a priority system, where better matches get picked over more generic ones (e.g. string vs timestamp)
        return CanGenerateMappings.FirstOrDefault(mapping => mapping.canGenerate(value)).type
               ?? throw new Exception($"No type found for value {value}");
    }

    private static List<(Type type, Func<object?, bool> canGenerate)> CanGenerateMappings { get; } = GetCanGenerateMappingsReflection();
    
    private static List<(Type type, Func<object?, bool> canGenerate)> GetCanGenerateMappingsReflection()
    {
        List<(string name, DataTypeBase? instance)> namedInstances = 
            typeof(DataTypeBase)
                .GetDerivedTypes()
                .Select(x => (x.Name, Activator.CreateInstance(x) as DataTypeBase))
                .ToList();
        
        if(namedInstances.Any(x => x.instance == null))
            throw new Exception($"Could not create instance of type {namedInstances.First(x => x.instance == null).name}");
                
        return namedInstances.Select(x => x.instance).Select<DataTypeBase?, (Type, Func<object?, bool>)>(
            x => x != null ? (x.GetType(), (x.CanGenerate)) : throw new Exception($"Could not create instance of type {x}"
            )).ToList();
    }
}