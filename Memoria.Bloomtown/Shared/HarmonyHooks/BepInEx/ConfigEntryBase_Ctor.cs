using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Memoria.Bloomtown.Configuration;

namespace Memoria.Bloomtown.HarmonyHooks;

/// <summary>
/// Replaces the normal description with an expanded one to store additional data.
/// </summary>
public static class ConfigEntryBase_Ctor
{
    public static ManualLogSource Log { get; set; }

    public static void Patch(Harmony harmony, ManualLogSource log)
    {
        Log = log;

        harmony.Patch(GetOriginalMethod(), prefix: GetPrefixMethod());

        Log.LogInfo($"[Harmony] {nameof(ConfigEntryBase_Ctor)} successfully applied.");
    }

    private static MethodBase GetOriginalMethod()
    {
        return typeof(ConfigEntryBase).GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Single();
    }

    private static HarmonyMethod GetPrefixMethod()
    {
        return new HarmonyMethod(typeof(ConfigEntryBase_Ctor).GetMethod(nameof(ConstructorPrefix)));
    }
    
    public static void ConstructorPrefix(ref ConfigDescription configDescription)
    {
        ConfigDescription descriptor = configDescription ?? ConfigDescription.Empty;
        if (descriptor is not MemoriaConfigDescription)
            configDescription = new MemoriaConfigDescription(descriptor.Description, descriptor.AcceptableValues, descriptor.Tags);
    }
}