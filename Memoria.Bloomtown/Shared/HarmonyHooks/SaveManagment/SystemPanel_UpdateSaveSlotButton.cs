using System;
using FMODUnity;
using HarmonyLib;
using Memoria.Bloomtown;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Core;
using UnityEngine.UI;

[HarmonyPatch(typeof(SystemPanel), "UpdateSaveSlotButton")]
public static class SystemPanel_UpdateSaveSlotButton
{
    public static void Postfix(Button button, SaveManager.SaveDataInfo saveDataInfo = null, Int32 index = -1)
    {
        try
        {
            if (button is null || saveDataInfo is null)
                return;

            GameSaveControl.TryColorLoadButton(button, saveDataInfo);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}