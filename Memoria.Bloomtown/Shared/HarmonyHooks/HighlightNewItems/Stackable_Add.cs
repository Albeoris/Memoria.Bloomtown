using System;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.BeepInEx;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(Stackable), "Add")]
public static class Stackable_Add
{
    public static void Prefix(Int32 amount,
        Stackable __instance)
    {
        try
        {
            if (amount < 1)
                return;
            
            // Deserialization
            if (Environment.StackTrace.Contains("SetCount"))
                return;

            ItemsPanel_SetupObjectButton.OnAddStackable(__instance, amount);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}