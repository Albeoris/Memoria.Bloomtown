using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.Core;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public static class EnumerationCache<T> where T : Enumeration
{
    public static OrderedDictionary<String, T> NamedValues { get; } = GetNamedValues();

    private static OrderedDictionary<String, T> GetNamedValues()
    {
        OrderedDictionary<String, T> result = new();

        Type enumerationType = typeof(T);

        IEnumerable<(String Name, T Value)> values = enumerationType.GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.DeclaringType == enumerationType && f.FieldType == enumerationType)
            .Select(f => (Name: f.Name, Value: (T)f.GetValue(null)));

        foreach ((String name, T value) in values)
        {
            if (!result.TryAdd(name, value))
                throw new Exception($"Duplicate enumeration value: {name} -> {value}");
        }

        return result;
    }
}