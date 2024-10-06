using System;
using Memoria.Bloomtown.Core;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public static class EnumCache<T> where T : Enum
{
    public static String[] Names { get; }
    public static T[] Values { get; }
    public static OrderedDictionary<String, T> NamedValues { get; }

    static EnumCache()
    {
        Type enumerationType = typeof(T);

        Names = Enum.GetNames(enumerationType);
        Values = (T[])Enum.GetValues(enumerationType);

        OrderedDictionary<String, T> result = new();
        for (Int32 i = 0; i < Names.Length; i++)
            result.TryAdd(Names[i], Values[i]);
        NamedValues = result;
    }
}