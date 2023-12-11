using Simmer.Generation.Model;
using Simmer.Generation.Model.DataTypes;
using Simmer.Generation.Model.Roots;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Simmer.Generation;

public static class YamlSerializer
{
    public static IPayloadGeneratorFactory Deserialize(string yamlString)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithTypeDiscriminatingNodeDeserializer((o) =>
            {
                o.AddKeyValueTypeDiscriminator<DataTypeBase>("type", DataTypeBase.GetTypeMappings());
            })
            .Build();


        if (yamlString.TrimStart().StartsWith("type"))
        {
            
        }
        
        
        if(yamlString.TrimStart().StartsWith('-'))
        {
            return deserializer.Deserialize<ListRoot>(yamlString);
        }
        
        return deserializer.Deserialize<MappingRoot>(yamlString);
    }
    
    public static string Serialize(IPayloadGeneratorFactory value)
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
            .Build();

        return serializer.Serialize(value);
    }
}