using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Memoria.Bloomtown.Shared.Framework.Unity;

internal static class ExtensionMethodsGameObject
{
    public static IEnumerable<GameObject> EnumerateChildren(this GameObject parent)
    {
        if (parent is null) throw new ArgumentNullException(nameof(parent));

        Transform transform = parent.transform;
        for (Int32 i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            yield return child.gameObject;
        }
    }

    [CanBeNull]
    public static GameObject FindChildByName(this GameObject parent, String name)
    {
        if (parent is null) throw new ArgumentNullException(nameof(parent));
        if (name is null) throw new ArgumentNullException(nameof(name));

        return parent.transform.FindChildByName(name)?.gameObject;
    }
    
    [NotNull]
    public static GameObject GetChildByName(this GameObject parent, String name)
    {
        if (parent is null) throw new ArgumentNullException(nameof(parent));
        if (name is null) throw new ArgumentNullException(nameof(name));

        return parent.transform.GetChildByName(name).gameObject;
    }

    public static T GetExactComponent<T>(this GameObject obj) where T : Component
    {
        T[] components = obj.GetComponents<T>();
        if (components.Length == 1)
        {
            T component = components[0];
            if (component.GetType() == TypeCache<T>.Type)
                return component;
        }

        Component[] allComponents = obj.GetComponents<Component>();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Cannot find component of type {TypeCache<T>.Type} for the object {obj.name} ({obj.GetInstanceID()}).");
        sb.AppendLine("Existing components: " + String.Join(", ", allComponents.Select(c => c.GetType().Name)));
        throw new ArgumentException(sb.ToString());
    }

    public static T GetExactComponent<T>(this GameObject obj, Int32 index) where T : Component
    {
        return obj.GetComponents<T>().First(c => c.GetType() == TypeCache<T>.Type && --index < 0);
    }

    public static T[] GetExactComponents<T>(this GameObject obj) where T : Component
    {
        return obj.GetComponents<T>().Where(c => c.GetType() == TypeCache<T>.Type).ToArray();
    }

    public static T EnsureComponent<T>(this GameObject obj) where T : Component
    {
        return obj.GetComponent<T>() ?? obj.AddComponent<T>();
    }
    
    public static T EnsureComponent<T>(this GameObject obj, Boolean initialEnabled) where T : MonoBehaviour
    {
        var component = obj.GetComponent<T>();
        if (component is not null)
            return component;
        
        component = obj.AddComponent<T>();
        component.enabled = initialEnabled;
        return component;
    }

    public static T EnsureExactComponent<T>(this GameObject obj) where T : Component
    {
        T[] result = obj.GetComponents<T>();
        if (result.Length == 0)
            return obj.AddComponent<T>();

        T component = result.SingleOrDefault(c => c.GetType() == TypeCache<T>.Type);
        if (component != null)
            return component;

        return obj.AddComponent<T>();
    }
}