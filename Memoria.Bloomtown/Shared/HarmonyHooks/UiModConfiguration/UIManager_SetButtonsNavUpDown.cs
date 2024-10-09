using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.UI;

[HarmonyPatch(typeof(UIManager), "SetButtonsNavUpDown", [typeof(List<Selectable>)])]
public static class UIManager_SetButtonsNavUpDown
{
    public static List<Selectable> LastAssignedSelectables { get; set; }

    [HarmonyPostfix]
    public static void SetButtonsNavUpDownPostfix(List<Selectable> selectables)
    {
        LastAssignedSelectables = selectables;
    }
}