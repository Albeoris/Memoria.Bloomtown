using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.Configuration.Hotkey;
using Rewired;
using UnityEngine;

namespace Memoria.Bloomtown.Core;

public static class InputManager
{
    public static Boolean GetKey(KeyCode keyCode) => Check(keyCode, Input.GetKey);
    public static Boolean GetKeyDown(KeyCode keyCode) => Check(keyCode, Input.GetKeyDown);
    public static Boolean GetKeyUp(KeyCode keyCode) => Check(keyCode, Input.GetKeyUp);
    
    public static Boolean GetKey(GameKey action) => Check(action, LazyInput.GetKey);
    public static Boolean GetKeyDown(GameKey action) => Check(action, LazyInput.GetKeyDown);
    
    public static Boolean GetKey(GamepadButton keyCode) => Check(keyCode, GamepadButtonGetKey);
    public static Boolean GetKeyDown(GamepadButton keyCode) => Check(keyCode, GamepadButtonGetKeyDown);
    public static Boolean GetKeyUp(GamepadButton keyCode) => Check(keyCode, GamepadButtonGetKeyUp);

    public static Boolean IsToggled(Hotkey hotkey)
    {
        if (!(GetKeyUp(hotkey.Key) || GetKeyUp(hotkey.Button) || GetKeyDown(hotkey.Action)))
            return false;

        return IsModifiersPressed(hotkey);
    }

    public static Boolean IsHold(Hotkey hotkey)
    {
        if (!(GetKey(hotkey.Key) || GetKey(hotkey.Button) || GetKey(hotkey.Action)))
            return false;

        return IsModifiersPressed(hotkey);
    }

    private static Boolean IsModifiersPressed(Hotkey hotkey)
    {
        if (hotkey.Control)
        {
            if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)))
                return false;
        }

        if (hotkey.Alt)
        {
            if (!(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)))
                return false;
        }

        if (hotkey.Shift)
        {
            if (!(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
                return false;
        }

        if (!hotkey.ModifierKeys.All(GetKey))
            return false;

        if (!hotkey.ModifierButtons.All(GetKey))
            return false;
        
        if (!hotkey.ModifierActions.All(GetKey))
            return false;
        
        return true;
    }

    private static Boolean Check(KeyCode keyCode, Func<KeyCode, Boolean> checker)
    {
        return keyCode != KeyCode.None && checker(keyCode);
    }
    
    private static Boolean Check(GamepadButton gamepadButton, Func<GamepadButton, Boolean> checker)
    {
        return gamepadButton != GamepadButton.None && checker(gamepadButton);
    }

    private static Boolean Check(GameKey action, Func<GameKey, Boolean> checker)
    {
        return action != GamepadButton.None && checker(action);
    }
    
    private static Boolean GamepadButtonGetKey(GamepadButton button)
    {
        Int32 rewiredCode = RewiredBindings[button];
        return InputPlayer.GetButton(rewiredCode);
    }

    private static Boolean GamepadButtonGetKeyDown(GamepadButton button)
    {
        Int32 rewiredCode = RewiredBindings[button];
        return InputPlayer.GetButtonDown(rewiredCode);
    }
    
    private static Boolean GamepadButtonGetKeyUp(GamepadButton button)
    {
        Int32 rewiredCode = RewiredBindings[button];
        return InputPlayer.GetButtonUp(rewiredCode);
    }

    private static Player InputPlayer = ReInput.players.GetPlayer(0);

    private static Dictionary<GamepadButton, int> RewiredBindings = new()
    {
        { GamepadButton.X, 2 },
        { GamepadButton.Y, 3 },
        { GamepadButton.A, 4 },
        { GamepadButton.B, 5 },
        { GamepadButton.LB, 6 },
        { GamepadButton.RB, 7 },
        { GamepadButton.LT, 8 },
        { GamepadButton.RT, 9 },
        { GamepadButton.Back, 10 },
        { GamepadButton.Start, 11 },
        { GamepadButton.DUp, 12 },
        { GamepadButton.DDown, 13 },
        { GamepadButton.DLeft, 14 },
        { GamepadButton.DRight, 15 }
    };
}