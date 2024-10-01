using System;
using UnityEngine;
using Object = System.Object;

namespace Memoria.Bloomtown.Core;

public sealed class GameSpeedControl : SafeComponent
{
    public GameSpeedControl()
    {
    }

    private readonly HotkeyControl _speedUpKey = new();
    private Single _nativeFactor;
    private Single _knownFactor;

    protected override void Update()
    {
        ProcessSpeedUp();
    }

    private void ProcessSpeedUp()
    {
        Single currentFactor = Time.timeScale;
        if (currentFactor == 0.0f) // Do not unpause the game 
            return;

        var config = ModComponent.Instance.Config.Speed;
        _speedUpKey.Update(config.Key);

        if (currentFactor != _knownFactor)
            _nativeFactor = currentFactor;

        if (_speedUpKey.IsHeld)
        {
            _knownFactor = _nativeFactor * config.HoldFactor;
            UpdateIndicator(flag: true);
        }
        else if (_speedUpKey.IsToggled)
        {
            _knownFactor = _nativeFactor * config.ToggleFactor;
            UpdateIndicator(flag: true);
        }
        else
        {
            _knownFactor = _nativeFactor;
            UpdateIndicator(flag: false);
        }

        Time.timeScale = _knownFactor;
    }

    private static void UpdateIndicator(Boolean flag)
    {
        if (flag)
        {
            //ModComponent.Instance.Drawer.Add("s");
        }
        else
        {
            //ModComponent.Instance.Drawer.Remove("s");
        }
    }
}