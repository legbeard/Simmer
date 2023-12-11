using System.Collections.Generic;

namespace Simmer.Tests;

public static class ObjectExtensions
{
    public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source)
        {
            someObjectType
                .GetProperty(item.Key)
                ?.SetValue(someObject, item.Value, null);
        }

        return someObject;
    }
}