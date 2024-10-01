using System;
using HarmonyLib;
using Memoria.Bloomtown;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Core;

[HarmonyPatch(typeof(SaveManager), "Loading")]
public static class SaveManager_Loading
{
    public static void Prefix(String dataStr)
    {
        try
        {
            GameSaveControl.TryDeleteExitSave(dataStr);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}