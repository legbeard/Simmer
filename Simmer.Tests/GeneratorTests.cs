using System.Collections.Generic;
using System.Linq;
using Simmer.Deserialization;
using Xunit;

namespace Simmer.Tests;

public class GeneratorTests
{
    public class TestObject
    {
      public string name { get; set; }
      public int positive { get; set; }
    }

    [Fact]
    public void Should_ParseMappingAndGenerateSimpleValue()
    {
      var modelYaml = @"
number:
  type: int
  min: 100
  max: 100";
      
      var generatorFunc = YamlSerializer.Deserialize(modelYaml).GetGenerator();
      var value = generatorFunc();
      Assert.True(value == 100);
    }
    
    [Fact]
    public void Should_ParseMappingAndGenerateSimpleObject()
    {
      var modelYaml =
        @"
obj:
  type: object
  content:
    name:
      type: name
    positive:
      type: int
      min: 0";

      var generatorFunc = YamlSerializer.Deserialize(modelYaml).GetGenerator();

      Dictionary<string, object> generatedObject = generatorFunc();
      var obj = generatedObject.ToObject<TestObject>();
      Assert.NotNull(obj.name);
      Assert.True(obj.positive >= 0);
    }
    
    [Fact]
    public void Should_ParseListAndGenerateListWithSimpleValues()
    {
      var modelYaml = @"
- type: int
  min: 100
  max: 100
- type: name";
      
      var generatorFunc = YamlSerializer.Deserialize(modelYaml).GetGenerator();
      IEnumerable<dynamic> value = generatorFunc();
      Assert.True(value.Count() == 2);
    }
}