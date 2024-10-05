using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.Configuration;
using LazyBearTechnology;
using UnityEngine;
using Object = System.Object;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public static class HotkeyTypeConverter
{
    private static Type SupportedType { get; } = typeof(Hotkey);

    public static void Init()
    {
        if (!TomlTypeConverter.CanConvert(SupportedType))
        {
            TypeConverter converter = new() { ConvertToObject = ConvertToObject, ConvertToString = ConvertToString };
            TomlTypeConverter.AddConverter(SupportedType, converter);
        }
    }

    private static String ConvertToString(Object boxed, Type type)
    {
        CheckSupportedType(type);

        Hotkey hotkey = (Hotkey)boxed;
        return ConvertToString(hotkey);
    }

    public static String ConvertToString(Hotkey hotkey)
    {
        if (hotkey == default)
            return KeyCode.None.ToString();

        StringBuilder sb = new();
        if (hotkey.Control) sb.Append("Ctrl+");
        if (hotkey.Alt) sb.Append("Alt+");
        if (hotkey.Shift) sb.Append("Shift+");
        foreach (var modifier in hotkey.ModifierKeys)
        {
            sb.Append(modifier);
            sb.Append('+');
        }
        
        foreach (var modifier in hotkey.ModifierButtons)
        {
            sb.Append('(');
            sb.Append(EnumerationMapping<GamepadButton>.Names[modifier]);
            sb.Append(')');
            sb.Append('+');
        }

        foreach (var modifier in hotkey.ModifierActions)
        {
            sb.Append('[');
            sb.Append(EnumerationMapping<GameKey>.Names[modifier]);
            sb.Append(']');
            sb.Append('+');
        }

        if (hotkey.Action != GameKey.None)
        {
            sb.Append('[');
            sb.Append(EnumerationMapping<GameKey>.Names[hotkey.Action]);
            sb.Append(']');
        }
        else if (hotkey.Button != GamepadButton.None)
        {
            sb.Append('(');
            sb.Append(EnumerationMapping<GamepadButton>.Names[hotkey.Button]);
            sb.Append(')');
        }
        else
        {
            sb.Append(hotkey.Key);
        }

        if (hotkey.MustHeld)
            sb.Append("(Hold)");

        return sb.ToString();
    }

    private static Object ConvertToObject(String value, Type type)
    {
        CheckSupportedType(type);

        return ConvertToObject(value);
    }

    public static Hotkey ConvertToObject(String value)
    {
        if (String.IsNullOrWhiteSpace(value))
            return Hotkey.None;

        String[] split = value
            .Split('+', '-')
            .Select(v => v.Trim())
            .ToArray();

        if (split.Length == 0)
            return Hotkey.None;

        String last = split.Last();
        Boolean mustHeld = CheckMustHeld(ref last);

        if (last.Length == 0)
            return Hotkey.None;

        Boolean alt = false;
        Boolean shift = false;
        Boolean control = false;
        List<KeyCode> modifierKeys = new();
        List<GamepadButton> modifierButtons = new();
        List<GameKey> modifierActions = new();
        for (Int32 i = 0; i < split.Length - 1; i++)
        {
            switch (split[i].ToLowerInvariant())
            {
                case "ctrl":
                case "control":
                    control = true;
                    break;
                case "alt":
                    alt = true;
                    break;
                case "shift":
                    shift = true;
                    break;
                default:
                    if (TryParseGamepadButton(split[i], out GamepadButton modButton))
                        modifierButtons.Add(modButton);
                    else if (TryParseAction(split[i], out GameKey modAction))
                        modifierActions.Add(modAction);
                    else
                    {
                        var modKey = ParseKey(split[i]);
                        if (modKey != KeyCode.None)
                            modifierKeys.Add(modKey);
                    }
                    break;
            }
        }

        Hotkey result;
        if (TryParseGamepadButton(last, out GamepadButton button))
            result = new Hotkey(button);
        else if (TryParseAction(last, out GameKey action))
            result = new Hotkey(action);
        else
            result = new Hotkey(ParseKey(last));

        result.MustHeld = mustHeld;
        result.Control = control;
        result.Alt = alt;
        result.Shift = shift;
        result.ModifierKeys = modifierKeys.Count > 0 ? modifierKeys : Array.Empty<KeyCode>();
        result.ModifierButtons = modifierButtons.Count > 0 ? modifierButtons : Array.Empty<GamepadButton>();
        result.ModifierActions = modifierActions.Count > 0 ? modifierActions : Array.Empty<GameKey>();

        return result;
    }

    private static Boolean TryParseAction(String last, out GameKey action)
    {
        if (last[0] == '[' && last[^1] == ']')
        {
            String name = last.Substring(1, last.Length - 2);
            if (!EnumerationCache<GameKey>.NamedValues.TryGetValue(name, out action))
                action = GameKey.None;

            return true;
        }

        action = GameKey.None;
        return false;
    }

    private static Boolean TryParseGamepadButton(String last, out GamepadButton button)
    {
        if (last[0] == '(' && last[^1] == ')')
        {
            String name = last.Substring(1, last.Length - 2);
            if (!EnumerationCache<GamepadButton>.NamedValues.TryGetValue(name, out button))
                button = GamepadButton.None;

            return true;
        }

        button = GamepadButton.None;
        return false;
    }

    private static KeyCode ParseKey(String last)
    {
        return Enum.TryParse(last, true, out KeyCode key) 
            ? key 
            : KeyCode.None;
    }

    private static Boolean CheckMustHeld(ref String last)
    {
        if (last.EndsWith("(Hold)", StringComparison.InvariantCultureIgnoreCase))
        {
            last = last.Substring(0, last.Length - "(Hold)".Length).Trim();
            return true;
        }

        return false;
    }

    private static void CheckSupportedType(Type type)
    {
        if (type != SupportedType)
            throw new NotSupportedException($"An unexpected type has occurred: {type.FullName}. Expected: {SupportedType.FullName}");
    }
}