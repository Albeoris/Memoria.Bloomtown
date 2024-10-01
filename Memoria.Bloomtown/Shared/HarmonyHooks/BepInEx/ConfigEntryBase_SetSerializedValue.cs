using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Memoria.Bloomtown.Configuration;

namespace Memoria.Bloomtown.HarmonyHooks;

/// <summary>
/// Stores information that an option has a value in a configuration file.
/// </summary>
public static class ConfigEntryBase_SetSerializedValue
{
    public static ManualLogSource Log { get; set; }

    public static readonly MethodInfo OrphanedEntriesGetter = AccessTools.PropertyGetter(typeof(ConfigFile), "OrphanedEntries");

    public static void Patch(Harmony harmony, ManualLogSource log)
    {
        Log = log;

        harmony.Patch(GetOriginalMethod(), postfix: GetPostfixMethod());

        Log.LogInfo($"[Harmony] {nameof(ConfigEntryBase_SetSerializedValue)} successfully applied.");
    }

    private static MethodBase GetOriginalMethod()
    {
        return typeof(ConfigEntryBase).GetMethod("SetSerializedValue", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
    }

    private static HarmonyMethod GetPostfixMethod()
    {
        return new HarmonyMethod(typeof(ConfigEntryBase_SetSerializedValue).GetMethod(nameof(SetSerializedValuePostfix)));
    }
    
    public static void SetSerializedValuePostfix(ConfigEntryBase __instance)
    {
        Dictionary<ConfigDefinition, String> orphanedEntries = (Dictionary<ConfigDefinition, String>)OrphanedEntriesGetter.Invoke(__instance.ConfigFile, null);
        if (!orphanedEntries.ContainsKey(__instance.Definition))
            return;
        
        MemoriaConfigDescription extendedConfig = (MemoriaConfigDescription)__instance.Description;
        extendedConfig.HasFileDefinedValue = true;
    }
}