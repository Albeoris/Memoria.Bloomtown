using System;
using System.Collections.Generic;
using HarmonyLib;
using Memoria.Bloomtown.BeepInEx;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(ItemsPanel), "SetupObjectButton")]
public static class ItemsPanel_SetupObjectButton
{
    private static readonly Dictionary<Stackable, Color> HighlightedItems = new();
    private static Boolean HasHighlighted;

    public static void Prefix(object descriptableButton)
    {
        try
        {
            ChangeItemOutline(descriptableButton, removeHighlighting: false);
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }

    public static void OnAddStackable(Stackable stackable, Int32 amount)
    {
        if (stackable.count == 0)
            HighlightedItems.TryAdd(stackable, Color.green);
        else
            HighlightedItems.TryAdd(stackable, Color.blue);
    }

    public static void OnRemoveStackable(Stackable stackable)
    {
        HighlightedItems.Remove(stackable);
    }

    public static void ChangeItemOutline(System.Object descriptableButton, Boolean removeHighlighting)
    {
        Stackable stackable = (Stackable)AccessTools.Field(descriptableButton.GetType(), "stackable").GetValue(descriptableButton);
        if (stackable is null)
            return;

        if (removeHighlighting)
            HighlightedItems.Remove(stackable);

        Button button = (Button)AccessTools.Field(descriptableButton.GetType(), "button").GetValue(descriptableButton);
        Transform icon = button.transform.Find("Icon");

        Outline outline = icon.gameObject.GetComponent<Outline>();
        if (HighlightedItems.TryGetValue(stackable, out Color color))
        {
            HasHighlighted = true;
            outline ??= icon.gameObject.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = new Vector2(3, 3);   
        }
        else if (outline is not null)
        {
            Object.Destroy(outline);
        }
    }

    public static void Clear(Boolean force)
    {
        if (force || HasHighlighted)
            HighlightedItems.Clear();
    }
}