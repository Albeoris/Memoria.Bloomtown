using System;
using HarmonyLib;
using Memoria.Bloomtown.BeepInEx;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(ItemsPanel), "UpdateDescription")]
public static class ItemsPanel_UpdateDescription
{
    public static void Prefix(object descriptableButton = null)
    {
        try
        {
            ItemsPanel_SetupObjectButton.ChangeItemOutline(descriptableButton, removeHighlighting: true);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}