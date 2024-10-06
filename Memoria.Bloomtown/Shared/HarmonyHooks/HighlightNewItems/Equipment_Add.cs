using System;
using HarmonyLib;
using Memoria.Bloomtown.BeepInEx;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(Equipment), "Add")]
public static class Equipment_Add
{
    public static void Prefix(Int32 amount,
        Equipment __instance)
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