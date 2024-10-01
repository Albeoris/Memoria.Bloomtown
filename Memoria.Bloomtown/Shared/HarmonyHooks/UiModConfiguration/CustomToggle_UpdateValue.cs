using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using HarmonyLib;
using Memoria.Bloomtown;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Configuration;
using Memoria.Bloomtown.Configuration.Hotkey;
using Memoria.Bloomtown.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[HarmonyPatch(typeof(CustomToggle), "UpdateValue")]
public static class CustomToggle_UpdateValue
{
    public static void Postfix(CustomToggle __instance)
    {
        try
        {
            UnityAction<Boolean> onValueChanged = __instance.onValueChanged;
            if (onValueChanged is null)
                return;

            // Fix the bug with notification
            
            String stackTrace = Environment.StackTrace;
            
            // Suppressed for a reason
            if (stackTrace.Contains("SetValueWithoutNotify"))
                return;
            
            // Already raised
            if (stackTrace.Contains("OnSubmit"))
                return;
            
            onValueChanged.Invoke(__instance.value);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}