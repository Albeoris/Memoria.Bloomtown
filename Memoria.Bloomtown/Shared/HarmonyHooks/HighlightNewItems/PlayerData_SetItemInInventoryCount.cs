using System;
using HarmonyLib;
using Memoria.Bloomtown.BeepInEx;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(PlayerData), "SetItemInInventoryCount")]
public static class PlayerData_SetItemInInventoryCount
{
    public static void Prefix(Stackable obj, Int32 count,
        PlayerData __instance)
    {
        try
        {
            if (count > 0)
                return;
            
            ItemsPanel_SetupObjectButton.OnRemoveStackable(obj);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}