namespace Simmer.Generation.Model;

public interface IPayloadGeneratorFactory
{
    Func<dynamic> GetGenerator();
}