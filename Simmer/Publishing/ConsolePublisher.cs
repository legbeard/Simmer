using System.Text.Json;

namespace Simmer.Publishing;

public class ConsolePublisher : IPublisher
{
    public void Publish(object obj)
    {
        Console.WriteLine(JsonSerializer.Serialize(obj));
    }
}