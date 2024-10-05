using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.BeepInEx;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch]
public static class RemoveNotSupportedExceptionSpam
{
    [HarmonyTargetMethods]
    private static MethodInfo[] GetTargetMethods()
    {
        return typeof(UnityEngine.Windows.Input).GetMethods().Where(m => m.Name == "ForwardRawInput").ToArray();
    }
    
    public static Boolean Prefix()
    {
        return false; // don't call original method
    }
}