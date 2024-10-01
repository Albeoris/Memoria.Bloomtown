using System;
using System.Collections.Generic;
using BepInEx.Logging;

namespace Memoria.Bloomtown.BeepInEx;

public static class ExtensionMethods
{
    public static IReadOnlyList<T> DistinctBy<T, TKey>(this IEnumerable<T> self, Func<T, TKey> selector)
    {
        List<T> result;
        if (self is IReadOnlyCollection<T> collection)
            result = new List<T>(collection.Count);
        else
            result = new();

        HashSet<TKey> set = new();
        foreach (var item in self)
        {
            TKey key = selector(item);
            if (set.Add(key))
                result.Add(item);
        }

        return result;
    }

    public static void LogException(this ManualLogSource logSource, Exception ex)
    {
        logSource.LogError(ex.ToString());
    }

    public static void LogException(this ManualLogSource logSource, Exception ex, String error)
    {
        logSource.LogError(error);
        logSource.LogError(ex.ToString());
    }
}