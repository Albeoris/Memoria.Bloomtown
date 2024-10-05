using System;
using System.Linq;
using System.Text;
using BepInEx.Configuration;
using LazyBearTechnology;

namespace Memoria.Bloomtown.Configuration.Hotkey;

public sealed class AcceptableHotkeyGroup : AcceptableValueBase, IAcceptableValue<HotkeyGroup>
{
    private readonly Boolean _canHold;

    public Boolean GamepadSupported { get; set; } = true;
    public Boolean GameKeysSupported { get; set; } = true;
    
    public AcceptableHotkeyGroup(Boolean canHold) : base(typeof(HotkeyGroup))
    {
        _canHold = canHold;
        HotkeyGroupTypeConverter.Init();
    }

    public override Object Clamp(Object value)
    {
        if (_canHold)
            return value;
        
        if (value is HotkeyGroup group)
        {
            if (group.Keys.Any(h => h.MustHeld))
                return HotkeyGroup.Create(group.Keys.Where(h => !h.MustHeld).ToArray());
        }

        return value;
    }

    public override Boolean IsValid(Object value)
    {
        return true;
    }
    
    private static readonly String AcceptableKeys = $"# Keyboard: https://docs.unity3d.com/ScriptReference/KeyCode.html";
    private static readonly String AcceptableButtons = $"# Buttons: ({String.Join("), (", EnumerationCache<GamepadButton>.NamedValues.Select(n => n.key))})";
    private static readonly String AcceptableActions = $"# Actions: [{String.Join("], [", EnumerationCache<GameKey>.NamedValues.Select(n => n.key))}]";

    public override String ToDescriptionString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("# Ctrl+Alt+Shift+Key");
        if (GamepadSupported) sb.Append("+(GamepadButton)");
        if (GameKeysSupported) sb.Append("+[GameAction]");
        sb.AppendLine(_canHold ? "(Hold)" : String.Empty);
        
        sb.AppendLine(AcceptableKeys);
        if (GamepadSupported) sb.AppendLine(AcceptableButtons);
        if (GameKeysSupported) sb.AppendLine(AcceptableActions);
        sb.Length -= Environment.NewLine.Length;
        return sb.ToString();
    }

    public HotkeyGroup FromConfig(HotkeyGroup value) => value;

    public HotkeyGroup ToConfig(HotkeyGroup value) => value;
}