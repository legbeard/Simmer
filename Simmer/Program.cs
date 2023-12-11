// See https://aka.ms/new-console-template for more information

using Bogus;
using Simmer;
using Simmer.Generation;
using Simmer.Publishing;

var jsonContent = File.ReadAllText("json.json");
var model = new ReverseEngineering().ReverseEngineer(jsonContent);
var modelYaml = YamlSerializer.Serialize(model);
Console.WriteLine(modelYaml);

// modelYaml = File.ReadAllText("model.yaml");
model = YamlSerializer.Deserialize(modelYaml);
var generatorFunc = model.GetGenerator();
var publisher = new ConsolePublisher();
for (var i = 0; i < 10; i++)
{
    publisher.Publish(generatorFunc());
}

var faker = new Faker();


