using System;
using BepInEx.Configuration;
using Memoria.Bloomtown.HarmonyHooks;

namespace Memoria.Bloomtown.Configuration;

public static class ExtensionsConfigEntryBase
{
    public static Boolean HasFileDefinedValue(this ConfigEntryBase entry)
    {
        return entry.Description is MemoriaConfigDescription { HasFileDefinedValue: true };
    }
}