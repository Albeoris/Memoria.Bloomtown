using System;
using System.Collections.Generic;
using LazyBearTechnology;
using UnityEngine;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public sealed class Hotkey
{
    public static Hotkey None => new(KeyCode.None);

    public Hotkey(KeyCode key)
    {
        Key = key;
    }
    
    public Hotkey(GamepadButton button)
    {
        Button = button;
    }
    
    public Hotkey(GameKey action)
    {
        Action = action;
    }

    public Boolean MustHeld { get; set; }

    public KeyCode Key { get; } = KeyCode.None;
    public GamepadButton Button { get; } = GamepadButton.None;
    public GameKey Action { get; } = GameKey.None;

    public Boolean Alt { get; set; }
    public Boolean Shift { get; set; }
    public Boolean Control { get; set; }
    
    public IReadOnlyList<KeyCode> ModifierKeys { get; set; } = Array.Empty<KeyCode>();
    public IReadOnlyList<GamepadButton> ModifierButtons { get; set; } = Array.Empty<GamepadButton>();
    public IReadOnlyList<GameKey> ModifierActions { get; set; } = Array.Empty<GameKey>();

    public KeyCode[] ToKeyboardBindingAdditionalKeys()
    {
        List<KeyCode> result = null;
        
        if (Alt) (result ??= new()).Add(KeyCode.LeftAlt);
        if (Shift) (result ??= new()).Add(KeyCode.LeftShift);
        if (Control) (result ??= new()).Add(KeyCode.LeftControl);

        foreach (KeyCode keyCode in ModifierKeys)
        {
            result ??= new();

            if (keyCode == KeyCode.LeftAlt) continue;
            if (keyCode == KeyCode.LeftShift) continue;
            if (keyCode == KeyCode.LeftControl) continue;

            if (keyCode == KeyCode.RightAlt) result.Remove(KeyCode.LeftAlt);
            if (keyCode == KeyCode.RightShift) result.Remove(KeyCode.LeftShift);
            if (keyCode == KeyCode.RightControl) result.Remove(KeyCode.LeftControl);

            result.Add(keyCode);
        }

        return result is null ? Array.Empty<KeyCode>() : result.ToArray();
    }
}