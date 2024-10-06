using System;
using HarmonyLib;
using Memoria.Bloomtown.BeepInEx;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(ItemsPanel), "Close")]
public static class ItemsPanel_Close
{
    public static void Prefix(bool showMenuPanel = true)
    {
        try
        {
            ItemsPanel_SetupObjectButton.Clear(force: false);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}