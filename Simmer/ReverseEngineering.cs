using System.Text.Json;
using Simmer.Generation.Model;
using Simmer.Generation.Model.DataTypes;
using Simmer.Generation.Model.DataTypes.Structures;
using Simmer.Generation.Model.Roots;
using Object = Simmer.Generation.Model.DataTypes.Structures.Object;

namespace Simmer;

public class ReverseEngineering
{
    public IPayloadGeneratorFactory ReverseEngineer(string jsonString)
    {
        var jsonElement = JsonDocument.Parse(jsonString).RootElement;

        return jsonElement.ValueKind switch
        {
            JsonValueKind.Object => GetMappingRoot(jsonElement),
            JsonValueKind.Array => GetListRoot(jsonElement),
            _ => GetValueRoot(jsonElement)
        };
    }

    private IPayloadGeneratorFactory GetMappingRoot(JsonElement jsonElement)
    {
        var mappingRoot = new MappingRoot();
        
        foreach(var element in jsonElement.EnumerateObject())
        {
            mappingRoot.Add(element.Name, GetDataType(element.Value));
        }
        
        return mappingRoot;
    }
    
    private IPayloadGeneratorFactory GetListRoot(JsonElement jsonElement)
    {
        var listRoot = new ListRoot();
        foreach(var element in jsonElement.EnumerateArray())
        {
            listRoot.Add(GetDataType(element));
        }
        return listRoot;
    }

    private IPayloadGeneratorFactory GetValueRoot(JsonElement jsonElement)
    {
        throw new NotImplementedException();
    }

    private DataTypeBase GetDataType(JsonElement jsonElement)
    {
        return jsonElement.ValueKind switch
        {
            JsonValueKind.Object => GetMapping(jsonElement),
            JsonValueKind.Array => GetList(jsonElement),
            _ => GetValue(jsonElement)
        };
    }

    private DataTypeBase GetValue(JsonElement jsonElement)
    {
        var jsonType = jsonElement.ValueKind switch
        {
            JsonValueKind.String => typeof(string),
            JsonValueKind.Number => typeof(long),
            JsonValueKind.True => typeof(bool),
            JsonValueKind.False => typeof(bool),
            JsonValueKind.Null => typeof(object),
            _ => throw new ArgumentException("This can't happen")
        };
        
        var value = jsonElement.Deserialize(jsonType);
        var generatingType = DataTypeBase.GetGeneratingType(value);
        return Activator.CreateInstance(generatingType, value) as DataTypeBase ??
               throw new Exception("Failed to reverse engineer data type");
    }

    private DataTypeBase GetList(JsonElement jsonElement)
    {
        var list = new List();
        foreach(var element in jsonElement.EnumerateArray())
        {
            list.Content.Add(GetDataType(element));
        }

        return list;
    }

    private DataTypeBase GetMapping(JsonElement jsonElement)
    {
        var mapping = new Object();
        foreach(var element in jsonElement.EnumerateObject())
        {
            mapping.Content.Add(element.Name, GetDataType(element.Value));
        }

        return mapping;
    }
}