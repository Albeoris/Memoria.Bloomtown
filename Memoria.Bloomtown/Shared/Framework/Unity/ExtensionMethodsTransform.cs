using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Memoria.Bloomtown.Shared.Framework.Unity;

internal static class ExtensionMethodsTransform
{
    public static IEnumerable<RectTransform> EnumerateChildren(this RectTransform parent)
    {
        return EnumerateChildren((Transform)parent).Select(c => (RectTransform)c);
    }
    
    public static IEnumerable<Transform> EnumerateChildren(this Transform parent)
    {
        if (parent is null) throw new ArgumentNullException(nameof(parent));

        for (Int32 i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            yield return child;
        }
    }
    
    public static IReadOnlyDictionary<String, RectTransform> GetChildrenByName(this RectTransform parent)
    {
        return parent.EnumerateChildren().ToDictionary(c => c.name);
    }

    public static IReadOnlyDictionary<String, Transform> GetChildrenByName(this Transform parent)
    {
        return parent.EnumerateChildren().ToDictionary(c => c.name);
    }
    
    [CanBeNull]
    public static RectTransform FindChildByName(this RectTransform parent, String name)
    {
        return (RectTransform)FindChildByName((Transform)parent, name);
    }
    
    [CanBeNull]
    public static Transform FindChildByName(this Transform parent, String name)
    {
        if (parent is null) throw new ArgumentNullException(nameof(parent));
        if (name is null) throw new ArgumentNullException(nameof(name));

        return parent.EnumerateChildren().FirstOrDefault(obj => obj.name == name);
    }

    [NotNull]
    public static RectTransform GetChildByName(this RectTransform parent, String name)
    {
        return (RectTransform)GetChildByName((Transform)parent, name);
    }
    
    [NotNull]
    public static Transform GetChildByName(this Transform parent, String name)
    {
        if (parent is null) throw new ArgumentNullException(nameof(parent));
        if (name is null) throw new ArgumentNullException(nameof(name));

        Transform child = parent.FindChildByName(name);
        if (child is not null)
            return child;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Cannot find child with name [{name}] for the object {parent.name} ({parent.GetInstanceID()}).");
        sb.AppendLine("Existing children: " + String.Join(", ", parent.EnumerateChildren().Select(c => c.name)));
        throw new ArgumentException(sb.ToString());
    }
}