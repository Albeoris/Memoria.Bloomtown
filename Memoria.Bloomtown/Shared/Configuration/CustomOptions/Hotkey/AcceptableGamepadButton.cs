using System;
using System.Linq;
using BepInEx.Configuration;
using LazyBearTechnology;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public sealed class AcceptableGamepadButton : AcceptableValueBase, IAcceptableValue<GamepadButton>
{
    public static AcceptableGamepadButton Instance { get; } = new();
    
    public AcceptableGamepadButton() : base(typeof(GamepadButton))
    {
        GamepadButtonTypeConverter.Init();
    }

    public override Object Clamp(Object value)
    {
        return value;
    }

    public override Boolean IsValid(Object value)
    {
        return true;
    }

    private static readonly String AcceptableValues = $"# Acceptable values: {String.Join(", ", EnumerationCache<GamepadButton>.NamedValues.Select(n => n.key))}";

    public override String ToDescriptionString()
    {
        return AcceptableValues;
    }

    public GamepadButton FromConfig(GamepadButton value) => value;

    public GamepadButton ToConfig(GamepadButton value) => value;
}