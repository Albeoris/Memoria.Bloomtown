using System;
using LazyBearTechnology;
using Memoria.Bloomtown.Core;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public static class EnumerationMapping<T> where T : Enumeration
{
    public static OrderedDictionary<T, String> Names { get; } = GetNames();

    private static OrderedDictionary<T, String> GetNames()
    {
        OrderedDictionary<T, String> result = new();
        
        foreach ((String key, T value) in EnumerationCache<T>.NamedValues)
        {
            if (!result.TryAdd(value, key))
                throw new Exception($"Duplicate enumeration value: {key} -> {value}");
        }

        return result;
    }
}