using BenchmarkDotNet.Attributes;
using Simmer.Generation;

namespace Simmer.Benchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
  private Func<dynamic> _listFunc;
  private Func<dynamic> _objFunc;
  private Func<dynamic> _valueFunc;
    public Benchmarks()
    {
        var listYaml = @"
- type: name
- type: int
  min: 0
  max: 100";

        var objYaml = @"
name:
  type: name
number:
  type: int";

        var valueYaml = @"
name:
  type: name";
        _listFunc = YamlSerializer.Deserialize(listYaml).GetGenerator();
        _objFunc = YamlSerializer.Deserialize(objYaml).GetGenerator();
        _valueFunc = YamlSerializer.Deserialize(valueYaml).GetGenerator();
    }

    [Benchmark]
    public dynamic GenerateList() => _listFunc();
    
    [Benchmark]
    public dynamic GenerateObject() => _objFunc();
    
    [Benchmark]
    public dynamic GenerateValue() => _valueFunc();
}