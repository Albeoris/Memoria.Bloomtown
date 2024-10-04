using System;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.BeepInEx;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(LazyInput), "GetKey")]
public static class LazyInput_GetKey
{
    public static void Postfix(GameKey key, ref Boolean __result)
    {
        try
        {
            if (key == GameKey.Sprint && ModComponent.Instance.Config.Speed.AlwaysSprint)
                __result = !__result;
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}