using System.Reflection;

namespace Simmer.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<Type> GetDerivedTypes(this Type baseType, Assembly[]? assemblies = null)
    {
        assemblies ??= AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.SelectMany(a => a.GetTypes()).Where(t => baseType.IsAssignableFrom(t) && t != baseType);
    }
}