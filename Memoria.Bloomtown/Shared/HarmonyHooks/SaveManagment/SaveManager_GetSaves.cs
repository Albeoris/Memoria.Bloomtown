using System;
using System.Collections.Generic;
using HarmonyLib;
using Memoria.Bloomtown;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Core;

[HarmonyPatch(typeof(SaveManager), "GetSaves")]
public static class SaveManager_GetSaves
{
    private static Boolean _isInitializing;
    
    public static void Prefix(List<SaveManager.SaveDataInfo> ____saves)
    {
        try
        {
            _isInitializing = ____saves is null;
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
    
    public static void Postfix(List<SaveManager.SaveDataInfo> ____saves)
    {
        try
        {
            if (!_isInitializing)
                return;
            
            GameSaveControl.AddSpecialSaves(____saves);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}