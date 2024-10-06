using System;
using HarmonyLib;
using Memoria.Bloomtown;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Core;
using Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(SaveManager), "Loading")]
public static class SaveManager_Loading
{
    public static void Prefix(String dataStr)
    {
        try
        {
            ItemsPanel_SetupObjectButton.Clear(force: true);
            GameSaveControl.TryDeleteExitSave(dataStr);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}