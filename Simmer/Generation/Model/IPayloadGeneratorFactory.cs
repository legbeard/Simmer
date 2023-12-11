namespace Simmer.Model;

public interface IPayloadGeneratorFactory
{
    Func<dynamic> GetGenerator();
}