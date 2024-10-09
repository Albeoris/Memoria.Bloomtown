using System;
using HarmonyLib;
using Memoria.Bloomtown;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Core;
using UnityEngine.Events;

[HarmonyPatch(typeof(SceneLoading), "ReturnToTitleScreenScene")]
public static class SceneLoading_ReturnToTitleScreenScene
{
    public static void Prefix()
    {
        try
        {
            GameSaveControl.TrySaveOnExit();
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}