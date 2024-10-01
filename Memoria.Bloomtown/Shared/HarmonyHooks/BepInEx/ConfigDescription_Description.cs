using System;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Memoria.Bloomtown.BeepInEx;
using UnityEngine;

namespace Memoria.Bloomtown.HarmonyHooks;

// ReSharper disable InconsistentNaming
public static class ConfigDescription_Description
{
    public static ManualLogSource Log { get; set; }

    public static void Patch(Harmony harmony, ManualLogSource log)
    {
        Log = log;

        harmony.Patch(GetOriginalMethod(), prefix: GetPrefixMethod());

        Log.LogInfo($"[Harmony] {nameof(ConfigDescription_Description)} successfully applied.");
    }

    private static MethodBase GetOriginalMethod()
    {
        return typeof(ConfigDescription).GetConstructors().Single();
    }

    private static HarmonyMethod GetPrefixMethod()
    {
        return new HarmonyMethod(typeof(ConfigDescription_Description).GetMethod(nameof(Prefix)));
    }

    public static void Prefix(ref String description)
    {
        try
        {
            SystemLanguage systemLanguage = Application.systemLanguage;

            Int32 startIndex = description.IndexOf("$[");
            if (startIndex < 0)
                return;

            String key = $"$[{systemLanguage}]:";
            Int32 newIndex = description.IndexOf(key, startIndex);
            if (newIndex < 0)
            {
                description = description.Substring(0, startIndex);
                return;
            }

            Int32 nextIndex = description.IndexOf("$[", newIndex + key.Length);
            if (nextIndex < 0)
                nextIndex = description.Length;

            Int32 begin = newIndex + key.Length;
            Int32 length = nextIndex - begin;
            description = description.Substring(begin, length).Trim();
        }
        catch (Exception ex)
        {
            Log.LogException(ex);
        }
    }
}