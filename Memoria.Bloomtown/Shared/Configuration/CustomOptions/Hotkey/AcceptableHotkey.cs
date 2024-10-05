using System;
using System.Linq;
using System.Text;
using BepInEx.Configuration;
using LazyBearTechnology;
using Object = System.Object;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public sealed class AcceptableHotkey : AcceptableValueBase, IAcceptableValue<Hotkey>
{
    private readonly String _optionName;

    public Boolean GamepadSupported { get; set; } = true;
    public Boolean GameKeysSupported { get; set; } = true;

    public AcceptableHotkey(String optionName) : base(typeof(Hotkey))
    {
        _optionName = optionName;
        HotkeyTypeConverter.Init();
    }
    
    public override Object Clamp(Object value)
    {
        return value;
    }

    public override Boolean IsValid(Object value)
    {
        return true;
    }

    private static readonly String AcceptableKeys = $"# Keyboard: https://docs.unity3d.com/ScriptReference/KeyCode.html";
    private static readonly String AcceptableButtons = $"# Gamepad: ({String.Join("), (", EnumerationCache<GamepadButton>.NamedValues.Select(n => n.key))})";
    private static readonly String AcceptableActions = $"# Actions: [{String.Join("], [", EnumerationCache<GameKey>.NamedValues.Select(n => n.key))}]";

    public override String ToDescriptionString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("# Ctrl+Alt+Shift+Key");
        if (GamepadSupported) sb.Append("+(GamepadButton)");
        if (GameKeysSupported) sb.Append("+[GameAction]");
        sb.AppendLine();
        
        sb.AppendLine(AcceptableKeys);
        if (GamepadSupported) sb.AppendLine(AcceptableButtons);
        if (GameKeysSupported) sb.AppendLine(AcceptableActions);
        sb.Length -= Environment.NewLine.Length;
        return sb.ToString();
    }

    public Hotkey FromConfig(Hotkey value)
    {
        return value;
    }

    public Hotkey ToConfig(Hotkey value) => value;
}