using System;
using System.Collections.Generic;
using UnityEngine;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public sealed class Hotkey
{
    public static Hotkey None => new Hotkey(KeyCode.None);
    
    public Boolean MustHeld { get; set; }

    public KeyCode Key { get; } = KeyCode.None;
    public String Action { get; } = "None";

    public Hotkey(KeyCode key)
    {
        Key = key;
    }

    public Hotkey(String action)
    {
        if (String.IsNullOrWhiteSpace(action) || action.Equals("None", StringComparison.InvariantCultureIgnoreCase))
            action = "None";
        
        Action = action;
    }
    
    public Boolean Alt { get; set; }
    public Boolean Shift { get; set; }
    public Boolean Control { get; set; }
    
    public IReadOnlyList<KeyCode> ModifierKeys { get; set; } = Array.Empty<KeyCode>();
    public IReadOnlyList<String> ModifierActions { get; set; } = Array.Empty<String>();
}