using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = System.Object;

namespace Memoria.Bloomtown.Core;

public static class UIBuilder
{
    public static GameObject CreateUiObject(String name, Transform parent)
    {
        GameObject gameObject = new GameObject();
        ConfigureUiObject(gameObject, name, parent);
        return gameObject;
    }

    public static void ConfigureUiObject(GameObject gameObject, String name, Transform parent)
    {
        gameObject.name = $"{ModConstants.Id}: {name}";
        gameObject.layer = LayerMask.NameToLayer("UI");

        if (!ReferenceEquals(parent, null))
        {
            gameObject.transform.SetParent(parent.transform, false);
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public static Canvas CreateCanvas(String name, Transform parent)
    {
        GameObject gameObject = CreateUiObject(name, parent);

        Canvas canvas = gameObject.AddComponent<Canvas>();
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 9999;
        }

        gameObject.AddComponent<GraphicRaycaster>();

        return canvas;
    }
    
    public static TMPButton CreateButton(String name, Transform parent)
    {
        TMP_DefaultControls.Resources resources = new TMP_DefaultControls.Resources();

        GameObject newButton = TMP_DefaultControls.CreateButton(resources);
        ConfigureUiObject(newButton, name, parent);

        TMPButton result = new TMPButton(newButton);
        result.Text = name;
        return result;
    }
}