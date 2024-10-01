using System;
using BepInEx.Configuration;
using Object = System.Object;

namespace Memoria.Bloomtown.Configuration.Color;

public sealed class AcceptableColor : AcceptableValueBase, IAcceptableValue<UnityEngine.Color>
{
    private readonly String _optionName;

    public AcceptableColor(String optionName) : base(typeof(UnityEngine.Color))
    {
        _optionName = optionName;
        ColorTypeConverter.Init();
    }

    public override Object Clamp(Object value)
    {
        return value;
    }

    public override Boolean IsValid(Object value)
    {
        return true;
    }

    public override String ToDescriptionString()
    {
        return $"# Acceptable values: {ColorTypeConverter.ColorNames}";
    }

    public UnityEngine.Color FromConfig(UnityEngine.Color value) => value;
    public UnityEngine.Color ToConfig(UnityEngine.Color value) => value;
}