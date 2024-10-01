using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Memoria.Bloomtown.Core;

public static class ExtensionMethodsUniteEvent
{
    public static void AddListener(this Button.ButtonClickedEvent self, Action callback)
    {
        self.AddListener(new UnityAction(callback));
    }
    
    public static void RemoveListener(this Button.ButtonClickedEvent self, Action callback)
    {
        self.RemoveListener(new UnityAction(callback));
    }
}